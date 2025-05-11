using Microsoft.AspNetCore.Mvc;
using RouteWise.BLL.Data;
using RouteWise.BLL.Interfaces;
using RouteWise.Models.TransportRoute;
using RouteWise.Models.ViewModels;
using RouteWise.Singletone;
using System.Threading.Tasks;

namespace RouteWise.Controllers
{
    public class TransportRouteController (ITransportApiService service, AppDBContext context) : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            var stops = context.Stops
                .Select(s => s.Name)
                .ToList();

            ViewBag.Stops = stops;

            return View(new RouteConfigViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(RouteConfigViewModel model)
        {
            var stops = context.Stops
                .Select(s => s.Name)
                .ToList();

            ViewBag.Stops = stops;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<TransportRoute> routes = new List<TransportRoute>();

            if (!model.IsFastestRoute)
            {
                routes = await service.GetAllRoutesAsync(model.Origin, model.Destination);
            }
            else
            {
                routes.Add(await service.GetFastestRouteAsync(model.Origin, model.Destination));
            }

            ViewBag.Routes = routes;
            Helper.Routes = routes;

            return View(model);
        }

        [HttpGet]
        public IActionResult BuildGraph(int id) 
        {
            var route = Helper.Routes.FirstOrDefault(r => r.Distance == id);

            return View(route);
        }
    }
}
