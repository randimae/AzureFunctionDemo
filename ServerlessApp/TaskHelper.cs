using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServerlessApp
{
    public class TaskHelper : ITaskHelper
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string GetBase64EncodedString(string text)
        {
           
            var bytes  = Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(bytes);
        }

        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
