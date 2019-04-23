
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

            //Parameter "num" is assigned from a GET request (getting it from the query request)
            string num = req.Query["num"];

            //Another block starts in order to get te same parameter from the POST request
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            num = num ?? data?.num;

            //ends by returning the number.
            return num != null
                ? (ActionResult)new OkObjectResult(ValidarNumero(num))
                : new BadRequestObjectResult("your number is not valid, check for mistakes in your typing - Remember it must have more than 8 numbers");
        }

        private static bool ValidarNumero(string num)
        {            
            int number = 0;
            bool canConvert = int.TryParse(num, out number);
            if (canConvert == true && num.Length > 8)
                return ("number now = {0} and it's correct!", number);
            else
                return false;
        }
    }
}
