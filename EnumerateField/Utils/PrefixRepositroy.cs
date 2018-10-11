using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnumerateField.Utils
{
    internal class PrefixRepositroy: IDisposable
    {
        protected SPList list;
        protected EnumField _field;

        protected SPSite site;
        protected SPWeb web;

        private static readonly object _locker = new object();

        public PrefixRepositroy(EnumField field)
        {
            this._field = field;

            site = new SPSite(SPContext.Current.Site.Url);
            web = site.OpenWeb(this._field.WebSite);
            this.list = web.Lists[new Guid(this._field.ListField)];
        }

        public int? GetNextNumber(string prefix)
        {
            lock (_locker)
            {
                SPQuery query = new SPQuery
                {
                    ViewFields = string.Format("<FieldRef Name=\"{0}\" />", this._field.SerialNumberField),
                    Query = string.Format("<Where><Eq><FieldRef Name=\"{0}\" /><Value Type=\"Text\">{1}</Value></Eq></Where>", this._field.PrefixField, prefix),
                    RowLimit = 1
                };

                SPListItemCollection items = this.list.GetItems(query);

                if (items.Count != 0)
                {
                    int n;
                    SPListItem item = items[0];
                    if (int.TryParse(item[this._field.SerialNumberField].ToString(), out n))
                    {
                        n++;
                        item[this._field.SerialNumberField] = n;
                        item.Update();
                        return n;
                    }
                    return null;
                }
            }
            return null;
        }

        public IEnumerable<string>GetAllPrefix()
        {
            SPQuery query = new SPQuery
            {
                ViewFields = string.Format("<FieldRef Name=\"{0}\" />", this._field.PrefixField)
            };

            SPListItemCollection items = this.list.GetItems(query);

            yield return string.Empty;

            foreach (SPListItem item in items)
                yield return item[this._field.PrefixField].ToString();
        }

        public void Dispose()
        {
            if (web != null) web.Dispose();
            if (site != null) site.Dispose();
        }
    }
}
