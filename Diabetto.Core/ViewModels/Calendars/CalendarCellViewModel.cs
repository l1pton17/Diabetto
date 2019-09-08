using System;
using System.Threading.Tasks;
using Diabetto.Core.ViewModels.Core;

namespace Diabetto.Core.ViewModels.Calendars
{
    public enum CalendarCellType
    {
        Normal,
        PreviousMonth
    }

    public sealed class CalendarCellViewModel : MvxReactiveViewModel
    {
        private bool _isHidden;
        public bool IsHidden
        {
            get => _isHidden;
            set => SetProperty(ref _isHidden, value);
        }

        public DateTime Date { get; }

        public int Day { get; }

        public CalendarCellType CellType { get; }

        public CalendarCellViewModel(CalendarCellType cellType, DateTime date)
        {
            CellType = cellType;
            Date = date;
            Day = date.Day;
        }

        public override Task Initialize()
        {
            return base.Initialize();
        }
    }
}