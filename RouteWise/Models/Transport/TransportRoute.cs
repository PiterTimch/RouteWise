namespace RouteWise.Models.Transport
{
    public class TransportRoute
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TransportStop> Stops { get; set; } = new();
        public string TransportType { get; set; }
    }
}
