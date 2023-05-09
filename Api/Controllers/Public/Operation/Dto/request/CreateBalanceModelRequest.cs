﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.request;

public class CreateBalanceModelRequest
{
    [Required]
    [JsonProperty("NewBalance")]
    public required int NewBalance { get; init; }
}