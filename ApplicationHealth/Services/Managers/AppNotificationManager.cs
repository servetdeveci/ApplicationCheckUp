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
    public class AppNotificationManager : IAppNotificationService
    {
        private readonly IAppNotificationRepository _AppNotificationRepository;
        private readonly IAppUnitOfWork _unitOfWork;

        public AppNotificationManager(IAppNotificationRepository AppNotificationRepository, IAppUnitOfWork unitOfWork)
        {
            _AppNotificationRepository = AppNotificationRepository;
            _unitOfWork = unitOfWork;
        }

        public WebUIToast Add(AppNotification app)
        {
            try
            {
                _AppNotificationRepository.Add(app);
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
                _AppNotificationRepository.Delete(GetById(id));
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

        public AppNotification GetByFilter(Expression<Func<AppNotification, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AppNotification GetById(int id)
        {
            throw new NotImplementedException();
        }

        public WebUIToast Update(string name, string url, ushort interval)
        {
            throw new NotImplementedException();
        }
    }
}
