using ApplicationHealth.Domain.DataTable;
using ApplicationHealth.Domain.DataTable.Base;
using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationHealth.Services.Services
{
    public interface IAppDefService
    {
       
        WebUIToast Add(AppDef app);
        WebUIToast Update(AppDef app);
        bool UpdateAppStatus(int id, DateTime now, bool isUp);
        AppDef GetByFilter(Expression<Func<AppDef, bool>> predicate);
        AppDef GetById(int id);
        List<AppDef> GetAll(Expression<Func<AppDef, bool>> predicate = null);
        WebUIToast Delete(int id);
        AppDefDataTable GetAppDefDataTable(BaseFilterParameters filters);
        Task CheckAppIsUp(AppDef item);
        Task<WebUIToast> CheckAppIsUp(int id);
        AppDetailDTO GetAppDetail(int id);
    }
}
