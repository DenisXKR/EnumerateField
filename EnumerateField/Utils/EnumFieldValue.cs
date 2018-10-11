using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnumerateField.Utils
{
    internal class EnumFieldValue

    {   //С разделитилем и сериализацией я немного схалтурил :)
        protected const char separator = '-';

        public string Prefix { get; set; }
        public string Number { get; set; }

        public override string ToString()
        {  
            return string.Concat(this.Prefix, separator, this.Number);
        }
        public EnumFieldValue() { }

        public EnumFieldValue(string value)
        {
            this.Deserialize(value);
        }

        public EnumFieldValue(object value)
        {
           this.Deserialize(value.ToString());
        }

        protected void Deserialize(string value)
        {
            string[] items = value.Split(new char[] { separator });
            if (items.Length != 0) this.Prefix = items[0];
            if (items.Length > 1) this.Number = items[1];
        }
    }
}
