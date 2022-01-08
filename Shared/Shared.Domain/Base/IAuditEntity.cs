using System;

namespace Shared.Domain.Base
{
    public interface IAuditEntity : IEntityBase, IUpdateEntity, IDeleteEntity, ICreateEntity
    {}
}
