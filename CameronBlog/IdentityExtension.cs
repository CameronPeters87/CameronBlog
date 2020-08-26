using CameronBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace CameronBlog
{
    public class IdentityExtension
    {
        public static string GetUserFirstName(IIdentity identity)
        {
            var db = ApplicationDbContext.Create();
            var user = db.Users.FirstOrDefault(u => u.Email.Equals(identity.Name));

            return user != null ? user.FirstName : String.Empty;
        }

    }
}