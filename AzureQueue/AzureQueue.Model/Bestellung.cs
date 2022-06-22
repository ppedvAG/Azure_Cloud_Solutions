namespace AzureQueue.Model
{
    public class Bestellung
    {
        public DateTime Datum { get; set; }
        public Kunde Kunde { get; set; }
        public IEnumerable<Ding> Dinge { get; set; }
    }
}