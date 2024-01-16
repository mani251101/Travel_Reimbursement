using app.Areas.Identity.Data;
using application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace emp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolemanager, ApplicationDbContext dbContext,
        IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _rolemanager = rolemanager;
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Profile()
        {
            int adminCount=0;
            int empcount = 0;
            int managercount = 0;
           var userlist=_userManager.Users.ToList();
           foreach (var user in userlist)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    adminCount++;
                }
                if (await _userManager.IsInRoleAsync(user, "Employee"))
                {
                    empcount++;
                }
                if (await _userManager.IsInRoleAsync(user, "Manager"))
                {
                    managercount++;
                }
            }
            ViewBag.count = adminCount;
            ViewBag.employeecount = empcount;
            ViewBag.mancount = managercount;
            var name = HttpContext.Session.GetString("email");
            var users = await _userManager.GetUserAsync(User);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        public ActionResult Employeelist()
        {
            var users = _dbContext.userlist.ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult EditEmployee(string Id )
        {
            var employee = _dbContext.userlist.FirstOrDefault(x=>x.Id == Id);
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(string Id, ApplicationUser user)
        {
            var emp = _dbContext.userlist.FirstOrDefault(x=>x.Id == Id);
            if(emp!=null)
            {
                emp.Firstname = user.Firstname;
                emp.Lastname = user.Lastname;
                emp.DateofBirth = user.DateofBirth;
                emp.PhoneNumber = user.PhoneNumber;
                emp.Gender = user.Gender;
                emp.Email = user.Email;
                if (user.ProfilePictureFile != null && user.ProfilePictureFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(user.ProfilePictureFile.FileName);
                        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "Images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await user.ProfilePictureFile.CopyToAsync(stream);
                        }

                        emp.ProfilePicture = "/Images/" + fileName;
                    }
                 _dbContext.userlist.Update(emp);
                _dbContext.SaveChanges();
            }
            return View(nameof(EditEmployee));
        }

        public ActionResult Delete(string Id)
        {
             var employee = _dbContext.userlist.SingleOrDefault(x=>x.Id == Id);
            if(employee!=null)
            {
                _dbContext.userlist.Remove(employee);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Employeelist");
        }

        public ActionResult Loginstatus()
        {
            string logFilePath = "C:/Users/HP/Desktop/Review/scaflogin/app/Log.txt"; 
            string[] logLines = System.IO.File.ReadAllLines(logFilePath);
            ViewBag.LogLines = logLines;
            return View();
        }
        
        public ActionResult ClearLog()
        {
            string logFilePath = "C:/Users/HP/Desktop/Review/scaflogin/app/Log.txt";
            System.IO.File.WriteAllText(logFilePath, string.Empty);
            return RedirectToAction("Loginstatus");
        }

        public FileContentResult DownloadLog()
        {
            string logFilePath = "C:/Users/HP/Desktop/Review/scaflogin/app/Log.txt";
            byte[] fileBytes = System.IO.File.ReadAllBytes(logFilePath);
            string fileName = "log.txt";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}