namespace Seven.Boundless.Persistence;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a composite registry that aggregates multiple <see cref="IItemDataProvider"/> instances.
/// When queried for item data, it searches each contained registry in order until a result is found.
/// </summary>
/// <remarks>
/// This class allows dynamic addition and removal of registries. Duplicate registries are automatically filtered out.
/// </remarks>
public sealed class CompositeItemDataRegistry : IItemDataProvider {
	private List<IItemDataProvider> _registries = [];

	/// <summary>
	/// Initializes a new instance of the <see cref="CompositeItemDataRegistry"/> class with the specified registries.
	/// </summary>
	/// <param name="registries">The initial set of registries to include in the composite.</param>
	public CompositeItemDataRegistry(params IItemDataProvider[] registries) {
		_registries.AddRange(registries);
	}

	/// <summary>
	/// Adds a new registry to the composite. Duplicate registries are ignored.
	/// </summary>
	/// <param name="registry">The registry to add.</param>
	public void AddRegistry(IItemDataProvider registry) {
		_registries.Add(registry);
		_registries = [.._registries.Distinct()];
	}
	/// <summary>
	/// Removes a registry from the composite.
	/// </summary>
	/// <param name="registry">The registry to remove.</param>
	public void RemoveRegistry(IItemDataProvider registry) {
		_registries.Remove(registry);
	}

	/// <inheritdoc/>
	public IItemData? Get(ItemKey key) {
		foreach (IItemDataProvider registry in _registries) {
			IItemData? data = registry.Get(key);
			if (data != null) {
				return data;
			}
		}
		return null;
	}
}