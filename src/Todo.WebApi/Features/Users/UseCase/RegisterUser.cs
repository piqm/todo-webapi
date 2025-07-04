﻿using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Todo.WebApi.Database;
using Todo.WebApi.Features.Users.Infrastructure;

namespace Todo.WebApi.Features.Users.UseCase
{
    internal sealed class RegisterUser(
     AppDbContext context,
     PasswordHasher passwordHasher,
     IFluentEmail fluentEmail,
     EmailVerificationLinkFactory emailVerificationLinkFactory)
    {
        public sealed record Request(string Email, string FirstName, string LastName, string Password);

    public async Task<User> Handle(Request request)
    {
        if (await context.Users.Exists(request.Email))
        {
            throw new Exception("The email is already in use");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = passwordHasher.Hash(request.Password),
            EmailVerified = true,
        };

        context.Users.Add(user);

        DateTime utcNow = DateTime.UtcNow;
        var verificationToken = new EmailVerificationToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            CreatedOnUtc = utcNow,
            ExpiresOnUtc = utcNow.AddDays(1)
        };

        context.EmailVerificationTokens.Add(verificationToken);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
            when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new Exception("The email is already in use", e);
        }

        // Email verification?
        //string verificationLink = emailVerificationLinkFactory.Create(verificationToken);

        //await fluentEmail
        //    .To(user.Email)
        //    .Subject("Email verification for todo")
        //    .Body($"To verify your email address <a href='{verificationLink}'>click here</a>", isHtml: true)
        //    .SendAsync();

        return user;
    }
}
}
