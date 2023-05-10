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
        var name = await _operationManager.GetNameCategoryByCategoryId(model.CategoryId);
        return Ok(new GetOperationModelResponse(newOperation.Id, newOperation.Price, newOperation.DateTime, name));
    }
    
    [HttpPut("update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateOperation([FromBody] UpdateOperationModelRequest model)
    {
        var newOperation = _mapper.Map<OperationDal>(model);
        await _operationManager.UpdateAsync(newOperation);
        var name = await _operationManager.GetNameCategory(newOperation.Id);
        return Ok(new GetOperationModelResponse(newOperation.Id, newOperation.Price, newOperation.DateTime, name));
    }
    
    [HttpGet("getOperation/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetOperation([FromRoute] Guid id)
    {
        var operation = await _operationManager.GetAsync(id);
        var name = await _operationManager.GetNameCategory(operation.Id);
        return operation != null ? 
            Ok(new GetOperationModelResponse(operation.Id, operation.Price, operation.DateTime, name)) : BadRequest();
    }
    
    [HttpPost("getOperationByCategory")]
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
    
    [HttpPost("getOperationsBothType")]
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
    
    [HttpPost("getLastFiveOperations")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetLastFiveOperations([FromBody] GetLastFiveOperationsModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var lastFiveOperation = await _operationManager.GetLastFiveOperationsAsync(token, model.DateTime);
        var result = new List<GetOperationModelResponse>();
        foreach (var i in lastFiveOperation)
        {
            result.Add(new GetOperationModelResponse(i.Id, i.Price, i.DateTime, await _operationManager.GetNameCategory(i.Id)));
        }
        /*var result = lastFiveOperation
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();*/
        return result != null  ? 
            Ok(new GetLastFiveOperationsModelResponse(result)) : BadRequest();
    }
    
    
    [HttpPost("getLastFiveOperationsBothTypeAsync")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetLastFiveOperationsBothTypeAsync([FromBody] GetOperationsBothTypeModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var lastFiveOperationIncome = await _operationManager.GetLastFiveOperationsBothTypeAsync(token, "income", model.DateTime);
        var lastFiveOperationExpenses = await _operationManager.GetLastFiveOperationsBothTypeAsync(token, "expenses", model.DateTime);
        var resultIncome = new List<GetOperationModelResponse>();
        foreach (var i in lastFiveOperationIncome)
        {
            resultIncome.Add(new GetOperationModelResponse(i.Id, i.Price, i.DateTime, await _operationManager.GetNameCategory(i.Id)));
        }
        var resultExpeses = new List<GetOperationModelResponse>();
        foreach (var i in lastFiveOperationExpenses)
        {
            resultExpeses.Add(new GetOperationModelResponse(i.Id, i.Price, i.DateTime, await _operationManager.GetNameCategory(i.Id)));
        }
        /*var resultIncome = lastFiveOperationIncome
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();
        var resultExpeses = lastFiveOperationExpenses
            .Select(x => _mapper.Map<GetOperationModelResponse>(x))
            .ToList();*/
        return resultIncome != null && resultExpeses != null? 
            Ok(new GetOperationsByTypeModelResponse(resultIncome, resultExpeses)) : BadRequest();
    }

    [HttpPost("getSumByCategory")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetSumByCategory([FromBody] SumByTypeCategoryModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var operations = await _operationManager.GetOperationsByCategoryAsync(token, model.CategoryId, model.DateTime);
        var sum = operations.Select(x => x.Price).Sum();
        return sum != null ? Ok(new GetSumByTypeCategoryModelResponse(sum)) : BadRequest();
    }
    
    [HttpPost("getSumByType")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetSumByType([FromBody] GetSumByTypeModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var operations = await _operationManager.GetOperationsByTypeAsync(token, model.Type, DateTime.UtcNow);
        var sum = operations.Select(x => x.Price).Sum();
        return sum != null ? Ok(new GetSumByTypeCategoryModelResponse(sum)) : BadRequest();
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