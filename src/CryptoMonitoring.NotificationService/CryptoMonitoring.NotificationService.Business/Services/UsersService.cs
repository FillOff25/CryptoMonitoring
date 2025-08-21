using AutoMapper;
using CryptoMonitoring.Common.DTOs.Auth;
using CryptoMonitoring.Common.Exceptions;
using CryptoMonitoring.Common.Interfaces.Auth;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.Common.Services.Auth;
using CryptoMonitoring.Models.Entities;
using CryptoMonitoring.NotificationService.Business.Interfaces;

namespace CryptoMonitoring.NotificationService.Business.Services;

public class UsersService : IUsersService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public UsersService(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    public async Task RegisterUserAsync(RegisterUserRequestDto dto)
    {
        if (await _unitOfWork.Users.GetByEmailAsync(dto.Email) != null)
        {
            throw new HttpException("User with that email already exist", 400);
        }

        if (await _unitOfWork.Users.GetByTelegramIdAsync(dto.TelegramId) != null)
        {
            throw new HttpException("User with that telegram ID already exist", 400);
        }

        var user = _mapper.Map<User>(dto);

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync();
    }

    public async Task<string> LoginUserAsync(LoginUserRequestDto dto)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);

        if (user == null || !user.IsActive)
        {
            throw new HttpException("User not found", 400);
        }

        if (!HasherService.Verify(dto.Password, user.PasswordHash))
        {
            throw new HttpException("Login failed", 400);
        }

        var token = _jwtService.GenerateToken(user);

        return token;
    }

    public async Task UpdateRoleAsync(UpdateRoleRequestDto dto)
    {
        if (dto.Role is not ("User" or "Admin"))
        {
            throw new HttpException("Role must be either 'User' or 'Admin'", 400);
        }

        var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);

        if (user == null || !user.IsActive)
        {
            throw new HttpException("User not found", 400);
        }

        user.Role = dto.Role;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveAsync();
    }
}
