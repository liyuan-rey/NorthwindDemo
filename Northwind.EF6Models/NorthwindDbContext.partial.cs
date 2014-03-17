// NorthwindDbContext.partial.cs

namespace Northwind.EF6Models
{
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    public partial class NorthwindDbContext
    {
        partial void InitializePartial()
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual ObjectResult<CustOrderHist_Result> CustOrderHist(string customerID)
        {
            ObjectParameter customerIDParameter = customerID != null
                ? new ObjectParameter("CustomerID", customerID)
                : new ObjectParameter("CustomerID", typeof (string));

            return ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<CustOrderHist_Result>("CustOrderHist",
                customerIDParameter);
        }

        public virtual ObjectResult<CustOrdersDetail_Result> CustOrdersDetail(int? orderID)
        {
            ObjectParameter orderIDParameter = orderID.HasValue
                ? new ObjectParameter("OrderID", orderID)
                : new ObjectParameter("OrderID", typeof (int));

            return
                ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<CustOrdersDetail_Result>(
                    "CustOrdersDetail", orderIDParameter);
        }

        public virtual ObjectResult<CustOrdersOrders_Result> CustOrdersOrders(string customerID)
        {
            ObjectParameter customerIDParameter = customerID != null
                ? new ObjectParameter("CustomerID", customerID)
                : new ObjectParameter("CustomerID", typeof (string));

            return
                ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<CustOrdersOrders_Result>(
                    "CustOrdersOrders", customerIDParameter);
        }

        public virtual ObjectResult<Employee_Sales_by_Country_Result> Employee_Sales_by_Country(
            DateTime? beginning_Date, DateTime? ending_Date)
        {
            ObjectParameter beginning_DateParameter = beginning_Date.HasValue
                ? new ObjectParameter("Beginning_Date", beginning_Date)
                : new ObjectParameter("Beginning_Date", typeof (DateTime));

            ObjectParameter ending_DateParameter = ending_Date.HasValue
                ? new ObjectParameter("Ending_Date", ending_Date)
                : new ObjectParameter("Ending_Date", typeof (DateTime));

            return
                ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<Employee_Sales_by_Country_Result>(
                    "Employee_Sales_by_Country", beginning_DateParameter, ending_DateParameter);
        }

        public virtual ObjectResult<Sales_by_Year_Result> Sales_by_Year(DateTime? beginning_Date, DateTime? ending_Date)
        {
            ObjectParameter beginning_DateParameter = beginning_Date.HasValue
                ? new ObjectParameter("Beginning_Date", beginning_Date)
                : new ObjectParameter("Beginning_Date", typeof (DateTime));

            ObjectParameter ending_DateParameter = ending_Date.HasValue
                ? new ObjectParameter("Ending_Date", ending_Date)
                : new ObjectParameter("Ending_Date", typeof (DateTime));

            return ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<Sales_by_Year_Result>("Sales_by_Year",
                beginning_DateParameter, ending_DateParameter);
        }

        public virtual ObjectResult<SalesByCategory_Result> SalesByCategory(string categoryName, string ordYear)
        {
            ObjectParameter categoryNameParameter = categoryName != null
                ? new ObjectParameter("CategoryName", categoryName)
                : new ObjectParameter("CategoryName", typeof (string));

            ObjectParameter ordYearParameter = ordYear != null
                ? new ObjectParameter("OrdYear", ordYear)
                : new ObjectParameter("OrdYear", typeof (string));

            return
                ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<SalesByCategory_Result>("SalesByCategory",
                    categoryNameParameter, ordYearParameter);
        }

        public virtual ObjectResult<Ten_Most_Expensive_Products_Result> Ten_Most_Expensive_Products()
        {
            return
                ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<Ten_Most_Expensive_Products_Result>(
                    "Ten_Most_Expensive_Products");
        }
    }
}