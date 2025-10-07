namespace CreditGraph.Domain;

public sealed record Album(
    string Id,//spotify album ID
    string Title,
    DateOnly? ReleasedDate = null,
    string? LeadtrackUri = null
);