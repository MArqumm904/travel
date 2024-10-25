using System.ComponentModel.DataAnnotations.Schema;

namespace travel
{
    public class guideimg
    {
        public int GudieId { get; set; }

        public string? GuideName { get; set; }

        public string? GuideDestination { get; set; }

        [NotMapped]
        public IFormFile? GuidePhoto { get; set; }

   
    }
}
