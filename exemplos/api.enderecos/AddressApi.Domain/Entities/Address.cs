namespace AddressApi.Domain.Entities;

public class Address
{
    public Guid Id { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string PostalCode { get; private set; }

    public Address(string street, string city, string state, string postalCode)
    {
        Id = Guid.NewGuid();
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
    }
}
