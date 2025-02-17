using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SterreWebApi.Models;

public class Environment2D
{
    [Key]

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, 10000, ErrorMessage = "MaxLength should be between 0 and 10000")]
    public int MaxLength { get; set; }

    [Required]
    [Range(0, 10000, ErrorMessage = "MaxHeight should be between 0 and 10000")]
    public int MaxHeight { get; set; }
}
