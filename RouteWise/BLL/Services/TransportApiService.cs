using RouteWise.BLL.Interfaces;
using RouteWise.Models.TransportData;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using RouteWise.Models.TransportRoute;

namespace RouteWise.BLL.Services
{
    public class TransportApiService : ITransportApiService
    {
        public TransportApiService()
        {
        }

        public async Task<List<TransportRoute>> GetAllRoutesAsync(string origin, string destination, string city)
        {
            throw new NotImplementedException();
        }

        public async Task<TransportRoute> GetFastestRouteAsync(string origin, string destination, string city)
        {
            throw new NotImplementedException();
        }
    }
}
