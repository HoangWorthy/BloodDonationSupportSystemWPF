using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Blog
{
    public long Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Thumbnail { get; set; }

    public long AuthorId { get; set; }

    public string? Status { get; set; }

    public DateOnly? CreationDate { get; set; }

    public DateOnly? LastModifiedDate { get; set; }

    public virtual Account Author { get; set; } = null!;
}
