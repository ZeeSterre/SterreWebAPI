using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class UserClaim
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string ClaimType { get; set; } = string.Empty;

    [Required]
    [StringLength(250)]
    public string ClaimValue { get; set; } = string.Empty;
}