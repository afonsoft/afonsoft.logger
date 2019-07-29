using Microsoft.Extensions.Options;
using System;

namespace Afonsoft.Logger.Internal
{
    internal class OptionsWrapperMonitor<T> : IOptionsMonitor<T>
    {
        public OptionsWrapperMonitor(T currentValue)
        {
            CurrentValue = currentValue;
        }

        public IDisposable OnChange(Action<T, string> listener)
        {
            return null;
        }

        public T Get(string name) => CurrentValue;

        public T CurrentValue { get; }
    }
}
