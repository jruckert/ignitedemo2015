namespace Ignite.Entities
{
    using System;

    public class Ticket : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual DateTime DateSubmitted { get; set; }

        public virtual string Description { get; set; }

        public virtual int TicketStatusId { get; set; }

        public virtual int CustomerId { get; set; }

        public virtual TicketStatus Status { get; set; }

        public virtual Customer Customer { get; set; }

    }
}
