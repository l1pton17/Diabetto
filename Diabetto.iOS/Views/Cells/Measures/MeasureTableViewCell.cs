using System;
using Diabetto.Core.Models;
using Diabetto.iOS.Combiners;
using Diabetto.iOS.Converters;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Diabetto.iOS.Views.Cells.Measures
{
    public partial class MeasureTableViewCell : BaseTableViewCell
    {
        private static readonly LevelMvxValueConverter _levelLabelConverter = new LevelMvxValueConverter
        {
            EmptyValue = "--"
        };

        public static readonly NSString Key = new NSString(nameof(MeasureTableViewCell));
        public static readonly UINib Nib;

        public string Time
        {
            get => TimeLabel.Text;
            set => TimeLabel.Text = value;
        }

        static MeasureTableViewCell()
        {
            Nib = UINib.FromName(Key, NSBundle.MainBundle);
        }

        protected MeasureTableViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(
                () =>
                {
                    var set = this.CreateBindingSet<MeasureTableViewCell, Measure>();

                    set.Bind(TimeLabel)
                        .For(v => v.Text)
                        .To(v => v.Date)
                        .WithConversion(TimeMvxValueConverter.Instance);

                    set.Bind(LevelLabel)
                        .To(v => v.Level)
                        .For(v => v.Text)
                        .WithConversion(_levelLabelConverter);

                    set.Bind(BreadUnitsLabel)
                        .For(v => v.Text)
                        .To(v => v.BreadUnits)
                        .WithConversion(BreadUnitsMvxValueConverter.Instance);

                    set.Bind(InsulinLabel)
                        .For(v => v.Text)
                        .ByCombining(
                            InsulinMvxValueCombiner.Instance,
                            v => v.ShortInsulin,
                            v => v.LongInsulin);

                    set.Apply();
                });
        }
    }
}
