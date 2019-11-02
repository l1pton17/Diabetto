using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Events.Measures;
using Diabetto.Core.Helpers;
using Diabetto.Core.Models;
using Diabetto.Core.Services.Repositories;
using Diabetto.iOS.Extensions;
using Foundation;
using HealthKit;
using MvvmCross.Logging;
using ReactiveUI;

namespace Diabetto.iOS.Services
{
    public interface IHealthKitService
    {
        bool GetStatus();
        Task Export();
        Task<bool> Enable();
        void Disable();
        IDisposable Suppress();
    }

    public sealed class HealthKitService : IHealthKitService, IDisposable
    {
        private const string _userDefaultsKey = "health.kit.enabled";

        private static class MetadataKey
        {
            public const string DiabettoOrigin = "diabetto.origin";

            public static string GetExternalUUID(int id)
            {
                return "diabetto." + id;
            }

            public static string GetBloodGlucoseIdentifier(int id)
            {
                return "diabetto." + id + ".glucose";
            }

            public static string GetInsulinIdentifier(int id, InsulinType type)
            {
                return "diabetto." + id + ".insulin." + type.ToString().ToLowerInvariant();
            }
        }

        private static readonly NSSet _dataTypesToWrite = NSSet.MakeNSObjectSet(
            new HKObjectType[]
            {
                HKQuantityType.Create(HKQuantityTypeIdentifier.BloodGlucose),
                HKQuantityType.Create(HKQuantityTypeIdentifier.InsulinDelivery)
            });

        private static readonly NSSet _dataTypesToRead = NSSet.MakeNSObjectSet(
            new HKObjectType[]
            {
                HKQuantityType.Create(HKQuantityTypeIdentifier.BloodGlucose),
                HKQuantityType.Create(HKQuantityTypeIdentifier.InsulinDelivery)
            });

        private readonly HKHealthStore _healthStore;
        private readonly IMeasureService _measureService;
        private readonly IMvxLog _log;

        private bool _suppressed;

        public ReactiveCommand<Measure, Unit> AddCommand { get; }

        public ReactiveCommand<Measure, Unit> EditCommand { get; }

        public ReactiveCommand<Measure, Unit> DeleteCommand { get; }

        public HealthKitService(
            IMeasureService measureService,
            IMvxLogProvider logProvider
        )
        {
            _log = logProvider.GetLogFor<HealthKitService>();
            _measureService = measureService ?? throw new ArgumentNullException(nameof(measureService));
            _healthStore = new HKHealthStore();

            AddCommand = ReactiveCommand.CreateFromTask<Measure>(
                v => AddSamples(GetQuantitySamples(v)));

            AddCommand.ThrownExceptions
                .Subscribe(e => _log.ErrorException("While add measure to health kit", e));

            EditCommand = ReactiveCommand.CreateFromTask<Measure>(EditMeasure);

            EditCommand.ThrownExceptions
                .Subscribe(e => _log.ErrorException("While edit measure to health kit", e));

            DeleteCommand = ReactiveCommand.CreateFromTask<Measure>(DeleteSamples);

            DeleteCommand.ThrownExceptions
                .Subscribe(e => _log.ErrorException("While deleting measure", e));

            MessageBus.Current
                .ListenIncludeLatest<MeasureAddedEvent>()
                .Select(v => v?.Value)
                .Where(v => v != null)
                .Where(v => GetStatus())
                .InvokeCommand(this, v => v.AddCommand);

            MessageBus.Current
                .ListenIncludeLatest<MeasureChangedEvent>()
                .Select(v => v?.Value)
                .Where(v => v != null)
                .Where(v => GetStatus())
                .InvokeCommand(this, v => v.EditCommand);

            MessageBus.Current
                .ListenIncludeLatest<MeasureDeletedEvent>()
                .Select(v => v?.Value)
                .Where(v => v != null)
                .Where(_ => GetStatus())
                .InvokeCommand(this, v => v.DeleteCommand);
        }

        public async Task Export()
        {
            var measures = await _measureService.GetAllAsync();

            var measureSamples = measures
                .SelectMany(GetQuantitySamples);

            await AddSamples(measureSamples);
        }

        public bool GetStatus()
        {
            var pList = NSUserDefaults.StandardUserDefaults;
            var isEnabled = pList.BoolForKey(_userDefaultsKey);
            var isBloodGlucoseStatus = _healthStore.GetAuthorizationStatus(HKQuantityType.Create(HKQuantityTypeIdentifier.BloodGlucose));
            var isInsulinStatus = _healthStore.GetAuthorizationStatus(HKQuantityType.Create(HKQuantityTypeIdentifier.InsulinDelivery));

            var hasAccessToHealthKit = isBloodGlucoseStatus == HKAuthorizationStatus.SharingAuthorized
             || isInsulinStatus == HKAuthorizationStatus.SharingAuthorized;

            return isEnabled && hasAccessToHealthKit;
        }

        /// <inheritdoc />
        public async Task<bool> Enable()
        {
            var currentStatus = GetStatus();

            if (currentStatus)
            {
                return true;
            }

            var accessGranted = await _healthStore.RequestAuthorizationToShareAsync(_dataTypesToWrite, _dataTypesToRead);

            if (accessGranted.Item1)
            {
                var pList = NSUserDefaults.StandardUserDefaults;
                pList.SetBool(true, _userDefaultsKey);
                pList.Synchronize();
            }
            else
            {
                _log.Warn("Can't obtain authorization to health kit");
            }

            return accessGranted.Item1;
        }

        /// <inheritdoc />
        public void Disable()
        {
            var currentStatus = GetStatus();

            if (!currentStatus)
            {
                return;
            }

            var pList = NSUserDefaults.StandardUserDefaults;
            pList.SetBool(false, _userDefaultsKey);
            pList.Synchronize();
        }

