using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessApp
{
    public interface ITaskHelper
    {
        T Deserialize<T>(string json);
        string Serialize(object data);
        string GetBase64EncodedString(string text);
    }
}
