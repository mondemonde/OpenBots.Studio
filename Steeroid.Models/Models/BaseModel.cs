using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommonLib.Model
{
    /// <summary>
    /// Base class all entities derive from this class.
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseModel"/> class.
        /// </summary>
        public BaseModel()
        {
            //this.Id = Guid.NewGuid();
            this.Created = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Required]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        //[Required]
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the modification date.
        /// </summary>        
        public DateTime? Modified { get; set; }
    }
}
