using Microsoft.SharePoint.WebControls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SharePoint;
using System.Linq;

namespace EnumerateField.ControlTemplates.EnumerateField
{
    public partial class EnumFieldPropertyEditor : UserControl, IFieldEditor
    {
        protected EnumField _field;

        public bool DisplayAsNewSection
        {
            get { return false; }
        }

        public void InitializeWithField(SPField field)
        {
            this._field = (EnumField)field;

            if (!this.Page.IsPostBack)
            {
                this.GetAllWeb();
            }
        }

        public void OnSaveChange(SPField field, bool isNewField)
        {
            EnumField f = (EnumField)field;
            if (f != null)
            {
                f.WebSite = this.webColumnField.SelectedValue;
                f.ListField = this.listColumnField.SelectedValue;
                f.PrefixField = this.prefixColumnField.SelectedValue;
                f.SerialNumberField = this.serialNumberColumnField.SelectedValue;
                f.NumberSymbolCount = this.numberCountColumnField.Text;
                //f.Update();
            }            
        }

        protected void GetAllWeb()
        {
            this.webColumnField.Items.Clear();
            this.webColumnField.Items.Add(string.Empty);
            SPSite site = SPContext.Current.Site;

            List<ListItem> items = new List<ListItem>();

            foreach (SPWeb web in site.AllWebs)
            {
                using (web)
                {
                    items.Add(new ListItem(web.Title, web.ServerRelativeUrl));
                }
            }

            this.webColumnField.Items.AddRange(items.OrderBy(i => i.Text).ToArray());

            if (this._field != null)
            {
                numberCountColumnField.Text = this._field.NumberSymbolCount;

                ListItem item = this.webColumnField.Items.FindByValue(this._field.WebSite);
                if (item != null)
                {
                    this.webColumnField.SelectedIndex = this.webColumnField.Items.IndexOf(item);
                    this.GetAllList(item.Value);
                }
            }
        }

        protected void GetAllList(string webUrl)
        {
            this.listColumnField.Items.Clear();
            this.listColumnField.Items.Add(string.Empty);

            using (SPWeb web = SPContext.Current.Site.OpenWeb(webUrl))
            {
                foreach (SPList list in web.Lists)
                {
                    if (!list.Hidden)
                    {
                        this.listColumnField.Items.Add(new ListItem(list.Title, list.ID.ToString()));
                    }
                }
            }

            if (this._field != null)
            {
                ListItem item = this.listColumnField.Items.FindByValue(this._field.ListField);
                if (item != null)
                {
                    this.listColumnField.SelectedIndex = this.listColumnField.Items.IndexOf(item);
                    this.GetAllFields(item.Value);
                }
            }
        }

        protected void GetAllFields(string listId)
        {
            this.prefixColumnField.Items.Clear();
            this.serialNumberColumnField.Items.Clear();
            using (SPWeb web = SPContext.Current.Site.OpenWeb(this.webColumnField.SelectedValue))
            {
                SPList list = web.Lists[new Guid(listId)];
                foreach (SPField field in list.Fields)
                {
                    if (!(field.DefaultListField || field.Hidden))
                    {
                        this.prefixColumnField.Items.Add(new ListItem(field.Title, field.StaticName));
                        this.serialNumberColumnField.Items.Add(new ListItem(field.Title, field.StaticName));
                    }
                }
            }

            if (this._field != null)
            {
                ListItem item = this.prefixColumnField.Items.FindByValue(this._field.PrefixField);
                if (item != null) this.prefixColumnField.SelectedIndex = this.prefixColumnField.Items.IndexOf(item);

                ListItem item1 = this.serialNumberColumnField.Items.FindByValue(this._field.SerialNumberField);
                if (item1 != null) this.serialNumberColumnField.SelectedIndex = this.serialNumberColumnField.Items.IndexOf(item1);
            }
        }

        protected void webColumnField_SelectedIndexChanged(object sender, EventArgs e)
        {
            string webUrl = this.webColumnField.SelectedValue;
            if (!string.IsNullOrEmpty(webUrl)) this.GetAllList(webUrl);
        }

        protected void listColumnField_SelectedIndexChanged(object sender, EventArgs e)
        {
            string listUrl = this.listColumnField.SelectedValue;
            if (!string.IsNullOrEmpty(listUrl)) this.GetAllFields(listUrl);
        }

    }
}
