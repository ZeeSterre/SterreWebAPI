using SterreWebApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Object2D
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string PrefabId { get; set; } = string.Empty;

    [Range(-10000, 10000)]
    public float? PositionX { get; set; }

    [Range(-10000, 10000)]
    public float? PositionY { get; set; }

    [Range(0.1, 10.0)]
    public float? ScaleX { get; set; }

    [Range(0.1, 10.0)]
    public float? ScaleY { get; set; }

    [Range(-360, 360)]
    public float? RotationZ { get; set; }

    public int? SortingLayer { get; set; }

    [Required]
    public Guid Environment2D_Id { get; set; }

    [ForeignKey("Environment2D_Id")]
    public virtual Environment2D Environment2D { get; set; } = null!;
}

public class CreateObject2DRequest
{
    [Required]
    public string PrefabId { get; set; } = string.Empty;

    public float? PositionX { get; set; }
    public float? PositionY { get; set; }
    public float? ScaleX { get; set; }
    public float? ScaleY { get; set; }
    public float? RotationZ { get; set; }
    public int? SortingLayer { get; set; }
    [Required]
    public Guid Environment2D_Id { get; set; }
}
