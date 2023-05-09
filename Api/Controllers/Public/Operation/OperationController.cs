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
        return Ok(new GetOperationModelResponse(newOperation.Id, newOperation.Price, newOperation.DateTime));
    }
    
    [HttpPut("update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateOperation([FromBody] UpdateOperationModelRequest model)
    {
        var newOperation = _mapper.Map<OperationDal>(model);
        await _operationManager.UpdateAsync(newOperation);
        return Ok(new GetOperationModelResponse(newOperation.Id, newOperation.Price, newOperation.DateTime));
    }
    
    [HttpGet("getOperation/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetOperation([FromRoute] Guid id)
    {
        var operation = await _operationManager.GetAsync(id);
        return operation != null ? 
            Ok(new GetOperationModelResponse(operation.Id, operation.Price, operation.DateTime)) : BadRequest();
    }
    
    [HttpGet("getOperationByCategory")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetOperationByCategory([FromBody] OperationsByCategoryModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var operations = await _operationManager.GetOperationsByCategoryAsync(token, model.CategoryId, model.Date);
        var result = operations
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        return result != null ? 
            Ok(new GetOperationsByCategoryModelResponse(result)) : BadRequest();
    }
    
    [HttpGet("getOperationsBothType")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetOperationsBothType([FromBody] GetOperationsBothTypeModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var operationsIncome = await _operationManager.GetOperationsByTypeAsync(token, "income", model.DateTime);
        var operationsExpenses = await _operationManager.GetOperationsByTypeAsync(token, "expenses", model.DateTime);
        var resultIncome = operationsIncome
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        var resultExpeses = operationsExpenses
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        return resultIncome != null && resultExpeses != null? 
            Ok(new GetOperationsByTypeModelResponse(resultIncome, resultExpeses)) : BadRequest();
    }
    
    [HttpGet("getLastFiveOperations")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetLastFiveOperations([FromBody] GetLastFiveOperationsModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var lastFiveOperation = await _operationManager.GetLastFiveOperationsAsync(token, model.DateTime);
        var result = lastFiveOperation
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        return result != null  ? 
            Ok(new GetLastFiveOperationsModelResponse(result)) : BadRequest();
    }
    
    
    [HttpGet("getLastFiveOperationsBothTypeAsync")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetLastFiveOperationsBothTypeAsync([FromBody] GetOperationsBothTypeModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var lastFiveOperationIncome = await _operationManager.GetLastFiveOperationsBothTypeAsync(token, "income", model.DateTime);
        var lastFiveOperationExpenses = await _operationManager.GetLastFiveOperationsBothTypeAsync(token, "expenses", model.DateTime);
        var resultIncome = lastFiveOperationIncome
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        var resultExpeses = lastFiveOperationExpenses
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        return resultIncome != null && resultExpeses != null? 
            Ok(new GetOperationsByTypeModelResponse(resultIncome, resultExpeses)) : BadRequest();
    }

    [HttpGet("getSumByCategory")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetSumByCategory([FromBody] SumByTypeCategoryModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var operations = await _operationManager.GetOperationsByCategoryAsync(token, model.CategoryId, model.DateTime);
        var sum = operations.Select(x => x.Price).Sum();
        return sum != null ? Ok(new GetSumByTypeCategoryModelResponse(sum)) : BadRequest();
    }
    
    [HttpGet("getSumByType")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetSumByType([FromBody] GetOperationByTypeModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var operations = await _operationManager.GetOperationsByTypeAsync(token, model.Type, model.DateTime);
        var result = operations
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        var sum = result.Select(x => x.Price).Sum();
        return result != null ? Ok(new GetSumByTypeCategoryModelResponse(sum)) : BadRequest();
    }
    
    [HttpGet("getBalance")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetBalance()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var balance = await _operationManager.GetBalanceAsync(token);
        return balance != null ? Ok(new GetBalanceModelResponse(balance)) : BadRequest();
    }

    [HttpPatch("createBalance")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateBalance([FromBody] CreateBalanceModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        await _operationManager.CreateBalanceAsync(token, model.NewBalance);
        return Ok();
    }
    
}