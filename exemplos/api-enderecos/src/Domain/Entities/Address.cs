namespace ApiEnderecos.Domain.Entities;

public class Address
{
    public Guid Id { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }

    private Address() { }

    public Address(string street, string city, string state, string zipCode)
    {
        Id = Guid.NewGuid();
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public void Update(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Address other) return false;
        return Street == other.Street && City == other.City && State == other.State && ZipCode == other.ZipCode;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, ZipCode);
    }
}
