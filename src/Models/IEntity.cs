using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace basic_api.Models
{
    public interface IEntity
    {
        List<Address>? Addresses { get; set; }
        List<Date> Dates { get; set; }
        bool Deceased { get; set; }
        string? Gender { get; set; }
        string Id { get; set; }
        List<Name> Names { get; set; }
    }
}