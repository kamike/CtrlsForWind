using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientCtrl
{
    class AllUtils
    {
        internal static Object jsonToObj(String jsonStr, Type type)
        {

            return JavaScriptConvert.DeserializeObject(jsonStr, type);
        }
       
    }
}
