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
            Console.Write("Enter the ID of the person to edit: ");
            var editId = Console.ReadLine();

            if (!int.TryParse(editId, out int editIdValue))
            {
                Console.WriteLine("Invalid ID.");
                break;
            }

            var personToEdit = readPeople.FirstOrDefault(p => p.Id == editIdValue);
            if (personToEdit == null)
            {
                Console.WriteLine("Person not found.");
                break;
            }

            Console.WriteLine("Current data:");
            Console.WriteLine(personToEdit);

            Console.Write("Enter new First Name (or ENTER to keep current): ");
            var newFirstName = Console.ReadLine();
            Console.Write("Enter new Last Name (or ENTER to keep current): ");
            var newLastName = Console.ReadLine();
            Console.Write("Enter new Phone (or ENTER to keep current): ");
            var newPhone = Console.ReadLine();
            Console.Write("Enter new City (or ENTER to keep current): ");
            var newCity = Console.ReadLine();
            Console.Write("Enter new Balance (or ENTER to keep current): ");
            var newBalance = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newFirstName)) personToEdit.FirstName = newFirstName;
            if (!string.IsNullOrWhiteSpace(newLastName)) personToEdit.LastName = newLastName;
            if (!string.IsNullOrWhiteSpace(newPhone)) personToEdit.Phone = newPhone;
            if (!string.IsNullOrWhiteSpace(newCity)) personToEdit.City = newCity;
            if (!string.IsNullOrWhiteSpace(newBalance) && decimal.TryParse(newBalance, out decimal newBal))
            {
                personToEdit.Balance = newBal;
            }

            Console.WriteLine("Person updated successfully.");
            break;

        case "5":
            Console.Write("Enter the ID of the person to delete: ");
            var deleteId = Console.ReadLine();

            if (!int.TryParse(deleteId, out int deleteIdValue))
            {
                Console.WriteLine("Invalid ID.");
                break;
            }

            var personToDelete = readPeople.FirstOrDefault(p => p.Id == deleteIdValue);
            if (personToDelete == null)
            {
                Console.WriteLine("Person not found.");
                break;
            }

            readPeople.Remove(personToDelete);
            Console.WriteLine("Person removed successfully.");
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
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");
    return Console.ReadLine() ?? "0";
}

