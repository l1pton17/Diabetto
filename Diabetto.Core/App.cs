using System.Net;
using Diabetto.Core.Services;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModels.Calendars;
using Diabetto.Core.ViewModels.Measures;
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

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Client")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ISettingViewModelsStorage, SettingViewModelsStorage>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMonthViewModelFactory, MonthViewModelFactory>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProductMeasureViewModelFactory, ProductMeasureViewModelFactory>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMeasureCellViewModelFactory, MeasureCellViewModelFactory>();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMeasureService, MeasureService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProductCategoryService, ProductCategoryService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProductService, ProductService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ITagService, TagService>();

            RegisterCustomAppStart<AppStart>();

            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        }
    }
}