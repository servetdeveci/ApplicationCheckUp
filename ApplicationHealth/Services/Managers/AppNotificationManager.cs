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
using System.Threading.Tasks;
using ApplicationHealth.Domain.Enums;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHealth.Services.Managers
{
    public class AppNotificationManager : IAppNotificationService
    {
        private readonly IAppNotificationRepository _appNotificationRepository;
        private readonly IAppUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IAppContactService _appContactService;
        public AppNotificationManager(IAppNotificationRepository appNotificationRepository, IAppUnitOfWork unitOfWork, IMailService mailService, IAppContactService appContactService)
        {
            _appNotificationRepository = appNotificationRepository;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _appContactService = appContactService;
        }

        public WebUIToast Add(AppNotification app)
        {
            try
            {
                _appNotificationRepository.Add(app);
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
                _appNotificationRepository.Delete(GetById(id));
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
        public AppNotificationDataTable GetNotificationDataTable(BaseFilterParameters filters)
        {
            IQueryable<AppNotification> filteredData;
            Expression<Func<AppNotification, bool>> expression = d => (string.IsNullOrEmpty(filters.mainFilter) || d.Message.ToLower().Contains(filters.mainFilter.ToLower()));
            var totalCount = _appNotificationRepository.CountAll();
            filteredData = _appNotificationRepository.Table.Where(expression).Include(d => d.AppDef).Include(m => m.Contact);
            var filteredCount = filteredData.Count();

            var newModel = filteredData.OrderBy(filters.sortColumnName + " " + filters.sortColumnDirection)
                .Skip(filters.start).Take(filters.length).ToList();

            var model = new AppNotificationDataTable
            {
                data = newModel,
                draw = filters.draw,
                recordsFiltered = filteredCount,
                recordsTotal = totalCount
            };

            return model;
        }
        public AppNotification GetByFilter(Expression<Func<AppNotification, bool>> predicate)
        {
            return _appNotificationRepository.Get(predicate);
        }
        public AppNotification GetById(int id)
        {
            return _appNotificationRepository.GetById(id);
        }
        public async Task SendNotification(AppDef app)
        {
            var interval = (int)(DateTime.Now - app.LastNotificationDateTime).TotalMinutes;
            if (interval >= app.Interval)
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

        }
        private async Task GenerateEmailAndSend(AppDef app, AppContact cont)
        {
            var email = new Email { toList = cont.Email, subject = "App Check Up", content = $"{app.Name} ({app.Url})  is down.  Last control-time {app.LastControlDateTime.ToShortTimeString()}" };
            await _mailService.SendMailViaSystemNetAsync(email);
            var noti = new AppNotification
            {
                AppDefId = app.AppDefId,
                AppNotificationContactId = cont.AppNotificationContactId,
                Message = email.content,
                SentDateTime = DateTime.Now,
            };
            Add(noti);
        }
        private List<AppContact> GetAppNotificationContact(int id)
        {
            return _appContactService.GetAll(m => m.AppDefId == id);
        }


    }
}
