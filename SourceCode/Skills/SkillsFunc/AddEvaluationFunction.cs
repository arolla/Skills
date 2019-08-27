using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using static Skills.Infrastructure.UserSkillEvaluation;

namespace SkillsFunc
{
    public static class AddEvaluationFunction
    {
        [FunctionName("AddEvaluationFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //  string name = req.Query["name"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var userSkills = JsonConvert.DeserializeObject<UserSkillsDto>(requestBody);
            var name = userSkills.user.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
