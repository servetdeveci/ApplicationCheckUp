using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.ViewModels;
using System;
using System.Linq.Expressions;

namespace ApplicationHealth.Services.Services
{
    public interface IAppContactService
    {
        WebUIToast Add(AppContact app);
        WebUIToast Update(string name, string url, ushort interval);
        AppContact GetByFilter(Expression<Func<AppContact, bool>> predicate);
        AppContact GetById(int id);
        WebUIToast Delete(int id);
    }
}
