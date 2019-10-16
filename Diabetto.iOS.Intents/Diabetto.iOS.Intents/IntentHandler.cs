using System;
using Diabetto.iOS.MeasureKit;
using Foundation;
using Intents;

namespace Diabetto.iOS.Intents
{
    [Register("IntentHandler")]
    public class IntentHandler : INExtension
    {
        protected IntentHandler(IntPtr handle)
            : base(handle)
        {
        }

        public override NSObject GetHandler(INIntent intent)
        {
            if (intent is AddMeasureIntent)
            {
                return new AddMeasureIntentHandler();
            }

            throw new Exception($"Unhandled intent type: {intent}");
        }
    }
}
