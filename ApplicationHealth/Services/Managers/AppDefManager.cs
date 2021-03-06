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
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHealth.Services.Managers
{
    public class AppDefManager : IAppDefService
    {
        private readonly IAppDefRepository _appDefRepository;
        private readonly IAppNotificationService _appNotificationService;
        private readonly IAppUnitOfWork _unitOfWork;
        private readonly ILogger<AppDefManager> _logger;

        public AppDefManager(IAppDefRepository appDefRepository, IAppNotificationService appNotificationService, IAppUnitOfWork unitOfWork, ILogger<AppDefManager> logger)
        {
            _appDefRepository = appDefRepository;
            _appNotificationService = appNotificationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public WebUIToast Add(AppDef app)
        {
            try
            {
                if (ValidateApp(app, out WebUIToast toast))
                    return toast;

                _appDefRepository.Add(app);
                _unitOfWork.Commit();
                _logger.LogTrace($"{CrudTwinProperty.CREATE} ==> AppDef: {app.Name} eklendi");
                return new WebUIToast
                {
                    header = "Başarılı",
                    icon = "success",
                    message = $"{app.Name} kaydı oluşturuldu",
                    data = app.AppDefId
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
        private bool ValidateApp(AppDef app, out WebUIToast mes)
        {
            mes = new WebUIToast
            {
                header = "Başarısız",
                icon = "error",
                message = ""
            };
            var validate = string.IsNullOrEmpty(app.Url) || string.IsNullOrEmpty(app.Name);
            if (validate)
                mes.message = "Alanlar boş olamaz. ";
            if (app.Interval < 1)
                mes.message += "Interval 1 den küçük olamaz";

            Uri uriResult;
            validate = !(Uri.TryCreate(app.Url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps));
            if (validate)
                mes.message += "Url doğru formatta değil";
            return validate;
        }
        public WebUIToast Delete(int id)
        {
            try
            {
                var deleted = GetById(id);
                _appDefRepository.Delete(deleted);
                var abc = _unitOfWork.Commit();
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
            try
            {
                return _appDefRepository.Get(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.GET} ==> GetByFilter  ex: {ex}");
                return null;
            }
        }
        public AppDef GetById(int id)
        {
            try
            {
                return _appDefRepository.GetById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.GET} ==> GetById  ex: {ex}");
                return null;
            }
        }
        public WebUIToast Update(AppDef app)
        {
            try
            {
                if (ValidateApp(app, out WebUIToast toast))
                    return toast;

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
                    message = $"{existing.Name} Güncellendi",
                    data = app.AppDefId
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
        public List<AppDef> GetAll(Expression<Func<AppDef, bool>> predicate = null)
        {
            try
            {
                return _appDefRepository.GetAll(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.GET} ==> GetAll predicate: {predicate.ToString()} Ex: {ex.Message}");
                return null;
            }
        }
        public bool UpdateAppStatus(int id, DateTime now, bool isUp)
        {
            try
            {
                var existing = GetById(id);
                existing.LastControlDateTime = now;
                existing.IsUp = isUp;
                _appDefRepository.Update(existing);
                _unitOfWork.Commit();
                _logger.LogTrace($"{CrudTwinProperty.UPDATE} ==> AppDefId: {id} güncellendi");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.UPDATE} ==> AppDefId: {id} güncellenirken hata oluştu. ex: {ex}");
                return false;
            }
        }
        public async Task CheckAppIsUp(AppDef app)
        {
            try
            {
                var interval = (DateTime.Now - app.LastControlDateTime).TotalMinutes;
                if (interval > app.Interval)
                {
                    var _httpClient = new HttpClient();
                    var response = await _httpClient.GetAsync(app.Url);
                    if (!response.IsSuccessStatusCode)
                        await _appNotificationService.SendNotification(app);
                    var res = UpdateAppStatus(app.AppDefId, DateTime.Now, response.IsSuccessStatusCode);
                    _httpClient.Dispose();
                }

            }
            catch (Exception ex)
            {
                var res = UpdateAppStatus(app.AppDefId, DateTime.Now, false);
                await _appNotificationService.SendNotification(app);
                Console.WriteLine($"Name: {app.Name} Response: {ex.Message}");
            }
        }
        public async Task<WebUIToast> CheckAppIsUp(int id)
        {
            var item = GetById(id);

            try
            {
                var _httpClient = new HttpClient();
                var response = await _httpClient.GetAsync(item.Url);
                var res = UpdateAppStatus(item.AppDefId, DateTime.Now, response.IsSuccessStatusCode);
                _httpClient.Dispose();

                return new WebUIToast
                {
                    header = "Başarılı",
                    icon = "success",
                    message = $"{item.Name} uygulama check edildi"
                };

            }
            catch (Exception ex)
            {
                UpdateAppStatus(item.AppDefId, DateTime.Now, false);
                _logger.LogError($"{CrudTwinProperty.CHECK} ==> AppDefId: {item.AppDefId} Ex: {ex.Message}");
                return new WebUIToast
                {
                    header = "Başarısız",
                    icon = "error",
                    message = "Uygulama ayakta değil"
                };
            }
        }
        public AppDetailDTO GetAppDetail(int id)
        {
            try
            {
                var model = new AppDetailDTO
                {
                    App = GetById(id),
                    Contacts = _appDefRepository.Table.Where(m => m.AppDefId == id).Include(m=> m.NotificationContacts).FirstOrDefault().NotificationContacts.ToList(),
                    Notifications = _appDefRepository.Table.Where(m => m.AppDefId == id).Include(m => m.Notifications).FirstOrDefault().Notifications.OrderByDescending(m=> m.SentDateTime).ToList(),
                };
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.GET} ==> GetAppDetail  ex: {ex}");
                return null;
            }
        }
    }
}
