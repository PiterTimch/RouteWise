namespace RouteWise.Models.TransportData
{
    public class TransportStop
    {
        public string Name { get; set; }
        public List<Transport> Transport { get; set; }
        public List<TransportStop> Next { get; set; }
    }
}
