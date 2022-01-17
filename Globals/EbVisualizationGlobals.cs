using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ExpressBase.CoreBase.Globals
{
    public class EbVisualizationGlobals
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

        public EbVisualizationGlobals()
        {
            T0 = new NTVDict();
            T1 = new NTVDict();
            T2 = new NTVDict();
            T3 = new NTVDict();
            T4 = new NTVDict();
            T5 = new NTVDict();
            T6 = new NTVDict();
            T7 = new NTVDict();
            T8 = new NTVDict();
            T9 = new NTVDict();
            Params = new NTVDict();
            Calc = new NTVDict();
            Summary = new NTVDict();
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

    public class NTVDict : DynamicObject
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
                var _data = x as GNTV;

                if (_data.Type == GlobalDbType.Int32)
                    result = Convert.ToDecimal((x as GNTV).Value);
                else if (_data.Type == GlobalDbType.Int64)
                    result = Convert.ToDecimal((x as GNTV).Value);
                else if (_data.Type == GlobalDbType.Int16)
                    result = Convert.ToDecimal((x as GNTV).Value);
                else if (_data.Type == GlobalDbType.Decimal)
                    result = Convert.ToDecimal((x as GNTV).Value);
                else if (_data.Type == GlobalDbType.String)
                    result = ((x as GNTV).Value).ToString();
                else if (_data.Type == GlobalDbType.DateTime)
                    result = Convert.ToDateTime((x as GNTV).Value);
                else if (_data.Type == GlobalDbType.Boolean)
                    result = Convert.ToBoolean((x as GNTV).Value);
                //else if (_data.Type == GlobalDbType.Object && _data.Value.GetType() == typeof(JObject))
                //    result = _data.Value as JObject;
                else
                    result = (x as GNTV).Value.ToString();

                return true;
            }

            result = null;
            return false;
        }

        public object GetValue(string name)
        {
            object result = null;

            dictionary.TryGetValue(name, out object x);
            if (x != null)
            {
                var _data = x as GNTV;

                if (_data.Type == GlobalDbType.Int32)
                    result = Convert.ToDecimal((x as GNTV).Value);
                else if (_data.Type == GlobalDbType.Int64)
                    result = Convert.ToDecimal((x as GNTV).Value);
                else if (_data.Type == GlobalDbType.Int16)
                    result = Convert.ToDecimal((x as GNTV).Value);
                else if (_data.Type == GlobalDbType.Decimal)
                    result = Convert.ToDecimal((x as GNTV).Value);
                else if (_data.Type == GlobalDbType.String)
                    result = ((x as GNTV).Value).ToString();
                else if (_data.Type == GlobalDbType.DateTime)
                    result = Convert.ToDateTime((x as GNTV).Value);
                else if (_data.Type == GlobalDbType.Boolean)
                    result = Convert.ToBoolean((x as GNTV).Value);
                else
                    result = (x as GNTV).Value.ToString();
            }
            return result;

        }
        public void Add(string name, GNTV value)
        {
            dictionary[name] = value;
        }
    }

    
}
