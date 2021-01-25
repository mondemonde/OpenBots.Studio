using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Steeroid.Models
{
    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public abstract class BaseModel
    {
        public BaseModel()
        {
            Created = DateTime.Now;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        //[Required]
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}