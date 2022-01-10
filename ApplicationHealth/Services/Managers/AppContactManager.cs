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
using System.Collections.Generic;

namespace ApplicationHealth.Services.Managers
{
    public class AppContactManager : IAppContactService
    {
        private readonly IAppContactRepository _appContactRepository;
        private readonly IAppUnitOfWork _unitOfWork;

        public AppContactManager(IAppContactRepository AppContactRepository, IAppUnitOfWork unitOfWork)
        {
            _appContactRepository = AppContactRepository;
            _unitOfWork = unitOfWork;
        }

        public WebUIToast Add(AppContact app)
        {
            try
            {
                _appContactRepository.Add(app);
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
            catch (Exception)
            {
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
            return _appContactRepository.GetAll(predicate);
        }

        public AppContact GetByFilter(Expression<Func<AppContact, bool>> predicate)
        {
            return _appContactRepository.Get(predicate);

        }

        public AppContact GetById(int id)
        {
            return _appContactRepository.GetById(id);
        }
    }
}
