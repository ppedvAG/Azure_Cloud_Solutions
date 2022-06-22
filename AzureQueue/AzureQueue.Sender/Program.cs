using Azure.Storage.Queues;

namespace AzureQueue.Sender
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("*** Azure Queue Sender ***");

            var conString = "BlobEndpoint=https://mystorageandre.blob.core.windows.net/;QueueEndpoint=https://mystorageandre.queue.core.windows.net/;FileEndpoint=https://mystorageandre.file.core.windows.net/;TableEndpoint=https://mystorageandre.table.core.windows.net/;SharedAccessSignature=sv=2021-06-08&ss=q&srt=sco&sp=rwdlacup&se=2022-06-29T17:27:07Z&st=2022-06-22T09:27:07Z&spr=https,http&sig=AAjo8dHqiTszShrNJLISMgqF6ZKT7oWsuMn5P7Ljih8%3D";
            var queueName = "halloqueue";
            var client = new QueueClient(conString, queueName);

            for (int i = 0; i < 100; i++)
            {
                string messageText = $"HALLO {DateTime.Now:G}:{DateTime.Now:ffff}";
                var resp = await client.SendMessageAsync(messageText);

                Console.WriteLine($"MSG {resp.Value.MessageId} wurde versendet: {messageText}");
            }

            Console.WriteLine("Ende");
            Console.ReadLine();
        }
    }
}