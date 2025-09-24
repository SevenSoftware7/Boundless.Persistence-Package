using System;

namespace Seven.Boundless.Persistence;

/// <summary>
/// A <see cref="PersistentState"/> implementation for items, which persists the item's key and additional state.
/// </summary>
/// <param name="name">The name of the item.</param>
/// <param name="item">The item to persist.</param>
/// <param name="persistence">The persistence state of the item.</param>
public class PersistentItem(string? name, IItem item, PersistentState persistence) : PersistentState {
	/// <inheritdoc/>
	public override string? DisplayName { get; } = name;

	/// <summary>
	/// The item being persisted.
	/// </summary>
	public IItem Item { get; } = item;
	/// <summary>
	/// The persistence state of the item.
	/// </summary>
	public PersistentState Persistence = persistence;

	/// <inheritdoc/>
	public override void Apply(IPersistentValue value, IItemDataProvider? registry) => Persistence.Apply(value, registry);
	/// <inheritdoc/>
	public override IPersistentValue Save() => new Value(this);

	/// <summary>
	/// The value representing the item's key and additional state.
	/// </summary>
	/// <param name="ItemKey">The key of the item.</param>
	/// <param name="PersistenceValue">The additional persistent value of the item.</param>
	[Serializable]
	public readonly record struct Value(ItemKey ItemKey, IPersistentValue PersistenceValue) : IInstantiablePersistentValue {
		/// <summary>
		/// Creates a new <see cref="Value"/> from the given <see cref="PersistentItem"/>.
		/// </summary>
		/// <param name="persistent">The persistent item.</param>
		public Value(PersistentItem persistent) : this(persistent.Item.ItemKey, persistent.Persistence.Save()) { }

		/// <summary>
		/// Gets the <see cref="IItemData"/> associated with the <see cref="ItemKey"/> from the given <see cref="IItemDataProvider"/>.
		/// </summary>
		/// <param name="registry">The item data provider to look up the item in.</param>
		/// <returns>The <see cref="IItemData"/> associated with the <see cref="ItemKey"/>, or null if not found.</returns>
		/// <exception cref="InvalidOperationException">Thrown if <see cref="ItemKey"/> is null.</exception>
		public IItemData? Get(IItemDataProvider registry) => registry.Get(ItemKey);

		/// <inheritdoc/>
		public object? Instantiate(IItemDataProvider registry) {
			object? item = Get(registry)?.Instantiate();
			if (item is IPersistent persistent) {
				persistent.Persistence.Apply(PersistenceValue, registry);
			}
			return item;
		}

		/// <inheritdoc/>
		public T? Instantiate<T>(IItemDataProvider registry) where T : class => Instantiate(registry) as T;
	}
}