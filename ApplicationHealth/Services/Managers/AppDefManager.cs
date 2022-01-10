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
using ApplicationHealth.Domain.Enums;

namespace ApplicationHealth.Services.Managers
{
    public class AppDefManager : IAppDefService
    {
        private readonly IAppDefRepository _appDefRepository;
        private readonly IAppUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly ILogger<AppDefManager> _logger;

        public AppDefManager(IAppDefRepository appDefRepository, IAppUnitOfWork unitOfWork, IMailService mailService, ILogger<AppDefManager> logger)
        {
            _appDefRepository = appDefRepository;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
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
                        await SendNotification(app);
                    var res = UpdateAppStatus(app.AppDefId, DateTime.Now, response.IsSuccessStatusCode);
                    Console.WriteLine($"Name: {app.Name} Response: {response.StatusCode}");
                    _httpClient.Dispose();
                }

            }
            catch (Exception ex)
            {
                var res = UpdateAppStatus(app.AppDefId, DateTime.Now, false);
                Console.WriteLine($"Name: {app.Name} Response: {ex.Message}");
                _logger.LogError($"WorkerService ==> AppDefId: {app.AppDefId} güncellenirken hata oluştu");
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
        private async Task SendNotification(AppDef app)
        {
            foreach (var cont in GetAppNotificationContact(app.AppDefId))
            {
                switch (cont.NotificationType)
                {
                    case NotificationType.None:
                        break;
                    case NotificationType.Email:
                        await GenerateEmailAndSend(app, cont);
                        break;
                    case NotificationType.Sms:
                        break;
                    case NotificationType.EmailSms:
                        break;
                    default:
                        break;
                }
            }
        }
        private async Task GenerateEmailAndSend(AppDef app, AppContact item)
        {
            var email = new Email { toList = item.Email, subject = "App Check Up", content = app.Name + "is down. Last control-time " + app.LastControlDateTime.ToShortTimeString()  };
            await _mailService.SendMailViaSystemNetAsync(email);
        }
        private IEnumerable<AppContact> GetAppNotificationContact(int id)
        {
            return _appDefRepository.Table.FirstOrDefault(m => m.AppDefId == id).NotificationContacts;
        }
    }
}
