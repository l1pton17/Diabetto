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
            Console.WriteLine("get the intent handler");

            return new AddMeasureIntentHandler();
        }
    }
}
