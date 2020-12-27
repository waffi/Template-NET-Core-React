using Microsoft.EntityFrameworkCore;
using NetCoreReact.Business.Models.Entity;
using NetCoreReact.Business.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreReact.Test.Mock
{
    public class RoleMock
    {
        public static void Generate(UnitOfWork unitofwork)
        {
            var role = new Role()
            {
                Name = "Administrator",
                Code = "ADMIN",
            };
            unitofwork.RoleRepository.Add(role);

            // Set explicit Id
            role.Id = Guid.Parse("55201968-F7A4-481B-991A-92E69383F372");

            unitofwork.SaveChanges();
        }
    }
}
