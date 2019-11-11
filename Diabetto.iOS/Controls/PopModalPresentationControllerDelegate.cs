using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Diabetto.iOS.Controls
{
    public class PopModalPresentationControllerDelegate : UIAdaptivePresentationControllerDelegate
    {
        private readonly Func<Task> _dismissModal;

        public PopModalPresentationControllerDelegate(Func<Task> disposeViewAsync)
        {
            _dismissModal = disposeViewAsync ?? throw new ArgumentNullException(nameof(disposeViewAsync));
        }

        [Export("presentationControllerDidDismiss:")]
        public override void DidDismiss(UIPresentationController presentationController)
        {
            _dismissModal();
        }
    }
}