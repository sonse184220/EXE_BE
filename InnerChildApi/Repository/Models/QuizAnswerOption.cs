﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class QuizAnswerOption
{
    public string QuizAnswerOptionId { get; set; }

    public string QuizContent { get; set; }

    public double? QuizScoreValue { get; set; }

    public string QuestionId { get; set; }

    public virtual QuizQuestion Question { get; set; }

    public virtual ICollection<QuizUserAnswer> QuizUserAnswers { get; set; } = new List<QuizUserAnswer>();
}