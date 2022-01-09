using ApplicationHealth.Domain.Identity;
using ApplicationHealth.MvcWebUI.AccountModel.ManageModel;

namespace ApplicationHealth.MvcWebUI.ManageModel
{
    public class ProfileModel
    {
        public CustomIdentityUser User { get; set; }
        public ChangePasswordModel ChangePasswordModel { get; set; }
    }
}
