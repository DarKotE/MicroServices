using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlatformService.Models;

public sealed record Platform(
    [property: Key, Required] Guid Id,
    [property: Required] string Name,
    [property: Required] string Publisher,
    [property: Required, Column(TypeName="Money")] decimal Cost
);


