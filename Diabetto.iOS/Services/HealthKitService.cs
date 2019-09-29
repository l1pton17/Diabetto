using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using Diabetto.Core.Services.Repositories;
using Foundation;
using HealthKit;

namespace Diabetto.iOS.Services
{
    public interface IHealthKitService
    {
        bool GetStatus();
        Task Export();
        Task<bool> Enable();
        void Disable();
    }

    public sealed class HealthKitService : IHealthKitService, IDisposable
    {
        private const string _userDefaultsKey = "health.kit.enabled";

        private static class MetadataKey
        {
            public const string DiabettoOrigin = "diabetto.origin";

            public static string GetIdentifier(int id)
            {
                return "diabetto." + id;
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

        public HealthKitService(IMeasureService measureService)
        {
            _measureService = measureService ?? throw new ArgumentNullException(nameof(measureService));
            _healthStore = new HKHealthStore();
        }

        public async Task Export()
        {
            var measures = await _measureService.GetAllAsync();

            var measureSamples = measures
                .Where(v => v.ShortInsulin > 0 || v.LongInsulin > 0)
                .SelectMany(v => GetInsulinSamples(v))
                .OfType<HKObject>()
                .ToArray();

            var saveMeasureSamplesResult = await _healthStore.SaveObjectsAsync(measureSamples);

            if (!saveMeasureSamplesResult.Item1)
            {
                throw new InvalidOperationException(saveMeasureSamplesResult.Item2.LocalizedDescription);
            }
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
            var accessGranted = await _healthStore.RequestAuthorizationToShareAsync(_dataTypesToWrite, _dataTypesToRead);

            if (accessGranted.Item1)
            {
                var pList = NSUserDefaults.StandardUserDefaults;
                pList.SetBool(true, _userDefaultsKey);
                pList.Synchronize();
            }

            return accessGranted.Item1;
        }

        /// <inheritdoc />
        public void Disable()
        {
            var pList = NSUserDefaults.StandardUserDefaults;
            pList.SetBool(false, _userDefaultsKey);
            pList.Synchronize();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _healthStore?.Dispose();
        }

        private static IEnumerable<HKQuantitySample> GetInsulinSamples(Measure measure)
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
                    isLong: false,
                    measure.ShortInsulin);
            }

            if (measure.LongInsulin > 0)
            {
                yield return CreateInsulinHKSample(
                    measure.Id,
                    measure.Date,
                    measure.Version,
                    isLong: true,
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
            var metadata = new NSDictionary
            {
                [MetadataKey.DiabettoOrigin] = (NSString) "true",
                [HKMetadataKey.SyncVersion] = (NSString) version.ToString(),
                [HKMetadataKey.SyncIdentifier] = (NSString) MetadataKey.GetIdentifier(measureId)
            };

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
            bool isLong,
            int value
        )
        {
            var metadata = new NSDictionary
            {
                [HKMetadataKey.InsulinDeliveryReason] = isLong
                    ? (NSString) HKInsulinDeliveryReason.Basal.ToString()
                    : (NSString) HKInsulinDeliveryReason.Bolus.ToString(),
                [MetadataKey.DiabettoOrigin] = (NSString) "true",
                [HKMetadataKey.SyncVersion] = (NSString) version.ToString(),
                [HKMetadataKey.SyncIdentifier] = (NSString) MetadataKey.GetIdentifier(measureId)
            };

            return HKQuantitySample.FromType(
                HKQuantityType.Create(HKQuantityTypeIdentifier.InsulinDelivery),
                HKQuantity.FromQuantity(HKUnit.InternationalUnit, value),
                (NSDate) date,
                (NSDate) date,
                metadata);
        }
    }
}