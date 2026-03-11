namespace Cadastro.Application.DTOs;

/// <summary>
/// Paginated response wrapper.
/// </summary>
/// <typeparam name="T">The type of items in the response.</typeparam>
public sealed record PaginatedResponse<T>(
    IEnumerable<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);
