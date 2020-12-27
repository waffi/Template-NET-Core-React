using Microsoft.EntityFrameworkCore;
using NetCoreReact.Business.Models.Entity;
using NetCoreReact.Business.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreReact.Test.Mock
{
    public class UserMock
    {
        public static void Generate(UnitOfWork unitofwork)
        {
            var user = new User()
            {
                Id = Guid.Parse("0528BD60-3D92-43CC-BFB4-A0D117D65CB6"),
                Role = Guid.Parse("55201968-F7A4-481B-991A-92E69383F372"),
                Username = "admin",
                Salt = "Ur9GcEl+GPgO21bNCL+fkQ==",
                Password = "ot54/ZJrGMJMFt5jVLJPpQGz/+FgmR7Gt0m+62kWk4o=",
                CreatedBy = Guid.Parse("0528BD60-3D92-43CC-BFB4-A0D117D65CB6"),
                CreatedDate = DateTime.Parse("2020-12-27 00:00:00.000"),
                ModifiedBy = Guid.Parse("0528BD60-3D92-43CC-BFB4-A0D117D65CB6"),
                ModifiedDate = DateTime.Parse("2020-12-27 00:00:00.000"),
            };
            unitofwork.Context.User.Add(user);

            unitofwork.SaveChanges();
        }
    }
}
