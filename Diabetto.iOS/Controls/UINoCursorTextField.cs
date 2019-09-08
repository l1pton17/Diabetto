using Foundation;
using ObjCRuntime;
using UIKit;

namespace Diabetto.iOS.Controls
{
    public sealed class UINoCursorTextField : UITextField
    {
        public UINoCursorTextField()
        {
            TintColor = UIColor.Clear;
        }

        public override bool CanPerform(Selector action, NSObject withSender)
        {
            return false;
        }
    }
}