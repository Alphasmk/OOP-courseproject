using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using gamelauncher.ViewModels;

namespace gamelauncher.Model
{
    public class UserContext : DbContext
    {
        public UserContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Phones { get; set; }
    }

}
