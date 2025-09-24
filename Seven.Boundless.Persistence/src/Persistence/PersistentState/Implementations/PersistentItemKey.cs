namespace Seven.Boundless.Persistence;

using System;

/// <summary>
/// A <see cref="PersistentState"/> implementation for handling an <see cref="ItemKey"/>.
/// </summary>
/// <param name="key">The default item key value</param>
public sealed class PersistentItemKey(ItemKey key) : PersistentState {
	/// <summary>
	/// Event triggered when the key changes.
	/// </summary>
	public event Action<ItemKey>? OnChanged;
	/// <summary>
	/// The item key handled by this state.
	/// </summary>
	public ItemKey Key {
		get => _key;
		set {
			_key = value;
			OnChanged?.Invoke(_key);
		}
	}
	private ItemKey _key = key;

	/// <inheritdoc/>
	public override void Apply(IPersistentValue value, IItemDataProvider? registry) {
		if (value is not ItemKey key) return;
		Key = key;
	}
	/// <inheritdoc/>
	public override IPersistentValue Save() => Key;
}