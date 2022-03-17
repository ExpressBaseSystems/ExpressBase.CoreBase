using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressBase.CoreBase.Globals
{
    public class ApiGlobalParent
    {

        public delegate dynamic GetResourceValueByIndexHandler(int index);

        public delegate dynamic GetResourceValueByNameHandler(string name);

        public delegate void GoToResourceByIndexHandler(int index);

        public delegate void GoToResourceByNameHandler(string name);

        public delegate void ExitWithResultHandler(object obj);


        public event GetResourceValueByIndexHandler ResourceValueByIndexHandler;

        public event GetResourceValueByNameHandler ResourceValueByNameHandler;

        public event GoToResourceByIndexHandler GoToByIndexHandler;

        public event GoToResourceByNameHandler GoToByNameHandler;

        public event ExitWithResultHandler ExitResultHandler;

        public dynamic GetResourceValue(int index)
        {
            return ResourceValueByIndexHandler.Invoke(index);
        }
        
        public dynamic GetResourceValue(string name)
        {
            return ResourceValueByNameHandler.Invoke(name);
        }

        public void GoToResourceByIndex(int index)
        {
            GoToByIndexHandler.Invoke(index);
        }

        public void GoToResourceByName(string name)
        {
            GoToByNameHandler.Invoke(name);
        }

        public void ExitWithResult(object obj)
        {
            ExitResultHandler.Invoke(obj);
        }
    }
    public class ApiScriptHelper
    {
        internal ApiGlobalsCoreBase Globals { set; get; }

        public ApiScriptHelper(ApiGlobalsCoreBase globals)
        {
            Globals = globals;
        }

        public void SetParam(string name, object value)
        {
            Globals.SetParam(name, value);
        }

        public dynamic GetResourceValue(int index)
        {
            return Globals.GetResourceValue(index);
        }

        public dynamic GetResourceValue(string name)
        {
            return Globals.GetResourceValue(name);
        }

        public void GoTo(int index)
        {
            Globals.GoToResourceByIndex(index);
        }

        public void GoTo(string name)
        {
            Globals.GoToResourceByName(name);
        }

        public void Exit()
        {
            throw new ExplicitExitException("Execution terminated explicitly!");
        }

        public void Exit(string message)
        {
            throw new ExplicitExitException(message);
        }

        public void ExitWithResult(object obj)
        {
            Globals.ExitWithResult(obj);
        }
    }


    public class ApiGlobalsCoreBase : ApiGlobalParent
    {
        private readonly Dictionary<string, object> globalParams;               

        public ApiScriptHelper Api { set; get; }

        public dynamic Params { get; set; }

        // public List<EbDataTable> Tables { set; get; }

        public ApiGlobalsCoreBase() { }

        public ApiGlobalsCoreBase(Dictionary<string, object> globalParameters)
        {
            this.globalParams = globalParameters;

            this.Api = new ApiScriptHelper(this);
            this.Params = new NTVDict();

            SetGlobalParams(globalParameters);
        }

        public dynamic this[string key]
        {
            get
            {
                if (key == "Params")
                    return this.Params;
                else
                    return null;
            }
        }

        private GlobalDbType GetEbDbType(object value)
        {
            Type type = value.GetType();

            try
            {
                if (type == typeof(JObject))
                {
                    return GlobalDbType.Object;
                }
                else if (type == typeof(JValue))
                {
                    return GlobalDbType.String;
                }
                else
                {
                    return (GlobalDbType)Enum.Parse(typeof(GlobalDbType), type.Name, true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse value '{value}', parse parameter before set, {ex.Message}");
            }
        }

        public void SetGlobalParams(Dictionary<string, object> globalParams)
        {
            foreach (KeyValuePair<string, object> kp in globalParams)
            {
                this["Params"].Add(kp.Key, new GNTV
                {
                    Name = kp.Key,
                    Type = GetEbDbType(kp.Value),
                    Value = kp.Value
                });
            }
        }

        internal void SetParam(string name, object value)
        {
            globalParams[name] = value;

            this["Params"].Add(name, new GNTV
            {
                Name = name,
                Type = GetEbDbType(value),
                Value = value
            });
        }       
    }

    [Serializable()]
    public class ExplicitExitException : Exception
    {
        public ExplicitExitException() : base() { }

        public ExplicitExitException(string message) : base(message) { }
    }

}
