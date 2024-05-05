using Newtonsoft.Json;

namespace FlaschenpostAPI.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string ShortDescription { get; set; } = "";
        public double Price { get; set; }
        public string? Unit { get; set; }
        public string PricePerUnitText { get; set; } = "";
        public string? Image { get; set; }

        // Custom Property
        public double PricePerLitre { get; set; }
        public int Bottles { get; set; }

    }
}
