using System;

namespace Shared.Domain.Base
{
    public abstract class DeleteEntity : EntityBase, IDeleteEntity
    {
        public bool IsDeleted { get;set; }
        public DateTime DeletedDate { get;set; }
        public string DeletedBy { get;set; }
    }
}
