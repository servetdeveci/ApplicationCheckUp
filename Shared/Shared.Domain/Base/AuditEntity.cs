using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Domain.Base
{
    public abstract class AuditEntity : EntityBase, IAuditEntity
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsUpdated { get; set; } 
        public bool IsDeleted {get; set; }
        public DateTime DeletedDate {get; set; }
        public string  DeletedBy {get; set; }

        public void SetCreator(string creator, DateTime dateTime)
        {
            this.CreatedBy = creator;
            this.CreatedDate = dateTime;
        }
        public void SetUpdater(string updater, DateTime dateTime)
        {
            this.UpdatedBy = updater;
            this.UpdatedDate = dateTime;
        }
    }
}
