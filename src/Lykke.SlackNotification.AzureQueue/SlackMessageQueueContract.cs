namespace Lykke.SlackNotification.AzureQueue
{
    /// <summary>
    /// DTO for Azure queue
    /// </summary>
    public class SlackMessageQueueContract
    {
        public string Type { get; set; }

        public string Sender { get; set; }

        public string Message { get; set; }
    }
}
