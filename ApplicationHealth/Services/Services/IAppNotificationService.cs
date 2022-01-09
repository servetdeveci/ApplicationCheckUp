using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.ViewModels;
using System;
using System.Linq.Expressions;

namespace ApplicationHealth.Services.Services
{
    public interface IAppNotificationService
    {
        WebUIToast Add(AppNotification app);
        WebUIToast Update(string name, string url, ushort interval);
        AppNotification GetByFilter(Expression<Func<AppNotification, bool>> predicate);
        AppNotification GetById(int id);
        WebUIToast Delete(int id);
    }
}
