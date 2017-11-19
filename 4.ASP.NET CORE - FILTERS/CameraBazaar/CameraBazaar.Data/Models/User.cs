using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CameraBazaar.Data.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<Camera> Cameras { get; set; } = new HashSet<Camera>();

        public DateTime LastLogin { get; set; }
    }
}
