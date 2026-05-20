using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models.Common;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

public abstract class CrudController<TModel, TRequest, TResponse>(ICrudService<TModel> service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedApiResponse<IReadOnlyList<object>>>> Get([FromQuery] ListQueryRequest query, CancellationToken cancellationToken)
    {
        var result = await service.GetAsync(query.ToBusinessQuery(), cancellationToken);
        var responseItems = result.Items.Select(ToResponse).ToList();

        return Ok(new PagedApiResponse<IReadOnlyList<object>>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = FieldSelector.SelectCollection(responseItems, query.Fields),
            Pagination = result.Pagination,
            Errors = null
        });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        if (result is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Resource was not found",
                Data = null,
                Errors = new[] { $"No resource exists with id {id}." }
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = ToResponse(result),
            Errors = null
        });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<TResponse>>> Create([FromBody] TRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<TResponse>
            {
                Success = false,
                Message = "Invalid request",
                Data = default,
                Errors = ModelState
            });
        }

        var created = await service.CreateAsync(ToModel(request), cancellationToken);
        var response = ToResponse(created);

        return CreatedAtAction(nameof(GetById), new { id = GetId(response) }, new ApiResponse<TResponse>
        {
            Success = true,
            Message = "Resource created successfully",
            Data = response,
            Errors = null
        });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] TRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid request",
                Data = null,
                Errors = ModelState
            });
        }

        var updated = await service.UpdateAsync(id, ToModel(request), cancellationToken);
        if (!updated)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Resource was not found",
                Data = null,
                Errors = new[] { $"No resource exists with id {id}." }
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Resource updated successfully",
            Data = new { id },
            Errors = null
        });
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Resource was not found",
                Data = null,
                Errors = new[] { $"No resource exists with id {id}." }
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Resource deleted successfully",
            Data = new { id },
            Errors = null
        });
    }

    protected abstract TModel ToModel(TRequest request);
    protected abstract TResponse ToResponse(TModel model);
    protected abstract int GetId(TResponse response);
}
