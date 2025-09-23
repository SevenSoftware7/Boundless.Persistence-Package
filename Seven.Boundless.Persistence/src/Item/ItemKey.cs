namespace Seven.Boundless.Persistence;

using System;

/// <summary>
/// A key that uniquely identifies an item.
/// </summary>
/// <param name="Value"></param>
[Serializable]
public readonly record struct ItemKey(IdKey Value) {
	/// <inheritdoc/>
	public ItemKey(string value) : this(new IdKey(value)) { }
	public static ItemKey? TryParse(string? str) {
		IdKey? key = IdKey.TryParse(str);
		if (key is null) return null;
		return new ItemKey(key.Value);
	}

	/// <inheritdoc/>
	public override string ToString() => Value.ToString();

	/// <inheritdoc/>
	public static implicit operator IdKey(ItemKey key) => key.Value;
	/// <inheritdoc/>
	public static implicit operator ItemKey(IdKey key) => new(key);
	/// <inheritdoc/>
	public static implicit operator string(ItemKey key) => key.Value;
}