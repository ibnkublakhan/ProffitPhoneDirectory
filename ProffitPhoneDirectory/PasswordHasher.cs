namespace ProffitPhoneDirectory;

public class PasswordHasher
{
    public static string HashPassword(string password)
    {
        // Генерируем случайную соль
        string salt = BCrypt.Net.BCrypt.GenerateSalt();

        // Хэшируем пароль с солью
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

        return hashedPassword;
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}


