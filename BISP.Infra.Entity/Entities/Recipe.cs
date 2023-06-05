using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BISP.Infra.Entity.Entities;

[Table("recipe")]
public partial class Recipe
{
    [Key]
    [Column("guid")]
    public Guid Guid { get; set; }

    [Column("item_name")]
    public string ItemName { get; set; } = null!;

    [Column("item_value")]
    public int ItemValue { get; set; }

    [Column("create_at", TypeName = "timestamp without time zone")]
    public DateTime CreateAt { get; set; }

}
