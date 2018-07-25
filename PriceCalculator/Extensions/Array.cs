using System;
using System.Collections.Generic;
using System.Text;

namespace Till.Extensions
{
    public static class ArrayExtension
    {
        public static string[] Add(this string[] array, string value)
        {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = value;
            return array;
        }
    }
}
