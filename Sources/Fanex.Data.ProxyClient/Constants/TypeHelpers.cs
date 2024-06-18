using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace Constants
{
    internal static class TypeHelpers
    {
        private static readonly IList<Type> SimpleTypes = new List<Type>
        {
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(bool),
            typeof(string),
            typeof(char),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(byte),
            typeof(byte?),
            typeof(sbyte?),
            typeof(short?),
            typeof(ushort?),
            typeof(int?),
            typeof(uint?),
            typeof(long?),
            typeof(ulong?),
            typeof(float?),
            typeof(double?),
            typeof(decimal?),
            typeof(bool?),
            typeof(char?),
            typeof(DateTime?),
            typeof(DateTimeOffset?),
            typeof(TimeSpan?),
            typeof(SqlDecimal),
            typeof(SqlDecimal?),
            typeof(SqlMoney),
            typeof(SqlMoney?)
        };

        public static bool IsSimpleType(Type type)
        {
            return type.IsEnum || SimpleTypes.Contains(type);
        }
    }
}

