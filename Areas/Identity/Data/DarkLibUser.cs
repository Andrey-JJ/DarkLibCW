using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DarkLibCW.Areas.Identity.Data;

// Add profile data for application users by adding properties to the DarkLibUser class
public class DarkLibUser : IdentityUser
{
    public string LastName { get; set; }
    public string Name { get; set; }
    public string MidName { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
}

