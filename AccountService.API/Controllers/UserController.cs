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
public class UserController(IUserService userService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user data.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<UserResponse>> GetUserById(Guid id)
    {
        var userDto = await userService.GetUserByIdAsync(id);
        return Ok(mapper.Map<UserResponse>(userDto));
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The user data.</returns>
    [HttpGet("by-username/{username}")]
    [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<UserResponse>> GetUserByUsername(string username)
    {
        var userDto = await userService.GetUserByUsernameAsync(username);
        return Ok(mapper.Map<UserResponse>(userDto));
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The request model containing the username and password.</param>
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var userDto = mapper.Map<UserDto>(request);

        await userService.CreateUserAsync(userDto);
        return StatusCode((int)HttpStatusCode.Created);
    }
}