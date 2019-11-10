using Diabetto.Core.ViewModels.ProductMeasures;
using Diabetto.iOS.Sources.Products;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Diabetto.iOS.Views.ProductMeasures
{
    [MvxModalPresentation(
        Animated = true,
        ModalPresentationStyle = UIModalPresentationStyle.FormSheet)]
    public partial class AddProductMeasureView : MvxTableViewController<AddProductMeasureViewModel>
    {
        private ProductSearchResultsTableViewSource _searchResultsSource;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ModalInPresentation = true;
            _searchResultsSource = new ProductSearchResultsTableViewSource(TableView);

            TableView.Source = _searchResultsSource;

            var searchBar = new UISearchBar();
            searchBar.SizeToFit();
            searchBar.SearchBarStyle = UISearchBarStyle.Prominent;
            searchBar.Placeholder = "Enter a product name";

            TableView.TableHeaderView = searchBar;

            DefinesPresentationContext = true;

            var bindingSet = this.CreateBindingSet<AddProductMeasureView, AddProductMeasureViewModel>();

            bindingSet
                .Bind(searchBar)
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