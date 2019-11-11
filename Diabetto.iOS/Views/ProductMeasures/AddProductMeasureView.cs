using Cirrious.FluentLayouts.Touch;
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
        private UIButton _closeButton;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ModalInPresentation = true;
            _searchResultsSource = new ProductSearchResultsTableViewSource(TableView);

            _closeButton = new UIButton
            {
                VerticalAlignment = UIControlContentVerticalAlignment.Center,
                HorizontalAlignment = UIControlContentHorizontalAlignment.Center,
                Font = UIFont.SystemFontOfSize(17)
            };

            _closeButton.SetTitleColor(TableView.TintColor, UIControlState.Normal);
            _closeButton.SetTitle("Close", UIControlState.Normal);

            TableView.Source = _searchResultsSource;

            TableView.TableFooterView = new UIView
            {
                BackgroundColor = TableView.BackgroundColor,
                Frame = new CoreGraphics.CGRect(
                    0,
                    0,
                    View.Frame.Width,
                    30)
            };

            TableView.TableFooterView.AddSubviews(_closeButton);
            TableView.TableFooterView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            TableView.TableFooterView.AddConstraints(
                _closeButton.WithSameHeight(TableView.TableFooterView),
                _closeButton.WithSameWidth(TableView.TableFooterView)
            );

            var searchBar = new UISearchBar();
            searchBar.SizeToFit();
            searchBar.SearchBarStyle = UISearchBarStyle.Prominent;
            searchBar.Placeholder = "Enter a product name";

            TableView.TableHeaderView = searchBar;

            DefinesPresentationContext = true;

            var bindingSet = this.CreateBindingSet<AddProductMeasureView, AddProductMeasureViewModel>();

            bindingSet
                .Bind(_closeButton)
                .To(v => v.CloseCommand);

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