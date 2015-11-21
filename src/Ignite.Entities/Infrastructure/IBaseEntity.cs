using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignite.Entities
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        int Version { get; set; }
        DateTime CreationDate { get; set; }
        DateTime? ModificationDate { get; set; }
    }
}
