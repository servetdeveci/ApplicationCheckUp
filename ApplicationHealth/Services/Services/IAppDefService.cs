using ApplicationHealth.Domain.DataTable;
using ApplicationHealth.Domain.DataTable.Base;
using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.ViewModels;
using System;
using System.Linq.Expressions;

namespace ApplicationHealth.Services.Services
{
    public interface IAppDefService
    {
       
        WebUIToast Add(AppDef app);
        WebUIToast Update(string name, string url, short interval);
        AppDef GetByFilter(Expression<Func<AppDef, bool>> predicate);
        AppDef GetById(int id);
        WebUIToast Delete(int id);
        AppDefDataTable GetAppDefDataTable(BaseFilterParameters filters);
        

    }
}
