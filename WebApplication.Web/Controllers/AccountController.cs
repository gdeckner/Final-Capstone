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
        private readonly ITaskDAL taskDAL;
        private readonly ILocationDAL locationDAL;
        private readonly IHoursDAL hoursDAL;
        private readonly IUserDAL userDAL;
        private readonly IPayrollDAL payrollDAL;
        private readonly ILogDAL logDAL;
        public AccountController(IAuthProvider authProvider, IJobDAL jobDAL, ITaskDAL taskDAL, ILocationDAL locationDAL, IUserDAL userDAL, IHoursDAL hoursDAL, IPayrollDAL payrollDAL, ILogDAL logDAL)
        {
            this.authProvider = authProvider;
            this.jobDAL = jobDAL;
            this.taskDAL = taskDAL;
            this.locationDAL = locationDAL;
            this.userDAL = userDAL;
            this.hoursDAL = hoursDAL;
            this.payrollDAL = payrollDAL;
            this.logDAL = logDAL;
        }

        //[AuthorizationFilter] // actions can be filtered to only those that are logged in
        [AuthorizationFilter("Admin", "Users")]  //<-- or filtered to only those that have a certain role
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
                ViewBag.Hours = hoursDAL.GetAllHours(user.UserId, true);

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

        [AuthorizationFilter("Admin")]
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


        [AuthorizationFilter("Admin", "Users")]
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

        public IActionResult ChangeRole(int UserId, string Role)
        {
            User currentUser = authProvider.GetCurrentUser();

            authProvider.ChangeRole(currentUser.UserId, UserId, Role);

            return RedirectToAction("Index", "Account");
        }

        [AuthorizationFilter("Admin")]
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

        [AuthorizationFilter("Admin")]
        [HttpGet]
        public IActionResult CreateProjectTasks()
        {

            List<Job> jobs = new List<Job>();

            jobs = jobDAL.GetJobList();

            ViewBag.Jobs = jobs;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProjectTasks(Tasks task)
        {
            bool isSuccessful = taskDAL.CreateNewTask(task);

            return RedirectToAction("Index", "Account");
        }

        [AuthorizationFilter("Admin", "Users")]
        [HttpGet]
        public IActionResult LogTime()
        {
            User currentUser = authProvider.GetCurrentUser();
            ViewBag.AvailableTasks = taskDAL.GetAllTasks(currentUser.UserId);

            ViewBag.Locations = locationDAL.GetAllLocations();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogTime(Hours hours)
        {
            hours.UserId = authProvider.GetCurrentUser().UserId; //Needs to be userName

            if (ModelState.IsValid)
            {
                hoursDAL.CreateNewHours(hours);

                return RedirectToAction("Index", "Account");
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
            }
            return View(hours);
        }

        [HttpGet]
        [AuthorizationFilter("Admin")]
        public IActionResult AddJobLocation()
        {
            // change make new task in job sql dal move to task dal
            // dao get all locations

            ViewBag.Locations = locationDAL.GetAllLocations();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddJobLocation(Location location)
        {
            bool isSuccessful = locationDAL.CreateLocation(location);

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        [AuthorizationFilter("Admin")]
        public IActionResult AuthorizeUser()
        {
            ViewBag.Users = userDAL.GetAllUsers();

            ViewBag.Jobs = jobDAL.GetJobList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AuthorizeUser(UserJob userJob)
        {
            bool isSuccessful = jobDAL.AssignUserToJob(userJob);

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        [AuthorizationFilter("Admin")]
        public IActionResult GetUserTimeLog(string username)
        {
            User user = userDAL.GetUser(username);

            IList<Hours> hoursList = hoursDAL.GetTimeReport(user.UserId, "1M");

            ViewBag.HoursList = hoursList;

            ViewBag.User = user;

            ViewBag.User.LogTimeSort = "1M";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SortUserTimeLog(User user)
        {
            IList<Hours> hoursList = hoursDAL.GetTimeReport(user.UserId, user.LogTimeSort);

            ViewBag.HoursList = hoursList;

            ViewBag.User = user;

            return View();
        }

        [AuthorizationFilter("Admin")]
        public IActionResult ApproveHoursHub(PayrollTable payrollTable)
        {
            if (payrollTable.StartDate < new DateTime(1753, 1, 1))
            {
                payrollTable.StartDate = new DateTime(1753, 1, 1);
            }
            ViewBag.PayPeriods = payrollDAL.GetListOfPayPeriods();
            ViewBag.TimeCards = payrollDAL.GetListOfTimeCards(payrollTable.StartDate);

            return View();
        }

        [HttpGet]
        [AuthorizationFilter("Admin")]
        public IActionResult CreatePayPeriod()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePayPeriod(DateTime StartDate, DateTime EndDate)
        {
            payrollDAL.CreatePayPeriod(StartDate, EndDate);

            return RedirectToAction("ApproveHoursHub", "Account");
        }

        [HttpGet]
        [AuthorizationFilter("Admin")]
        public IActionResult PeriodTimeCard(PayrollTable UserPeriod)
        {
            ViewBag.TimeCard = hoursDAL.GetTimeCard(UserPeriod.UserId, UserPeriod.StartDate, UserPeriod.EndDate);
            ViewBag.PayrollLine = UserPeriod;

            return View();
        }

        [HttpGet]
        [AuthorizationFilter("Admin")]
        public IActionResult AuditedLogs(string username)
        {
            User user = userDAL.GetUser(username);

            IList<Log> logList = logDAL.GetUserLog(user.UserId, "1M");

            ViewBag.LogList = logList;

            ViewBag.User = user;

            ViewBag.User.LogTimeSort = "1M";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SortAuditedLogs(User user)
        {
            IList<Log> logList = logDAL.GetUserLog(user.UserId, user.LogTimeSort);

            ViewBag.Loglist = logList;

            ViewBag.User = user;

            return View();
        }

        [HttpGet]
        public IActionResult EditLogs(int id)
        {
            // use id to pull specific log information

            // prefill log information

            // display data in dropdowns

            //

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveTimeCard(PayrollTable payrollLine)
        {
            payrollLine.IsApproved = true;
            bool success = payrollDAL.ApproveTime(payrollLine);

            return RedirectToAction("ApproveHoursHub", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitTimeCard(PayrollTable payrollLine)
        {
            payrollLine.IsApproved = true;
            bool success = payrollDAL.ApproveTime(payrollLine);

            return RedirectToAction("ApproveHoursHub", "Account");
        }
    }
}