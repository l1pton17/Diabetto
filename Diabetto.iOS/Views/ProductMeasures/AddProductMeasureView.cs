
using System;
using Cirrious.FluentLayouts.Touch;
using Diabetto.Core.MvxInteraction.ProductMeasures;
using Diabetto.Core.ViewModels.ProductMeasures;
using Diabetto.iOS.Dialogs;
using Diabetto.iOS.Sources.Products;
using Diabetto.iOS.ViewModels.Dialogs;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using UIKit;

namespace Diabetto.iOS.Views.ProductMeasures
{
    [MvxChildPresentation(Animated = true)]
    public partial class AddProductMeasureView : MvxTableViewController<AddProductMeasureViewModel>
    {
        private ProductSearchResultsTableViewSource _searchResultsSource;

        private IMvxInteraction<AddNewProductMeasureInteraction> _addNewProductInteraction;
        public IMvxInteraction<AddNewProductMeasureInteraction> AddNewProductInteraction
        {
            get
            {
                return _addNewProductInteraction;
            }
            set
            {
                if (_addNewProductInteraction != null)
                {
                    _addNewProductInteraction.Requested += OnAddNewProductInteractionRequested;
                }

                _addNewProductInteraction = value;
                _addNewProductInteraction.Requested += OnAddNewProductInteractionRequested;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _searchResultsSource = new ProductSearchResultsTableViewSource(TableView);

            TableView.Source = _searchResultsSource;

            var searchController = new UISearchController(searchResultsController: null);

            searchController.SearchBar.SizeToFit();
            searchController.SearchBar.SearchBarStyle = UISearchBarStyle.Prominent;
            searchController.SearchBar.Placeholder = "Enter a product name";
            searchController.HidesNavigationBarDuringPresentation = false;
            searchController.DimsBackgroundDuringPresentation = false;

            TableView.TableHeaderView = searchController.SearchBar;

            DefinesPresentationContext = true;

            var bindingSet = this.CreateBindingSet<AddProductMeasureView, AddProductMeasureViewModel>();

            bindingSet
                .Bind(searchController.SearchBar)
                .For(v => v.Text)
                .To(v => v.SearchQuery);

            bindingSet
                .Bind(_searchResultsSource)
                .For(v => v.ItemsSource)
                .To(v => v.SearchResults);

            bindingSet
                .Bind(_searchResultsSource)
                .For(v => v.SelectionChangedCommand)
                .To(v => v.ProductSelectedCommand);

            bindingSet
                .Bind(this)
                .For(v => v.AddNewProductInteraction)
                .To(v => v.AddNewInteraction);

            bindingSet.Apply();
        }

        private void OnAddNewProductInteractionRequested(object sender, MvxValueEventArgs<AddNewProductMeasureInteraction> eventArgs)
        {
            var dialog = new PickerDialog<AddProductMeasureUIPickerViewModel>();
            var pickerViewModel = new AddProductMeasureUIPickerViewModel();

            dialog
                .Show(
                "Добавить",
                callback: v =>
                {
                    eventArgs.Value.Callback(
                        new AddNewProductMeasureInteractionResult
                        {
                            ProductName = eventArgs.Value.ProductName,
                            Amount = v.Amount,
                            Carbohydrates = v.Carbohydrates
                        });
                },
                pickerViewModel,
                cancelCallback: () => eventArgs.Value.Callback(null));
        }
    }
}