// IActiveAware.cs

namespace Northwind.WpfClient.Common
{
    using System;

    public interface IActiveAware
    {
        bool IsActive { get; }
        event EventHandler IsActiveChanged;
    }
}