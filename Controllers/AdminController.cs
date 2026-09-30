using MASH.Models;
using MASH.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MASH.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly DatabaseService _databaseService;

        public AdminController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _databaseService.GetUsers();

            return View(users);
        }
    }
}