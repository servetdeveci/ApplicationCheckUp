using System;

namespace Shared.Domain.Base
{
    public interface IDeleteEntity
    {
        bool IsDeleted { get; set; }
        DateTime DeletedDate { get; set; }
        string DeletedBy { get; set; }
    }
}
