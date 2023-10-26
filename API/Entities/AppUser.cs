using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string? KnownAs { get; set; }
        public DateTime? Created { get; set; }  = DateTime.UtcNow;
        public DateTime? LastActive { get; set; } = DateTime.UtcNow;
        public string? Gender { get; set; }  
        public string? LookingFor { get; set; }  
        public string? City { get; set; }    
        public string? Country { get; set; }
        public ICollection<Photo>? Photos { get; set; }
        public List<UserLike> LikedByUsers { get; set; } // Ovo pokazuje ko je sve lajkovao trenutno ulogovanog korisnika
        public List<UserLike> LikedUsers { get; set; } // Korisnici koje je ulogovani korisnik lajkovani
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessageRecived { get; set; }
        public ICollection<AppUserRole>? UserRoles { get; set; }
    
    }
}



