﻿using Microsoft.AspNetCore.Mvc;

using Orders.Backend.UnitsOfWork.Interfaces;

namespace Orders.Backend.Controllers;

public class GenericController<T> : Controller where T : class
{
    private readonly IGenericUnitOfWork<T> _unitOfWork;

    public GenericController(IGenericUnitOfWork<T> unitOfWork)
	{
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAsync()
    {
        var response = await _unitOfWork.GetAsync();
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetAsync(int id)
    {
        var response = await _unitOfWork.GetAsync(id);
        if (!response.WasSuccess)
        {
            return NotFound(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpPost]
    public virtual async Task<IActionResult> PostAsync(T entity)
    {
        var response = await _unitOfWork.AddAsync(entity);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }
    [HttpPut]
    public virtual async Task<IActionResult> PutAsync(T entity)
    {
        var response = await _unitOfWork.UpdateAsync(entity);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _unitOfWork.DeleteAsync(id);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        //return Ok(response.Result);
        return NoContent();
    }
}
