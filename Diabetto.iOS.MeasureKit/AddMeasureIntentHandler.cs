using System;
using Diabetto.Core.Models;
using Diabetto.Core.Services;
using Diabetto.Core.Services.Repositories;
using Intents;
using MvvmCross;
using MvvmCross.Logging;

namespace Diabetto.iOS.MeasureKit
{
    public sealed class AddMeasureIntentHandler : AddMeasureIntentHandling
    {
        /// <inheritdoc />
        public override void HandleAddMeasure(AddMeasureIntent intent, Action<AddMeasureIntentResponse> completion)
        {
            var level = intent.Level;
            var shortInsulin = intent.Shortinsulin;
            var breadUnits = intent.Breadunits;

            var hasLevel = level.DoubleValue > 0;
            var hasShortInsulin = shortInsulin.Int32Value > 0;
            var hasBreadUnits = breadUnits.DoubleValue > 0;

            var hasSomeValues = hasLevel || hasBreadUnits || hasShortInsulin;

            if (!hasSomeValues)
            {
                completion(new AddMeasureIntentResponse(AddMeasureIntentResponseCode.Failure, null));

                return;
            }

            try
            {
                var measureService = Mvx.IoCProvider.Resolve<IMeasureService>();
                var timeProvider = Mvx.IoCProvider.Resolve<ITimeProvider>();

                var measure = new Measure
                {
                    Date = timeProvider.UtcNow,
                    Level = hasLevel ? (int?) level * 10 : null,
                    LongInsulin = 0,
                    ShortInsulin = shortInsulin.Int32Value
                };

                measureService.AddAsync(measure).GetAwaiter().GetResult();
                completion(new AddMeasureIntentResponse(AddMeasureIntentResponseCode.Success, null));
            }
            catch (Exception e)
            {
                var logger = Mvx.IoCProvider
                    .Resolve<IMvxLogProvider>()
                    .GetLogFor<AddMeasureIntentHandler>();

                logger.ErrorException("While handling add measure intent", e);
                completion(new AddMeasureIntentResponse(AddMeasureIntentResponseCode.Failure, null));
            }
        }

        /// <inheritdoc />
        public override void ResolveLevelForAddMeasure(AddMeasureIntent intent, Action<AddMeasureLevelResolutionResult> completion)
        {
            completion((AddMeasureLevelResolutionResult) INDoubleResolutionResult.GetSuccess(intent.Level.DoubleValue));
        }

        /// <inheritdoc />
        public override void ResolveShortinsulinForAddMeasure(AddMeasureIntent intent, Action<AddMeasureShortinsulinResolutionResult> completion)
        {
            completion((AddMeasureShortinsulinResolutionResult) INIntegerResolutionResult.GetSuccess(intent.Shortinsulin.Int32Value));
        }

        /// <inheritdoc />
        public override void ResolveBreadunitsForAddMeasure(AddMeasureIntent intent, Action<AddMeasureBreadunitsResolutionResult> completion)
        {
            completion((AddMeasureBreadunitsResolutionResult) INDoubleResolutionResult.GetSuccess(intent.Level.DoubleValue));
        }
    }
}