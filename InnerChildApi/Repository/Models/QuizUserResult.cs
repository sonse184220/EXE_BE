﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class QuizUserResult
{
    public string QuizUserResultId { get; set; }

    public double? QuizUserResultScore { get; set; }

    public string QuizUserAttemptId { get; set; }

    public string QuizId { get; set; }

    public string ProfileId { get; set; }

    public virtual Profile Profile { get; set; }

    public virtual Quiz Quiz { get; set; }

    public virtual QuizUserAttempt QuizUserAttempt { get; set; }
}