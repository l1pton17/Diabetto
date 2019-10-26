using System;

namespace Diabetto.Core.Helpers
{
    public sealed class ActionDisposable : IDisposable
    {
        private readonly Action _action;

        public ActionDisposable(Action action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _action();
        }
    }
}