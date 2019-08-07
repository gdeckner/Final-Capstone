using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthProvider authProvider;
        private readonly IJobDAL jobDAL;
        public AccountController(IAuthProvider authProvider, IJobDAL jobDAL)
        {
            this.authProvider = authProvider;
            this.jobDAL = jobDAL;
        }

        //[AuthorizationFilter] // actions can be filtered to only those that are logged in
        [AuthorizationFilter("Admin", "Author", "Manager", "Users")]  //<-- or filtered to only those that have a certain role
        [HttpGet]
        public IActionResult Index()
        {
            var user = authProvider.GetCurrentUser();
            if (user.Role == "Admin")
            {
                ViewBag.Users = authProvider.GetAllUsers();
                return View("AdminIndex", user);
            }
            else
            {
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            // Ensure the fields were filled out
            if (ModelState.IsValid)
            {
                // Check that they provided correct credentials
                bool validLogin = authProvider.SignIn(loginViewModel.UserName, loginViewModel.Password);
                if (validLogin)
                {
                    // Redirect the user where you want them to go after successful login
                    return RedirectToAction("Index", "Account");
                }
            }

            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult LogOff()
        {
            // Clear user from session
            authProvider.LogOff();

            // Redirect the user where you want them to go after logoff
            return RedirectToAction("Index", "Account");
        }

        [AuthorizationFilter("Admin", "Author", "Manager")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                // Register them as a new user (and set default role)
                // When a user registeres they need to be given a role. If you don't need anything special
                // just give them "User".
                authProvider.Register(registerViewModel.FullName, registerViewModel.UserName, registerViewModel.Password, registerViewModel.Role);

                // Redirect the user where you want them to go after registering
                return RedirectToAction("Index", "Account");
            }

            return View(registerViewModel);
        }


        [AuthorizationFilter("Admin", "Author", "Manager", "Users")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {

                authProvider.ChangePassword(changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword);

                // Register them as a new user (and set default role)
                // When a user registeres they need to be given a role. If you don't need anything special
                // just give them "User".


                // Redirect the user where you want them to go after registering
                return RedirectToAction("Index", "Account");
            }

            return View(changePasswordViewModel);
        }

        public IActionResult Delete(int id)
        {
            // todo fix delete user route
            User currentUser = authProvider.GetCurrentUser();

            authProvider.DeleteUser(id, currentUser.UserId);

            return RedirectToAction("Index", "Account");
        }

        [AuthorizationFilter("Admin", "Author", "Manager")]
        [HttpGet]
        public IActionResult CreateJob()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateJob(Job job)
        {
            jobDAL.CreateNewJob(job);

            return RedirectToAction("Index", "Account");
        }
    }
}