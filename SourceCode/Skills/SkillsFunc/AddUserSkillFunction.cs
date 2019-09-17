using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Skills.Infrastructure;

using System.Threading.Tasks;

using static Skills.Infrastructure.EvaluationInterop;

namespace SkillsFunc
{
    public static class AddUserSkillFunction
    {
        [FunctionName(nameof(AddUserSkillFunction))]
        public static async Task Run([QueueTrigger("%EventQueueName%", Connection = "%SkillsConnectionString%")]string myQueueItem, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = config["SkillsStorageConnectionString"];
            EvaluationAddedDto evaluationAddedEvent = null;

            try
            {
                evaluationAddedEvent = JsonConvert.DeserializeObject<EvaluationAddedDto>(myQueueItem);
                await AddEvaluationAsync(connectionString, evaluationAddedEvent);
                log.LogInformation($"C# Queue trigger function user skill saved: {evaluationAddedEvent.data} ");
            }
            catch (System.Exception ex)
            {
                log.LogError(ex, $"Event: {evaluationAddedEvent}");
            }

        }
    }
}
