using Lykke.SlackNotifications;

namespace Lykke.SlackNotification.AzureQueue
{
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
