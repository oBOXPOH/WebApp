using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.ViewModels;
using Microsoft.AspNetCore.Identity;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        ApplicationContext applicationContext;
        readonly UserManager<User> userManager;
        readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> usrManager, SignInManager<User> sgnInManager, ApplicationContext appContext)
        {
            applicationContext = appContext;
            userManager = usrManager;
            signInManager = sgnInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Login,
                    Email = model.EMail,
                    DOB = model.DOB,
                    UserRole = "Пользователь"
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(user.Email), user.UserRole);

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, userCode = code },
                        protocol: HttpContext.Request.Scheme);

                    EmailService emailService = new EmailService();

                    await emailService.SendEmailAsync(model.EMail, "Подтвердите аккаунт",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>ссылка</a>");

                    return RedirectToAction("Message", "Home", new { message = "Подтверждение отправлено на указанную вами почту." });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string userCode)
        {
            if (userId == null || userCode == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Error", "Home", new { message = "Произошла какая-то ошибка! <br><br><a class='button' href='/Home/Index'>Вернуться на главную</a>" });
            }

            var result = await userManager.ConfirmEmailAsync(user, userCode);

            if (result.Succeeded)
            {
                return RedirectToAction("Message", "Home", new { message = "Подтверждение прошло успешно! Теперь вы можете авторизоваться на сайте!<br><br><a class='button' href='/Account/Login'>Авторизоваться</a>" });
            }

            else
            {
                return RedirectToAction("Error", "Home", new { message = "Произошла какая-то ошибка! <br><br><a class='button' href='/Home/Index'>Вернуться на главную</a>" });
            }
        }

        [HttpGet]
        public IActionResult Login(string url = null)
        {
            return View(new LoginUserViewModel { ReturnUrl = url });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByNameAsync(model.LoginOrEmail);

                if (user != null)
                {
                    if (!await userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                        return View(model);
                    }
                }

                var result = await signInManager.PasswordSignInAsync(model.LoginOrEmail, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин/Email или пароль");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Forgot(ForgotUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByEmailAsync(model.EMail);

                if (user != null)
                {
                    var code = await userManager.GeneratePasswordResetTokenAsync(user);

                    var link = Url.Action(
                        "ResetPassword",
                        "Account",
                        new { userId = user.Id, userCode = code },
                        HttpContext.Request.Scheme);

                    EmailService emailService = new EmailService();

                    await emailService.SendEmailAsync(model.EMail, "Восстановление пароля", $"Для восстановления пароля перейдите по ссылке: <a href='{link}'>ссылка</a>");

                    return RedirectToAction("Message", "Home", new { message = "Ссылка на восстановление пароля отправлена на указанную вами почту." });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Такой Email не зарегистрирован");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId, string userCode)
        {
            if (userId == null || userCode == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Error", "Home", new { message = "Произошла какая-то ошибка! <br><br><a class='button' href='/Home/Index'>Вернуться на главную</a>" });
            }

            return View(new ResetPasswordUserViewModel { UserId = user.Id, UserCode = userCode });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(model.UserId);

                if (user == null)
                {
                    return RedirectToAction("Error", "Home", new { message = "Произошла какая-то ошибка! <br><br><a class='button' href='/Home/Index'>Вернуться на главную</a>" });
                }

                var result = await userManager.ResetPasswordAsync(user, model.UserCode, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Error", "Home", new { message = "Произошла какая-то ошибка! <br><br><a class='button' href='/Home/Index'>Вернуться на главную</a>" });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
