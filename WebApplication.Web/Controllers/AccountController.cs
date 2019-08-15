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

        [AuthorizationFilter("Admin", "User FT", "User PT")]
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

                // alert section
                IList<PayrollTable> payrollList = payrollDAL.GetTimeReport(user.UserId);

                if (payrollList.Count > 0)
                {
                    int isOver = hoursDAL.IsOverWeeklyHoursAlert(user.UserId, payrollList[0].StartDate, payrollList[0].EndDate);

                    ViewBag.IsOver = isOver;

                    User userRole = authProvider.GetCurrentUser();

                    ViewBag.UserRole = userRole.Role;
                }

                // end of alert section
                // reminder section
                
                DateTime startPayPeriodDate = payrollList[0].StartDate;
                DateTime endPayPeriodDate = payrollList[0].EndDate;
                DateTime dateToday = DateTime.Now;
                int numOfWeekendDays = CalculateWeekends(startPayPeriodDate, endPayPeriodDate); // not correct

                int CalculateWeekends(DateTime DateTime1, DateTime DateTime2)
                {
                    int iReturn = 0;
                    TimeSpan xTimeSpan;
                    if (DateTime2 > DateTime1)
                        xTimeSpan = DateTime2.Subtract(DateTime1);
                    else
                        xTimeSpan = DateTime1.Subtract(DateTime2);
                    int iDays = 5 + System.Convert.ToInt32(xTimeSpan.TotalDays);
                    iReturn = (iDays / 7);
                    return iReturn;
                }

                TimeSpan expectedDaysTimeSpan = dateToday.Subtract(startPayPeriodDate);
                int expectedDaysInt = (int)expectedDaysTimeSpan.TotalDays - numOfWeekendDays;
                int numberOfDaysLoggedWithCurrentPayPeriod = logDAL.GetUserLogWithinPayPeriod(user.UserId, startPayPeriodDate, endPayPeriodDate);
                bool allLogsEntered = true;
                if (expectedDaysInt != numberOfDaysLoggedWithCurrentPayPeriod)
                {
                    allLogsEntered = false;
                }

                ViewBag.AllLogsEntered = allLogsEntered;
                // end of reminder section


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
            if (ModelState.IsValid && !userDAL.CheckIfUserNameExists(registerViewModel.UserName))
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


        [AuthorizationFilter("Admin", "User FT", "User PT")]
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

        [HttpGet]
        [AuthorizationFilter("Admin", "User FT", "User PT")]
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

        [HttpGet]
        [AuthorizationFilter("Admin")]
        public IActionResult ApproveHoursHub()
        {
            ViewBag.PayPeriods = payrollDAL.GetListOfPayPeriods();
            ViewBag.TimeCards = new List<PayrollTable>();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            // use id to pull specific log information hours by id
            Hours selectedHour = hoursDAL.GetHoursById(id);

            // prefill log information
            ViewBag.SelectedHour = selectedHour;

            User currentUser = authProvider.GetCurrentUser();

            ViewBag.AvailableTasks = taskDAL.GetAllTasks(currentUser.UserId);

            ViewBag.Locations = locationDAL.GetAllLocations();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateLog(Hours hours)
        {
            bool isSuccessful = hoursDAL.UpdateHours(hours);

            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveTimeCard(PayrollTable payrollLine)
        {
            payrollLine.IsApproved = true;
            bool success = payrollDAL.ApproveTime(payrollLine);

            return RedirectToAction("ApproveHoursHub", "Account");
        }

        [HttpGet]
        [AuthorizationFilter("Admin", "User FT", "User PT")]
        public IActionResult SubmitTimeCard()
        {
            User currentUser = authProvider.GetCurrentUser();
            ViewBag.UnsubbedPayPeriods = payrollDAL.GetTimeReport(currentUser.UserId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitTimeCard(PayrollTable payrollLine)
        {
            payrollLine.IsSubmitted = true;
            bool success = payrollDAL.SubmitTime(payrollLine);

            return RedirectToAction("SubmitTimeCard", "Account");
        }
    }
}