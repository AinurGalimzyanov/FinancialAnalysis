using Api.Controllers.Operation.Dto.request;
using Api.Controllers.Operation.Dto.response;
using Api.Controllers.Public.Base;
using AutoMapper;
using Dal.Operation.Entity;
using Logic.Managers.Operation.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Operation;

public class OperationController : BasePublicController
{
    private readonly IOperationManager _operationManager;
    private readonly IMapper _mapper;

    public OperationController(IOperationManager operationManager, IMapper mapper)
    {
        _operationManager = operationManager;
        _mapper = mapper;
    }
    
    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateOperation([FromBody] CreateOperationModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var newOperation = _mapper.Map<OperationDal>(model);
        await _operationManager.CreateOperation(token, newOperation, model.CategoryId);
        return Ok(newOperation.Id);
    }
    
    [HttpPost("update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateOperation([FromBody] UpdateOperationModelRequest model)
    {
        var newOperation = _mapper.Map<OperationDal>(model);
        await _operationManager.UpdateAsync(newOperation);
        return Ok();
    }
    
    [HttpGet("getOperation/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetOperation([FromRoute] Guid id)
    {
        var operation = await _operationManager.GetAsync(id);
        return operation != null ? 
            Ok(new GetOperationModelResponse(operation.Id, operation.Price, operation.DateTime)) : BadRequest();
    }
    
    [HttpGet("getOperationByTypeCategory")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetOperationByTypeCategory([FromBody] GetOperationByTypeModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var operations = await _operationManager.GetOperationByType(token, model.CategoryId, model.Type);
        var result = operations
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        return result != null ? Ok(new GetOperationsModelResponse(result)) : BadRequest();
    }
    
    [HttpGet("getOperationByType")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetOperationByType([FromBody] GetOperationByTypeModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var operations = await _operationManager.GetOperationByType(token, model.CategoryId, model.Type);
        var result = operations
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        return result != null ? Ok(new GetOperationsModelResponse(result)) : BadRequest();
    }
    
    [HttpGet("getAllOperationCategory/{categoryId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllOperationCategory([FromRoute] Guid categoryId)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var operations = await _operationManager.GetAllOperation(token, categoryId);
        var result = operations
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        return result != null ? Ok(new GetOperationsModelResponse(result)) : BadRequest();
    }
    
    [HttpGet("getSumByTypeCategory")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetSumByTypeCategory([FromBody] GetOperationByTypeModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var operations = await _operationManager.GetOperationByType(token, model.CategoryId, model.Type);
        var result = operations
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        return result != null ? Ok(result.Select(x => x.Price).Sum()) : BadRequest();
    }
    
}