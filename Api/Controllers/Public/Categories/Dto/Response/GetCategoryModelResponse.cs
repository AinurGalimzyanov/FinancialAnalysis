﻿using System.ComponentModel.DataAnnotations;
using Dal.Categories.Entity;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Categories.Dto.Response;

public class GetCategoryModelResponse
{
    [Required] 
    [JsonProperty("Name")] 
    public string name { get; init; }
    
    [Required] 
    [JsonProperty("Id")] 
    public Guid Id { get; init; }
    
    [Required] 
    [JsonProperty("Type")] 
    public string Type { get; init; }

    [Required] [JsonProperty("Sum")] public int? Sum { get; init; } = 0;

    public GetCategoryModelResponse(string name, Guid id, string type, int? sum)
    {
        this.name = name;
        Id = id;
        Type = type;
        Sum = sum;  
    }
}