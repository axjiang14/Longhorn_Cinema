using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AWO_Team14.DAL;

//Change this using statement to match your project
using AWO_Team14.Models;
using System.Data.Entity;
using System.Collections.Generic;
using AWO_Team14.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;

//Change this namespace to match your project
namespace AWO_Team14.Controllers
{
    public enum EmploymentAction { Hire, Fire}

    [Authorize]
    public class AccountsController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private AppUserManager _userManager;
        private AppDbContext db = new AppDbContext();

        public AccountsController()
        {
        }

        public SelectList GetAllEmployees()
        {
            var roles = db.Roles.Where(r => r.Name == "Employee");
            if (roles.Any())
            {
                var roleId = roles.First().Id;
                var dbEmployees = from user in db.Users
                                  where user.Roles.Any(r => r.RoleId == roleId)
                                  select user;
                List<AppUser> Employees = dbEmployees.ToList();

                SelectList EmployeesList = new SelectList(Employees.OrderBy(u => u.Id), "Id", "UserName");

                return EmployeesList;

            }

            return null;
        }

        public SelectList GetAllUsers()
        {
            
        var eroles = db.Roles.Where(r => r.Name == "Customer");
        var eroleId = eroles.First().Id;
        var dbEmployees = from user in db.Users
                            where user.Roles.Any(r => r.RoleId == eroleId)
                            select user;
        List <AppUser> Employees = dbEmployees.ToList();


        var croles = db.Roles.Where(r => r.Name == "Customer");
        var croleId = croles.First().Id;
        var dbCustomers = from user in db.Users
                            where user.Roles.Any(r => r.RoleId == croleId)
                            select user;
        List<AppUser> Customers = dbCustomers.ToList();

        if (User.IsInRole("Employee"))
        {

            SelectList UsersList = new SelectList(Customers.OrderBy(u => u.Id), "Id", "UserName");

            return UsersList;
        }

        if (User.IsInRole("Manager"))
        {
                List<AppUser> AllUsers = Customers.Union(Employees).ToList();
                SelectList UsersList = new SelectList(AllUsers.OrderBy(u => u.Id), "Id", "UserName");

                return UsersList;
            }


            return null;
        }

        [Authorize(Roles = "Manager, Employee")]
        public ActionResult EmployeeHome()
        {
            return View();
        }

