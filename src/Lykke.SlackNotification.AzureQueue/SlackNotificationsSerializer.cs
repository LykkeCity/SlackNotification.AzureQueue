using Common;
using Lykke.AzureQueueIntegration.Publisher;
using Lykke.SlackNotifications;

namespace Lykke.SlackNotification.AzureQueue
{
    public class SlackNotificationsSerializer : IAzureQueueSerializer<SlackMessageQueueEntity>
    {
        public string Serialize(SlackMessageQueueEntity model)
        {
            return model.ToContract().ToJson();
        }
    }
}
