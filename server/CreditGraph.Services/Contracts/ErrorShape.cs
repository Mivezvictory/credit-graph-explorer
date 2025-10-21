namespace CreditGraph.Services.Contracts;
public sealed record ErrorShape(
    int status,
    string message
);