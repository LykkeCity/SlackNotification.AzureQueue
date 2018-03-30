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
}
