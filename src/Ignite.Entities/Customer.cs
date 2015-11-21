namespace Ignite.Entities
{
    using System.Collections.Generic;

    public class Customer : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual string Address { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
