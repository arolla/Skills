using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System.IO;

using static Skills.Infrastructure.UserSkillEvaluation;

namespace SkillsFunc
{
    public static class AddEvaluationFunction
    {
        [FunctionName("AddEvaluationFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var userSkills = JsonConvert.DeserializeObject<UserSkillsDto>(requestBody);

            return userSkills != null
                ? (ActionResult)new OkObjectResult($"Hello, {userSkills}")
                : new BadRequestObjectResult($"Issue with the input {userSkills}");
        }
    }
}
