using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Diabetto.Core;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UIKit;

namespace Diabetto.iOS.Intents.Shared
{
    public sealed class EmptyMvxViewsContainer : IMvxViewsContainer
    {
        /// <inheritdoc />
        public Type GetViewType(Type viewModelType)
        {
            return viewModelType;
        }

        /// <inheritdoc />
        public void AddAll(IDictionary<Type, Type> viewModelViewLookup)
        {
        }

        /// <inheritdoc />
        public void Add(Type viewModelType, Type viewType)
        {
        }

        /// <inheritdoc />
        public void Add<TViewModel, TView>()
            where TViewModel : IMvxViewModel
            where TView : IMvxView
        {
        }

        /// <inheritdoc />
        public void AddSecondary(IMvxViewFinder finder)
        {
        }

        /// <inheritdoc />
        public void SetLastResort(IMvxViewFinder finder)
        {
        }
    }

    public sealed class EmptyMvxViewDispatcher : MvxIosUIThreadDispatcher, IMvxViewDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        /// <inheritdoc />
        public override bool IsOnMainThread => _uiSynchronizationContext == SynchronizationContext.Current;

        public EmptyMvxViewDispatcher()
        {
            _uiSynchronizationContext = SynchronizationContext.Current;

            if (_uiSynchronizationContext == null)
            {
                throw new MvxException("SynchronizationContext must not be null - check to make sure Dispatcher is created on UI thread");
            }
        }

        /// <inheritdoc />
        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                ExceptionMaskedAction(action, maskExceptions);
            }
            else
            {
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                });
            }

            return true;
        }

        /// <inheritdoc />
        public Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            return Task.FromResult(true);
        }
    }

    
    public sealed class Setup : MvxSetup<App>
    {
        /// <inheritdoc />
        protected override IMvxViewsContainer CreateViewsContainer()
        {
            return new EmptyMvxViewsContainer();
        }

        /// <inheritdoc />
        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new EmptyMvxViewDispatcher();
        }

        /// <inheritdoc />
        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxViewToViewModelNameMapping();
        }
    }

    public sealed class SetupSingleton : MvxSetupSingleton
    {
        private readonly IMvxSetup _setup = new Setup();

        protected override IMvxSetup Setup => _setup;
    }
}
 