using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using ApplicationHealth.MvcWebUI.AccountModel;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using ApplicationHealth.Domain.Identity;
using ApplicationHealth.Services.Services;
using ApplicationHealth.Domain.ViewModels;

namespace ApplicationHealth.MvcWebUI.Controllers
{
    [AllowAnonymous]
    public class AccountController : CustomBaseController
    {
        #region PROP - INJECTIONS
        private readonly IMailService _mailService;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly RoleManager<CustomIdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;


        public AccountController(IMailService mailService, SignInManager<CustomIdentityUser> signInManager, UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, ILogger<AccountController> logger)
        {
            _mailService = mailService;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        #endregion

        #region LOGIN

        public IActionResult Login(string returnUrl)
        {
            string host = HttpContext.Request.Path;
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            int interval = 0;
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/Home/Index");
            if (ModelState.IsValid)
            {
                if (Regex.IsMatch(model.Email, @"('(''|[^'])*')|(;)|(\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1})\b)", RegexOptions.IgnoreCase))
                {
                    ModelState.AddModelError(string.Empty, "İllegal Karakter!");
                    return View(model);
                }
                CustomIdentityUser user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, $"Başarısız Kullanıcı Girişi!");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogTrace($"{user.Name} adlı kullanıcı {user.UserName} kullanıcı adı ile sisteme giriş yaptı.");
                    return Redirect(model.ReturnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogTrace($"{model.Email.ToUpper()} kullanıcısı 5 kez hatalı giriş yaptığı için 5 dk süre ile kilitlendi.");
                    return Redirect("/Account/Lockout");
                }

                ModelState.AddModelError(string.Empty, $"Başarısız Kullanıcı Girişi!");
                return View(model);

            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Başarısız Giriş! Lütfen bilgileri doğru bir şekilde girdiğinizden emin olun. ");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

       

        #region CONFIRM EMAIL

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Login");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Kullanıcı bulunamadı. '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Kullanıcı maili doğrulanırken hata oluştu '{userId}':");
            }

            return View();
        }

        #endregion

