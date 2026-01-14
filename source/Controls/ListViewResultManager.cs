using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesFinder
{
    internal class ListViewResultManager
    {
        private ListView _lv;
        private delegate void ListViewInvoke(ListView listView, string value, string reason);

        public ListViewResultManager(ListView listview)
        {
            _lv = listview;
        }
        private static void SetListViewText(ListView listView, string value, string reason)
        {
            listView.Items.Add(new ListViewItem(new[] { value, reason }));
        }
    }
}
