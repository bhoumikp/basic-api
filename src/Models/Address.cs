using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace basic_api.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? AddressLine { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        [ForeignKey("Entity")]
        public string? EntityId { get; set; }

        public Entity? Entity { get; set; }
    }
}