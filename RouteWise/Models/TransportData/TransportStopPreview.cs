namespace RouteWise.Models.TransportData
{
    public class TransportStopPreview
    {
        public string Name { get; set; }
        public List<string> Routes { get; set; }
        public List<StopNeighbour> Neighbours { get; set; }
    }
}
