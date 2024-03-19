using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Dtos;

public class BankAccountRequestDto
{
    [Required]
    public AccountStatus Status { get; set; }
}
