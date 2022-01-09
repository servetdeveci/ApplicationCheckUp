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
using Microsoft.Extensions.Logging;

namespace ApplicationHealth.Services.Managers
{
    public class AppDefManager : IAppDefService
    {
        private readonly IAppDefRepository _appDefRepository;
        private readonly IAppUnitOfWork _unitOfWork;
        private readonly ILogger<AppDefManager> _logger;

        public AppDefManager(IAppDefRepository appDefRepository, IAppUnitOfWork unitOfWork, ILogger<AppDefManager> logger)
        {
            _appDefRepository = appDefRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public WebUIToast Add(AppDef app)
        {
            try
            {
                _appDefRepository.Add(app);
                _unitOfWork.Commit();
                _logger.LogTrace($"{CrudTwinProperty.CREATE} ==> AppDef: {app.Name} eklendi");

                return new WebUIToast
                {
                    header = "Başarılı",
                    icon = "success",
                    message = $"{app.Name} kaydı oluşturuldu"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.CREATE} ==> {app.Name} Ex: {ex.Message}");
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
                var deleted = GetById(id);
                _appDefRepository.Delete(deleted);
                _unitOfWork.CommitAsync();
                _logger.LogTrace($"{CrudTwinProperty.DELETE} ==> AppDefId: {id} silindi");

                return new WebUIToast
                {
                    header = "Başarılı",
                    icon = "success",
                    message = $"{deleted.Name} Silindi"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.DELETE} ==> AppDefId: {id} Ex: {ex.Message}");
                return new WebUIToast
                {
                    header = "Başarısız",
                    icon = "error",
                    message = "Silinirken bir istisna oluştu"
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
            return _appDefRepository.GetById(id);
        }

        public WebUIToast Update(AppDef app)
        {
            try
            {
                var existing = GetById(app.AppDefId);
                existing.UpdatedBy = "UpdatedUser";
                existing.UpdatedDate = DateTime.Now;
                existing.Name = app.Name;
                existing.Url = app.Url;
                existing.Interval = app.Interval;

                _appDefRepository.Update(existing);
                _unitOfWork.Commit();
                _logger.LogTrace($"{CrudTwinProperty.UPDATE} ==> AppDefId: {app.AppDefId} güncellendi");

                return new WebUIToast
                {
                    header = "Başarılı",
                    icon = "success",
                    message = $"{existing.Name} Güncellendi"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.UPDATE} ==> AppDefId: {app.AppDefId} Ex: {ex.Message}");
                return new WebUIToast
                {
                    header = "Başarısız",
                    icon = "error",
                    message = "Güncellerken bir istisna oluştu"
                };
            }
           

        }
    }
}
