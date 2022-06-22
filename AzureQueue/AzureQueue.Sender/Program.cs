using Azure.Storage.Queues;
using AzureQueue.Model;
using Bogus;
using System.Text;

namespace AzureQueue.Sender
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("*** Azure Queue Sender ***");

            var conString = "BlobEndpoint=https://mystorageandre.blob.core.windows.net/;QueueEndpoint=https://mystorageandre.queue.core.windows.net/;FileEndpoint=https://mystorageandre.file.core.windows.net/;TableEndpoint=https://mystorageandre.table.core.windows.net/;SharedAccessSignature=sv=2021-06-08&ss=q&srt=sco&sp=rwdlacup&se=2022-06-29T17:27:07Z&st=2022-06-22T09:27:07Z&spr=https,http&sig=AAjo8dHqiTszShrNJLISMgqF6ZKT7oWsuMn5P7Ljih8%3D";
            var queueName = "halloqueue";
            var client = new QueueClient(conString, queueName);

            var seed = 4;

            var kundenFaker = new Faker<Kunde>("de");
            kundenFaker.UseSeed(seed);
            kundenFaker.RuleFor(x => x.Name, x => x.Name.FullName());
            kundenFaker.RuleFor(x => x.Strasse, x => x.Address.StreetAddress());
            kundenFaker.RuleFor(x => x.Ort, x => x.Address.City());
            kundenFaker.RuleFor(x => x.PLZ, x => x.Address.ZipCode());
            kundenFaker.RuleFor(x => x.Land, x => x.Address.Country());

            var dingFaker = new Faker<Ding>();
            dingFaker.UseSeed(seed);
            dingFaker.RuleFor(x => x.Bezeichnung, x => $"{x.Commerce.Color()} {x.Commerce.ProductName()}");
            dingFaker.RuleFor(x => x.Preis, x => x.Random.Decimal(0, 100));
            dingFaker.RuleFor(x => x.Menge, x => x.Random.Int(1, 10));

            var bestFaker = new Faker<Bestellung>();
            bestFaker.UseSeed(seed);
            bestFaker.RuleFor(x => x.Datum, x => x.Date.Past());
            bestFaker.RuleFor(x => x.Kunde, x => x.PickRandom(kundenFaker.Generate(100)));
            bestFaker.RuleFor(x => x.Dinge, x => dingFaker.Generate(x.Random.Int(1, 4)));


            for (int i = 0; i < 100; i++)
            {
                var best = bestFaker.Generate();
                ShowBestellung(best);

                var json = System.Text.Json.JsonSerializer.Serialize<Bestellung>(best);
                var resp = await client.SendMessageAsync(json);

                //string messageText = $"HALLO {DateTime.Now:G}:{DateTime.Now:ffff}";
                //var resp = await client.SendMessageAsync(messageText);

                Console.WriteLine($"MSG {resp.Value.MessageId} wurde versendet");
            }

            Console.WriteLine("Ende");
            Console.ReadLine();
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