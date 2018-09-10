using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSpec
{
    public class AssertionUtil
    {
        private bool isReversed = false;
        private object caller;

        public AssertionUtil(object caller)
        {
            this.caller = caller;
        }

        private static PropertyInfo[] PublicPropertiesOf(object o)
        {
            return o.GetType().GetProperties();
        }

        private bool Condition(bool value)
        {
            return isReversed? !value : value;
        }

        public AssertionUtil Equal(object other)
        {
            var callerJson = JsonConvert.SerializeObject(caller, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, Formatting = Formatting.Indented });
            var otherJson = JsonConvert.SerializeObject(other, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, Formatting = Formatting.Indented });

            if (Condition(!JToken.DeepEquals(callerJson, otherJson)))
            {
                throw new Exceptions.AssertionException($"Expected: {callerJson}\n\nActual: {otherJson}");
            }
            return this;
        }
        public AssertionUtil Not()
        {
            this.isReversed = !this.isReversed;
            return this;
        }
    }
}
