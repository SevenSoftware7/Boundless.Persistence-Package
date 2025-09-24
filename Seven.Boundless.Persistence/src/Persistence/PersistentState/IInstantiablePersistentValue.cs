namespace Seven.Boundless.Persistence;

/// <summary>
/// A persistent value that can instantiate an object from its data.
/// </summary>
public interface IInstantiablePersistentValue : IPersistentValue {
	/// <summary>
	/// Instantiates an object from the persistent value, using the given item data provider to look up any necessary item data.
	/// Any and all implementations of this method are responsible for applying any persistent state to the instantiated object where applicable.
	/// </summary>
	/// <param name="registry">The item data provider to look up item data in.</param>
	/// <returns>The instantiated object.</returns>
	public object? Instantiate(IItemDataProvider registry);

	/// <summary>
	/// Instantiates an object of type <typeparamref name="T"/> from the persistent value, using the given item data provider to look up any necessary item data.
	/// Any and all implementations of this method are responsible for applying any persistent state to the instantiated object where applicable.
	/// </summary>
	/// <typeparam name="T">The type of the object to instantiate.</typeparam>
	/// <param name="registry">The item data provider to look up item data in.</param>
	/// <returns>The instantiated object of type <typeparamref name="T"/>.</returns>
	public T? Instantiate<T>(IItemDataProvider registry) where T : class => Instantiate(registry) as T;
}