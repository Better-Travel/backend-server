using System.ComponentModel.DataAnnotations;

namespace BetterTravel.Infrastructure.Warsaw;

internal record WarsawClientOptions
{
    public const string Name = "Cities:Warsaw";
    
    [Required(ErrorMessage = "Token is required")]
    public required string Token { get; init; }
}