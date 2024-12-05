﻿using App.Domain.Entities.Common;
using App.Repositories.Products;

namespace App.Domain.Entities;

public class Category : BaseEntity<int>, IAuditEntity
{
    public string Name { get; set; } = default!;
    public List<Product>? Products { get; set; } //Navigation Property
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}