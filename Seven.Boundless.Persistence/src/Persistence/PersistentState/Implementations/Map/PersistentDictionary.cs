namespace Seven.Boundless.Persistence;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// A <see cref="PersistentMap"/> implementation backed by an IDictionary.
/// </summary>
public class PersistentDictionary : PersistentMap, IDictionary<IdKey, PersistentState> {
	/// <summary>
	/// An event invoked when instantiating data with a key that did not exist in the current map.
	/// </summary>
	public event Action<object>? OnInstantiateMissingKey;

	/// <summary>
	/// The current state of the persistent data, arranged as a map of <see cref="IdKey"/> to <see cref="PersistentState"/>.
	/// </summary>
	public IDictionary<IdKey, PersistentState> State { get; private set; }

	/// <inheritdoc/>
	public PersistentDictionary(IDictionary<IdKey, PersistentState>? initial = null) {
		State = initial ?? new Dictionary<IdKey, PersistentState>();

		OnDeserializeMissingKey += (key, value, registry) => {
			if (registry is null) return;
			if (value is not IInstantiablePersistentValue instantiable) return;

			object? instance = instantiable.Instantiate(registry);
			if (instance is null) return;

			if (instance is IPersistent persistent) {
				Add(persistent.Identifier, persistent.Persistence);
			}

			OnInstantiateMissingKey?.Invoke(instance);
		};
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing) {
		base.Dispose(disposing);
		if (disposing) {
			foreach (PersistentState state in State.Values) {
				state.Dispose();
			}
		}
		State.Clear();
		State = null!;
	}


	/// <inheritdoc/>
	public override PersistentState GetState(IdKey key) => State[key];
	/// <inheritdoc/>
	public PersistentState this[IdKey key] {
		get => GetState(key);
		set => State[key] = value;
	}

	/// <inheritdoc/>
	public override int Count => State.Count;
	/// <inheritdoc/>
	public bool IsReadOnly => State.IsReadOnly;

	/// <inheritdoc/>
	public override ICollection<IdKey> Keys => State.Keys;
	/// <inheritdoc/>
	public override ICollection<PersistentState> Values => State.Values;

	/// <inheritdoc/>
	public override bool ContainsKey(IdKey key) => State.ContainsKey(key);
	/// <inheritdoc/>
	public override bool TryGetValue(IdKey key, [MaybeNullWhen(false)] out PersistentState value) => State.TryGetValue(key, out value);

	/// <inheritdoc/>
	public void Add(KeyValuePair<IdKey, PersistentState> item) => State.Add(item);
	/// <inheritdoc/>
	public void Add(IdKey key, PersistentState value) => State.Add(key, value);

	/// <inheritdoc/>
	public bool Remove(KeyValuePair<IdKey, PersistentState> item) => State.Remove(item);
	/// <inheritdoc/>
	public bool Remove(IdKey key) => State.Remove(key);
	/// <inheritdoc/>
	public void Clear() => State.Clear();

	/// <inheritdoc/>
	public bool Contains(KeyValuePair<IdKey, PersistentState> item) => State.Contains(item);

	/// <inheritdoc/>
	public void CopyTo(KeyValuePair<IdKey, PersistentState>[] array, int arrayIndex) => State.CopyTo(array, arrayIndex);

	/// <inheritdoc/>
	public override IEnumerator<KeyValuePair<IdKey, PersistentState>> GetEnumerator() => State.GetEnumerator();
}