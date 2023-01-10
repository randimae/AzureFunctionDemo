using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServerlessApp
{
    public class CreateOrderFunction
    {
        public const string Url = "Url";
        public const string UserName = "User";
        public const string Password = "Password";

        private readonly ITaskHelper _taskHelper;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public CreateOrderFunction(ITaskHelper taskHelper, IConfiguration configuration, HttpClient httpClient, IMapper mapper)
        {
            _taskHelper = taskHelper ?? throw new ArgumentNullException(nameof(taskHelper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //[FunctionName("CreateOrderFunction2")]
        //public static async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        //    ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    string name = req.Query["name"];

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    dynamic data = JsonConvert.DeserializeObject(requestBody);
        //    name = name ?? data?.name;

        //    string responseMessage = string.IsNullOrEmpty(name)
        //        ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
        //        : $"Hello, {name}. This HTTP triggered function executed successfully.";

        //    return new OkObjectResult(responseMessage);
        //}

        [FunctionName("CreateOrderFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            // Console.WriteLine("Sample debug output");

            /* TODO: Implement Azure Function logic here */

            // Retreive data from body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = _taskHelper.Deserialize<FunctionRequest>(requestBody);

            var legacyRequest = _mapper.Map<LegacySystemRequest>(data);

            var postMessage = GetLegacySystemHttpRequest(legacyRequest);
            var auth = _configuration[UserName] + ":" + _configuration[Password];
            var header = _taskHelper.GetBase64EncodedString(auth);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", header);

            var result = await _httpClient.SendAsync(postMessage);

            var response =  await result.Content.ReadAsStringAsync();
            var legacyResponce = _taskHelper.Deserialize<LegacySystemResponse>(response);
            var functionResponse = _mapper.Map<FunctionResponse>(legacyResponce);



            return new OkObjectResult(functionResponse);
        }

        public HttpRequestMessage GetLegacySystemHttpRequest(LegacySystemRequest legacySystemRequest)
        {
            /* TODO: Implement getting HttpRequestMessage, that is sent to Legacy System here */
           
            var postUrl = _configuration[Url];
            var content = new StringContent(_taskHelper.Serialize(legacySystemRequest), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
           
            request.Content = content;
            request.RequestUri = new Uri(postUrl);
            
            return request;
        }
    }
}
