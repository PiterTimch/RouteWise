using RouteWise.Models.TransportData;
using System.Text.Json;

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
            var json = File.ReadAllText(filePath);
            var stops = JsonSerializer.Deserialize<List<TransportStopPreview>>(json);

            var stopDict = new Dictionary<string, TransportStop>();

            foreach (var stop in stops)
            {
                stopDict[stop.Name] = new TransportStop
                {
                    Name = stop.Name,
                    Transport = new List<Transport>(),
                    Next = new List<TransportStop>()
                };
            }

            foreach (var stop in stops)
            {
                var currentStop = stopDict[stop.Name];
                foreach (var neighbour in stop.Neighbours)
                {
                    if (stopDict.TryGetValue(neighbour.Name, out var neighbourStop))
                    {
                        currentStop.Next.Add(neighbourStop);
                    }
                }
            }

            return stopDict.Values.ToList();
        }

        private List<Transport> LoadTransports(string filePath, List<TransportStop> allStops)
        {
            var json = File.ReadAllText(filePath);
            var transportPreviews = JsonSerializer.Deserialize<List<TransportPreview>>(json);

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
