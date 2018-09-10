using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSpec
{
    public static class Util
    {
        public static void RunAll()
        {
            Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Test)))
                .ToList()
                .ForEach(t => (Activator.CreateInstance(t) as Test).Start());
        }

        public static string Repeat(this string s, int n)
        {
            var result = s;

            for (var i = 0; i < n - 1; i++)
            {
                result += s;
            }

            return result;
        }

        public static AssertionUtil Should(this object caller)
        {
            return new AssertionUtil(caller);
        }
    }
}
