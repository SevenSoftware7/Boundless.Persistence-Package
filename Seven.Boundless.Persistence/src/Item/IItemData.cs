namespace Seven.Boundless.Persistence;

/// <summary>
/// An interface representing data associated with an item, identified by an <see cref="ItemKey"/>.
/// This will act as a factory for creating instances of the item, in the context of persistence Serialization/Deserialization.
/// </summary>
public interface IItemData {
	/// <summary>
	/// The unique key identifying this item data.
	/// This key is used for looking up the item data in an <see cref="IItemDataProvider"/>.
	/// </summary>
	public ItemKey ItemKey { get; }

	/// <summary>
	/// Creates an instance of the item associated with this data.
	/// </summary>
	/// <returns>An instance of the item, or null if instantiation fails.</returns>
	public object? Instantiate();

	/// <summary>
	/// Creates an instance of the item associated with this data, cast to the specified type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type to cast the instantiated item to.</typeparam>
	/// <returns>An instance of the item cast to <typeparamref name="T"/>, or null if instantiation fails or the cast is invalid.</returns>
	public T? Instantiate<T>() where T : class => Instantiate() as T;
}