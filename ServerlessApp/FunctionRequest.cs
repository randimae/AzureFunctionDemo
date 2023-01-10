using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessApp
{
    public class FunctionRequest
    {
        public string CorrelationId { get; set; }
        public string Reference { get; set; }
    }


    public class LegacySystemRequest
    {
        public string CorrelationId { get; set; }
        public string Id { get; set; }
    }

    public class FunctionResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class LegacySystemResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
