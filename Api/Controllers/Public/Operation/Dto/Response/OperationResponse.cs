﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Logic.Managers.Operation.Dto.Response;

public class OperationResponse
{
    [Required]
    [JsonProperty("Id")]
    public Guid Id { get; init; }
    
    [Required]
    [JsonProperty("Price")]
    public int? Price { get; init; }
    
    [Required]
    [JsonProperty("DateTime")]
    public DateTime? DateTime { get; init; }
    
    [Required]
    [JsonProperty("NameCategory")]
    public string? NameCategory { get; init; }

    public OperationResponse(Guid id, int? price, DateTime? dateTime, string? nameCategory)
    {
        Id = id;
        Price = price;
        DateTime = dateTime;
        NameCategory = nameCategory;
    }
}