using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Range(1, 25, ErrorMessage = "Username must be between 1-25 characters")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string NormalizedUserName { get; set; } = string.Empty;

    public virtual ICollection<UserClaim> UserClaims { get; set; } = new HashSet<UserClaim>();
}