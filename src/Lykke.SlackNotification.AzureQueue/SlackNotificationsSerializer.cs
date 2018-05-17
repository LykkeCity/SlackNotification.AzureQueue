using Common;
using Lykke.AzureQueueIntegration.Publisher;
using Lykke.SlackNotifications;

namespace Lykke.SlackNotification.AzureQueue
{
    /// <summary>
    /// Serializer for SlackMessageQueueEntity
    /// </summary>
    public class SlackNotificationsSerializer : IAzureQueueSerializer<SlackMessageQueueEntity>
    {
        private const int _maxQueueMessageBase64Size = 49152; //48 Kb

        /// <summary>
        /// Converts SlackMessageQueueEntity to string with size within base64 encoded max message length range
        /// </summary>
        /// <param name="model">Model to be serialized</param>
        /// <returns>Serialized string</returns>
        public string Serialize(SlackMessageQueueEntity model)
        {
            string result = model.ToContract().ToJson();
            if (result.Length > _maxQueueMessageBase64Size)
            {
                int overhead = result.Length - _maxQueueMessageBase64Size;
                do
                {
                    result = model.ToContract(overhead).ToJson();
                    overhead += 10;
                }
                while (result.Length > _maxQueueMessageBase64Size);
            }
            return result;
        }
    }
}
