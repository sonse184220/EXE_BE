﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Purchase
{
    public string PurchaseId { get; set; }

    public string UserId { get; set; }

    public string SubscriptionId { get; set; }

    public DateTime? PurchasedAt { get; set; }

    public DateTime? ExpireAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual Subscription Subscription { get; set; }

    public virtual User User { get; set; }
}