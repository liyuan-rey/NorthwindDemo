
namespace Northwind.WpfClient.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;
    using Northwind.Common;

    public abstract class NotifyPropertyChangedDispatcherObject : INotifyPropertyChanged, IWeakEventListener
    {
        private readonly Dictionary<string, HashSet<string>> _collectionPropertyDependencies;
        private readonly Lazy<Dictionary<INotifyCollectionChanged, string>> _collectionSources;
        private readonly Dispatcher _dispatcher;
        private readonly Dictionary<string, Dictionary<string, HashSet<string>>> _externalPropertyDependencies;
        private readonly Lazy<Dictionary<INotifyPropertyChanged, string>> _externalPropertySources;
        private readonly Dictionary<string, List<string>> _internalPropertyDependencies;

        public event PropertyChangedEventHandler PropertyChanged;

        public NotifyPropertyChangedDispatcherObject()
            : this(Dispatcher.CurrentDispatcher)
        {
        }

        public NotifyPropertyChangedDispatcherObject(Dispatcher dispatcher)
        {
            ContractUtil.RequiresNotNull(dispatcher, "dispatcher");

            this._dispatcher = dispatcher;
            this._externalPropertySources = new Lazy<Dictionary<INotifyPropertyChanged, string>>();
            this._collectionSources = new Lazy<Dictionary<INotifyCollectionChanged, string>>();
            this._internalPropertyDependencies = this.BuildInternalPropertyDependencies();
            this._externalPropertyDependencies = this.BuildExternalPropertyDependencies();
            this._collectionPropertyDependencies = this.BuildCollectionPropertyDependencies();
        }

        protected void AddDependencySource(string name, INotifyCollectionChanged source)
        {
            ContractUtil.RequiresStringNotNullOrWhiteSpace(name, "name");
            ContractUtil.RequiresNotNull(source, "source");
            this._collectionSources.Value.Add(source, name);
            CollectionChangedEventManager.AddListener(source, this);
        }

        protected void AddDependencySource(string name, INotifyPropertyChanged source)
        {
            ContractUtil.RequiresStringNotNullOrWhiteSpace(name, "name");
            ContractUtil.RequiresNotNull(source, "source");

            this._externalPropertySources.Value.Add(source, name);
            PropertyChangedEventManager.AddListener(source, this, string.Empty);
        }

        private Dictionary<string, HashSet<string>> BuildCollectionPropertyDependencies()
        {
            Dictionary<string, HashSet<string>> dictionary = null;
            foreach (PropertyInfo info in base.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (ValueDependsOnCollectionAttribute attribute in info.GetCustomAttributes<ValueDependsOnCollectionAttribute>(false))
                {
                    HashSet<string> set;
                    if (dictionary == null)
                    {
                        dictionary = new Dictionary<string, HashSet<string>>();
                    }
                    if (!dictionary.TryGetValue(attribute.SourceName, out set))
                    {
                        set = new HashSet<string>();
                        dictionary.Add(attribute.SourceName, set);
                    }
                    set.Add(info.Name);
                }
            }
            return dictionary;
        }

        private Dictionary<string, Dictionary<string, HashSet<string>>> BuildExternalPropertyDependencies()
        {
            Dictionary<string, Dictionary<string, HashSet<string>>> dictionary = null;
            foreach (PropertyInfo info in base.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (ValueDependsOnExternalPropertyAttribute attribute in info.GetCustomAttributes<ValueDependsOnExternalPropertyAttribute>(false))
                {
                    Dictionary<string, HashSet<string>> dictionary2;
                    HashSet<string> set;
                    if (dictionary == null)
                    {
                        dictionary = new Dictionary<string, Dictionary<string, HashSet<string>>>();
                    }
                    if (!dictionary.TryGetValue(attribute.SourceName, out dictionary2))
                    {
                        dictionary2 = new Dictionary<string, HashSet<string>>();
                        dictionary.Add(attribute.SourceName, dictionary2);
                    }
                    if (!dictionary2.TryGetValue(attribute.PropertyName, out set))
                    {
                        set = new HashSet<string>();
                        dictionary2.Add(attribute.PropertyName, set);
                    }
                    set.Add(info.Name);
                }
            }
            return dictionary;
        }

        private Dictionary<string, List<string>> BuildInternalPropertyDependencies()
        {
            Dictionary<string, List<string>> dictionary = null;
            foreach (PropertyInfo info in base.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (DependsOnPropertyAttribute attribute in info.GetCustomAttributes(typeof(DependsOnPropertyAttribute), true).Cast<DependsOnPropertyAttribute>().ToArray<DependsOnPropertyAttribute>())
                {
                    if (dictionary == null)
                    {
                        dictionary = new Dictionary<string, List<string>>();
                    }
                    foreach (string str in attribute.PropertyNames)
                    {
                        List<string> list;
                        string str2 = str;
                        if (string.IsNullOrEmpty(str2))
                        {
                            str2 = string.Empty;
                        }
                        if (!dictionary.TryGetValue(str2, out list))
                        {
                            list = new List<string>();
                            dictionary.Add(str2, list);
                        }
                        list.Add(info.Name);
                    }
                }
            }
            return dictionary;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CheckAccess()
        {
            return this.Dispatcher.CheckAccess();
        }

        public void CheckAccessBeginExecute(Action action)
        {
            ContractUtil.RequiresNotNull(action, "action");

            if (this.CheckAccess())
            {
                action();
            }
            else
            {
                this.Dispatcher.BeginInvoke(action, DispatcherPriority.Normal, new object[0]);
            }
        }

        public void CheckAccessExecute(Action action)
        {
            ContractUtil.RequiresNotNull(action, "action");

            if (this.CheckAccess())
            {
                action();
            }
            else
            {
                this.Dispatcher.Invoke(action, DispatcherPriority.Normal);
            }
        }

        public TResult CheckAccessExecute<TResult>(Func<TResult> func)
        {
            ContractUtil.RequiresNotNull(func, "action");

            if (this.CheckAccess())
            {
                return func();
            }

            return this.Dispatcher.Invoke<TResult>(func, DispatcherPriority.Normal);
        }

        protected string GetDependencySourceName(INotifyCollectionChanged source)
        {
            string str = null;
            if (this._collectionSources.IsValueCreated)
            {
                this._collectionSources.Value.TryGetValue(source, out str);
            }
            return str;
        }

        protected string GetDependencySourceName(INotifyPropertyChanged source)
        {
            string str = null;
            if (this._externalPropertySources.IsValueCreated)
            {
                this._externalPropertySources.Value.TryGetValue(source, out str);
            }
            return str;
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            this.VerifyAccess();
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            if ((this._internalPropertyDependencies != null) && !string.IsNullOrEmpty(propertyName))
            {
                this.RaisePropertyChanged<string, List<string>>(this._internalPropertyDependencies, propertyName, new string[0]);
                this.RaisePropertyChanged<string, List<string>>(this._internalPropertyDependencies, string.Empty, new string[] { propertyName });
            }
        }

        private void RaisePropertyChanged<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, params string[] propertiesToExclude) where TValue : IEnumerable<string>
        {
            TValue local;
            if (dictionary.TryGetValue(key, out local))
            {
                foreach (string str in local.Except<string>(propertiesToExclude))
                {
                    this.RaisePropertyChanged(str);
                }
            }
        }

        protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(PropertyChangedEventManager))
            {
                Dictionary<string, HashSet<string>> dictionary;
                INotifyPropertyChanged changed = (INotifyPropertyChanged)sender;
                PropertyChangedEventArgs args = (PropertyChangedEventArgs)e;
                string key = this.GetDependencySourceName(changed);
                if (((key != null) && (this._externalPropertyDependencies != null)) && this._externalPropertyDependencies.TryGetValue(key, out dictionary))
                {
                    if (!string.IsNullOrEmpty(args.PropertyName))
                    {
                        this.RaisePropertyChanged<string, HashSet<string>>(dictionary, args.PropertyName, new string[0]);
                        this.RaisePropertyChanged<string, HashSet<string>>(dictionary, string.Empty, new string[0]);
                    }
                    else
                    {
                        foreach (HashSet<string> set in dictionary.Values)
                        {
                            foreach (string str2 in set)
                            {
                                this.RaisePropertyChanged(str2);
                            }
                        }
                    }
                }
                return true;
            }
            if (!(managerType == typeof(CollectionChangedEventManager)))
            {
                return false;
            }
            INotifyCollectionChanged source = (INotifyCollectionChanged)sender;
            string dependencySourceName = this.GetDependencySourceName(source);
            if (dependencySourceName != null)
            {
                this.RaisePropertyChanged<string, HashSet<string>>(this._collectionPropertyDependencies, dependencySourceName, new string[0]);
            }
            return true;
        }

        protected void RemoveDependencySource(string name, INotifyCollectionChanged source)
        {
            ContractUtil.RequiresStringNotNullOrWhiteSpace(name, "name");
            ContractUtil.RequiresNotNull(source, "source");
            CollectionChangedEventManager.RemoveListener(source, this);
            this._collectionSources.Value.Remove(source);
        }

        protected void RemoveDependencySource(string name, INotifyPropertyChanged source)
        {
            ContractUtil.RequiresStringNotNullOrWhiteSpace(name, "name");
            ContractUtil.RequiresNotNull(source, "source");
            PropertyChangedEventManager.RemoveListener(source, this, string.Empty);
            this._externalPropertySources.Value.Remove(source);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged(ref bool propertyDataField, bool value, [CallerMemberName] string propertyName = null)
        {
            if (propertyDataField != value)
            {
                propertyDataField = value;
                this.RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged(ref int propertyDataField, int value, [CallerMemberName] string propertyName = null)
        {
            if (propertyDataField != value)
            {
                propertyDataField = value;
                this.RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged(ref string propertyDataField, string value, [CallerMemberName] string propertyName = null)
        {
            if (!string.Equals(propertyDataField, value, StringComparison.Ordinal))
            {
                propertyDataField = value;
                this.RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged<T>(ref T propertyDataField, T value, [CallerMemberName] string propertyName = null)
        {
            bool flag;
            IEquatable<T> equatable = value as IEquatable<T>;
            if (equatable != null)
            {
                flag = equatable.Equals(propertyDataField);
            }
            else if (typeof(T).IsSubclassOf(typeof(Enum)))
            {
                flag = object.Equals(value, (T)propertyDataField);
            }
            else
            {
                flag = object.ReferenceEquals(value, (T)propertyDataField);
            }
            if (!flag)
            {
                propertyDataField = value;
                this.RaisePropertyChanged(propertyName);
            }
            return !flag;
        }

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (!this.ReceiveWeakEvent(managerType, sender, e))
            {
                return false;
            }
            return true;
        }

        [Conditional("DEBUG")]
        private void ValidateExternalPropertyDependencies(string sourceName, INotifyPropertyChanged source)
        {
            if (this._externalPropertyDependencies != null)
            {
                HashSet<string> second = new HashSet<string>(from property in source.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance) select property.Name) {
                    string.Empty
                };
                using (IEnumerator<string> enumerator = this._externalPropertyDependencies[sourceName].Keys.Except<string>(second).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        string current = enumerator.Current;
                    }
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void VerifyAccess()
        {
            this.Dispatcher.VerifyAccess();
        }

        public System.Windows.Threading.Dispatcher Dispatcher
        {
            get
            {
                return this._dispatcher;
            }
        }
    }
}
