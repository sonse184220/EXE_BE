﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class QuizUserAnswer
{
    public string QuizUserAnswerId { get; set; }

    public string QuizId { get; set; }

    public string QuizQuestionId { get; set; }

    public string QuizAnswerOptionId { get; set; }

    public string QuizUserAttemptId { get; set; }

    public string ProfileId { get; set; }

    public virtual Profile Profile { get; set; }

    public virtual Quiz Quiz { get; set; }

    public virtual QuizAnswerOption QuizAnswerOption { get; set; }

    public virtual QuizQuestion QuizQuestion { get; set; }

    public virtual QuizUserAttempt QuizUserAttempt { get; set; }
}