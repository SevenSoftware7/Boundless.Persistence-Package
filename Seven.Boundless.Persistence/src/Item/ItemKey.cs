namespace Seven.Boundless.Persistence;

using System;

/// <summary>
/// A key that uniquely identifies an item.
/// </summary>
/// <param name="Value"></param>
[Serializable]
public readonly record struct ItemKey(IdKey Value) : IPersistentValue {
	/// <summary>
	/// Initializes a new instance of the <see cref="ItemKey"/> struct with the specified string.
	/// </summary>
	public ItemKey(string value) : this(IdKey.Create(value)) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="ItemKey"/> struct with the specified hash code.
	/// </summary>
	public ItemKey(int hash) : this(new IdKey(hash)) { }

	/// <inheritdoc cref="IdKey.Create(string)"/>
	public static IdKey Create(string @string) => IdKey.Create(@string);
	/// <inheritdoc cref="IdKey.TryCreate(string)"/>
	public static IdKey? TryCreate(string? @string) => IdKey.TryCreate(@string);
	/// <inheritdoc cref="IdKey.IsValid(string?)"/>
	public static IdKey Random() => IdKey.Random();
	/// <inheritdoc cref="IdKey.CreateOrDefault(string?, IdKey)"/>
	public static IdKey CreateOrDefault(string? @string, IdKey defaultKey) => IdKey.CreateOrDefault(@string, defaultKey);
	/// <inheritdoc cref="IdKey.CreateOrRandom(string?)"/>
	public static IdKey CreateOrRandom(string? @string) => IdKey.CreateOrRandom(@string);


	/// <inheritdoc/>
	public override string ToString() => Value.ToString();

	/// <inheritdoc/>
	public static implicit operator IdKey(ItemKey key) => key.Value;
	/// <inheritdoc/>
	public static implicit operator ItemKey(IdKey key) => new(key);
}