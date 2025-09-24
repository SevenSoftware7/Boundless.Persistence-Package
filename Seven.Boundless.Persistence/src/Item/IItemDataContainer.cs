namespace Seven.Boundless.Persistence;

/// <summary>
/// An interface for containers that can hold and manage <see cref="IItemData"/> instances.
/// </summary>
public interface IItemDataContainer : IItemDataProvider {
	/// <summary>
	/// Registers the given <see cref="IItemData"/> instance in the container.
	/// If an entry with the same <see cref="ItemKey"/> already exists and <paramref name="overwrite"/> is true, it will be overwritten.
	/// </summary>
	/// <param name="data">The <see cref="IItemData"/> instance to register.</param>
	/// <param name="overwrite">Whether to overwrite an existing entry.</param>
	/// <returns>True if the registration was successful; otherwise, false.</returns>
	public bool Register(IItemData data, bool overwrite = false);

	/// <summary>
	/// Unregisters the given <see cref="IItemData"/> instance from the container.
	/// If the instance is not found, no action is taken.
	/// </summary>
	/// <param name="data">The <see cref="IItemData"/> instance to unregister.</param>
	/// <returns>True if the unregistration was successful; otherwise, false.</returns>
	public bool Unregister(IItemData data);

	/// <summary>
	/// Clears all registered <see cref="IItemData"/> instances from the container.
	/// </summary>
	/// <returns>True if the clear operation was successful; otherwise, false.</returns>
	public bool Clear();
}