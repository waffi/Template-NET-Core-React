using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreReact.Business.Models
{
    public interface IHasAudit
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

}
