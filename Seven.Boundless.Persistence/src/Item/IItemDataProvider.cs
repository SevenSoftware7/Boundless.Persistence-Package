namespace Seven.Boundless.Persistence;

/// <summary>
/// A provider interface for retrieving <see cref="IItemData"/> instances by their associated <see cref="ItemKey"/>.
/// </summary>
public interface IItemDataProvider {
	/// <summary>
	/// Gets the item data associated with the specified <see cref="ItemKey"/>.
	/// </summary>
	/// <param name="key">The key of the item data to retrieve.</param>
	/// <returns>The item data associated with the key, or null if not found.</returns>
	public IItemData? Get(ItemKey key);
}