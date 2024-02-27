using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace basic_api.Models
{
    public class Entity : IEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool Deceased { get; set; }
        public string? Gender { get; set; }
        public List<Address>? Addresses { get; set; } = new List<Address>();
        public List<Date> Dates { get; set; } = new List<Date>();
        public List<Name> Names { get; set; } = new List<Name>();
    }
}