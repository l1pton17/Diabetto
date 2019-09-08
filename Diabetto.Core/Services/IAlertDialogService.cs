using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using ReactiveUI;

namespace Diabetto.Core.Services
{
    public interface IAlertDialogService
    {
        Task<bool> PromptYesNo(string title, string message);

        Task Alert(string title, string message);

        Task<string> PromptTextBox(string title, string message, string defaultValue, string okTitle);

        void Show(string text);

        void Hide();
    }

    public static class AlertDialogServiceExtensions
    {
        public static IDisposable Activate(this IAlertDialogService @this, string text)
        {
            @this.Show(text);
            return Disposable.Create(@this.Hide);
        }

        public static IDisposable Activate(this IAlertDialogService @this, IObservable<bool> observable, string text)
        {
            return observable.Subscribe(x =>
            {
                if (x)
                {
                    @this.Show(text);
                }
                else
                {
                    @this.Hide();
                }
            });
        }

        public static IDisposable Activate<TParam, TResult>(this IAlertDialogService @this, ReactiveCommand<TParam, TResult> command, string text)
        {
            return command.IsExecuting.Subscribe(x =>
            {
                if (x)
                {
                    @this.Show(text);
                }
                else
                {
                    @this.Hide();
                }
            });
        }

        public static IDisposable AlertExecuting<TParam, TResult>(this ReactiveCommand<TParam, TResult> @this, IAlertDialogService dialogFactory, string text)
        {
            return @this.IsExecuting.Subscribe(x =>
            {
                if (x)
                {
                    dialogFactory.Show(text);
                }
                else
                {
                    dialogFactory.Hide();
                }
            });
        }
    }
}