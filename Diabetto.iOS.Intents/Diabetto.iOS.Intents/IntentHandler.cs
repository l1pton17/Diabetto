using System;
using Diabetto.iOS.Intents.Shared;
using Foundation;
using Intents;

namespace Diabetto.iOS.Intents
{
    [Register("IntentHandler")]
    public class IntentHandler : INExtension
    {
        public IntentHandler(IntPtr handle)
            : base(handle)
        {
        }

        public override NSObject GetHandler(INIntent intent)
        {
            Console.WriteLine("Get the intent handler");

            switch (intent)
            {
                case AddMeasureIntent _:
                    return new AddMeasureIntentHandler();

                case AddShortInsulinIntent _:
                    return new AddShortInsulinIntentHandler();
            }

            throw new InvalidOperationException($"Unhandled intent type: ${intent}");
        }
    }
}
