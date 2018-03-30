namespace Lykke.SlackNotification.AzureQueue
{
    public class SlackMessageQueueContract
    {
        public string Type { get; set; }

        public string Sender { get; set; }

        public string Message { get; set; }
    }
}
