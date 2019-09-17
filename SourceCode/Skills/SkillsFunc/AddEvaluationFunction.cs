using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Skills.Infrastructure;

using System.IO;
using System.Threading.Tasks;

using static Skills.Infrastructure.EvaluationInterop;

namespace SkillsFunc
{
    public static class AddEvaluationFunction
    {
        [FunctionName(nameof(AddEvaluationFunction))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = config["SkillsStorageConnectionString"];
            DatedUserEvaluationDto evaluationAddedEvent = null;

            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEnd();
                evaluationAddedEvent = JsonConvert.DeserializeObject<DatedUserEvaluationDto>(requestBody);

                await AddEvaluationAddedEventAsync(connectionString, evaluationAddedEvent);
                return new OkObjectResult($"{evaluationAddedEvent} added!");
            }
            catch (System.Exception ex)
            {
                var message = $"Issue with the input {evaluationAddedEvent}";
                log.LogError(ex, message);
                return new BadRequestObjectResult(message);
            }
        }
    }
}
