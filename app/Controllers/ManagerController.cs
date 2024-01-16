using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reimbursementform.Models;
using scaflogindbcontext;

namespace managercontrol
{
    [Authorize(Roles ="Manager")]
    public class ManagerController : Controller
    {
        private readonly TravelDbContext dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public ManagerController(TravelDbContext context, UserManager<IdentityUser> userManager)
        {
            dbContext = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Profile()
        {
            var emp = dbContext.reimbursementforms.ToList();
            var count = emp.Where(emp=>emp.Status=="Pending").Count();   
            ViewBag.Pending = count;         
            var users = await _userManager.GetUserAsync(User);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }
        public IActionResult Inbox()
        {
            var employees = dbContext.reimbursementforms.ToList();
            IEnumerable<Reimbursementforms> add = from emp in employees where emp.Status=="Pending" select emp;
            return View(add);
        }

        public IActionResult Reimbursementdetails(int? id)
        {
            var reimbursement = dbContext.reimbursementforms.SingleOrDefault(m => m.FormId == id);
            return View(reimbursement);
        }

        public ActionResult Approved(int id)
        {
            var reimbursement = dbContext.reimbursementforms.Find(id);
            if(reimbursement != null)
            {
                reimbursement.Status = "Approved";
                dbContext.reimbursementforms.Update(reimbursement);
                dbContext.SaveChanges();
                return RedirectToAction("Inbox", "Manager");
            }
            return RedirectToAction("Inbox", "Manager");
        }

        public ActionResult Rejected(int id)
        {
            var reimbursement = dbContext.reimbursementforms.Find(id);
            if(reimbursement != null)
            {
                reimbursement.Status = "Rejected";
                dbContext.SaveChanges();
                return RedirectToAction("Inbox", "Manager");
            }
            return RedirectToAction("Inbox", "Manager");
        }
    }
}