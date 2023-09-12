using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class UserLike
    {
        public AppUser SourceUser { get; set; }  // User koji lajkuje drugog usera
        public int SourceUserId { get; set; }
        public AppUser LikedUser { get; set; } // User koji je lajkovan
        public int LikedUserId { get; set; }

    }
}