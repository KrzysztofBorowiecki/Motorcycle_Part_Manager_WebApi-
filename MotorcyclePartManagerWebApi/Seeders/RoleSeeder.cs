using Microsoft.EntityFrameworkCore;
using MotorcyclePartManagerWebApi.Data;
using MotorcyclePartManagerWebApi.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MotorcyclePartManagerWebApi.Seeders
{
    public class RoleSeeder
    {
        private readonly ProjectContext _context;
        public RoleSeeder(ProjectContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Database.CanConnect())
            {
                var pendingMigrations = _context.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _context.Database.Migrate();
                }

                if (!_context.Roles.Any())
                {
                    var roles = GetRoles();
                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Admin"
                },
                //new Role()
                //{
                //    Name = "Admin"
                //},
            };

            return roles;
        }
    }
}
