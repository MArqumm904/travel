using System.ComponentModel.DataAnnotations.Schema;

namespace travel
{
    public class adddest
    {
        public int DestinationId { get; set; }

        public string? DestinationCountry { get; set; }

        public int? DestinationPrice { get; set; }


        [NotMapped]
        public IFormFile? DestinationPhoto { get; set; }

  

        public string? DestinationGuider { get; set; }

        public DateTime? DestinationDate1 { get; set; }

        public DateTime? DestinationDate2 { get; set; }

        public DateTime? DestinationDate3 { get; set; }

        public bool? ActiveDestination { get; set; }

        public bool? PopularDestination { get; set; }
    }
}
