using System.ComponentModel.DataAnnotations;

namespace TodoMinimalAPI
{
    public class Todo
    {

        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public bool isComplete { get; set; }


    }
}
