using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Auto_Barter.Models.Common;

namespace Auto_Barter.Models
{
    public class OurDbContext : DbContext
    {
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
    }
}