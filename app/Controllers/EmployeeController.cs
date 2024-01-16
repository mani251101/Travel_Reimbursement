using System.ComponentModel.DataAnnotations;
using filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using Microsoft.EntityFrameworkCore;
using Reimbursementform.Models;
using scaflogindbcontext;
#nullable disable

namespace emp.Controllers
{
    [Authorize(Roles ="Employee")]
    public class EmployeeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TravelDbContext dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        public EmployeeController(UserManager<IdentityUser> userManager, TravelDbContext context, 
        IWebHostEnvironment hostEnvironment )
        {
            _userManager = userManager;
            dbContext = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Profile()
        {
            var employee = HttpContext.Session.GetString("email");
            var emp = dbContext.reimbursementforms.Where(emp=>emp.EmailId==employee).ToList();
            var count = emp.Where(emp=>emp.Status=="Pending").Count();
            var approvecount = emp.Where(emp=>emp.Status=="Approved").Count();
            var rejectcount = emp.Where(emp=>emp.Status=="Rejected").Count();
            ViewBag.Pending = count;
            ViewBag.Approved = approvecount;
            ViewBag.Rejected = rejectcount;

            var users = await _userManager.GetUserAsync(User);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        [HttpGet]
          public IActionResult Reimbursementform()
          {
            Reimbursementforms reimbursementforms = new Reimbursementforms();
            return View(reimbursementforms);
          }

         private string UploadFile(Reimbursementforms forms)
         {
            string uniqueFileName = null;
            if(forms.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "Receipt");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + forms.Image.FileName;
                string filepath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    forms.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
         }

         
         [HttpPost]
         [Reimbursementfilters]
          public IActionResult Reimbursementform(Reimbursementforms forms)
         {
                if(ModelState.IsValid)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(forms.Description))
                        {
                            throw new ValidationException(" Please Enter the Description.");
                        }
                        HttpContext.Session.SetString("email", forms.EmailId);
                        HttpContext.Session.SetString("name", forms.Name);
                        string uniqueFileName = UploadFile(forms);
                        forms.Receipt = uniqueFileName;
                        forms.Status = "Pending";
                        dbContext.Attach(forms);
                        dbContext.Entry(forms).State = EntityState.Added;
                        dbContext.SaveChanges();
                    }
                    catch(ValidationException ex)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                        return View("Error");
                    }
                    
                }
                return RedirectToAction(nameof(Reimbursementform));

         }
        public IActionResult Myrequest()
        {
            var name = HttpContext.Session.GetString("email");
            var employee = dbContext.reimbursementforms.ToList();
            IEnumerable<Reimbursementforms> add = from emp in employee where emp.EmailId == name select emp;
            return View(add);
        }
    }
}