using System;
using System.Collections.Generic;

namespace CargoHubWeb.Models
{
    public partial class Order
    {
        public string Number { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Weight { get; set; }
        public int ToDepot { get; set; }
        public int FromDepot { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = null!;
        public int EmployeeNum { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Employee EmployeeNumNavigation { get; set; } = null!;
        public virtual Depot FromDepotNavigation { get; set; } = null!;
        public virtual Depot ToDepotNavigation { get; set; } = null!;
    }
}
