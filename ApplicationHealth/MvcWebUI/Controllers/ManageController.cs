using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using ApplicationHealth.MvcWebUI.AccountModel;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;
using ApplicationHealth.Services.Services;
using ApplicationHealth.Domain.Identity;
using ApplicationHealth.Domain.ViewModels;
using ApplicationHealth.MvcWebUI.ManageModel;
using ApplicationHealth.MvcWebUI.AccountModel.ManageModel;

namespace ApplicationHealth.MvcWebUI.Controllers
{
    [Authorize]
    public class ManageController : CustomBaseController
    {
        #region PROP - INJECTIONS
       
        private readonly IMailService _mailService;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly RoleManager<CustomIdentityRole> _roleManager;
        private readonly ILogger<ManageController> _logger;

        public ManageController(IMailService mailService, SignInManager<CustomIdentityUser> signInManager, UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, UrlEncoder urlEncoder, ILogger<ManageController> logger)
        {
           
            _mailService = mailService;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }


        #endregion

        #region MANAGE INDEX

        public IActionResult Index()
        {
            try
            {
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

                var model = new ProfileModel
                {
                    User = user,
                    ChangePasswordModel = new ChangePasswordModel()
                };

                return View(model);
            }
            catch (Exception)
            {
                return NotFound($"Hata Kullanıcı Adı'{_userManager.GetUserId(User)}'.");
            }
        }
        [HttpPost]
        public IActionResult Index(CustomIdentityUser indexModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            _signInManager.RefreshSignInAsync(user).RunSynchronously();
            return View(indexModel);
        }
        [HttpPost]
        public async Task<IActionResult> OnPostSendVerificationEmailAsync(IndexModel indexModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _mailService.SendMailViaSystemNetAsync(new Email
            {
                toList = email,
                subject = "E-Posta Doğrulama",
                content = $" <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Buraya </a> tıklayarak e-posta adresinizi güncelleyebilirsiniz."
            });

            indexModel.StatusMessage = "Doğrulama epostası belirttiğiniz e-posta adresine gönderildi.";
            return View(indexModel);
        }

        #endregion

        #region CHANGE PASSWORD

        public IActionResult ChangePassword()
        {

            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (user == null)
            {
                return NotFound($"Bu '{_userManager.GetUserId(User)}' Id ile kullanıcı bulanamadı .");
            }
            ChangePasswordModel changePasswordModel = new ChangePasswordModel { UserName = user.UserName };
            return View(changePasswordModel);

        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(changePasswordModel);
            }

            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword, changePasswordModel.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(changePasswordModel);
            }

            _logger.LogTrace($"{user.UserName} adlı kullanıcı kendi şifresini değiştirdi.");
            return View(changePasswordModel);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePasswordInProfile(ProfileModel profileModel)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            profileModel.User = user;

