using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ApplicationHealth.Services.Services
{
    public interface IAppContactService
    {
        WebUIToast Add(AppContact app);
        AppContact GetByFilter(Expression<Func<AppContact, bool>> predicate);
        List<AppContact> GetAll(Expression<Func<AppContact, bool>> predicate = null);
        AppContact GetById(int id);
        WebUIToast Delete(int id);
    }
}
