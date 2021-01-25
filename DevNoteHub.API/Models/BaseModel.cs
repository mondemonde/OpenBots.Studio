using System;
using System.ComponentModel.DataAnnotations;

namespace DevNoteHub.Models
{
    public abstract class BaseModel
    {
        public BaseModel()
        {
            Created = DateTime.Now;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}