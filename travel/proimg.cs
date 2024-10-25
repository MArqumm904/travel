using System.ComponentModel.DataAnnotations.Schema;

namespace travel
{
    public class proimg
    {
        public int ProcessId { get; set; }

        public string? ProcessName { get; set; }

        public string? ProcessDescription { get; set; }

        [NotMapped]
        public IFormFile? ProcessPhoto { get; set; }

        
    }
}
