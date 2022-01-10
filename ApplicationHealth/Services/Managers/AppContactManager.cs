using ApplicationHealth.Domain;
using ApplicationHealth.Domain.Entities;
using ApplicationHealth.Domain.EntityInterfaces;
using ApplicationHealth.Domain.ViewModels;
using ApplicationHealth.Services.Services;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ApplicationHealth.Services.Managers
{
    public class AppContactManager : IAppContactService
    {
        private readonly IAppContactRepository _appContactRepository;
        private readonly IAppUnitOfWork _unitOfWork;
        private readonly ILogger<AppContactManager> _logger;

        public AppContactManager(IAppContactRepository appContactRepository, IAppUnitOfWork unitOfWork, ILogger<AppContactManager> logger)
        {
            _appContactRepository = appContactRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public WebUIToast Add(AppContact cont)
        {
            try
            {
                _appContactRepository.Add(cont);
                if (_unitOfWork.Commit() > 0)
                {
                    return new WebUIToast
                    {
                        header = "Başarılı",
                        icon = "success",
                        message = "Uygulama bildirim kişişi eklendi"
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
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.CREATE} ==> ContactId: {cont.NotificationContactName} oluşturulurken hata oluştu. ex: {ex}");

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
                _appContactRepository.Delete(GetById(id));
                if (_unitOfWork.Commit() > 0)
                {
                    return new WebUIToast
                    {
                        header = "Başarılı",
                        icon = "success",
                        message = "Kişi Silindi"
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
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.DELETE} ==> ContactId: {id} silinirken hata oluştu. ex: {ex}");

                return new WebUIToast
                {
                    header = "Başarısız",
                    icon = "error",
                    message = "Silinirken bir istisna oluştu"
                };
            }
        }
        public List<AppContact> GetAll(Expression<Func<AppContact, bool>> predicate = null)
        {
            try
            {
                return _appContactRepository.GetAll(predicate);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.GET} ==> GetAll  ex: {ex}");
                return null;
            }
        }
        public AppContact GetByFilter(Expression<Func<AppContact, bool>> predicate)
        {
            try
            {
                return _appContactRepository.Get(predicate);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.GET} ==> GetByFilters  ex: {ex}");
                return null;
            }
        }
        public AppContact GetById(int id)
        {
            try
            {
                return _appContactRepository.GetById(id);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{CrudTwinProperty.GET} ==> GetById  ex: {ex}");
                return null;
            }
        }
    }
}
