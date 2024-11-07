using AutoMapper;
using Core.Dtos.Identity;
using Core.Entities.Identity;
using Core.Services.Contracts;
using demo.Errors;
using demo.Extenstions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace demo.Controllers;
public class AccountsController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountsController(
        IUserService userService,
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userService = userService;
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(Ok<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedHttpResult), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        if (loginDto is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid data!"));
        var user = await _userService.LoginAsync(loginDto);
        if (user is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
        return Ok(user);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(Ok<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedHttpResult), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (registerDto is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid data!"));
        var user = await _userService.RegisterAsync(registerDto);
        if (user is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
        return Ok(user);
    }

    [Authorize]
    [HttpGet("GetCurrentUser")]
    [ProducesResponseType(typeof(Ok<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (userEmail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

        return Ok(new UserDto()
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = await _tokenService.CreateTokenAsync(user, _userManager)
        });
    }

    [Authorize]
    [HttpGet("Address")]
    [ProducesResponseType(typeof(Ok<AddressDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequest<ApiErrorResponse>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
    {

        var user = await _userManager.FindByEmailWithAddressAsync(User);
        if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

        var mappedAddress = _mapper.Map<AddressDto>(user.Address);

        return Ok(mappedAddress);
    }


}
