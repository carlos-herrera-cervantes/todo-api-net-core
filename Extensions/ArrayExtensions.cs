using System;

namespace TodoApiNet.Extensions
{
    public static class ArrayExtensions
    {
        public static bool IsEmpty(this Array array) => array.Length == 0 ? true : false;
    }
}