using Microsoft.AspNetCore.Mvc;
using RouteWise.BLL.Interfaces;
using System.Threading.Tasks;

namespace RouteWise.Controllers
{
    public class TransportRouteController (ITransportApiService service) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetFastestRoute()
        {
            var route = await service.GetFastestRouteAsync("центр", "вокзал", "Ternopil");
            ViewBag.Route = route;
            return View("Index");
        }
    }
}
