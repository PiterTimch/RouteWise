using RouteWise.BLL.Interfaces;
using RouteWise.Models.TransportData;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using RouteWise.Models.TransportRoute;
using RouteWise.BLL.Data;

namespace RouteWise.BLL.Services
{
    public class TransportApiService : ITransportApiService
    {
        private readonly AppDBContext _context;
        private readonly RouteGraphService _routeService;

        public TransportApiService()
        {
            string basePath = Directory.GetCurrentDirectory();
            string stopsPath = Path.Combine(basePath, "BLL", "Data", "Json", "TransportStops.json");
            string transportsPath = Path.Combine(basePath, "BLL", "Data", "Json", "AllTransport.json");

            _context = new AppDBContext(stopsPath, transportsPath);
            _routeService = new RouteGraphService(_context.Stops, _context.Transports);
        }

        public async Task<TransportRoute> GetFastestRouteAsync(string origin, string destination)
        {
            var path = _routeService.FindShortestPath(origin, destination);
            return path == null ? null : BuildRouteFromPath(path, origin, destination);
        }

        public async Task<List<TransportRoute>> GetAllRoutesAsync(string origin, string destination)
        {
            var paths = _routeService.FindAllRoutes(origin, destination);
            return paths.Select(path => BuildRouteFromPath(path, origin, destination)).ToList();
        }

        private TransportRoute BuildRouteFromPath(List<TransportStop> path, string origin, string destination)
        {
            var points = new List<RoutePoint>();
            string currentTransport = null;

            for (int i = 0; i < path.Count; i++)
            {
                var stop = path[i];
                var nextStop = i < path.Count - 1 ? path[i + 1] : null;

                var transport = stop.Transport
                    .FirstOrDefault(t => t.Stops.Contains(nextStop));

                bool isTransplantation = false;

                if (currentTransport != null && transport?.RouteId != currentTransport)
                {
                    isTransplantation = true;
                }

                currentTransport = transport?.RouteId ?? currentTransport;

                points.Add(new RoutePoint
                {
                    StopName = stop.Name,
                    Transport = currentTransport,
                    IsTransplantation = isTransplantation,
                    IsFinish = i == path.Count - 1
                });
            }

            int totalDistance = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                var current = path[i];
                var next = path[i + 1];
                var link = current.Neighbours.FirstOrDefault(n => n.Stop.Name == next.Name);
                if (link != null)
                {
                    totalDistance += link.Distance;
                }
            }

            return new TransportRoute
            {
                Origin = origin,
                Destination = destination,
                Distance = totalDistance,
                RoutePoints = points
            };
        }
    }
}
