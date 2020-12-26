using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreReact.Business.Models
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
    }

}
