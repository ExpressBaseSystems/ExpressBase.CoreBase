﻿using System;
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
            T0 = new PdfNTVDict();
            T1 = new PdfNTVDict();
            T2 = new PdfNTVDict();
            T3 = new PdfNTVDict();
            T4 = new PdfNTVDict();
            T5 = new PdfNTVDict();
            T6 = new PdfNTVDict();
            T7 = new PdfNTVDict();
            T8 = new PdfNTVDict();
            T9 = new PdfNTVDict();
            Params = new PdfNTVDict();
            Calc = new PdfNTVDict();
            Summary = new PdfNTVDict();
            CurrentField = new PdfNTVDict();
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
                else if (tableIndex == "CurrentField")
                    return this.CurrentField;
                else
                    return this.T0;
            }
        }
    }

    public class PdfNTVDict : DynamicObject
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
         
    public class PdfGReportField
    {
        public float Left { get; set; }

        public float Width { get; set; }

        public float Top { get; set; }

        public float Height { get; set; }

        public string BackColor { get; set; }

        public string ForeColor { get; set; }

        public PdfGEbFont Font { get; set; }

        public PdfGTextAlign TextAlign { get; set; }

        public bool IsHidden { get; set; }

        public PdfGReportField()
        {
        }

        public PdfGReportField(float left, float width, float top, float height, string backColor, string foreColor, bool isHidden, PdfGEbFont font)
        {
            Left = left;
            Width = width;
            Top = top;
            Height = height;
            BackColor = backColor;
            ForeColor = foreColor;
            IsHidden = isHidden;
            Font = font;
        }
    }

    public class PdfGEbFont
    {
        public PdfGEbFont() { }

        public string FontName { get; set; } = "Roboto";

        public int Size { get; set; }

        public PdfGFontStyle Style { get; set; }


        public string color { get; set; }

        public bool Caps { get; set; }

        public bool Strikethrough { get; set; }

        public bool Underline { get; set; }

        public enum PdfGFontStyle
        {
            NORMAL = 0,
            ITALIC = 2,
            BOLD = 1,
            BOLDITALIC = 3
        }
    }

    public enum PdfGTextAlign
    {
        Left = 0,
        Center = 1,
        Right = 2,
        Justify = 3,
        Top = 4,
        Middle = 5,
        Bottom = 6,
        Baseline = 7,
        JustifiedAll = 8,
        Undefined = -1
    }
}
