using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static Skills.Infrastructure.EventStore;
using static Skills.Infrastructure.EvaluationInterop;

namespace SkillsFunc
{
    public static class AddUserSkillFunction
    {
        [FunctionName("AddUserSkillFunction")]
        public static void Run([QueueTrigger("%EventQueueName%", Connection = "%SkillsConnectionString%")]string myQueueItem, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = config["SkillsStorageConnectionString"];
            
            var evaluationAddedEvent = JsonConvert.DeserializeObject<EvaluationAddedDto>(myQueueItem);
            AddEvaluation(connectionString, evaluationAddedEvent);
            log.LogInformation($"C# Queue trigger function user skill saved: {evaluationAddedEvent.data} ");
        }
    }
}
