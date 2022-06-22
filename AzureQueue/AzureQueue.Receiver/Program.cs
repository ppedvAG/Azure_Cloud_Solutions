using Azure.Storage.Queues;
using AzureQueue.Model;

namespace AzureQueue.Receiver
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("*** Azure Queue Receiver ***");

            var conString = "BlobEndpoint=https://mystorageandre.blob.core.windows.net/;QueueEndpoint=https://mystorageandre.queue.core.windows.net/;FileEndpoint=https://mystorageandre.file.core.windows.net/;TableEndpoint=https://mystorageandre.table.core.windows.net/;SharedAccessSignature=sv=2021-06-08&ss=q&srt=sco&sp=rwdlacup&se=2022-06-29T17:27:07Z&st=2022-06-22T09:27:07Z&spr=https,http&sig=AAjo8dHqiTszShrNJLISMgqF6ZKT7oWsuMn5P7Ljih8%3D";
            var queueName = "halloqueue";
            var client = new QueueClient(conString, queueName);

            while (true)
            {
                var msg = await client.ReceiveMessageAsync();

                if (msg.Value == null)
                {
                    Console.WriteLine("Keine neue Message");
                    Thread.Sleep(1000);
                }
                else
                {
                    //Console.WriteLine($"{msg.Value.MessageId} {msg.Value.Body}");

                    var best = System.Text.Json.JsonSerializer.Deserialize<Bestellung>(msg.Value.Body);


                    client.DeleteMessage(msg.Value.MessageId, msg.Value.PopReceipt);

                    ShowBestellung(best);
                }
            }

        }

        private static void ShowBestellung(Bestellung best)
        {
            Console.WriteLine($"{best.Datum} von {best.Kunde.Name}, {best.Kunde.Strasse}, {best.Kunde.PLZ} {best.Kunde.Ort}, {best.Kunde.Land}");
            foreach (var item in best.Dinge)
            {
                Console.WriteLine($"\t{item.Menge}x {item.Bezeichnung} {item.Preis:c}");
            }
        }
    }
}