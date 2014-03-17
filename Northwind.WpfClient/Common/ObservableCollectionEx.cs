// ObservableCollectionEx.cs

namespace Northwind.WpfClient.Common
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    internal class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        public void CopyFrom(IEnumerable<T> products)
        {
            Items.Clear();
            foreach (T p in products)
                Items.Add(p);

            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}