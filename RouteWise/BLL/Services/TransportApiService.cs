using RouteWise.BLL.Interfaces;
using RouteWise.Models.Transport;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace RouteWise.BLL.Services
{
    public class TransportApiService : ITransportApiService
    {
        public TransportApiService(City city)
        {
            _httpClient = new HttpClient();
            _city = city;
        }

        public async Task<List<TransportRoute>> GetAllRoutesAsync(string origin, string destination)
        {
            var query = $@"
[out:json][timeout:25];
{{{{geocodeArea:{_city.Name}}}}}->.searchArea;
(
  relation[""type""=""route""][""route""~""bus|tram|trolleybus""](area.searchArea);
);
out body;
>;
out skel qt;
";

            var content = new StringContent($"data={query}", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync("https://overpass-api.de/api/interpreter", content);

            if (!response.IsSuccessStatusCode)
                return new List<TransportRoute>();

            var json = await response.Content.ReadAsStringAsync();
            var parsed = JsonDocument.Parse(json);

            var routes = new List<TransportRoute>();

            foreach (var element in parsed.RootElement.GetProperty("elements").EnumerateArray())
            {
                if (element.GetProperty("type").GetString() == "relation")
                {
                    var tags = element.GetProperty("tags");
                    var name = tags.TryGetProperty("name", out var n) ? n.GetString() : "Unnamed";
                    var type = tags.TryGetProperty("route", out var t) ? t.GetString() : "unknown";

                    routes.Add(new TransportRoute
                    {
                        Id = element.GetProperty("id").GetRawText(),
                        Name = name,
                        TransportType = type
                    });
                }
            }

            return routes;
        }

        public async Task<TransportRoute> GetFastestRouteAsync(string origin, string destination)
        {
            // Для MVP: просто повернемо перший маршрут із GetAllRoutesAsync
            var allRoutes = await GetAllRoutesAsync(origin, destination);
            return allRoutes.FirstOrDefault(); // TODO: додати логіку пошуку найкоротшого
        }

        public async Task<Transport> GetTransportFromRouteAsync(TransportRoute route)
        {
            return new Transport
            {
                RouteId = route.Id,
                Type = route.TransportType,
                Operator = "Unknown"
            };
        }

        private readonly HttpClient _httpClient;
        private readonly City _city;
    }
}
