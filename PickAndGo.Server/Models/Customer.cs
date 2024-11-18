using System.ComponentModel.DataAnnotations.Schema;

public class Customer
{
    public int Id { get; set; }

    [Column("first_name")]
    public required string FirstName { get; set; }

    [Column("last_name")]
    public required string LastName { get; set; }

    [Column("email")]
    public required string Email { get; set; }

    [Column("phone")]
    public required string Phone { get; set; }

    [Column("password_hash")]
    public required string PasswordHash { get; set; }

    [Column("role")]
    public string Role { get; set; } = "customer";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public static implicit operator Customer?(Employee? v)
    {
        throw new NotImplementedException();
    }
}
