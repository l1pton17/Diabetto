using System;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Diabetto.iOS.Dialogs
{
    public sealed class PickerDialog<T> : UIView
        where T : UIPickerViewModel
    {
        private const float _datePickerDialogDefaultButtonHeight = 50;
        private const float _datePickerDialogDefaultButtonSpacerHeight = 1;
        private const float _datePickerDialogCornerRadius = 7;
        private const int _datePickerDialogCancelButtonTag = 1;
        private const int _datePickerDialogDoneButtonTag = 2;

        private Action<T> _callback;
        private Action<UIPickerView> _pickerCallback;
        private UIButton _cancelButton;
        private String _cancelButtonTitle;
        private Action _cancelCallback;
        private T _pickerModel;

        /* Views */
        private UIView _dialogView;
        private UIButton _doneButton;
        private String _doneButtonTitle;

        private bool _isDialogShown; // flag to show whether dialog is opened

        /* Vars */
        private String _title;
        private UILabel _titleLabel;
        private UIPickerView _pickerView;

        public void ReloadAllComponents()
        {
            _pickerView.ReloadAllComponents();
        }

        public void Show(
            string title,
            Action<T> callback,
            T pickerModel,
            Action<UIPickerView> pickerCallback = null,
            Action cancelCallback = null
        )
        {
            Show(
                title,
                doneButtonTitle: "Done",
                cancelButtonTitle: "Cancel",
                callback,
                pickerModel,
                pickerCallback,
                cancelCallback);
        }

        public void Show(
            string title,
            string doneButtonTitle,
            string cancelButtonTitle,
            Action<T> callback,
            T pickerModel,
            Action<UIPickerView> pickerCallback = null,
            Action cancelCallback = null
        )
        {
            if (_isDialogShown)
            {
                return; // if already dialog is shown, do not open another dialog
            }

            _isDialogShown = true; // update the flag
            _title = title;
            _doneButtonTitle = doneButtonTitle;
            _cancelButtonTitle = cancelButtonTitle;
            _pickerModel = pickerModel;
            _pickerCallback = pickerCallback;
            _callback = callback;
            _cancelCallback = cancelCallback;
            _dialogView = CreateContainerView();
            _dialogView.Layer.ShouldRasterize = true;
            _dialogView.Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            _dialogView.Layer.Opacity = 0.5f;
            _dialogView.Layer.Transform = CATransform3D.MakeScale(sx: 1.3f, sy: 1.3f, sz: 1);

            BackgroundColor = UIColor.FromRGBA(
                red: 0,
                green: 0,
                blue: 0,
                alpha: 0);

            AddSubview(_dialogView);
            var widthHeight = Frame.Width > Frame.Height ? Frame.Width : Frame.Height;

            Frame = new CGRect(
                x: 0,
                y: 0,
                widthHeight,
                widthHeight); // create overlay with max value from height and width so that on rotation whole screen is covered

            UIApplication.SharedApplication.Windows[0].AddSubview(this);

            Animate(
                duration: 0.2,
                delay: 0d,
                UIViewAnimationOptions.CurveEaseInOut,
                animation: () =>
                {
                    BackgroundColor = UIColor.Black.ColorWithAlpha(alpha: 0.4f);
                    _dialogView.Layer.Opacity = 1;
                    _dialogView.Layer.Transform = CATransform3D.MakeScale(sx: 1, sy: 1, sz: 1);
                },
                completion: () =>
                {
                    _pickerCallback?.Invoke(_pickerView);
                });
        }

        /// <summary>
        ///   On removal of dialog, reset the flag that dialog is not shown
        /// </summary>
        /// <param name="uiView"></param>
        public override void WillRemoveSubview(UIView uiView)
        {
            base.WillRemoveSubview(uiView);
            _isDialogShown = false; // rest the flag
        }

        public void Select(int row, int component, bool animated)
        {
            _pickerView?.Select(row, component, animated);
        }

        /* Helper function: count and return the screen's size */
        private CGSize CountScreenSize()
        {
            var screenWidth = UIScreen.MainScreen.Bounds.Size.Width;
            var screenHeight = UIScreen.MainScreen.Bounds.Size.Height;

            return new CGSize(screenWidth, screenHeight);
        }

        /* Creates the container view here: create the dialog, then add the custom content and buttons */
        private UIView CreateContainerView()
        {
            var screenSize = CountScreenSize();

            var dialogSize = new CGSize(
                width: 300,
                230
                + _datePickerDialogDefaultButtonHeight
                + _datePickerDialogDefaultButtonSpacerHeight);

            // For the black background
            Frame = new CGRect(
                x: 0,
                y: 0,
                screenSize.Width,
                screenSize.Height);

            // This is the dialog's container; we attach the custom content and the buttons to this one
            var dialogContainer = new UIView(
                new CGRect(
                    (screenSize.Width - dialogSize.Width) / 2,
                    (screenSize.Height - dialogSize.Height) / 2,
                    dialogSize.Width,
                    dialogSize.Height));

            // First, we style the dialog to match the iOS8 UIAlertView >>>
            var gradient = new CAGradientLayer
            {
                Frame = dialogContainer.Bounds,
                Colors = new[]
                {
                    UIColor.FromRGB(red: 218, green: 218, blue: 218).CGColor,
                    UIColor.FromRGB(red: 233, green: 233, blue: 233).CGColor,
                    UIColor.FromRGB(red: 218, green: 218, blue: 218).CGColor
                }
            };

            var cornerRadius = _datePickerDialogCornerRadius;
            gradient.CornerRadius = cornerRadius;
            dialogContainer.Layer.InsertSublayer(gradient, index: 0);
            dialogContainer.Layer.CornerRadius = cornerRadius;
            dialogContainer.Layer.BorderColor = UIColor.FromRGB(red: 198, green: 198, blue: 198).CGColor;
            dialogContainer.Layer.BorderWidth = 1;
            dialogContainer.Layer.ShadowRadius = cornerRadius + 5;
            dialogContainer.Layer.ShadowOpacity = 0.1f;
            dialogContainer.Layer.ShadowOffset = new CGSize(0 - (cornerRadius + 5) / 2, 0 - (cornerRadius + 5) / 2);
            dialogContainer.Layer.ShadowColor = UIColor.Black.CGColor;
            dialogContainer.Layer.ShadowPath = UIBezierPath.FromRoundedRect(dialogContainer.Bounds, dialogContainer.Layer.CornerRadius).CGPath;

            // There is a line above the button
            var lineView = new UIView(
                new CGRect(
                    x: 0,
                    dialogContainer.Bounds.Size.Height - _datePickerDialogDefaultButtonHeight - _datePickerDialogDefaultButtonSpacerHeight,
                    dialogContainer.Bounds.Size.Width,
                    _datePickerDialogDefaultButtonSpacerHeight))
            {
                BackgroundColor = UIColor.FromRGB(red: 198, green: 198, blue: 198)
            };

            dialogContainer.AddSubview(lineView);

            // ˆˆˆ
            //Title
            _titleLabel = new UILabel(
                new CGRect(
                    x: 10,
                    y: 10,
                    width: 280,
                    height: 30))
            {
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.BoldSystemFontOfSize(size: 17),
                Text = _title
            };

            dialogContainer.AddSubview(_titleLabel);

            _pickerView = new UIPickerView(
                new CGRect(
                    x: 0,
                    y: 30,
                    width: 0,
                    height: 0))
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin
            };

            _pickerView.Frame = new CGRect(_pickerView.Frame.Location, new CGSize(width: 300, _pickerView.Frame.Size.Height));
            _pickerView.Model = _pickerModel;

            dialogContainer.AddSubview(_pickerView);
            AddButtonsToView(dialogContainer);

            return dialogContainer;
        }

        private void ButtonTapped(object sender, EventArgs e)
        {
            var button = sender as UIButton;

            if (button?.Tag == _datePickerDialogDoneButtonTag)
            {
                _callback?.Invoke(_pickerModel);
            }
            else if (button?.Tag == _datePickerDialogCancelButtonTag)
            {
                _cancelCallback?.Invoke();
            }

            Close();
        }

        /* Add buttons to container */
        private void AddButtonsToView(UIView container)
        {
            var buttonWidth = container.Bounds.Size.Width / 2;

            _cancelButton = new UIButton(UIButtonType.Custom)
            {
                Frame = new CGRect(
                    x: 0,
                    container.Bounds.Size.Height - _datePickerDialogDefaultButtonHeight,
                    buttonWidth,
                    _datePickerDialogDefaultButtonHeight),
                Tag = _datePickerDialogCancelButtonTag
            };

            _cancelButton.SetTitle(_cancelButtonTitle, UIControlState.Normal);
            _cancelButton.SetTitleColor(UIColor.FromRGB(red: 0f, green: 0.5f, blue: 1f), UIControlState.Normal);

            _cancelButton.SetTitleColor(
                UIColor.FromRGBA(
                    red: 0.2f,
                    green: 0.2f,
                    blue: 0.2f,
                    alpha: 0.5f),
                UIControlState.Highlighted);

            _cancelButton.TitleLabel.Font = UIFont.BoldSystemFontOfSize(size: 14);
            _cancelButton.Layer.CornerRadius = _datePickerDialogCornerRadius;
            _cancelButton.TouchUpInside += ButtonTapped;
            container.AddSubview(_cancelButton);

            _doneButton = new UIButton(UIButtonType.Custom)
            {
                Frame = new CGRect(
                    buttonWidth,
                    container.Bounds.Size.Height - _datePickerDialogDefaultButtonHeight,
                    buttonWidth,
                    _datePickerDialogDefaultButtonHeight),
                Tag = _datePickerDialogDoneButtonTag
            };

            _doneButton.SetTitle(_doneButtonTitle, UIControlState.Normal);
            _doneButton.SetTitleColor(UIColor.FromRGB(red: 0f, green: 0.5f, blue: 1f), UIControlState.Normal);

            _doneButton.SetTitleColor(
                UIColor.FromRGBA(
                    red: 0.2f,
                    green: 0.2f,
                    blue: 0.2f,
                    alpha: 0.5f),
                UIControlState.Highlighted);

            _doneButton.TitleLabel.Font = UIFont.BoldSystemFontOfSize(size: 14);
            _doneButton.Layer.CornerRadius = _datePickerDialogCornerRadius;
            _doneButton.TouchUpInside += ButtonTapped;
            container.AddSubview(_doneButton);
        }

        /* Dialog close animation then cleaning and removing the view from the parent */
        private void Close()
        {
            var currentTransform = _dialogView.Layer.Transform;

            if (ValueForKeyPath(new NSString(str: "layer.transform.rotation.z")) is NSNumber startRotation)
            {
                var startRotationAngle = startRotation.DoubleValue;

                var rotation = CATransform3D.MakeRotation(
                    (nfloat)(-startRotationAngle + Math.PI * 270 / 180d),
                    x: 0f,
                    y: 0f,
                    z: 0f);

                _dialogView.Layer.Transform = rotation.Concat(CATransform3D.MakeScale(sx: 1, sy: 1, sz: 1));
            }

            _dialogView.Layer.Opacity = 1;

            Animate(
                duration: 0.2,
                delay: 0d,
                UIViewAnimationOptions.TransitionNone,
                animation: () =>
                {
                    BackgroundColor = UIColor.FromRGBA(
                        red: 0,
                        green: 0,
                        blue: 0,
                        alpha: 0);

                    _dialogView.Layer.Transform = currentTransform.Concat(CATransform3D.MakeScale(sx: 0.6f, sy: 0.6f, sz: 1f));
                    _dialogView.Layer.Opacity = 0;
                },
                completion: () =>
                {
                    var subViews = Subviews.ToArray();

                    foreach (var v in subViews)
                    {
                        v.RemoveFromSuperview();
                    }
                });

            RemoveFromSuperview();
        }
    }
}