using MASH.Models;
using MASH.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MASH.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly DatabaseDAL _databaseDAL;

        public AdminController(DatabaseDAL databaseDAL)
        {
            _databaseDAL = databaseDAL;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _databaseDAL.GetUsers();

            return View(users);
        }
    }
}