﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class User
{
    public string UserId { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string FullName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Gender { get; set; }

    public string PhoneNumber { get; set; }

    public string ProfilePicture { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public string Status { get; set; }

    public bool? Verified { get; set; }

    public string RoleId { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual Role Role { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}