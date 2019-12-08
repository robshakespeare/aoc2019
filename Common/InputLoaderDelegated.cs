using System;

namespace Common
{
    public class InputLoaderDelegated<TInputValue> : IInputLoader<TInputValue>
    {
        private readonly Func<TInputValue> loadInputDelegate;

        public InputLoaderDelegated(Func<TInputValue> loadInputDelegate)
        {
            this.loadInputDelegate = loadInputDelegate;
        }

        public TInputValue LoadInput() => loadInputDelegate();
    }
}
