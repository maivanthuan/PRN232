﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models;

public partial class Tag
{
    [Key]
    public int TagId { get; set; }

    public string? TagName { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
