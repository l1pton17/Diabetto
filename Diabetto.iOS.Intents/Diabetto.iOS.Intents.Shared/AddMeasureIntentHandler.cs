using System;
using Diabetto.Core.Models;
using Diabetto.Core.Services.Repositories;
using Intents;
using MvvmCross;
using MvvmCross.Logging;
using ObjCRuntime;

namespace Diabetto.iOS.Intents.Shared
{

    public sealed class AddMeasureIntentHandler : AddMeasureIntentHandling
    {
        public override void ConfirmAddMeasure(AddMeasureIntent intent, Action<AddMeasureIntentResponse> completion)
        {
            completion(new AddMeasureIntentResponse(AddMeasureIntentResponseCode.Success, null));
        }

        /// <inheritdoc />
        public override void HandleAddMeasure(AddMeasureIntent intent, Action<AddMeasureIntentResponse> completion)
        {
            try
            {
                if (intent == null)
                {
                    completion(new AddMeasureIntentResponse(AddMeasureIntentResponseCode.Failure, null));

                    return;
                }

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

                var measureService = new MeasureService(new SharedDatabaseConnectionStringHolder());

                var measure = new Measure
                {
                    Date = DateTime.UtcNow,
                    Level = hasLevel ? (int?) (level.DoubleValue * 10) : null,
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
        public override void ResolveLevelForAddMeasure(AddMeasureIntent intent, Action<INDoubleResolutionResult> completion)
        {
            try
            {
                Console.WriteLine(intent.Level.DoubleValue);

                completion(intent.Level.DoubleValue <= Double.Epsilon
                    ? INDoubleResolutionResult.NeedsValue
                    : INDoubleResolutionResult.GetSuccess(intent.Level.DoubleValue));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <inheritdoc />
        public override void ResolveShortinsulinForAddMeasure(AddMeasureIntent intent, Action<INIntegerResolutionResult> completion)
        {
            try
            {
                Console.WriteLine(intent.Shortinsulin.Int32Value);

                completion(INIntegerResolutionResult.GetSuccess(intent.Shortinsulin.Int32Value));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <inheritdoc />
        public override void ResolveBreadunitsForAddMeasure(AddMeasureIntent intent, Action<INDoubleResolutionResult> completion)
        {
            try
            {
                Console.WriteLine(intent.Breadunits.DoubleValue);

                completion(INDoubleResolutionResult.GetSuccess(intent.Breadunits.DoubleValue));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}