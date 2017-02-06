using Common;
using Common.Log;
using Lykke.AzureQueueIntegration;
using Lykke.AzureQueueIntegration.Publisher;
using Lykke.SlackNotifications;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.SlackNotification.AzureQueue
{
    public static class SlackNotificationViaAzureQueueBinder
    {

        public static ISlackNotificationsSender UseSlackNotificationsSenderViaAzureQueue(this IServiceCollection serviceCollection, 
            AzureQueueSettings settings, ILog log = null)
        {
            var applicationName =
                Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationName;

            var azureQueuePublisher = 
                new AzureQueuePublisher<SlackMessageQueueEntity>(applicationName, settings)
                .SetLogger(log)
                .SetSerializer(new SlackNotificationsSerializer())
                .Start();

            var result = new SlackNotificationsSender(azureQueuePublisher);

            serviceCollection.AddSingleton<ISlackNotificationsSender>(result);

            return result;
        }
    }


    public class SlackNotificationsSerializer : IAzureQueueSerializer<SlackMessageQueueEntity>
    {
        public string Serialize(SlackMessageQueueEntity model)
        {
            return model.ToContract().ToJson();
        }
    }

    public class SlackMessageQueueContract
    {
        public string Type { get; set; }

        public string Sender { get; set; }

        public string Message { get; set; }

    }

    public static class SlackMessageQueueContractExts
    {
        public static SlackMessageQueueContract ToContract(this SlackMessageQueueEntity entity)
        {
            return new SlackMessageQueueContract
            {
                Type = entity.Type,
                Message = entity.Message,
                Sender = entity.Sender
            };
        }
    }


}
