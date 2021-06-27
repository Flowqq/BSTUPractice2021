using System;

namespace Program
{
    public class DataFieldDef
    {
        public string fieldName;
        public object value;

        public DataFieldDef(string fieldName, object value)
        {
            this.fieldName = fieldName;
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            return obj is DataFieldDef def &&
                   fieldName == def.fieldName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(fieldName);
        }
    }
}