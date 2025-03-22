using AccountService.API.Contracts.Requests;
using AccountService.API.Contracts.Responses;
using AccountService.Application.Dto;
using AccountService.Application.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AccountService.API.Controllers;

/// <summary>
/// Controller for managing users.
/// </summary>
[Route("api/users")]
[ApiController]
public class UserController(
    IUserService userService,
    IMapper mapper,
    ILogger<UserController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<UserResponse>> GetUserById(Guid id)
    {
        logger.LogInformation("HTTP GET /api/users/{UserId}", id);

        var userDto = await userService.GetUserByIdAsync(id);
        return Ok(mapper.Map<UserResponse>(userDto));
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    [HttpGet("by-username/{username}")]
    [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<UserResponse>> GetUserByUsername(string username)
    {
        logger.LogInformation("HTTP GET /api/users/by-username/{Username}", username);

        var userDto = await userService.GetUserByUsernameAsync(username);
        return Ok(mapper.Map<UserResponse>(userDto));
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        logger.LogInformation("HTTP POST /api/users | Creating user: {Username}", request.Username);

        var userDto = mapper.Map<UserDto>(request);
        await userService.CreateUserAsync(userDto);

        logger.LogInformation("User '{Username}' created successfully", request.Username);
        return StatusCode((int)HttpStatusCode.Created);
    }
}