using Diabetto.Core.ViewModels.Products;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;

namespace Diabetto.iOS.Views.Products
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Products")]
    public sealed class ProductsView : MvxViewController<ProductsViewModel>
    {
        
    }
}