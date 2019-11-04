using Diabetto.Core.ViewModels.Products;
using Diabetto.iOS.Extensions;
using Diabetto.iOS.Sources.Products;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Diabetto.iOS.Views.Products
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Products")]
    public sealed class ProductsView : MvxTableViewController<ProductsViewModel>
    {
        private ProductTableViewSource _source;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Products";

            this.HideKeyboardWhenTappedAround();

            _source = new ProductTableViewSource(TableView);
            TableView.Source = _source;

            var searchBar = new UISearchBar();
            searchBar.SizeToFit();
            searchBar.SearchBarStyle = UISearchBarStyle.Prominent;
            searchBar.Placeholder = "Enter a product name";

            TableView.TableHeaderView = searchBar;

            DefinesPresentationContext = true;
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add);

            var bindingSet = this.CreateBindingSet<ProductsView, ProductsViewModel>();

            bindingSet
                .Bind(NavigationItem.RightBarButtonItem)
                .To(v => v.AddCommand);

            bindingSet
                .Bind(searchBar)
                .For(v => v.Text)
                .To(v => v.SearchName);

            bindingSet
                .Bind(_source)
                .For(v => v.ItemsSource)
                .To(vm => vm.Products);

            bindingSet
                .Bind(_source)
                .For(v => v.SelectionChangedCommand)
                .To(vm => vm.SelectedCommand);

            bindingSet
                .Bind(_source)
                .For(v => v.DeleteItemCommand)
                .To(vm => vm.DeleteCommand);

            bindingSet.Apply();
        }
    }
}