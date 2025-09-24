namespace Seven.Boundless.Persistence;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

/// <summary>
/// A <see cref="PersistentMap"/> that combines multiple other <see cref="PersistentMap"/> instances.
/// It checks each contained state in order for keys, allowing for a layered approach to persistence.
/// </summary>
/// <param name="name">The name of the composite state, see <see cref="PersistentState.DisplayName"/>.</param>
/// <param name="dataStates">The child states to combine.</param>
public sealed class CompositePersistentMap(string? name, params PersistentMap[] dataStates) : PersistentMap {
	/// <inheritdoc/>
	public event Action<object[]?>? OnUpdate;
	/// <inheritdoc/>
	public override string? DisplayName { get; } = name;

	private int FirstContainsKey(IdKey key) {
		for (int i = 0; i < dataStates.Length; i++) {
			if (dataStates[i].ContainsKey(key)) {
				return i;
			}
		}
		return -1;
	}

	/// <inheritdoc/>
	public override PersistentState GetState(IdKey key) {
		int idx = FirstContainsKey(key);
		if (idx < 0) {
			throw new KeyNotFoundException($"The given key '{key}' was not present in the composite state.");
		}
		return dataStates[idx].GetState(key);
	}

	/// <inheritdoc/>
	public PersistentState this[IdKey key] {
		get => GetState(key);
	}

	/// <inheritdoc/>
	public override IEnumerable<IdKey> Keys => dataStates.SelectMany(ds => ds.Keys);
	/// <inheritdoc/>
	public override IEnumerable<PersistentState> Values => dataStates.SelectMany(ds => ds.Values);

	/// <inheritdoc/>
	public override int Count => dataStates.Sum(ds => ds.Count);

	/// <inheritdoc/>
	public override bool ContainsKey(IdKey key) => FirstContainsKey(key) >= 0;
	/// <inheritdoc/>
	public override bool TryGetValue(IdKey key, [MaybeNullWhen(false)] out PersistentState value) {
		int idx = FirstContainsKey(key);
		if (idx < 0) {
			value = null;
			return false;
		}
		return dataStates[idx].TryGetValue(key, out value);
	}

	/// <inheritdoc/>
	public override IEnumerator<KeyValuePair<IdKey, PersistentState>> GetEnumerator() {
		IEnumerable<KeyValuePair<IdKey, PersistentState>> entries = dataStates
			.SelectMany(ds => ds)
			.DistinctBy(kv => kv.Key);

		foreach (var item in entries) {
			yield return item;
		}
	}
}