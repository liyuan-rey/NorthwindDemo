// AsyncRelayCommand.cs

namespace Northwind.WpfClient.Common
{
    using System;
    using System.Threading.Tasks;

    public class AsyncRelayCommand : RelayCommand
    {
        public AsyncRelayCommand(string text, Func<Task> execute, bool ignoreCancellation = true)
            : this(text, param => execute(), null, ignoreCancellation)
        {
        }

        public AsyncRelayCommand(string text, Func<object, Task> execute,
            Predicate<object> canExecute = null, bool ignoreCancellation = true)
            : base(text, WrapExecute(execute, ignoreCancellation), canExecute)
        {
        }

        private static Action<object> WrapExecute(Func<object, Task> execute, bool ignoreCancellation)
        {
            Action<object> action = null;
            if (!ignoreCancellation)
                action = param => execute(param);

            action = param => FireAndForget(execute, param);

            return action;
        }

        private static async void FireAndForget<T>(Func<T, Task> execute, T param)
        {
            await execute(param);
        }
    }
}