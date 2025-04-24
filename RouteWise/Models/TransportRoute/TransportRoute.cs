using System.Text;

namespace RouteWise.Models.TransportRoute
{
    public class TransportRoute
    {
        public List<RoutePoint> RoutePoints { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Distance { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Маршрут з \"{Origin}\" до \"{Destination}\"");
            sb.AppendLine($"Загальна відстань: {Distance} м");
            sb.AppendLine("Зупинки:");

            foreach (var point in RoutePoints)
            {
                var marker = point.IsFinish ? "🏁" : point.IsTransplantation ? "🔁" : "➡️";
                sb.AppendLine($"{marker} {point.StopName} (транспорт: {point.Transport})");
            }

            return sb.ToString();
        }

    }
}
