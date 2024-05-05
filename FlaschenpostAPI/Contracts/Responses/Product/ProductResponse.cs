using FlaschenpostAPI.Data.Models;

namespace FlaschenpostAPI.Contracts.Responses.General
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string? BrandName { get; set; }
        public string? Name { get; set; }

        public string? DescriptionText { get; set; }

        public Article? Article { get; set; }

    }
}
