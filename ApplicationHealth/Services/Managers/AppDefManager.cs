using ApplicationHealth.Domain;
using ApplicationHealth.Domain.DataTable;
using ApplicationHealth.Domain.DataTable.Base;
using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.EntityInterfaces;
using ApplicationHealth.Domain.ViewModels;
using ApplicationHealth.Services.Services;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
namespace ApplicationHealth.Services.Managers
{
    public class AppDefManager : IAppDefService
    {
        private readonly IAppDefRepository _appDefRepository;
        private readonly IAppUnitOfWork _unitOfWork;

        public AppDefManager(IAppDefRepository appDefRepository, IAppUnitOfWork unitOfWork)
        {
            _appDefRepository = appDefRepository;
            _unitOfWork = unitOfWork;
        }

        public WebUIToast Add(AppDef app)
        {
            try
            {
                _appDefRepository.Add(app);
                _unitOfWork.CommitAsync();
                if (_unitOfWork.Commit() > 0)
                {
                    return new WebUIToast
                    {
                        header = "Başarılı",
                        icon = "success",
                        message = "Yeni App eklendi"
                    };
                }
                else
                {
                    return new WebUIToast
                    {
                        header = "Başarısız",
                        icon = "error",
                        message = "Veritabanına kayıt yapılamadı"
                    };
                }

            }
            catch (Exception)
            {
                return new WebUIToast
                {
                    header = "Başarısız",
                    icon = "error",
                    message = "Eklenirken bir istisna oluştu"
                };
            }
        }

        public WebUIToast Delete(int id)
        {
            try
            {
                _appDefRepository.Delete(GetById(id));
                _unitOfWork.CommitAsync();
                if (_unitOfWork.Commit() > 0)
                {
                    return new WebUIToast
                    {
                        header = "Başarılı",
                        icon = "success",
                        message = "Uygulama Silindi"
                    };
                }
                else
                {
                    return new WebUIToast
                    {
                        header = "Başarısız",
                        icon = "error",
                        message = "Veritabanına kayıt yapılamadı"
                    };
                }

            }
            catch (Exception)
            {
                return new WebUIToast
                {
                    header = "Başarısız",
                    icon = "error",
                    message = "Eklenirken bir istisna oluştu"
                };
            }
        }

        public AppDefDataTable GetAppDefDataTable(BaseFilterParameters filters)
        {

            IQueryable<AppDef> filteredData;
            Expression<Func<AppDef, bool>> expression = d => (string.IsNullOrEmpty(filters.mainFilter) || d.Name.ToLower().Contains(filters.mainFilter.ToLower()));
            var totalCount = _appDefRepository.CountAll();
            filteredData = _appDefRepository.Table.Where(expression);
            var filteredCount = filteredData.Count();

            var newModel = filteredData.OrderBy(filters.sortColumnName + " " + filters.sortColumnDirection)
                .Skip(filters.start).Take(filters.length).ToList();

            var model = new AppDefDataTable
            {
                data = newModel,
                draw = filters.draw,
                recordsFiltered = filteredCount,
                recordsTotal = totalCount
            };

            return model;
        }

        public AppDef GetByFilter(Expression<Func<AppDef, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AppDef GetById(int id)
        {
            throw new NotImplementedException();
        }

        public WebUIToast Update(string name, string url, ushort interval)
        {
            throw new NotImplementedException();
        }
    }
}
