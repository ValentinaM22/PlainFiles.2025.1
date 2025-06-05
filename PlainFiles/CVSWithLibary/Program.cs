using CVSWithLibary;

var helper = new CsvHelperExample();
var readPeople = helper.Read("people.csv").ToList();
var opc = "0";

do
{
    opc = Menu();
    Console.WriteLine("=======================================");
    switch (opc)
    {
        case "1":
            foreach (var person in readPeople)
            {
                Console.WriteLine(person);
            }
            break;

        case "2":
            Console.Write("Enter the ID: ");
            var id = Console.ReadLine();
            Console.Write("Enter the First name: ");
            var firstName = Console.ReadLine();
            Console.Write("Enter the Last name: ");
            var lastName = Console.ReadLine();
            Console.Write("Enter the phone: ");
            var phone = Console.ReadLine();
            Console.Write("Enter the city: ");
            var city = Console.ReadLine();
            Console.Write("Enter the balance: ");
            var balance = Console.ReadLine();

            var newPerson = new Person
            {
                Id = int.Parse(id!),
                FirstName = firstName ?? string.Empty,
                LastName = lastName ?? string.Empty,
                Phone = phone ?? string.Empty,
                City = city ?? string.Empty,
                Balance = decimal.Parse(balance!)
            };

            readPeople.Add(newPerson);
            break;

        case "3":
            SaveChanges();
            break;

        case "4":
            Console.Write("Enter ID to edit: ");
            var idEditInput = Console.ReadLine();
            if (!int.TryParse(idEditInput, out int idToEdit))
            {
                Console.WriteLine("Invalid ID.");
                break;
            }

            var personToEdit = readPeople.FirstOrDefault(p => p.Id == idToEdit);
            if (personToEdit == null)
            {
                Console.WriteLine("Person not found.");
                break;
            }

            Console.WriteLine("Press ENTER to keep current value.");
            Console.Write($"First name ({personToEdit.FirstName}): ");
            var newFirstName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newFirstName)) personToEdit.FirstName = newFirstName;

            Console.Write($"Last name ({personToEdit.LastName}): ");
            var newLastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newLastName)) personToEdit.LastName = newLastName;

            Console.Write($"Phone ({personToEdit.Phone}): ");
            var newPhone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPhone)) personToEdit.Phone = newPhone;

            Console.Write($"City ({personToEdit.City}): ");
            var newCity = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newCity)) personToEdit.City = newCity;

            Console.Write($"Balance ({personToEdit.Balance}): ");
            var newBalance = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newBalance) && decimal.TryParse(newBalance, out var newBal))
                personToEdit.Balance = newBal;

            break;

        case "5":
            Console.Write("Enter ID to delete: ");
            var idDeleteInput = Console.ReadLine();
            if (!int.TryParse(idDeleteInput, out int idToDelete))
            {
                Console.WriteLine("Invalid ID.");
                break;
            }

            var personToDelete = readPeople.FirstOrDefault(p => p.Id == idToDelete);
            if (personToDelete == null)
            {
                Console.WriteLine("Person not found.");
                break;
            }

            Console.WriteLine("Do you really want to delete this person? (y/n)");
            Console.WriteLine(personToDelete);
            var confirm = Console.ReadLine();
            if (confirm?.ToLower() == "y")
            {
                readPeople.Remove(personToDelete);
                Console.WriteLine("Person deleted.");
            }
            break;

        case "6":
            var resumen = readPeople
                .GroupBy(p => p.City)
                .Select(g => new
                {
                    Ciudad = g.Key,
                    Cantidad = g.Count(),
                    TotalBalance = g.Sum(p => p.Balance)
                });

            foreach (var grupo in resumen)
            {
                Console.WriteLine($"Ciudad: {grupo.Ciudad}");
                Console.WriteLine($"Cantidad de personas: {grupo.Cantidad}");
                Console.WriteLine($"Balance total: {grupo.TotalBalance:C2}");
                Console.WriteLine();
            }
            break;
    }
} while (opc != "0");

SaveChanges();

void SaveChanges()
{
    helper.Write("people.csv", readPeople);
}

string Menu()
{
    Console.WriteLine("=======================================");
    Console.WriteLine("1. Show content");
    Console.WriteLine("2. Add person");
    Console.WriteLine("3. Save changes");
    Console.WriteLine("4. Edit person");
    Console.WriteLine("5. Delete person");
    Console.WriteLine("6. Show summary by city");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");
    return Console.ReadLine() ?? "0";
}


