using Foundation;

namespace Diabetto.iOS.Helpers
{
    public static class NSUserActivityHelper
    {
        public static string AddMeasureActivityType = "com.diabetto.main.addMeasure";

        public static NSUserActivity AddMeasureActivity
        {
            get
            {
                var userActivity = new NSUserActivity(AddMeasureActivityType)
                {
                    Title = "Add measure",
                    EligibleForSearch = true,
                    EligibleForPrediction = true,
                    SuggestedInvocationPhrase = "Add measure"
                };

                return userActivity;
            }
        }
    }
}
 