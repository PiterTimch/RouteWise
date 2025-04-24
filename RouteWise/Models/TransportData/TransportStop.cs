namespace RouteWise.Models.TransportData
{
    public class TransportStop
    {
        public string Name { get; set; }
        public List<Transport> Transport { get; set; }
        public List<NeighbourStopLink> Neighbours { get; set; }
    }

    public class NeighbourStopLink
    {
        public TransportStop Stop { get; set; }
        public int Distance { get; set; }
    }

}
