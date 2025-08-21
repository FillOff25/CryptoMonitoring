using CryptoMonitoring.Common.DTOs.NotificationService;
using CryptoMonitoring.Common.Exceptions;
using CryptoMonitoring.Common.Persistence.Interfaces;
using CryptoMonitoring.NotificationService.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using Telegram.Bot;

namespace CryptoMonitoring.NotificationService.Business.Services;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SmtpClient _smtpClient;
    private readonly TelegramBotClient _botClient;

    private readonly string _smtpFromEmail;

    public NotificationService(
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _smtpClient = new SmtpClient()
        {
            Host = configuration["SMTP_HOST"]!,
            Port = int.Parse(configuration["SMTP_PORT"]!),
            Credentials = new NetworkCredential(configuration["SMTP_USERNAME"]!, configuration["SMTP_PASSWORD"]!),
            EnableSsl = true
        };
        _botClient = new TelegramBotClient(configuration["TELEGRAM_BOT_TOKEN"]!);

        _smtpFromEmail = configuration["SMTP_FROM_EMAIL"]!;
    }

    public async Task NotifyUserByEmailAsync(NotifyUserByEmailRequestDto dto)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);

        if (user == null || !user.IsActive)
        {
            throw new HttpException("User not found", 400);
        }

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpFromEmail),
            Subject = dto.Subject,
            Body = dto.Message,
            IsBodyHtml = true
        };

        mailMessage.To.Add(dto.Email);

        _smtpClient.Send(mailMessage);
    }

    public async Task NotifyUserByTelegramAsync(NotifyUserByTelegramRequestDto dto)
    {
        var user = await _unitOfWork.Users.GetByTelegramIdAsync(dto.TelegramId);

        if (user == null || !user.IsActive)
        {
            throw new HttpException("User not found", 400);
        }

        await _botClient.SendMessage(
            chatId: dto.TelegramId,
            text: dto.Message);
    }
}