using RouteWise.Models.TransportRoute;

namespace RouteWise.BLL.Interfaces
{
    public interface ITransportApiService
    {
        Task<TransportRoute> GetFastestRouteAsync(string origin, string destination);
        Task<List<TransportRoute>> GetAllRoutesAsync(string origin, string destination);
    }
}
