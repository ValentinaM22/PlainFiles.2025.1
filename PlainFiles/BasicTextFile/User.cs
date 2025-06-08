using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTextFile;

public class User
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool Active { get; set; }

    public static User? Authenticate(string filePath)
    {
        int attempts = 0;

        while (attempts < 3)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";
            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 3 &&
                    parts[0] == username &&
                    parts[1] == password &&
                    bool.TryParse(parts[2], out var active))
                {
                    if (!active)
                    {
                        Console.WriteLine("User is blocked.");
                        return null;
                    }

                    return new User { Username = username, Password = password, Active = active };
                }
            }

            Console.WriteLine("Invalid credentials.");
            attempts++;
        }

        Console.WriteLine("Too many failed attempts. Access denied.");
        return null;
    }

    public static User? FromLine(string line)
    {
        var parts = line.Split(',');
        if (parts.Length != 3) return null;

        return new User
        {
            Username = parts[0],
            Password = parts[1],
            Active = bool.TryParse(parts[2], out bool active) && active
        };
    }

    public string ToLine()
    {
        return $"{Username},{Password},{Active}";
    }
}
