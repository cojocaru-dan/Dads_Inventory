using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DadsInventory.Authentication;
using DadsInventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DadsInventory.Controllers
{
    public class LoginController : Controller
    {
        private readonly FamilyRepository _familyRepository;
        private readonly BasicAuthenticationHandler _basicAuthenticationHandler;
        public LoginController(FamilyRepository familyRepository, BasicAuthenticationHandler basicAuthenticationHandler)
        {
            _familyRepository = familyRepository;
            _basicAuthenticationHandler = basicAuthenticationHandler;
        }
        public IActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AuthorizeLogin(FamilyMember loginMember)
        {
            var familyMember = _familyRepository
                                        .GetAllMembers()
                                        .Where(member => member.Name == loginMember.Name && member.Password == loginMember.Password)
                                        .FirstOrDefault();
            if (familyMember == null) return NotFound("Wrong Name or Password.");
            _basicAuthenticationHandler.AddClaimsTo(familyMember);
            TempData["loggedInMember"] = familyMember.Name;
            return RedirectToAction("List", "Item"); // View(_familyRepository);
        }
        public IActionResult Logout()
        {
            TempData["loggedInMember"] = null;
            return RedirectToAction("List", "Item");
        }
    }
}