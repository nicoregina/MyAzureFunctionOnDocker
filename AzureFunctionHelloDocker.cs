
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MyAzureFunctionOnDocker
{
    public static class AzureFunctionHelloDocker
    {
        [FunctionName("AzureFunctionHelloDocker")]
        //HttpTrigger function is gettin the AuthorizationLevel, GET, POST, and the route ||| WE MUST CHANGE AuthorizationLevel.Function (DEFAULT) TO AuthorizationLevel.Anonymus
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Parameter "name" is assigned from a GET request (getting it from the query request)
            string name = req.Query["name"];

            //Another block starts in order to get te same parameter from the POST request
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            //ends by returning the name.
            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
