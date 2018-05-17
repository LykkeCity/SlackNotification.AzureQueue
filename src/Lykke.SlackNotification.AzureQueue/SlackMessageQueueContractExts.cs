using Lykke.SlackNotifications;

namespace Lykke.SlackNotification.AzureQueue
{
    /// <summary>
    /// Extensions for SlackMessageQueueEntity conversion to SlackMessageQueueContract
    /// </summary>
    public static class SlackMessageQueueContractExts
    {
        private const int _kb = 1024;
        // 48 Kb - max base64 encoded Azure queue message size
        private const int _maxMessageBese64SizeInKb = 48;

        /// <summary>
        /// Default conversion SlackMessageQueueEntity to SlackMessageQueueContract
        /// </summary>
        /// <param name="entity">Entity to be converted</param>
        /// <returns>Entity converted to SlackMessageQueueContract</returns>
        public static SlackMessageQueueContract ToContract(this SlackMessageQueueEntity entity)
        {
            return ToContract(entity, 0);
        }

        /// <summary>
        /// Conversion SlackMessageQueueEntity to SlackMessageQueueContractwith regard of message size overhead
        /// </summary>
        /// <param name="entity">Entity to be converted</param>
        /// <param name="overheadSymbolsCount">Overhead size</param>
        /// <returns>Entity converted to SlackMessageQueueContract with possibly resized message</returns>
        public static SlackMessageQueueContract ToContract(this SlackMessageQueueEntity entity, int overheadSymbolsCount)
        {
            // 1 Kb - reserved for json format, model.Sender, model.Type and message headers
            int maxMessageLength = (_maxMessageBese64SizeInKb - 1) * _kb - overheadSymbolsCount;

            var message = entity.Message.Length > maxMessageLength
                ? $"{entity.Message.Substring(0, maxMessageLength - 3)}..."
                : entity.Message;

            return new SlackMessageQueueContract
            {
                Type = entity.Type,
                Message = message,
                Sender = entity.Sender
            };
        }
    }
}