        #region FORGOT PASSWORD

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(forgotPasswordModel.Email))
                {
                    if (Regex.IsMatch(forgotPasswordModel.Email, @"('(''|[^'])*')|(;)|(\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1})\b)", RegexOptions.IgnoreCase))
                    {
                        forgotPasswordModel.Email = "";
                        _logger.LogTrace($"Yasak kelimeler giriş için denendi => {forgotPasswordModel.Email}");
                        ModelState.AddModelError(string.Empty, "Kullanıcı bilgilerinde illegal karakter var. Bu şekilde giriş yapılamaz.");
                        return View();
                    }
                }

                CustomIdentityUser user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Böyle bir kullanıcı yok yada e-posta adresi doğrulanmamış");
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please 
                string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                string host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                string callbackUrl = host + "/Account/ResetPassword?code=" + code;

                string emailTemplate = "<!DOCTYPE html 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'> <head> <meta name='viewport' content='width=device-width, initial-scale=1.0' /> <meta name='x-apple-disable-message-reformatting' /> <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' /> <meta name='color-scheme' content='light dark' /> <meta name='supported-color-schemes' content='light dark' /> <title></title> <style type='text/css' rel='stylesheet' media='all'> /* Base ------------------------------ */  body { width: 100% !important; height: 100%; margin: 0; -webkit-text-size-adjust: none; } a { color: #3869D4; } a img { border: none; } td { word-break: break-word; } .preheader { display: none !important; visibility: hidden; mso-hide: all; font-size: 1px; line-height: 1px; max-height: 0; max-width: 0; opacity: 0; overflow: hidden; } /* Type ------------------------------ */ body, td, th { font-family: 'Nunito Sans', Helvetica, Arial, sans-serif; } h1 { margin-top: 0; color: #333333; font-size: 22px; font-weight: bold; text-align: left; } h2 { margin-top: 0; color: #333333; font-size: 16px; font-weight: bold; text-align: left; } h3 { margin-top: 0; color: #333333; font-size: 14px; font-weight: bold; text-align: left; } td, th { font-size: 16px; } p, ul, ol, blockquote { margin: .4em 0 1.1875em; font-size: 16px; line-height: 1.625; } p.sub { font-size: 13px; } /* Utilities ------------------------------ */ .align-right { text-align: right; } .align-left { text-align: left; } .align-center { text-align: center; } /* Buttons ------------------------------ */ .button { background-color: #3869D4; border-top: 10px solid #3869D4; border-right: 18px solid #3869D4; border-bottom: 10px solid #3869D4; border-left: 18px solid #3869D4; display: inline-block; color: #FFF; text-decoration: none; border-radius: 3px; box-shadow: 0 2px 3px rgba(0, 0, 0, 0.16); -webkit-text-size-adjust: none; box-sizing: border-box; } .button--green { background-color: #22BC66; border-top: 10px solid #22BC66; border-right: 18px solid #22BC66; border-bottom: 10px solid #22BC66; border-left: 18px solid #22BC66; } .button--red { background-color: #FF6136; border-top: 10px solid #FF6136; border-right: 18px solid #FF6136; border-bottom: 10px solid #FF6136; border-left: 18px solid #FF6136; } @media only screen and (max-width: 500px) { .button { width: 100% !important; text-align: center !important; } } /* Attribute list ------------------------------ */ .attributes { margin: 0 0 21px; } .attributes_content { background-color: #F4F4F7; padding: 16px; } .attributes_item { padding: 0; } /* Related Items ------------------------------ */ .related { width: 100%; margin: 0; padding: 25px 0 0 0; -premailer-width: 100%; -premailer-cellpadding: 0; -premailer-cellspacing: 0; } .related_item { padding: 10px 0; color: #CBCCCF; font-size: 15px; line-height: 18px; } .related_item-title { display: block; margin: .5em 0 0; } .related_item-thumb { display: block; padding-bottom: 10px; } .related_heading { border-top: 1px solid #CBCCCF; text-align: center; padding: 25px 0 10px; } /* Discount Code ------------------------------ */ .discount { width: 100%; margin: 0; padding: 24px; -premailer-width: 100%; -premailer-cellpadding: 0; -premailer-cellspacing: 0; background-color: #F4F4F7; border: 2px dashed #CBCCCF; } .discount_heading { text-align: center; } .discount_body { text-align: center; font-size: 15px; } /* Social Icons ------------------------------ */ .social { width: auto; } .social td { padding: 0; width: auto; } .social_icon { height: 20px; margin: 0 8px 10px 8px; padding: 0; } /* Data table ------------------------------ */ .purchase { width: 100%; margin: 0; padding: 35px 0; -premailer-width: 100%; -premailer-cellpadding: 0; -premailer-cellspacing: 0; } .purchase_content { width: 100%; margin: 0; padding: 25px 0 0 0; -premailer-width: 100%; -premailer-cellpadding: 0; -premailer-cellspacing: 0; } .purchase_item { padding: 10px 0; color: #51545E; font-size: 15px; line-height: 18px; } .purchase_heading { padding-bottom: 8px; border-bottom: 1px solid #EAEAEC; } .purchase_heading p { margin: 0; color: #85878E; font-size: 12px; } .purchase_footer { padding-top: 15px; border-top: 1px solid #EAEAEC; } .purchase_total { margin: 0; text-align: right; font-weight: bold; color: #333333; } .purchase_total--label { padding: 0 15px 0 0; } body { background-color: #FFF; color: #333; } p { color: #333; } .email-wrapper { width: 100%; margin: 0; padding: 0; -premailer-width: 100%; -premailer-cellpadding: 0; -premailer-cellspacing: 0; } .email-content { width: 100%; margin: 0; padding: 0; -premailer-width: 100%; -premailer-cellpadding: 0; -premailer-cellspacing: 0; } /* Masthead ----------------------- */ .email-masthead { padding: 25px 0; text-align: center; } .email-masthead_logo { width: 94px; } .email-masthead_name { font-size: 16px; font-weight: bold; color: #A8AAAF; text-decoration: none; text-shadow: 0 1px 0 white; } /* Body ------------------------------ */ .email-body { width: 100%; margin: 0; padding: 0; -premailer-width: 100%; -premailer-cellpadding: 0; -premailer-cellspacing: 0; } .email-body_inner { width: 570px; margin: 0 auto; padding: 0; -premailer-width: 570px; -premailer-cellpadding: 0; -premailer-cellspacing: 0; } .email-footer { width: 570px; margin: 0 auto; padding: 0; -premailer-width: 570px; -premailer-cellpadding: 0; -premailer-cellspacing: 0; text-align: center; } .email-footer p { color: #A8AAAF; } .body-action { width: 100%; margin: 30px auto; padding: 0; -premailer-width: 100%; -premailer-cellpadding: 0; -premailer-cellspacing: 0; text-align: center; } .body-sub { margin-top: 25px; padding-top: 25px; border-top: 1px solid #EAEAEC; } .content-cell { padding: 35px; } /*Media Queries ------------------------------ */ @media only screen and (max-width: 600px) { .email-body_inner, .email-footer { width: 100% !important; } } @media (prefers-color-scheme: dark) { body { background-color: #333333 !important; color: #FFF !important; } p, ul, ol, blockquote, h1, h2, h3 { color: #FFF !important; } .attributes_content, .discount { background-color: #222 !important; } .email-masthead_name { text-shadow: none !important; } } :root { color-scheme: light dark; supported-color-schemes: light dark; } </style> <!--[if mso]> <style type='text/css'> .f-fallback { font-family: Arial, sans-serif; } </style> <![endif]--> </head> <body> <span class='preheader'>24 saat içinde şifrenizi yenilemeniz gerekir.</span> <table class='email-wrapper' width='100%' cellpadding='0' cellspacing='0' role='presentation'> <tr> <td align='center'> <table class='email-content' width='100%' cellpadding='0' cellspacing='0' role='presentation'><!-- Email Body --> <tr> <td class='email-body' width='570' cellpadding='0' cellspacing='0'> <table class='email-body_inner' align='center' width='570' cellpadding='0' cellspacing='0' role='presentation'> <!-- Body content --> <tr> <td class='content-cell'> <div class='f-fallback'> <h1>Merhaba " + user.Name + ", </h1> <p>Şifrenizi değiştirmek istediniz. <strong>Bu şifre değiştirme baglantısı 24 saat sonra pasif olacaktır.</strong></p> <!-- Action --> <table class='body-action' align='center' width='100%' cellpadding='0' cellspacing='0' role='presentation'> <tr> <td align='center'> <table width='100%' border='0' cellspacing='0' cellpadding='0' role='presentation'> <tr> <td align='center'> <a href='" + callbackUrl + "' class='f-fallback button button--green' target='_blank'>Şifremi Resetle</a> </td> </tr> </table> </td> </tr> </table> <p>Teşekkürler, <!-- Sub copy --> <table class='body-sub' role='presentation'> <tr> <td> <p class='f-fallback sub'>Bağlantıyı Şifremi Resetle butonuna tıklayarak açamadıysanız, aşağıdaki bağlantıyı kopyalayıp tarayıcınıza yapıştırın.</p> <p class='f-fallback sub'>" + callbackUrl + "</p> </td> </tr> </table> </div> </td> </tr> </table> </td> </tr> <tr> <td> <table class='email-footer' align='center' width='570' cellpadding='0' cellspacing='0' role='presentation'> <tr> <td class='content-cell' align='center'> <p class='f-fallback sub align-center'>&copy; [www.atlasserver.io]. Tüm hakları saklıdır.</p> <p class='f-fallback sub align-center'> </p> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </body></html>";
                await _mailService.SendMailViaSystemNetAsync(new Email
                {
                    toList = forgotPasswordModel.Email,
                    subject = "Şifre Yenileme",
                    content = emailTemplate
                });
                _logger.LogTrace($"{forgotPasswordModel.Email} kullanıcısına şifre yenileme e-postası gönderildi.");

                return View("ForgotPasswordConfirmation");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı Bilgilerinde Hata Var. Bu şekilde giriş yapılamaz.");
                return View();
            }
        }

        #endregion

        #region FORGOT PASSWORD CONFIRMATION

        [HttpGet]
        public async Task<IActionResult> ForgotPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region LOGOUT

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return View("Logout");
        }
        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            //_logger.LogInformation("User logged out.");""
            if (returnUrl != null)
            {
                return RedirectToPage(returnUrl);
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region RESET PASSWORD

        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return BadRequest("Parola yenileme kodu doğru değil");
            }
            else
            {
                ResetPasswordModel.InputModel resetPasswordModel = new ResetPasswordModel.InputModel
                {
                    Code = code
                };
                return View(resetPasswordModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel.InputModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            CustomIdentityUser user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ModelState.AddModelError(string.Empty, "Bu e-posta adresine bağlı bir kullanıcı mevcut değil.");
                return View();
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Code.Replace(' ', '+'), resetPasswordModel.Password);
            if (result.Succeeded)
            {
                _logger.LogTrace($"{resetPasswordModel.Email} kullanıcısı şifresini resetledi.");

                user.EmailConfirmed = true;
                result = _userManager.UpdateAsync(user).Result;
                return View("ResetPasswordConfirmation");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        #endregion

        public IActionResult LockOut(string returnUrl)
        {
            return View();
        }
    }
}