using System.ComponentModel.DataAnnotations;

namespace TodoApiNet.Models
{
    public class Todo
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public bool Done { get; set; } = false;
    }
}
