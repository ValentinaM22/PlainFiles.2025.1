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
}
