namespace LegoStore.Domain;

/// <summary>
/// The smallest storage unit. Holds exactly one LotId + Quantity.
/// </summary>
public class Section
{
    public string? LotId { get; private set; }
    public int Quantity { get; private set; }

    public bool IsEmpty => LotId is null;

    public void Assign(string lotId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(lotId))
            throw new ArgumentException("LotId cannot be null or whitespace.", nameof(lotId));
        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");

        LotId = lotId;
        Quantity = quantity;
    }

    public void Deduct(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");
        if (amount > Quantity)
            throw new InvalidOperationException("Cannot deduct more than the current quantity.");

        Quantity -= amount;
        if (Quantity == 0)
            Clear();
    }

    public void Clear()
    {
        LotId = null;
        Quantity = 0;
    }
}
