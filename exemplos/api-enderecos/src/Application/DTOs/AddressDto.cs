namespace ApiEnderecos.Application.DTOs;

public record AddressDto(Guid Id, string Street, string City, string State, string ZipCode);
