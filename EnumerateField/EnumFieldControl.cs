using EnumerateField.Utils;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EnumerateField
{
    class EnumFieldControl : BaseTextField
    {
        protected DropDownList prefixSelect;
        protected Label serialNumberLabel;

        protected override void CreateChildControls()
        {
            if (this.ControlMode == SPControlMode.New || this.ControlMode == SPControlMode.Edit)
            {
                using (PrefixRepositroy repositroy = new PrefixRepositroy(this.Field as EnumField))
                {
                    IEnumerable<string> prefixList = repositroy.GetAllPrefix();

                    this.serialNumberLabel = new Label { Text = string.Empty };
                    this.prefixSelect = new DropDownList();
                    this.prefixSelect.SelectedIndexChanged += prefixSelect_SelectedIndexChanged;
                    this.prefixSelect.DataSource = prefixList;
                    this.prefixSelect.DataBind();

                    if (this.Value != null)
                    {
                        EnumFieldValue enumFieldValue = (EnumFieldValue)this.Value as EnumFieldValue;
                        ListItem item = this.prefixSelect.Items.FindByText(enumFieldValue.Prefix);

                        if (item != null) this.prefixSelect.SelectedIndex = this.prefixSelect.Items.IndexOf(item);

                        this.serialNumberLabel.Text = string.Concat("-" + enumFieldValue.Number);
                    }

                    this.Controls.Add(this.prefixSelect);
                    this.Controls.Add(this.serialNumberLabel);
                }
            }
        }

        protected override void RenderFieldForDisplay(HtmlTextWriter output)
        {
            if (this.Value != null) output.Write(this.Value.ToString());
        }

        public override object Value
        {   //Базовое свойство как то криво реализовано
            get
            {
                this.EnsureChildControls();
                return this.ItemFieldValue;
            }
            set
            {
                this.EnsureChildControls();
                this.ItemFieldValue = value;
            }
        }

        void prefixSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.prefixSelect.SelectedItem.Text)) return;

            using (PrefixRepositroy repositroy = new PrefixRepositroy(this.Field as EnumField))
            {
                int? n = repositroy.GetNextNumber(this.prefixSelect.SelectedItem.Text);

                if (n != null)
                {
                    EnumFieldValue val = new EnumFieldValue
                    {
                        Prefix = this.prefixSelect.SelectedItem.Text,
                        Number = n.Value.ToString()
                    };

                    this.Value = val;
                }
            }
        }
    }
}
