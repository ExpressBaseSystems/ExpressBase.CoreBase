using CodingSeb.ExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressBase.CoreBase.Globals
{
    public class EbSciptEvaluator : ExpressionEvaluator
    {
        public object Execute(string script)
        {
            this.ClearVariables();

            return this.ScriptEvaluate(script);
        }

        public T Execute<T>(string script)
        {
            this.ClearVariables();

            return this.ScriptEvaluate<T>(script);
        }

        private void ClearVariables()
        {
            Variables.ToList().FindAll(kvp => kvp.Value is StronglyTypedVariable).ForEach(kvp => Variables.Remove(kvp.Key));
        }

        public void SetVariable(string key, object value)
        {
            Variables.Add(key, value);
        }

        public void SetVariable(Dictionary<string, object> dict)
        {
            Variables = dict;
        }

        public void RemoveVariable(string key)
        {
            if (Variables.ContainsKey(key))
                Variables.Remove(key);
        }
    }
}
