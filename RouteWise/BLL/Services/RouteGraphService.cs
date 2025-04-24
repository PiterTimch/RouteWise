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
            var allRoutes = new List<List<TransportStop>>();
            var visited = new HashSet<string>();
            var path = new List<TransportStop>();
            var start = _stops.FirstOrDefault(s => s.Name == origin);
            var end = _stops.FirstOrDefault(s => s.Name == destination);

            if (start == null || end == null) return allRoutes;

            DFS(start, end, visited, path, allRoutes);
            return allRoutes;
        }

        private void DFS(
            TransportStop current,
            TransportStop destination,
            HashSet<string> visited,
            List<TransportStop> path,
            List<List<TransportStop>> allRoutes)
        {
            visited.Add(current.Name);
            path.Add(current);

            if (current == destination)
            {
                allRoutes.Add(new List<TransportStop>(path));
            }
            else
            {
                foreach (var neighbourLink in current.Neighbours)
                {
                    var nextStop = neighbourLink.Stop;
                    if (!visited.Contains(nextStop.Name))
                    {
                        DFS(nextStop, destination, visited, path, allRoutes);
                    }
                }
            }

            visited.Remove(current.Name);
            path.RemoveAt(path.Count - 1);
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
