// ViewModelBase.cs

namespace Northwind.WpfClient.Common
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Threading;

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            Dispatcher.CurrentDispatcher.VerifyAccess();

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged(ref bool propertyDataField, bool value,
            [CallerMemberName] string propertyName = null)
        {
            if (propertyDataField != value)
            {
                propertyDataField = value;
                RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged(ref int propertyDataField, int value,
            [CallerMemberName] string propertyName = null)
        {
            if (propertyDataField != value)
            {
                propertyDataField = value;
                RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged(ref string propertyDataField, string value,
            [CallerMemberName] string propertyName = null)
        {
            if (!string.Equals(propertyDataField, value, StringComparison.Ordinal))
            {
                propertyDataField = value;
                RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged<T>(ref T propertyDataField, T value,
            [CallerMemberName] string propertyName = null)
        {
            bool flag;

            var equatable = value as IEquatable<T>;
            if (equatable != null)
                flag = equatable.Equals(propertyDataField);
            else if (typeof (T).IsSubclassOf(typeof (Enum)))
                flag = Equals(value, propertyDataField);
            else
                flag = ReferenceEquals(value, propertyDataField);

            if (!flag)
            {
                propertyDataField = value;
                RaisePropertyChanged(propertyName);
            }

            return !flag;
        }

        #endregion
    }
}