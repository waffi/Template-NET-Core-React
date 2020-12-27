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
                Id = Guid.Parse("55201968-F7A4-481B-991A-92E69383F372"),
                Name = "Administrator",
                Code = "ADMIN",
            };
            unitofwork.Context.Role.Add(role);

            unitofwork.SaveChanges();
        }
    }
}
