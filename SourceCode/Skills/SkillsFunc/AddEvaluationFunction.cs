using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System.IO;
using System.Threading.Tasks;
using static Skills.Infrastructure.EvaluationInterop;
using static Skills.Infrastructure.EventStore;

namespace SkillsFunc
{
    public static class AddEvaluationFunction
    {
        [FunctionName("AddEvaluationFunction")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = config["SkillsStorageConnectionString"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var evaluationAddedEvent = JsonConvert.DeserializeObject<UserEvalutationDto>(requestBody);

            await AddEvaluationAddedEventAsync(connectionString, evaluationAddedEvent);

            return evaluationAddedEvent != null
                ? (ActionResult)new OkObjectResult($"Hello, {evaluationAddedEvent}, {connectionString}")
                : new BadRequestObjectResult($"Issue with the input {evaluationAddedEvent}");
        }
    }
}
