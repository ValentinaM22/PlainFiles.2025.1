namespace CVSWithLibary;

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string City { get; set; } = null!;
    public decimal Balance { get; set; }

    public override string ToString()
    {
        return $"{Id}\t{FirstName} {LastName}\n\t" +
               $"Phone: {Phone}\n\t" +
               $"City: {City}\n\t" +
               $"Balance: {Balance,20:C2}\n";
    }

    public static Person FromLine(string line)
    {
        var parts = line.Split(',');
        return new Person
        {
            Id = int.Parse(parts[0]),
            FirstName = parts[1],
            LastName = parts[2],
            Phone = parts[3],
            City = parts[4],
            Balance = decimal.Parse(parts[5])
        };
    }
    public string ToLine()
    {
        return $"{Id},{FirstName},{LastName},{Phone},{City},{Balance}";
    }
}
