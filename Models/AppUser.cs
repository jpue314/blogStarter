using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;



public class AppUser : IdentityUser
{
    public string? GuestUser { get; set; }
    public IEnumerable<Blog> Blogs { get; set; }

}
