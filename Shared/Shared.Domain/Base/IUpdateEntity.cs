using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Domain.Base
{
    public interface IUpdateEntity
    {
        bool IsUpdated { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }
    }
}
