using System;

namespace Common
{
    public class InputLoaderDelegated<TInputValue> : InputLoader<TInputValue>
    {
        private readonly Func<TInputValue> loadInputDelegate;

        public InputLoaderDelegated(Func<TInputValue> loadInputDelegate)
        {
            this.loadInputDelegate = loadInputDelegate;
        }

        public override TInputValue LoadInput() => loadInputDelegate();
    }
}
