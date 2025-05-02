using RouteWise.Models.TransportData;
using System.Collections.Generic;
using System.Linq;

namespace RouteWise.BLL.Services
{
    public class RouteGraphService
    {
        private readonly List<TransportStop> _stops;
        private readonly List<Transport> _transports;

        public RouteGraphService(List<TransportStop> stops, List<Transport> transports)
        {
            _stops = stops;
            _transports = transports;
        }


        public List<List<TransportStop>> FindAllRoutes(string origin, string destination)
        {
            var start = _stops.FirstOrDefault(s => s.Name == origin);
            var end = _stops.FirstOrDefault(s => s.Name == destination);

            if (start == null || end == null)
                return new List<List<TransportStop>>();

            var allRoutes = new List<List<TransportStop>>();
            var currentPath = new List<TransportStop>();
            var visited = new HashSet<TransportStop>();

            void DFS(TransportStop current)
            {
                visited.Add(current);
                currentPath.Add(current);

                if (current == end)
                {
                    allRoutes.Add(new List<TransportStop>(currentPath));
                }
                else
                {
                    if (current.Neighbours.Count == 0)
                    {

                    }

                    foreach (var link in current.Neighbours)
                    {
                        if (!visited.Contains(link.Stop))
                        {
                            DFS(link.Stop);
                        }
                    }
                }

                visited.Remove(current);
                currentPath.RemoveAt(currentPath.Count - 1);
            }

            DFS(start);
            return allRoutes;
        }

        public List<TransportStop> FindShortestPath(string origin, string destination)
        {
            var start = _stops.FirstOrDefault(s => s.Name == origin);
            var end = _stops.FirstOrDefault(s => s.Name == destination);

            if (start == null || end == null) return null;

            // Алгоритм Дейкстри
            var distances = new Dictionary<TransportStop, int>();
            var previous = new Dictionary<TransportStop, TransportStop>();
            var queue = new PriorityQueue<TransportStop, int>();

            foreach (var stop in _stops)
            {
                distances[stop] = int.MaxValue;
                previous[stop] = null;
            }

            distances[start] = 0;
            queue.Enqueue(start, 0);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var link in current.Neighbours)
                {
                    var neighbor = link.Stop;
                    var alt = distances[current] + link.Distance;

                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = current;
                        queue.Enqueue(neighbor, alt);
                    }
                }
            }

            // Відновлення шляху
            var path = new List<TransportStop>();
            var curr = end;

            while (curr != null)
            {
                path.Insert(0, curr);
                curr = previous[curr];
            }

            return path.Count > 0 && path[0] == start ? path : null;
        }
    }
}
