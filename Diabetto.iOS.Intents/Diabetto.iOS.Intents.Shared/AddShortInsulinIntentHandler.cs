using System;
using Diabetto.Core.Models;
using Diabetto.Core.Services.Repositories;
using Intents;
using MvvmCross;
using MvvmCross.Logging;

namespace Diabetto.iOS.Intents.Shared
{
    public sealed class AddShortInsulinIntentHandler : AddShortInsulinIntentHandling
    {
        public override void HandleAddShortInsulin(AddShortInsulinIntent intent, Action<AddShortInsulinIntentResponse> completion)
        {
            try
            {
                if (intent == null)
                {
                    completion(new AddShortInsulinIntentResponse(AddShortInsulinIntentResponseCode.Failure, null));

                    return;
                }

                var shortInsulin = intent.ShortInsulin;
                var breadUnits = intent.BreadUnits;

                var hasShortInsulin = shortInsulin.Int32Value > 0;
                var hasBreadUnits = breadUnits.DoubleValue > 0;

                var hasSomeValues = hasBreadUnits || hasShortInsulin;

                if (!hasSomeValues)
                {
                    completion(new AddShortInsulinIntentResponse(AddShortInsulinIntentResponseCode.Failure, null));

                    return;
                }

                var measureService = new MeasureService(new SharedDatabaseConnectionStringHolder());

                var measure = new Measure
                {
                    Date = DateTime.UtcNow,
                    Level = null,
                    LongInsulin = 0,
                    ShortInsulin = shortInsulin.Int32Value
                };

                measureService.AddAsync(measure).GetAwaiter().GetResult();
                completion(new AddShortInsulinIntentResponse(AddShortInsulinIntentResponseCode.Success, null));
            }
            catch (Exception e)
            {
                var logger = Mvx.IoCProvider
                    .Resolve<IMvxLogProvider>()
                    .GetLogFor<AddShortInsulinIntentHandler>();

                logger.ErrorException("While handling add short insulin intent", e);

                completion(new AddShortInsulinIntentResponse(AddShortInsulinIntentResponseCode.Failure, null));
            }
        }

        public override void ResolveShortInsulinForAddShortInsulin(AddShortInsulinIntent intent, Action<INIntegerResolutionResult> completion)
        {
            try
            {
                Console.WriteLine(intent.ShortInsulin.Int32Value);

                completion(INIntegerResolutionResult.GetSuccess(intent.ShortInsulin.Int32Value));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override void ResolveBreadUnitsForAddShortInsulin(AddShortInsulinIntent intent, Action<INDoubleResolutionResult> completion)
        {
            try
            {
                Console.WriteLine(intent.BreadUnits.DoubleValue);

                completion(INDoubleResolutionResult.GetSuccess(intent.BreadUnits.DoubleValue));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}