using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Domain.Base
{
    public interface ICreateEntity
    {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
    }
}
