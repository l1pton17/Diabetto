using UIKit;

namespace Diabetto.iOS.Extensions
{
    public static class UIViewControllerExtensions
    {
        public static void HideKeyboardWhenTappedAround(this UIViewController source)
        {
            source.View.AddGestureRecognizer(
                new UITapGestureRecognizer(() => source.View.EndEditing(true))
                {
                    CancelsTouchesInView = false
                });
        }
    }
}