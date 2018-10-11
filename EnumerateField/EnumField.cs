using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnumerateField.Utils;
using System.Reflection;

namespace EnumerateField
{
    public class EnumField : SPFieldText
    {
        public EnumField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public EnumField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                return new EnumFieldControl
                {
                    FieldName = this.InternalName
                };
            }
        }

        #region Property

        //Почему стандартный способ работы с полями не функциклирует
     
        /// <summary>
        /// список для выбора веб-узла SharePoint списка серий номеров
        /// </summary>
        public string WebSite
        {
            get
            {
                //return this.GetCustomProperty("WebSite").ToString();
                return GetFieldAttribute("WebSite");
            }
            set
            {
                //this.SetCustomProperty("WebSite", value);
                this.SetFieldAttribute("WebSite", value);
            }
        }

        /// <summary>
        /// список для выбора списка SharePoint серий номеров
        /// </summary>
        public string List
        {
            get
            {
                //return this.GetCustomProperty("List").ToString();
                return GetFieldAttribute("List");
            }
            set
            {
                //this.SetCustomProperty("List", value);
                this.SetFieldAttribute("List", value);
            }
        }

        /// <summary>
        /// выпадающий список для выбора названия поля наименования префикса серии номеров в списке SharePoint серий номеров
        /// </summary>
        public string PrefixField
        {
            get
            {
                //return this.GetCustomProperty("PrefixField").ToString();
                return GetFieldAttribute("PrefixField");
            }
            set
            {
                //this.SetCustomProperty("PrefixField", value);
                this.SetFieldAttribute("PrefixField", value);
            }
        }

        /// <summary>
        /// список для выбора названия поля порядкового номера в списке SharePoint серий номеров
        /// </summary>
        public string SerialNumberField
        {
            get
            {
                //return this.GetCustomProperty("SerialNumberField").ToString();
                return GetFieldAttribute("SerialNumberField");
            }
            set
            {
                //this.SetCustomProperty("SerialNumberField", value);
                this.SetFieldAttribute("SerialNumberField", value);
            }
        }

        /// <summary>
        /// список для выбора названия поля URI-адресов списков SharePoint для использования в них серии номеров списка SharePoint серий номеров
        /// </summary>
        public string ListField
        {
            get
            {
                //return this.GetCustomProperty("ListField").ToString();
                return GetFieldAttribute("ListField");
            }
            set
            {
                //this.SetCustomProperty("ListField", value);
                this.SetFieldAttribute("ListField", value);
            }
        }

        /// <summary>
        /// поле для ввода количества символов в порядковом номере серии номеров
        /// </summary>
        public string NumberSymbolCount
        {
            get
            {
                //return this.GetCustomProperty("NumberSymbolCount").ToString();
                return GetFieldAttribute("NumberSymbolCount");
            }
            set
            {
                //this.SetCustomProperty("NumberSymbolCount", value);
                this.SetFieldAttribute("NumberSymbolCount", value);
            }
        }

        #endregion

        #region Workarround

        //Ужос :(
        //http://gunnarpeipman.com/2009/02/sharepoint-temporary-solution-for-getcustomproperty-and-setcustomproperty-errors/
        //ну, а что ты хотел - это же шарепойнт :)        

        private void SetFieldAttribute(string attribute, string value)
        {
            Type baseType;
            BindingFlags flags;
            MethodInfo mi;

            baseType = typeof(EnumField);
            flags = BindingFlags.Instance | BindingFlags.NonPublic;
            mi = baseType.GetMethod("SetFieldAttributeValue", flags);
            mi.Invoke(this, new object[] { attribute, value });
        }

        private string GetFieldAttribute(string attribute)
        {
            Type baseType;
            BindingFlags flags;
            MethodInfo mi;

            baseType = typeof(EnumField);
            flags = BindingFlags.Instance | BindingFlags.NonPublic;
            mi = baseType.GetMethod("GetFieldAttributeValue",
                                        flags,
                                        null,
                                        new Type[] { typeof(String) },
                                        null);
            object obj = mi.Invoke(this, new object[] { attribute });

            if (obj == null) return string.Empty;
            else return obj.ToString();
        }

        #endregion

        public override object GetFieldValue(string value)
        {
            EnumFieldValue enumFieldValue = new EnumFieldValue(value);
            enumFieldValue.Number = this.FormatSerialNumber(enumFieldValue.Number);
            return enumFieldValue;

        }

        public override string GetFieldValueAsText(object value)
        {
            EnumFieldValue enumFieldValue = new EnumFieldValue(value);
            enumFieldValue.Number = this.FormatSerialNumber(enumFieldValue.Number);
            return enumFieldValue.ToString();
        }

        protected string FormatSerialNumber(string value)
        {
            string f = value;
            while(f.Length < int.Parse(this.NumberSymbolCount))
            {
                f = string.Concat("0", f);
            }
            return f;
        }

    }
}
