namespace Seven.Boundless.Persistence;

/// <summary>
/// An interface representing an item with an associated <see cref="ItemKey"/>.
/// The key is used for identifying and managing the item within the persistence framework.
/// </summary>
public interface IItem {
	/// <summary>
	/// The unique key identifying this item.
	/// </summary>
	public ItemKey ItemKey { get; }
}