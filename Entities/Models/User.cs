﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class User : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }


        public string ?UrlPicture { get; set; }
        public string ?Bio { get; set; }
        public string? Discriminator { get; set; }
        public bool IsBanned { get; set; }=false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? RefreshToken { get; set; }
        public string? SubscriptionId { get; set; }
		public DateTime RefreshTokenExpiryTime { get; set; }
        public University University { get; set; }
        public Company Company { get; set; }
        public Student Student { get; set; }
        public Admin Admin { get; set; }
        public virtual ICollection<UserCode> userCodes { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public ICollection<ChatUsers> ChatMemberships { get; set; }

    }
}
