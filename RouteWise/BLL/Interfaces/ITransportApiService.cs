using RouteWise.Models.Transport;

namespace RouteWise.BLL.Interfaces
{
    public interface ITransportApiService
    {
        Task<TransportRoute> GetFastestRouteAsync(string origin, string destination);
        Task<List<TransportRoute>> GetAllRoutesAsync(string origin, string destination);
        Task<Transport> GetTransportFromRouteAsync(TransportRoute route);
    }
}
