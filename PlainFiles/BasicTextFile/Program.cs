using BasicTextFile;

var textFile = new SimpleTextFile("data.txt");
var lines = textFile.ReadLines();

using (var logger = new LogWriter("log.txt"))
{
    var user = Login("Users.txt", logger);
    if (user == null)
    {
        Console.WriteLine("Login failed. Exiting...");
        return;
    }

    var opc = "0";
    logger.WriteLog("INFO", "Application started.");
    do
    {
        opc = Menu();
        Console.WriteLine("=======================================");
        switch (opc)
        {
            case "1":
                logger.WriteLog("INFO", "Show content.");
                if (lines.Length == 0)
                {
                    Console.WriteLine("Empty file.");
                    logger.WriteLog("ERROR", "Empty file.");
                    break;
                }
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
                break;

            case "2":
                logger.WriteLog("INFO", "Add line.");
                Console.Write("Enter the line to add: ");
                var newLine = Console.ReadLine();
                if (!string.IsNullOrEmpty(newLine))
                {
                    lines = lines.Append(newLine).ToArray();
                }
                break;

            case "3":
                logger.WriteLog("INFO", "Update line.");
                Console.Write("Enter the line to update: ");
                var lineToUpdate = Console.ReadLine();
                Console.Write("Enter the new value: ");
                var newValue = Console.ReadLine();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == lineToUpdate)
                    {
                        lines[i] = newValue!;
                        break;
                    }
                }
                break;

            case "4":
                logger.WriteLog("INFO", "Remove line.");
                Console.Write("Enter the line to remove: ");
                var lineToRemove = Console.ReadLine();
                if (!string.IsNullOrEmpty(lineToRemove))
                {
                    lines = lines.Where(line => line != lineToRemove).ToArray();
                }
                break;

            case "5":
                SaveChanges();
                logger.WriteLog("INFO", "Save file.");
                break;

            case "0":
                Console.WriteLine("Exiting...");
                break;

            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    } while (opc != "0");
    logger.WriteLog("INFO", "Application ended.");
    SaveChanges();
}

void SaveChanges()
{
    Console.WriteLine("Saving changes...");
    textFile.WriteLines(lines);
    Console.WriteLine("Changes saved.");
}

string Menu()
{
    Console.WriteLine("=======================================");
    Console.WriteLine("1. Show content");
    Console.WriteLine("2. Add line");
    Console.WriteLine("3. Update line");
    Console.WriteLine("4. Remove line");
    Console.WriteLine("5. Save changes");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your option: ");
    return Console.ReadLine() ?? "0";
}

User? Login(string path, LogWriter logger)
{
    var users = File.ReadAllLines(path)
        .Select(User.FromLine)
        .Where(u => u != null)
        .Cast<User>()
        .ToList();

    int attempts = 0;

    while (attempts < 3)
    {
        Console.Write("Usuario: ");
        var username = Console.ReadLine();

        Console.Write("Contraseña: ");
        var password = Console.ReadLine();

        var user = users.FirstOrDefault(u => u.Username == username);

        if (user == null)
        {
            Console.WriteLine("Usuario no encontrado.");
            logger.WriteLog("WARN", $"Login failed. Usuario '{username}' no existe.");
            return null;
        }

        if (!user.Active)
        {
            Console.WriteLine("Usuario inactivo o bloqueado.");
            logger.WriteLog("WARN", $"Login failed. Usuario '{username}' bloqueado.");
            return null;
        }

        if (user.Password != password)
        {
            attempts++;
            Console.WriteLine("Contraseña incorrecta.");
            logger.WriteLog("WARN", $"Intento fallido {attempts}/3 para '{username}'.");

            if (attempts >= 3)
            {
                Console.WriteLine("Demasiados intentos. Usuario bloqueado.");
                logger.WriteLog("ERROR", $"Usuario '{username}' bloqueado por intentos fallidos.");

                var updatedLines = File.ReadAllLines(path)
                    .Select(line =>
                    {
                        var parts = line.Split(',');
                        if (parts.Length == 3 && parts[0] == username)
                        {
                            return $"{parts[0]},{parts[1]},false";
                        }
                        return line;
                    });
                File.WriteAllLines(path, updatedLines);
            }
        }
        else
        {
            logger.WriteLog("INFO", $"Login exitoso: {username}");
            return user;
        }
    }

    return null;
}
