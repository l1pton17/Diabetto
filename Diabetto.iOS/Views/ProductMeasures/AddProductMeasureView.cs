using Diabetto.Core.ViewModels.ProductMeasures;
using Diabetto.iOS.Sources.Products;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Diabetto.iOS.Views.ProductMeasures
{
    [MvxChildPresentation(Animated = true)]
    public partial class AddProductMeasureView : MvxTableViewController<AddProductMeasureViewModel>
    {
        private ProductSearchResultsTableViewSource _searchResultsSource;

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

            bindingSet.Apply();
        }
    }
}