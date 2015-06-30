using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Data;

namespace EventManagementSystem.Helpers
{
    public class SumClerksFunction : EnumerableAggregateFunction
    {
        protected override string AggregateMethodName
        {
            get
            {
                return "SumClerks";
            }
        }

        protected override Type ExtensionMethodsType
        {
            get
            {
                return typeof(Statistics);
            }
        }
    }

    class SumTillsFunction : EnumerableAggregateFunction
    {
        protected override string AggregateMethodName
        {
            get { return "SumTills"; }
        }

        protected override Type ExtensionMethodsType
        {
            get
            {
                return typeof(Statistics);
            }
        }
    }

    public static class Statistics
    {
        public static string SumClerks<T>(IEnumerable<T> source) 
        {
            int itemCount = source.Count();
            if (itemCount == 1)
            {
                dynamic d = (dynamic)source.First();
                return (string)d.GetType().GetProperty("ClerkName").GetValue(d, null);
            }
            if (itemCount > 1)
            {
                IEnumerable<dynamic> collection = (IEnumerable<dynamic>)source;
                IEnumerable<string> values = from i in collection select (string)i.GetType().GetProperty("ClerkName").GetValue(i, null);
                var disctinctValues = values.Distinct();
                var sum = String.Join(", ", disctinctValues);
                return sum;
            }
            return String.Empty;
        }

        public static string SumTills<T>(IEnumerable<T> source)
        {
            int itemCount = source.Count();
            if (itemCount == 1)
            {
                dynamic d = source.First();
                return ((int)d.GetType().GetProperty("TillID").GetValue(d, null)).ToString();
            }
            if (itemCount > 1)
            {
                IEnumerable<dynamic> collection = (IEnumerable<dynamic>)source;
                IEnumerable<int> values = from i in collection select (int)i.GetType().GetProperty("TillID").GetValue(i, null);
                var disctinctValues = values.Distinct();
                var sum = String.Join(", ", disctinctValues);
                return sum;
            }
            return String.Empty;
        }
    }

}


