using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Seven.Boundless.Persistence;

/// <summary>
/// A registry for managing item data instances, allowing for registration, retrieval, and unregistration of item data by their associated <see cref="ItemKey"/>.
/// </summary>
public class ItemDataRegistry : IItemDataContainer {
	/// <summary>
	/// An optional logger action for logging registry events.
	/// </summary>
	public readonly Action<string>? Logger;
	private readonly Dictionary<ItemKey, IItemData> _registry = [];

	/// <summary>
	/// Initializes a new instance of the <see cref="ItemDataRegistry"/> class.
	/// </summary>
	/// <param name="logger">An optional logger action for logging registry events.</param>
	public ItemDataRegistry(Action<string>? logger = null) {
		Logger = logger;
	}

	/// <inheritdoc/>
	public IItemData? Get(ItemKey key) => _registry.TryGetValue(key, out IItemData? data) ? data : null;

	/// <inheritdoc/>
	public bool Register(IItemData data, bool overwrite = false) {
		ItemKey key = data.ItemKey;

		ref IItemData? existingData = ref CollectionsMarshal.GetValueRefOrAddDefault(_registry, key.Value, out bool exists);

		if (!overwrite && exists) {
			Logger?.Invoke($"Data with key {key.Value} already exists.");
			return false;
		}

		existingData = data;
		Logger?.Invoke($"Registered {key.Value} => {data}");
		return true;
	}

	/// <inheritdoc/>
	public bool Unregister(IItemData data) {
		ItemKey key = data.ItemKey;

		_registry.Remove(key.Value);
		Logger?.Invoke($"Unregistered {key.Value} => {data}");
		return true;
	}

	/// <inheritdoc/>
	public bool Clear() {
		_registry.Clear();
		Logger?.Invoke("Cleared all data.");
		return true;
	}
}