﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Transaction
{
    public string TransactionId { get; set; }

    public string TransactionPaymentGateway { get; set; }

    public double? TransactionAmount { get; set; }

    public string TransactionStatus { get; set; }

    public string TransactionCode { get; set; }

    public DateTime? TransactionCreatedAt { get; set; }

    public string PurchaseId { get; set; }

    public virtual Purchase Purchase { get; set; }
}