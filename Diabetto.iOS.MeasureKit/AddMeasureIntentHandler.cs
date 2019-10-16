using System;
using Diabetto;
using Foundation;
using ObjCRuntime;

namespace Diabetto.iOS.MeasureKit
{
    public sealed class AddMeasureIntentHandler : AddMeasureIntentHandling
    {
        public void HandleAddMeasure(AddMeasureIntent intent, Action<AddMeasureIntentResponse> completion)
        {

        }

        public void ResolveLevelForAddMeasure(AddMeasureIntent intent, Action<AddMeasureLevelResolutionResult> completion)
        {

        }

        public void ResolveBreadunitsForAddMeasure(AddMeasureIntent intent, Action<AddMeasureBreadunitsResolutionResult> completion)
        {

        }

        public void ResolveShortinsulinForAddMeasure(AddMeasureIntent intent, Action<AddMeasureShortinsulinResolutionResult> completion)
        {

        }

        public void ConfirmAddMeasure(AddMeasureIntent intent, Action<AddMeasureIntentResponse> completion)
        {
        }
    }
}