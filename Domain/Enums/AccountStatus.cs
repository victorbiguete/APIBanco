using System.ComponentModel;
namespace APIBanco.Domain.Enums;

public enum AccountStatus
{
    [Description(description: "Active")]
    Active = 0,
    [Description(description: "Inactive")]
    Inactive = 1,
    [Description(description: "Blocked")]
    Blocked = 2,
    [Description(description: "Closed")]
    Closed = 3,
}
