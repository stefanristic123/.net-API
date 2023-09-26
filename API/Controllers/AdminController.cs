using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> __userManager;
        public AdminController(UserManager<AppUser> _userManager)
        { 
            __userManager = _userManager;
        }

        [Authorize(Policy = "RequreAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles() {
             var users = await __userManager.Users
             .Include(r => r.UserRoles)
             .ThenInclude(r => r.Role)
             .OrderBy(u => u.UserName)
             .Select(u => new
             {
                 u.Id,
                 Username = u.UserName,
                 Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
             })
                .ToListAsync();

            return Ok(users);
        }


        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

            var selectedRoles = roles.Split(",").ToArray();
            var user = await __userManager.FindByNameAsync(username); // daje listu rola za datog usera

            if (user == null) return NotFound();

            var userRoles = await __userManager.GetRolesAsync(user);
            var result = await __userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));  // dodajemo usera roli

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await __userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await __userManager.GetRolesAsync(user));
        }


        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration(){
            return Ok("Admins or moderators can see this");
        }
        
    }
}