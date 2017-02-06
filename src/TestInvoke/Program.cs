using System;
using Common.Log;
using Lykke.AzureQueueIntegration;
using Lykke.SlackNotification.AzureQueue;
using Lykke.SlackNotifications;
using Microsoft.Extensions.DependencyInjection;

namespace TestInvoke
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var settings = new AzureQueueSettings
            {
                ConnectionString = "",
                QueueName = ""
            };


            var log = new LogToConsole();

            var serviceCollection = new ServiceCollection();

            var sender = serviceCollection.UseSlackNotificationsSenderViaAzureQueue(settings, log);


            sender.SendErrorAsync("Test message").Wait();

            Console.WriteLine("Done");

            Console.ReadLine();

        }
    }
}
