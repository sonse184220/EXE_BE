﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class QuizCategory
{
    public string QuizCategoryId { get; set; }

    public string QuizCategoryName { get; set; }

    public string QuizCategoryDescription { get; set; }

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}