using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using static Skills.Infrastructure.UserSkillsInterop;
using static Skills.Infrastructure.UserSkillEvaluation;

namespace SkillsFunc
{
    public static class GetUserSkillFunction
    {
        [FunctionName("GetUserSkillFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = config["SkillsStorageConnectionString"];

            var user = JsonConvert.DeserializeObject<UserDto>(
                JsonConvert.SerializeObject(
                    req.GetQueryParameterDictionary()));

            return new OkObjectResult(ReadUserSkills(connectionString, user));
        }
    }
}
