using BookApp.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Infrastructure;

public static class SeedData
{
    private const string DemoUsername = "testuser";
    private const string DemoPassword = "test12345";

    public static void Seed(DbContext context)
    {
        var db = (AppDbContext)context;
        if (db.Users.Any(u => u.Username == DemoUsername))
            return;

        var user = CreateDemoUser();
        db.Users.Add(user);
        db.Quotes.AddRange(CreateQuotes(user.Id));
        db.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken ct)
    {
        var db = (AppDbContext)context;
        if (await db.Users.AnyAsync(u => u.Username == DemoUsername, ct))
            return;

        var user = CreateDemoUser();
        db.Users.Add(user);
        db.Quotes.AddRange(CreateQuotes(user.Id));
        await db.SaveChangesAsync(ct);
    }

    private static User CreateDemoUser()
    {
        var user = new User { Username = DemoUsername, PasswordHash = string.Empty };
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, DemoPassword);
        return user;
    }

    private static Quote[] CreateQuotes(Guid userId) =>
        [
            new()
            {
                Text = "The unexamined life is not worth living.",
                Author = "Socrates",
                UserId = userId,
            },
            new()
            {
                Text = "Simplicity is the ultimate sophistication.",
                Author = "Leonardo da Vinci",
                UserId = userId,
            },
            new()
            {
                Text = "Premature optimization is the root of all evil.",
                Author = "Donald Knuth",
                UserId = userId,
            },
            new()
            {
                Text = "Programs must be written for people to read.",
                Author = "Harold Abelson",
                UserId = userId,
            },
            new()
            {
                Text = "Perfection is achieved when there is nothing left to take away.",
                Author = "Antoine de Saint-Exupéry",
                UserId = userId,
            },
        ];
}
