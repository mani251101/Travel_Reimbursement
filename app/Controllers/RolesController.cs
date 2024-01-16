using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace app.Controllers;

[Authorize(Roles ="Admin")]
public class RolesController : Controller
{
    private readonly RoleManager<IdentityRole> _rolemanager;
    public RolesController(RoleManager<IdentityRole> rolemanager)
    {
        _rolemanager = rolemanager;
    }

    public IActionResult Index()
    {
        var roles = _rolemanager.Roles;
        return View(roles);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(IdentityRole roles)
    {
        if(!_rolemanager.RoleExistsAsync(roles.Name).GetAwaiter().GetResult())
        {
            _rolemanager.CreateAsync(new IdentityRole(roles.Name)).GetAwaiter().GetResult();
        }
        return RedirectToAction(nameof(Index));
    }

}