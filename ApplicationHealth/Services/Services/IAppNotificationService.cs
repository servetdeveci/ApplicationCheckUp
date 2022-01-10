using ApplicationHealth.Domain.DataTable;
using ApplicationHealth.Domain.DataTable.Base;
using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.ViewModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationHealth.Services.Services
{
    public interface IAppNotificationService
    {
        WebUIToast Add(AppNotification app);
        AppNotification GetByFilter(Expression<Func<AppNotification, bool>> predicate);
        AppNotification GetById(int id);
        WebUIToast Delete(int id);
        Task SendNotification(AppDef app);
        AppNotificationDataTable GetNotificationDataTable(BaseFilterParameters filters);

        
    }
}
