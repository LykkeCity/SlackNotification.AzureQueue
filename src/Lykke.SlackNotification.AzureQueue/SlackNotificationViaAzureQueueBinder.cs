using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.AzureQueueIntegration;
using Lykke.AzureQueueIntegration.Publisher;
using Lykke.SlackNotifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Autofac;

namespace Lykke.SlackNotification.AzureQueue
{
    [PublicAPI]
    public static class SlackNotificationViaAzureQueueBinder
    {
        public static ISlackNotificationsSender UseSlackNotificationsSenderViaAzureQueue(
            this IServiceCollection serviceCollection, 
            AzureQueueSettings settings,
            ILog log = null)
        {
            var applicationName = PlatformServices.Default.Application.ApplicationName;

            var azureQueuePublisher = 
                new AzureQueuePublisher<SlackMessageQueueEntity>(applicationName, settings)
                .SetLogger(log)
                .SetSerializer(new SlackNotificationsSerializer())
                .Start();

            var result = new SlackNotificationsSender(azureQueuePublisher, ownQueue: true);

            serviceCollection.AddSingleton<ISlackNotificationsSender>(result);

            return result;
        }

        public static ISlackNotificationsSender UseSlackNotificationsSenderViaAzureQueue(
            this ContainerBuilder containerBuilder,
            AzureQueueSettings settings,
            ILog log = null)
        {
            var applicationName = PlatformServices.Default.Application.ApplicationName;

            var azureQueuePublisher =
                new AzureQueuePublisher<SlackMessageQueueEntity>(applicationName, settings)
                .SetLogger(log)
                .SetSerializer(new SlackNotificationsSerializer())
                .Start();

            var result = new SlackNotificationsSender(azureQueuePublisher);

            containerBuilder.RegisterInstance(result).As<ISlackNotificationsSender>().SingleInstance();

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
            // 48 Kb - max base64 encoded Azure queue message size
            // 1 Kb - reserved for json format, model.Sender, model.Type and message headers
            const int kb = 1024;
            const int maxLength = 48 * kb - 1 * kb;
            
            var message = entity.Message.Length > maxLength ? entity.Message.Substring(0, maxLength) : entity.Message;

            return new SlackMessageQueueContract
            {
                Type = entity.Type,
                Message = message,
                Sender = entity.Sender
            };
        }
    }
}
