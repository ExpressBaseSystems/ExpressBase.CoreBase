using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ExpressBase.CoreBase.Globals
{
    public class EbPdfGlobals
    {
        public dynamic T0 { get; set; }
        public dynamic T1 { get; set; }
        public dynamic T2 { get; set; }
        public dynamic T3 { get; set; }
        public dynamic T4 { get; set; }
        public dynamic T5 { get; set; }
        public dynamic T6 { get; set; }
        public dynamic T7 { get; set; }
        public dynamic T8 { get; set; }
        public dynamic T9 { get; set; }
        public dynamic Params { get; set; }
        public dynamic Calc { get; set; }
        public dynamic Summary { get; set; }
        public dynamic CurrentField { get; set; }

        public EbPdfGlobals()
        {
            T0 = new PDF_NTVDict();
            T1 = new PDF_NTVDict();
            T2 = new PDF_NTVDict();
            T3 = new PDF_NTVDict();
            T4 = new PDF_NTVDict();
            T5 = new PDF_NTVDict();
            T6 = new PDF_NTVDict();
            T7 = new PDF_NTVDict();
            T8 = new PDF_NTVDict();
            T9 = new PDF_NTVDict();
            Params = new PDF_NTVDict();
            Calc = new PDF_NTVDict();
            Summary = new PDF_NTVDict();
        }

        public dynamic this[string tableIndex]
        {
            get
            {
                if (tableIndex == "T0")
                    return this.T0;
                else if (tableIndex == "T1")
                    return this.T1;
                else if (tableIndex == "T2")
                    return this.T2;
                else if (tableIndex == "T3")
                    return this.T3;
                else if (tableIndex == "T4")
                    return this.T4;
                else if (tableIndex == "T5")
                    return this.T5;
                else if (tableIndex == "T6")
                    return this.T6;
                else if (tableIndex == "T7")
                    return this.T7;
                else if (tableIndex == "T8")
                    return this.T8;
                else if (tableIndex == "T9")
                    return this.T9;
                else if (tableIndex == "Params")
                    return this.Params;
                else if (tableIndex == "Calc")
                    return this.Calc;
                else if (tableIndex == "Summary")
                    return this.Summary;
                else
                    return this.T0;
            }
        }
    }

    public class PDF_NTVDict : DynamicObject
    {
        private Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;

            object x;
            dictionary.TryGetValue(name, out x);
            if (x != null)
            {
                var _data = x as PDF_NTV;

                if (_data.Type == PDF_EbDbTypes.Int32)
                    result = Convert.ToDecimal((x as PDF_NTV).Value);
                else if (_data.Type == PDF_EbDbTypes.Int64)
                    result = Convert.ToDecimal((x as PDF_NTV).Value);
                else if (_data.Type == PDF_EbDbTypes.Int16)
                    result = Convert.ToDecimal((x as PDF_NTV).Value);
                else if (_data.Type == PDF_EbDbTypes.Decimal)
                    result = Convert.ToDecimal((x as PDF_NTV).Value);
                else if (_data.Type == PDF_EbDbTypes.String)
                    result = ((x as PDF_NTV).Value).ToString();
                else if (_data.Type == PDF_EbDbTypes.DateTime)
                    result = Convert.ToDateTime((x as PDF_NTV).Value);
                else if (_data.Type == PDF_EbDbTypes.Boolean)
                    result = Convert.ToBoolean((x as PDF_NTV).Value);
                else
                    result = (x as PDF_NTV).Value.ToString();
                return true;
            }
            result = null;
            return false;
        }

        public void Add(string name, PDF_NTV value)
        {
            dictionary[name] = value;
        }
    }

    public class PDF_NTV
    {
        public string Name { get; set; }

        public PDF_EbDbTypes Type { get; set; }

        public object Value { get; set; }
    }

    public enum PDF_EbDbTypes
    {
        AnsiString = 0,
        Binary = 1,
        Byte = 2,
        Boolean = 3,
        Currency = 4,
        Date = 5,
        DateTime = 6,
        Decimal = 7,
        Double = 8,
        Guid = 9,
        Int16 = 10,
        Int32 = 11,
        Int64 = 12,
        Object = 13,
        SByte = 14,
        Single = 15,
        String = 16,
        Time = 17,
        UInt16 = 18,
        UInt32 = 19,
        UInt64 = 20,
        VarNumeric = 21,
        AnsiStringFixedLength = 22,
        StringFixedLength = 23,
        Xml = 25,
        DateTime2 = 26,
        DateTimeOffset = 27,
        Json = 28,
        Bytea = 29,
        BooleanOriginal = 30,
        Int = 31,
        VarChar = 32
    }
}
