using System.Net;
using Diabetto.Core.Services;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModels.Calendars;
using Diabetto.Core.ViewModels.ProductMeasures;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace Diabetto.Core
{
    public sealed class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IBreadUnitsCalculator, BreadUnitsCalculator>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ITimeProvider, TimeProvider>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDatabaseConnectionStringHolder, DatabaseConnectionStringHolder>();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Client")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMonthViewModelFactory, MonthViewModelFactory>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProductMeasureViewModelFactory, ProductMeasureViewModelFactory>();

            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMeasureService, MeasureService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IProductCategoryService, ProductCategoryService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IProductMeasureService, ProductMeasureService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IProductMeasureUnitService, ProductMeasureUnitService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IProductService, ProductService>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<ITagService, TagService>();

            //Mvx.IoCProvider.RegisterSingleton(() => UserDialogs.Instance);

            // register the appstart object
            RegisterCustomAppStart<AppStart>();

            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        }
    }
}