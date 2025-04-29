using Newtonsoft.Json;
using RouteWise.Models.TransportData;

namespace RouteWise.BLL.Data
{
    public class AppDBContext
    {
        public List<TransportStop> Stops { get; private set; }
        public List<Transport> Transports { get; private set; }

        public AppDBContext(string stopsFilePath, string transportsFilePath)
        {
            Stops = LoadStops(stopsFilePath);
            Transports = LoadTransports(transportsFilePath, Stops);
        }

        private List<TransportStop> LoadStops(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var jsonReader = new JsonTextReader(reader);
            var stopsPreview = JsonSerializer.Create().Deserialize<List<TransportStopPreview>>(jsonReader);

            var stopDict = new Dictionary<string, TransportStop>();

            foreach (var preview in stopsPreview)
            {
                stopDict[preview.Name] = new TransportStop
                {
                    Name = preview.Name,
                    Transport = new List<Transport>(),
                    Neighbours = new List<NeighbourStopLink>()
                };
            }

            foreach (var preview in stopsPreview)
            {
                var currentStop = stopDict[preview.Name];

                foreach (var neighbour in preview.Neighbours)
                {
                    if (stopDict.TryGetValue(neighbour.Name, out var neighbourStop))
                    {
                        currentStop.Neighbours.Add(new NeighbourStopLink
                        {
                            Stop = neighbourStop,
                            Distance = neighbour.Distance
                        });
                    }
                }
            }

            return stopDict.Values.ToList();
        }

        private List<Transport> LoadTransports(string filePath, List<TransportStop> allStops)
        {
            using var reader = new StreamReader(filePath);
            using var jsonReader = new JsonTextReader(reader);
            var transportPreviews = JsonSerializer.Create().Deserialize<List<TransportPreview>>(jsonReader);

            var stopDict = allStops.ToDictionary(s => s.Name, s => s);

            var transports = new List<Transport>();

            foreach (var preview in transportPreviews)
            {
                var transport = new Transport
                {
                    RouteId = preview.RouteId,
                    Type = preview.Type,
                    Stops = new List<TransportStop>()
                };

                foreach (var stopName in preview.Stops)
                {
                    if (stopDict.TryGetValue(stopName, out var stop))
                    {
                        transport.Stops.Add(stop);
                        stop.Transport.Add(transport);
                    }
                }

                transports.Add(transport);
            }

            return transports;
        }
    }
}
