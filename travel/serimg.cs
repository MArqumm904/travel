
using System.ComponentModel.DataAnnotations.Schema;

namespace travel
{
    public class serimg
    {
        public int ServiceId { get; set; }

        [NotMapped]
        public IFormFile? Photo { get; set; }



        public string? Title { get; set; }

        public string? Description { get; set; }



       
    }
}
