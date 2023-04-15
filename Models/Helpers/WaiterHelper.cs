using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumcad.Models.Helpers
{
    internal class WaiterHelper
    {
        private static readonly ObservableCollection<bool> waiterList = new ObservableCollection<bool>();

        internal static event EventHandler CollectionChanged;

        static WaiterHelper()
        {
            waiterList.CollectionChanged += OnCollectionChanged;
        }

        internal static void AddWaiter()
        {
            waiterList.Add(true);
        }

        internal static void RemoveWaiter()
        {
            waiterList.Remove(true);
        }

        internal static bool GetWaiterStatus()
        {
            return waiterList.Count == 0;
        }

        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(sender, args);
        }
    }
}
