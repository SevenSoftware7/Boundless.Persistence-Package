using System;

namespace Seven.Boundless.Persistence;

public interface IPersistenceData {
	object Load(IItemDataProvider registry);
}

public interface IPersistenceData<out T> : IPersistenceData where T : class {
	object IPersistenceData.Load(IItemDataProvider registry) => Load(registry);
	public new T Load(IItemDataProvider registry);
}

public abstract class PersistenceData<T>(T item) : IPersistenceData<T> where T : class {

	public T Load(IItemDataProvider registry) {
		T instance = Instantiate(registry);

		LoadInternal(instance, registry);

		return instance;
	}

	protected abstract T Instantiate(IItemDataProvider registry);
	protected virtual void LoadInternal(T item, IItemDataProvider registry) { }
}

public class ItemPersistenceData<T>(T item) : PersistenceData<T>(item) where T : class, IItem {
	private readonly ItemKey DataKey = item.Data?.ItemKey ?? throw new ArgumentException("Item must have a Data assigned.", nameof(item));

	protected sealed override T Instantiate(IItemDataProvider registry) =>
		registry.Get<T>(DataKey)?.Instantiate()
			?? throw new InvalidOperationException($"Could not find Data of type {typeof(T)} with key {DataKey.Value}.");
}