using ApplicationHealth.Domain.Entities;
using System.Collections.Generic;

namespace ApplicationHealth.Domain.ViewModels
{
    public class AppContactDTO
    {
        private List<AppContact> appContactList;
        private int appDefId;

        public AppContactDTO(int id, List<AppContact> appContacts)
        {
            appContactList = appContacts;
            appDefId = id;
        }
        public int AppDefId { get => appDefId; set => appDefId = value; }
        public List<AppContact> AppContactList { get => appContactList; set => appContactList = value; }
    }
}
