namespace RouteWise.Models.TransportData
{
    public class Transport
    {
        public string Type { get; set; }
        public string RouteId { get; set; }
        public List<TransportStop> Stops { get; set; }
    }
}