        /// <inheritdoc />
        public IDisposable Suppress()
        {
            _suppressed = true;

            return new ActionDisposable(() => _suppressed = false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _healthStore?.Dispose();
        }

        private async Task DeleteSamples(Measure measure)
        {
            if (_suppressed)
            {
                return;
            }

            var predicate = HKQuery.GetPredicateForMetadataKey(
                HKMetadataKey.ExternalUuid,
                new[] {NSObject.FromObject(MetadataKey.GetExternalUUID(measure.Id))});

            var result = await _healthStore.DeleteObjectsAsync(
                HKQuantityType.Create(HKQuantityTypeIdentifier.BloodGlucose),
                predicate);

            if (!result.Item1)
            {
                _log.Warn($"Can't delete blood glucose data by reason: {result.Item2.LocalizedDescription}");
            }

            result = await _healthStore.DeleteObjectsAsync(
                HKQuantityType.Create(HKQuantityTypeIdentifier.InsulinDelivery),
                predicate);

            if (!result.Item1)
            {
                _log.Warn($"Can't delete insulin data by reason: {result.Item2.LocalizedDescription}");
            }
        }

        private async Task EditMeasure(Measure measure)
        {
            if (_suppressed)
            {
                return;
            }

            if (!measure.Level.HasValue)
            {
                var predicate = HKQuery.GetPredicateForMetadataKey(
                    HKMetadataKey.ExternalUuid,
                    new[] { NSObject.FromObject(MetadataKey.GetExternalUUID(measure.Id)) });

                await _healthStore.DeleteObjectsAsync(
                    HKQuantityType.Create(HKQuantityTypeIdentifier.BloodGlucose),
                    predicate);
            }

            var samples = GetQuantitySamples(measure);

            await AddSamples(samples);
        }

        private async Task AddSamples(IEnumerable<HKObject> objects)
        {
            if (_suppressed)
            {
                return;
            }

            var samples = objects.ToArray();

            if (samples.Length == 0)
            {
                return;
            }

            var result = await _healthStore.SaveObjectsAsync(samples);

            if (!result.Item1)
            {
                throw new InvalidOperationException($"Can't sync health kit: {result.Item2.LocalizedDescription}");
            }
        }

        private static IEnumerable<HKObject> GetQuantitySamples(Measure measure)
        {
            if (measure.Level.HasValue)
            {
                yield return CreateBloodGlucoseHKSample(
                    measure.Id,
                    measure.Date,
                    measure.Version,
                    measure.Level.Value);
            }

            if (measure.ShortInsulin > 0)
            {
                yield return CreateInsulinHKSample(
                    measure.Id,
                    measure.Date,
                    measure.Version,
                    type: InsulinType.Bolus,
                    measure.ShortInsulin);
            }

            if (measure.LongInsulin > 0)
            {
                yield return CreateInsulinHKSample(
                    measure.Id,
                    measure.Date,
                    measure.Version,
                    type: InsulinType.Basal,
                    measure.LongInsulin);
            }
        }

        private static HKQuantitySample CreateBloodGlucoseHKSample(
            int measureId,
            DateTime date,
            int version,
            int level
        )
        {
            var metadata = new NSDictionary(
                MetadataKey.DiabettoOrigin,
                NSObject.FromObject(true),
                HKMetadataKey.TimeZone,
                NSObject.FromObject("UTC"),
                HKMetadataKey.SyncVersion,
                NSObject.FromObject(version),
                HKMetadataKey.WasUserEntered,
                NSObject.FromObject(true),
                HKMetadataKey.ExternalUuid,
                NSObject.FromObject(MetadataKey.GetExternalUUID(measureId)),
                HKMetadataKey.SyncIdentifier,
                NSObject.FromObject(MetadataKey.GetBloodGlucoseIdentifier(measureId)));

            return HKQuantitySample.FromType(
                HKQuantityType.Create(HKQuantityTypeIdentifier.BloodGlucose),
                HKQuantity.FromQuantity(
                    HKUnit
                        .CreateMoleUnit(HKMetricPrefix.Milli, HKUnit.MolarMassBloodGlucose)
                        .UnitDividedBy(HKUnit.Liter),
                    level / 10.0),
                (NSDate) date,
                (NSDate) date,
                metadata);
        }

        private static HKQuantitySample CreateInsulinHKSample(
            int measureId,
            DateTime date,
            int version,
            InsulinType type,
            int value
        )
        {
            var metadata = new NSDictionary(
                MetadataKey.DiabettoOrigin,
                NSObject.FromObject(true),
                HKMetadataKey.InsulinDeliveryReason,
                NSObject.FromObject(
                    type == InsulinType.Basal
                        ? HKInsulinDeliveryReason.Basal
                        : HKInsulinDeliveryReason.Bolus),
                HKMetadataKey.TimeZone,
                NSObject.FromObject("UTC"),
                HKMetadataKey.ExternalUuid,
                NSObject.FromObject(MetadataKey.GetExternalUUID(measureId)),
                HKMetadataKey.WasUserEntered,
                NSObject.FromObject(true),
                HKMetadataKey.SyncVersion,
                NSObject.FromObject(version),
                HKMetadataKey.SyncIdentifier,
                NSObject.FromObject(MetadataKey.GetInsulinIdentifier(measureId, type)));

            return HKQuantitySample.FromType(
                HKQuantityType.Create(HKQuantityTypeIdentifier.InsulinDelivery),
                HKQuantity.FromQuantity(HKUnit.InternationalUnit, value),
                (NSDate) date,
                (NSDate) date,
                metadata);
        }
    }
}