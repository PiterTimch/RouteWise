using Microsoft.AspNetCore.Mvc;
using RouteWise.BLL.Data;
using RouteWise.BLL.Interfaces;
using RouteWise.Models.ViewModels;
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

            var routes = await service.GetAllRoutesAsync(model.Origin, model.Destination);
            ViewBag.Routes = routes;

            return View(model);
        }
    }
}