        //NOTE: This creates a user manager and a sign-in manager every time someone creates a request to this controller
        public AccountsController(AppUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // NOTE:  This is the logic for the login page
        // GET: /Accounts/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated) //user has been redirected here from a page they're not authorized to see
            {
                return View("Error", new string[] { "Access Denied" });
            }
            AuthenticationManager.SignOut(); //this removes any old cookies hanging around
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Accounts/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var Employee = from u in db.Users
                            where u.Email == model.Email
                            select u;

            List<AppUser> EmployeeList = Employee.ToList();

            AppUser Employee1 = EmployeeList.FirstOrDefault();

            if (EmployeeList.Count() != 0 && EmployeeList != null)
            {
                if (Employee1.Archived == true)
                {
                    return View(model);
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


        // GET: /Accounts/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // NOTE: Here is your logic for registering a new user
        // POST: /Accounts/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var age = DateTime.Now.Year - model.Birthday.Year;

            if (age < 13)
            {
                ViewBag.Error = "You must be 13 to create a customer account.";
                return View(model);
            }

            List<String> AllEmails = new List<String>();

            foreach (AppUser user in db.Users)
            {
                AllEmails.Add(user.Email);
            }

            if(AllEmails.Contains(model.Email))
            {
                ViewBag.Error = "An account already exists for that email address.";
                return View(model);
            }
                if (ModelState.IsValid)
                {
                //Add fields to user here so they will be saved to do the database
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    MiddleInitial = model.MiddleInitial,
                        LastName = model.LastName,
                        Street = model.Street,
                        City = model.City,
                        State = model.State,
                        Zip = model.Zip,
                        PhoneNumber = model.PhoneNumber,
                        Birthday = model.Birthday

                    };
                    var result = await UserManager.CreateAsync(user, model.Password);

                    //Once you get roles working, you may want to add users to roles upon creation
                    await UserManager.AddToRoleAsync(user.Id, "Customer");
                // --OR--
                // await UserManager.AddToRoleAsync(user.Id, "Employee");


                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    String Message = "Hello " + model.FirstName + ",\n\n" + "A Longhorn Cinemas account has been created for you." +
                                  "\n\n" + "Love,\n" + "Dan";
                    Emailing.SendEmail(model.Email, "Your Longhorn Cinemas Account", Message);

                    return RedirectToAction("Index", "Home");
                    }
                    AddErrors(result);
                }
            

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Accounts/Register
        [Authorize(Roles = "Manager, Employee")]
        public ActionResult RegisterCustomer()
        {
            return View();
        }

        // NOTE: Here is your logic for registering a new user
        // POST: /Accounts/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult> RegisterCustomer(RegisterViewModel model)
        {
            var age = DateTime.Now.Year - model.Birthday.Year;

            if (age < 13)
            {
                ViewBag.Error = "Customers must be 13 years of age.";
                return View(model);
            }

            List<String> AllEmails = new List<String>();

            foreach (AppUser user in db.Users)
            {
                AllEmails.Add(user.Email);
            }

            if (AllEmails.Contains(model.Email))
            {
                ViewBag.Error = "An account already exists for that email address.";
                return View(model);
            }
            if (ModelState.IsValid)
            {
                //Add fields to user here so they will be saved to do the database
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    MiddleInitial = model.MiddleInitial,
                    LastName = model.LastName,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    Zip = model.Zip,
                    PhoneNumber = model.PhoneNumber,
                    Birthday = model.Birthday

                };
                var result = await UserManager.CreateAsync(user, model.Password);

                //Once you get roles working, you may want to add users to roles upon creation
                await UserManager.AddToRoleAsync(user.Id, "Customer");
                // --OR--
                // await UserManager.AddToRoleAsync(user.Id, "Employee");


                if (result.Succeeded)
                {


                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    String Message = "Hello " + model.FirstName + ",\n" + "a Longhorn Cinemas account has been created for you." +
                                  ".\n\n" + "Love,\n" + "Dan";
                    Emailing.SendEmail(model.Email, "Your Longhorn Cinemas Account", Message);

                    return RedirectToAction("EmployeeHome");
                }
                AddErrors(result);
            }


            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult RegisterEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> RegisterEmployee(RegisterViewModel model)
        {
            var age = DateTime.Now.Year - model.Birthday.Year;

            if (age < 18)
            {
                ViewBag.Error = "Employees must be at least 18 years of age";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                //Add fields to user here so they will be saved to do the database
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    //Firstname is an example - you will need to add the rest
                    FirstName = model.FirstName,
                    MiddleInitial = model.MiddleInitial,
                    LastName = model.LastName,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    Zip = model.Zip,
                    PhoneNumber = model.PhoneNumber,
                    Birthday = model.Birthday

                };
                var result = await UserManager.CreateAsync(user, model.Password);

                //Once you get roles working, you may want to add users to roles upon creation
                //await UserManager.AddToRoleAsync(user.Id, "Customer");
                // --OR--
                 await UserManager.AddToRoleAsync(user.Id, "Employee");


                if (result.Succeeded)
                {
                    ////await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    String Message = "Hello " + model.FirstName + "!\n\ns" + "Welcome to the Longhorn Cinemas family!" +
                                  "\n\n" + "Love,\n" + "Dan";
                    Emailing.SendEmail(model.Email, "Welcome new Longhorn Cinemas Employee!", Message);

                    return RedirectToAction("EmployeeHome");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult ChangeEmployeeStatus()
        {

            ViewBag.AllEmployees = GetAllEmployees();

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult ChangeEmployeeStatus( string id, EmploymentAction EmploymentAction)
        {
            AppUser Employee = db.Users.Find(id);

            if (EmploymentAction == EmploymentAction.Hire)
            {
                String Message = "Hello " + Employee.FirstName + ",\n" + "congratulations on your renewed employment with Longhorn Cinemas." +
                                  "\n\n" + "Love,\n" + "Dan";
                Emailing.SendEmail(Employee.Email, "Welcome Back to Longhorn Cinemas", Message);
                Employee.Archived = false;

            }
            if (EmploymentAction == EmploymentAction.Fire)
            {
                String Message = "Hello " + Employee.FirstName + ",\n\n" + "Your employment with Longhorn Cinemas has ended. We're sorry to hear you're leaving the Longhorn Family." +
                                 "\n\n" + "Love,\n" + "Dan";
                Emailing.SendEmail(Employee.Email, "Goodbye, Longhorn Cinemas", Message);
                Employee.Archived = true;
            }

            if (ModelState.IsValid)
            {
                db.Entry(Employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EmployeeHome");

            }

            ViewBag.AllEmployees = GetAllEmployees();

            // If we got this far, something failed, redisplay form
            return View();
        }

        [Authorize(Roles = "Manager, Employee")]
        public ActionResult ChangeUserProfile()
        {
            ViewBag.AllUsers = GetAllUsers();

            return View();
        }

        [Authorize(Roles = "Manager, Employee")]
        [HttpPost]
        public ActionResult ChangeUserProfile(String Id)
        {
            AppUser user = db.Users.Find(Id);

            return RedirectToAction("ChangeUserInfo", "Accounts", new { id = user.Id });
        }

        //GET: Accounts/Index
        public ActionResult Index()
        {
            IndexViewModel ivm = new IndexViewModel();

            //get user info
            String id = User.Identity.GetUserId();
            AppUser user = db.Users.Find(id);

            //populate the view model
            ivm.Email = user.Email;
            ivm.HasPassword = true;
            ivm.UserID = user.Id;
            ivm.UserName = user.UserName;

            
            ViewBag.FirstName = user.FirstName;
            ViewBag.MiddleInitial = user.MiddleInitial;
            ViewBag.LastName = user.LastName;
            ViewBag.Street = user.Street;
            ViewBag.City = user.City;
            ViewBag.State = user.State;
            ViewBag.Zip = user.Zip;
            ViewBag.Birthday = user.Birthday;
            ViewBag.PopcornPoints = user.PopcornPoints;

            String ccType1 = (CreditCard.GetCreditCardType(user.CreditCardNumber1));
            if (ccType1 != "Invalid")
            {
                ViewBag.CreditCard1 = String.Format("{0}{1}{2}", "**** **** **** ", (user.CreditCardNumber1.Substring(user.CreditCardNumber1.Length - 4, 4)), " " + ccType1);
            }
            else { ViewBag.CreditCard1 = "None"; }

            String ccType2 = (CreditCard.GetCreditCardType(user.CreditCardNumber2));
            if (ccType2 != "Invalid")
            {
                ViewBag.CreditCard2 = String.Format("{0}{1}{2}", "**** **** **** ", (user.CreditCardNumber2.Substring(user.CreditCardNumber2.Length - 4, 4)), " " + ccType2);
            }
            else { ViewBag.CreditCard2 = "None"; }

            //ViewBag.CreditCard1 = user.CreditCardNumber1;
            //ViewBag.CreditCard2 = user.CreditCardNumber2;
            ViewBag.PhoneNumber = user.PhoneNumber;



            return View(ivm);
        }

        public ActionResult ResetPassword()
        {
            ViewBag.AllUsers = GetAllUsers();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model, string Id)
        {
            UserStore<AppUser> store = new UserStore<AppUser>(db);
            String userId = Id; //"<YourLogicAssignsRequestedUserId>";
            String newPassword = model.NewPassword; //"<PasswordAsTypedByUser>";
            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);
            AppUser cUser = await store.FindByIdAsync(userId);
            await store.SetPasswordHashAsync(cUser, hashedNewPassword);
            await store.UpdateAsync(cUser);

            return RedirectToAction("EmployeeHome");

            //string resetToken = await UserManager.GeneratePasswordResetTokenAsync(model.Id);
            //IdentityResult passwordChangeResult = await UserManager.ResetPasswordAsync(model.Id, resetToken, model.NewPassword);
        }


        //Logic for change password
        // GET: /Accounts/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Accounts/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", "Home");
            }
            AddErrors(result);
            return View(model);
        }

        public ActionResult ChangeUserInfo(string id)
        {
            AppUser user = db.Users.Find(id);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUserInfo([Bind(Include = "FirstName, LastName, Street, City, State, Zip, Birthday, PhoneNumber, Email")] AppUser user)
        {
            AppUser AppUser = db.Users.First(u => u.Email == user.Email);
            AppUser.Street = user.Street;
            AppUser.City = user.City;
            AppUser.State = user.State;
            AppUser.Zip = user.Zip;
            AppUser.Birthday = user.Birthday;
            AppUser.PhoneNumber = user.PhoneNumber;

            if (ModelState.IsValid)
            {

                db.Entry(AppUser).State = EntityState.Modified;
                db.SaveChanges();
                if (AppUser.Id != User.Identity.GetUserId())
                {
                    return RedirectToAction("EmployeeHome", "Accounts");
                }

                return RedirectToAction("Index", "Accounts");
            }
            return View(user);
        }

        public ActionResult AddCreditCard(string id)
        {
            AppUser user = db.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCreditCard([Bind(Include = "CreditCardNumber1, CreditCardNumber2, FirstName, LastName, Street, City, State, Zip, Birthday, PhoneNumber, Email")] AppUser user)
        {
            //Find user to change
            AppUser AppUser = db.Users.First(u => u.Email == user.Email);

            //Change other properties
            if (user.CreditCardNumber1 !=null)
            {
                String ccType1 = (CreditCard.GetCreditCardType(user.CreditCardNumber1));

                if(ccType1 == "Invalid")
                {
                    ViewBag.Error = "Invalid card number";
                    return View(user);
                }

                AppUser.CreditCardNumber1 = user.CreditCardNumber1;
            }

            if (user.CreditCardNumber2 != null)
            {
                String ccType2 = (CreditCard.GetCreditCardType(user.CreditCardNumber2));

                if (ccType2 == "Invalid")
                {
                    ViewBag.Error = "Invalid card number";
                    return View(user);
                }

                AppUser.CreditCardNumber2 = user.CreditCardNumber2;
            }



            if (ModelState.IsValid)
            {
                db.Entry(AppUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Accounts");
            }
            return View(user);
        }
        //

        // POST: /Accounts/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region 

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}