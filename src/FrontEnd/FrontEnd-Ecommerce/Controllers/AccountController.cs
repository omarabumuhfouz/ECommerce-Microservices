using FrontEnd_Ecommerce.DTOs.Auth.Requests;
using FrontEnd_Ecommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FrontEnd_Ecommerce.Controllers;

public class AccountController(IAuthService authService, JwtHelper jwtHelper, IStringLocalizer<AccountController> localizer, ICookieManager cookieManager) : Controller
{
    private string clientId = "myapp";

    [HttpGet]
    public IActionResult Login() => View(LoginRequest.Empty());

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return View(loginRequest);

        var response = await authService.LoginAsync(loginRequest); // sets cookies

        if (response is null)
        {
            TempData[MessageType.Error] = localizer[LocalizationKeys.Auth.LoginFailed];
            return View(loginRequest);
        }

        cookieManager.SetAccessToken(response.Token);
        cookieManager.SetRefreshToken(response.RefreshToken);

        return RedirectToAction("PostLogin");
    }

    [HttpGet]
    public IActionResult PostLogin()
    {
        var userName = jwtHelper.GetUserName();
        TempData[MessageType.Success] = localizer[LocalizationKeys.Auth.WelcomeMessage, userName].Value;

        var role = jwtHelper.GetRole(); 
        if (string.Equals(role, SystemRoles.Admin, StringComparison.OrdinalIgnoreCase))
            return RedirectToAction("Index", "Admin");

        return RedirectToAction("Index", "Home");
    }



    [HttpGet]
    public IActionResult Register() => View(RegisterRequest.Empty());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid) return View(registerRequest);

        var response = await authService.RegisterAsync(registerRequest);

        if (response is null)
        {
            TempData[MessageType.Error] = LocalizationKeys.Auth.RegisterFailed;
            return View(registerRequest);
        }

        TempData[MessageType.Success] = LocalizationKeys.Auth.RegisterSuccess;

        return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await authService.LogoutAsync(true);

            TempData[MessageType.Success] =  localizer[LocalizationKeys.Auth.LogoutSuccess].Value;
            cookieManager.ClearAuthCookies();

        return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public IActionResult ChangePassword() => View(ChangePasswordRequest.Empty());

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePassword)
    {
        if (ModelState.IsValid) return View(changePassword);

        await authService.ChangePasswordAsync(changePassword);

        ViewData["Message"] = LocalizationKeys.Auth.ChangePasswordSuccess;

        return View("Profile");
    }



    [HttpGet]
    public IActionResult Profile()
    {
        var userId = jwtHelper.GetUserId();

        var profile = new ProfileViewModel
        {
            Id = userId,
            FirstName = "Omar",
            LastName = "Abumuhfouz",
            Email = "omarabumufhouz@gmail.com",
            Roles = new List<string> { "Customer" },
            IsActive = true,
            ActiveSince = DateTime.Now.AddDays(-50),
            TotalOrders = 6,
            Addresses = new List<AddressViewModel>
            {
                new AddressViewModel
                {
                    AddressType = "Home",
                    City = "Amman",
                    Country = "Jordan",
                    Street = "Souq-Almarcize",
                    State = "No State",
                    IsDefault = true,
                    ZipCode = "2323-23"
                },
                new AddressViewModel
                {
                    AddressType = "|Work",
                    City = "Maa'n",
                    Country = "Jordan",
                    Street = "Alushine-University",
                    State = "No State",
                    IsDefault = false,
                    ZipCode = "3332-23"
                }
            }
        };

        return View(profile);
    }

    [HttpGet]
    public IActionResult EditProfile()
    {
        TempData["Warning"] = "Edit Profile Waringn";
        // Send Id And Recived DAta as EditProfileResponseDto
        var editProfile = new EditProfileRequestDto("Omar", "AbuMuhouz", "omarabumuhfouz@gmail.com");
        return View();
    }

    // [HttpPost]
    // public async Task<IActionResult> UpdateProfile(EditProfileRequestDto request)
    // {
    //     if (!ModelState.IsValid)
    //         return View(request);


    //     var response = await authService.UpdateProfileAsync(request);

    //     return View("Profile");
    // }
}