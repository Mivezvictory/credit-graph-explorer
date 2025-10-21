namespace CreditGraph.Services.Contracts;

/// <summary>
/// Represents a void success result. Use for endpoints that return 204 (No Content).
/// </summary>
public readonly struct Unit
{
    public static readonly Unit Value = new();
    public override string ToString() => "()";
}
