using System;
using System.Collections.Generic;

namespace CargoHubWeb.Models
{
    public partial class Depot
    {
        public Depot()
        {
            OrderFromDepotNavigations = new HashSet<Order>();
            OrderToDepotNavigations = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Address { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public virtual ICollection<Order> OrderFromDepotNavigations { get; set; }
        public virtual ICollection<Order> OrderToDepotNavigations { get; set; }
    }
}
