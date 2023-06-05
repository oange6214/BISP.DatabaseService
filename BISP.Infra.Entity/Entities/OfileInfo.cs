using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BISP.Infra.Entity.Entities;

[Table("ofile_info")]
public partial class OfileInfo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("guid")]
    public Guid? Guid { get; set; }

    [Column("attributes")]
    public string? Attributes { get; set; }

    [Column("create_time", TypeName = "timestamp without time zone")]
    public DateTime? CreateTime { get; set; }

    [Column("create_time_utc", TypeName = "timestamp without time zone")]
    public DateTime? CreateTimeUtc { get; set; }

    [Column("directory")]
    public string? Directory { get; set; }

    [Column("directory_name")]
    public string? DirectoryName { get; set; }

    [Column("exists")]
    public bool? Exists { get; set; }

    [Column("extension")]
    public string? Extension { get; set; }

    [Column("full_name")]
    public string? FullName { get; set; }

    [Column("full_path")]
    public string? FullPath { get; set; }

    [Column("is_readonly")]
    public bool? IsReadonly { get; set; }

    [Column("last_access_time", TypeName = "timestamp without time zone")]
    public DateTime? LastAccessTime { get; set; }

    [Column("last_access_time_utc", TypeName = "timestamp without time zone")]
    public DateTime? LastAccessTimeUtc { get; set; }

    [Column("last_write_time", TypeName = "timestamp without time zone")]
    public DateTime? LastWriteTime { get; set; }

    [Column("last_write_time_utc", TypeName = "timestamp without time zone")]
    public DateTime? LastWriteTimeUtc { get; set; }

    [Column("length")]
    public int? Length { get; set; }

    [Column("link_target")]
    public string? LinkTarget { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("original_path")]
    public string? OriginalPath { get; set; }

    [Column("unix_file_mode")]
    public int? UnixFileMode { get; set; }
}
