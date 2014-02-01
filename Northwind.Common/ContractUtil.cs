// ContractUtil.cs

namespace Northwind.Common
{
    using System;
    using System.Collections;

    public class ContractUtil
    {
        public static Exception Unreachable
        {
            get { return new InvalidOperationException("Code supposed to be unreachable"); }
        }

        #region Requires

        public static void Requires(bool precondition)
        {
            if (!precondition)
            {
                throw new ArgumentException("Method precondition violated");
            }
        }

        public static void Requires(bool precondition, string paramName)
        {
            if (!precondition)
            {
                throw new ArgumentException("Invalid argument value", paramName);
            }
        }

        public static void RequiresNotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void RequiresStringNotNullOrWhiteSpace(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("String can not be null or white space", paramName);
            }
        }

        public static void RequiresListNotNullOrEmpty(IList list, string listName)
        {
            RequiresNotNull(list, listName);
            if (list.Count == 0)
            {
                throw new ArgumentException("List can not be null of empty", listName);
            }
        }

        public static void RequiresListRange(IList list, int offset, int count, string offsetName, string countName)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(countName);
            }

            if ((offset < 0) || (list.Count - offset) < count)
            {
                throw new ArgumentOutOfRangeException(offsetName);
            }
        }

        public static void RequiresListNotNullItems(IList list, string listName)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    throw new ArgumentNullException(string.Format("{0}[{1}]", new object[] {listName, i}));
                }
            }
        }

        #endregion

        #region Ensure

        public static void EnsureIsNull(object value, string paramName)
        {
            if (value != null)
            {
                throw new ArgumentException("Null value is necessary", paramName);
            }
        }

        #endregion
    }
}