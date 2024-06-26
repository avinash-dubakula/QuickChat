﻿using Microsoft.AspNetCore.Identity;

namespace QuickChat.BusinessLogicLayer.Models.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? ProfilePhotoUrl { get; set; }
    }
}
