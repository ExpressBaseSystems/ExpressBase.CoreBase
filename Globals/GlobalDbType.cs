﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressBase.CoreBase.Globals
{
    public enum GlobalDbType
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
   
    public class GNTV
    {
        public string Name { get; set; }

        public GlobalDbType Type { get; set; }

        public object Value { get; set; }
    }

}