            if (!ModelState.IsValid)
            {
                return View("Index", profileModel);
            }

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, profileModel.ChangePasswordModel.OldPassword, profileModel.ChangePasswordModel.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View("Index", profileModel);
            }

            _logger.LogTrace($"{user.UserName} adlı kullanıcı kendi şifresini değiştirdi.");

            return View("Index", profileModel);
        }

        #endregion

        #region SET PASSWORD
        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return View("ChangePassword");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SetPassword(SetPasswordModel setPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, setPasswordModel.Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await _signInManager.RefreshSignInAsync(user);
            setPasswordModel.StatusMessage = "Your password has been set.";

            return View(setPasswordModel);
        }
        #endregion

        #region REGISTER

        [HttpGet]
        public async Task<IActionResult> Register(string returnUrl = null, bool ajax = false)
        {
            RegisterModel registerModel = new RegisterModel();
            IdentityResult result;
            if (!_roleManager.RoleExistsAsync("Sistem Yöneticisi").Result)
            {
                result = _roleManager.CreateAsync(new CustomIdentityRole { Name = "Sistem Yöneticisi", NormalizedName = "SISTEMYONETICISI" }).Result;
                result = _roleManager.CreateAsync(new CustomIdentityRole { Name = "Standart Kullanıcı", NormalizedName = "STANDARTKULLANICI" }).Result;
            }

            List<CustomIdentityRole> roles = _roleManager.Roles.ToList();
            CustomIdentityUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;

            IList<string> currentUserRoles = _userManager.GetRolesAsync(currentUser).Result;
           
            registerModel.Roles = roles.ToList();

            registerModel.ReturnUrl = returnUrl;
            return View(registerModel);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            registerModel.ReturnUrl = registerModel.ReturnUrl ?? Url.Content("/Manage/EditUser?Id=");
            if (ModelState.IsValid)
            {
                CustomIdentityUser user = new CustomIdentityUser
                {
                    UserName = registerModel.RegisterView.UserName,
                    Email = registerModel.RegisterView.Email,
                    Name = registerModel.RegisterView.Name,
                    PhoneNumber = registerModel.RegisterView.Phone,
                    EmailConfirmed = true,
                };
                IdentityResult result = await _userManager.CreateAsync(user, "Atlas.1254");
                CustomIdentityUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
                CustomIdentityUser savedUser = _userManager.FindByEmailAsync(registerModel.RegisterView.Email).Result;
                if (result.Succeeded)
                {
                    _logger.LogTrace($"{currentUser.UserName}, {savedUser.UserName} kullanıcısı oluşturuldu");
                    if (registerModel.RegisterView.Roles != null)
                    {
                        result = await _userManager.AddToRolesAsync(savedUser, registerModel.RegisterView.Roles);
                    }
                    _ = _userManager.AddClaimsAsync(savedUser, AddClaimsToContext(savedUser)).Result;
                    return LocalRedirect(registerModel.ReturnUrl + savedUser.Id);
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        #endregion
        private List<Claim> AddClaimsToContext(CustomIdentityUser user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("Name", user.Name));
            claims.Add(new Claim("UserId", user.Id));

            var userRoles = _userManager.GetRolesAsync(user).Result.ToList();
            foreach (var item in userRoles)
            {
                claims.Add(new Claim("rN", item));
            }
            var roleIds = _roleManager.Roles.Where(m => userRoles.Contains(m.Name)).Select(m => m.Id).ToList();
            foreach (var item in roleIds)
            {
                claims.Add(new Claim("rId", item));
            }

            return claims;
        }

        #region EDIT USER

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            var EditUser = _userManager.FindByIdAsync(id).Result;
            var UserRoles = _userManager.GetRolesAsync(EditUser).Result.ToList();

            List<CustomIdentityRole> roles = _roleManager.Roles.ToList();
            CustomIdentityUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            IList<string> currentUserRoles = _userManager.GetRolesAsync(currentUser).Result;
            CustomIdentityRole maxLevelRole = roles.LastOrDefault();
            var Roles = roles.ToList();

            
            EditUserModel editUserModel = new EditUserModel
            {
                EditUser = EditUser,
                UserId = id,
                UserRoles = UserRoles,
                Roles = roles,
                Email = EditUser.Email,
                Name = EditUser.Name,
                UserName = EditUser.UserName,
                Phone = EditUser.PhoneNumber,
              
            };
            return View(editUserModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserModel editUserModel)
        {

            CustomIdentityUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            CustomIdentityUser realUser = _userManager.FindByIdAsync(editUserModel.UserId).Result;
            
            _logger.LogTrace($"{currentUser.UserName} kullanıcısı, {realUser.UserName} kullanıcısını düzenlemeye başladı. {realUser.UserName.ToUpper()} kullanısıcın önceki bilgileri: {JsonConvert.SerializeObject(realUser)}");

            realUser.Name = editUserModel.EditUser.Name;
            realUser.UserName = editUserModel.EditUser.UserName;
            realUser.NormalizedUserName = editUserModel.EditUser.UserName.ToUpper();
            realUser.Email = editUserModel.EditUser.Email;
            realUser.NormalizedEmail = editUserModel.EditUser.Email.ToUpper();
            realUser.PhoneNumber = editUserModel.EditUser.PhoneNumber;
           
            IdentityResult result = await _userManager.UpdateAsync(realUser);
            IList<string> realUserRoles = _userManager.GetRolesAsync(realUser).Result;
            if (result.Succeeded)
            {
                _logger.LogTrace($"{currentUser.UserName}, {realUser.UserName} kullanıcısını düzenledi.");
                if (editUserModel.UserRoles != null)
                {
                    result = _userManager.RemoveFromRolesAsync(realUser, realUserRoles).Result;
                    result = await _userManager.AddToRolesAsync(realUser, editUserModel.UserRoles);
                }
                var exist = _userManager.GetClaimsAsync(realUser).Result;
                _ = _userManager.RemoveClaimsAsync(realUser, exist).Result;
                _ = _userManager.AddClaimsAsync(realUser, AddClaimsToContext(realUser)).Result;

                // bunları tüm kulanıcıların  userclaim bilgilerini doldurmak için ekledik  
                //foreach (var item in _userManager.Users.ToList())
                //{
                //    _ = _userManager.RemoveClaimsAsync(item, exist).Result;
                //    _ = _userManager.AddClaimsAsync(item, AddClaimsToContext(item)).Result;
                //}

            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Redirect("/Manage/EditUser?id=" + realUser.Id);
        }
        [HttpPost]
        public JsonResult EditInProfile(string userName, string mail, string phone)
        {
            try
            {
                var editedUser = _userManager.FindByNameAsync(userName).Result;

                editedUser.Email = mail;
                editedUser.PhoneNumber = phone;
                


                IdentityResult result = _userManager.UpdateAsync(editedUser).Result;

                if (result.Succeeded)
                {
                    _logger.LogTrace($"{userName} kullanıcısı, kendi email veya telefonunu güncelledi. Email: {mail}, Telefon:{phone}");
                    return new JsonResult(new { head = "Başarılı", status = "success", message = "Profiliniz güncellendi" });
                }
                return new JsonResult(new { head = "Başrısız!", status = "warning", message = "Profiliniz güncellenemedi" });
            }
            catch (Exception)
            {
                return new JsonResult(new { head = "Başrısız!", status = "warning", message = "Profiliniz güncellenemedi. Hata oluştu." });
            }
        }

        #endregion

        #region USERS

        [HttpGet]
        public IActionResult Users(bool ajax = false, int firmId = 0)
        {
            UsersModel usersModel = new UsersModel
            {
                _ajax = ajax,
                FirmId = firmId
            };
            return View(usersModel);
        }        
        [HttpPost]
        public JsonResult UsersDeleteUser(string userId)
        {
            try
            {
                var deletedUser = _userManager.FindByIdAsync(userId).Result;
                IdentityResult result = _userManager.DeleteAsync(deletedUser).Result;
                if (result.Succeeded)
                {
                    var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                    _logger.LogTrace($"{user.UserName}, {deletedUser.UserName} kullanıcısını sildi.");
                    return new JsonResult(new { result = true, message = "Kullanıcı başarılı bir şekilde silindi" });
                }
                return new JsonResult(new { result = false, message = "Kullanıcı Silinemedi" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { result = false, message = "Kullanıcı Silinemedi exception oluştu" });

            }
        }
        [HttpPost]
        public JsonResult UsersChangeUserState(string userId, bool state)
        {
            CustomIdentityUser user = _userManager.FindByIdAsync(userId).Result;

            IdentityResult result = _userManager.UpdateAsync(user).Result;
            var currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            _logger.LogTrace($"{currentUser.UserName}, {user.UserName} kullanıcısını durumunu değiştirdi, IsActive={state}");

            if (result.Succeeded)
            {
                return new JsonResult(new { result = true, message = "Kullanıcı durumu başarılı bir şekilde değiştirildi" });
            }
            return new JsonResult(new { result = false, message = "Kullanıcı durumu değiştirilemedi" });
        }

        #endregion
    }
}