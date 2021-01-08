using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreReact.Business.Models.Entity;
using NetCoreReact.Business.UnitOfWork;
using NetCoreReact.Models.Request.User;
using NetCoreReact.Models.Response;
using NetCoreReact.Models.Response.User;
using NetCoreReact.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static NetCoreReact.Utils.Constants;

namespace NetCoreReact.Controllers
{
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/math")]
    public class MathController : ControllerBase
    {
        public class ModelRequest
        {
            [Required]
            public int Value1 { get; set; }

            [Required]
            public int Value2 { get; set; }
        }

        public MathController()
        {

        }

        [HttpPost()]
        public ActionResult<int> Plus([FromBody] ModelRequest model)
        {

            return Ok(model.Value1 + model.Value2);
        }


        [HttpPost()]
        public ActionResult<int> Minus([FromBody] ModelRequest model)
        {

            return Ok(model.Value1 - model.Value2);
        }
    }

}
