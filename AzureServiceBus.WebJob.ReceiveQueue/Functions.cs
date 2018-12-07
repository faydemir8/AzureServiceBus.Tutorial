using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;

namespace AzureServiceBus.WebJob.ReceiveQueue
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static async Task ProcessQueueMessage([ServiceBusTrigger("queue")]  BrokeredMessage message, TextWriter log)
        {
            Stream stream = message.GetBody<Stream>();
            StreamReader reader = new StreamReader(stream);
            string queueMessage = reader.ReadToEnd();

            await PostWebApiAsync(queueMessage);
            //Log
            log.WriteLine("Message Body : " + queueMessage);
        }


        public static async Task<string> PostWebApiAsync(string postData)
        {
            var client = new HttpClient {Timeout = TimeSpan.FromMinutes(2)};
            var result = await client.GetAsync($"url");
            string resultContent = await result.Content.ReadAsStringAsync();
            client.Dispose();
            return resultContent;

        }
    }
}
