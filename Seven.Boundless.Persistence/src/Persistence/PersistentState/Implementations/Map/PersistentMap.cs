namespace Seven.Boundless;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Seven.Boundless.Persistence;

/// <summary>
/// A collection of persistent data, arranged as a map-like structure of <see cref="IdKey"/> to <see cref="IPersistentValue"/>s.
/// </summary>
public abstract class PersistentMap : PersistentState, IReadOnlyDictionary<IdKey, PersistentState> {
	/// <summary>
	/// An event invoked when deserializing data with a key that does not exist in the current map.
	/// </summary>
	public event Action<IdKey, IPersistentValue, IItemDataProvider?>? OnDeserializeMissingKey;

	/// <inheritdoc/>
	public override void Apply(IPersistentValue data, IItemDataProvider? registry) {
		if (data is not Value mapData) return;
		foreach ((IdKey key, IPersistentValue value) in mapData) {
			if (TryGetValue(key, out PersistentState? state)) {
				state.Apply(value, registry);
			}
			else {
				OnDeserializeMissingKey?.Invoke(key, value, registry);
			}
		}
	}
	/// <inheritdoc/>
	public override IPersistentValue Save() => new Value(this);


	/// <summary>
	/// Gets the <see cref="PersistentState"/> associated with the given <see cref="IdKey"/>.
	/// </summary>
	/// <param name="key">The key to look up.</param>
	/// <returns>The <see cref="PersistentState"/> associated with the given <see cref="IdKey"/>.</returns>
	public abstract PersistentState GetState(IdKey key);
	/// <inheritdoc/>
	PersistentState IReadOnlyDictionary<IdKey, PersistentState>.this[IdKey key] { get => GetState(key); }


	/// <inheritdoc/>
	public abstract int Count { get; }

	/// <inheritdoc/>
	public abstract IEnumerable<IdKey> Keys { get; }
	/// <inheritdoc/>
	public abstract IEnumerable<PersistentState> Values { get; }

	/// <inheritdoc/>
	public abstract bool ContainsKey(IdKey key);

	/// <inheritdoc/>
	public abstract bool TryGetValue(IdKey key, [MaybeNullWhen(false)] out PersistentState value);

	/// <inheritdoc/>
	public abstract IEnumerator<KeyValuePair<IdKey, PersistentState>> GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


	/// <summary>
	/// A collection of persistent data, arranged as a dictionary of <see cref="IdKey"/> to <see cref="IPersistentValue"/>.
	/// </summary>
	[Serializable]
	public readonly record struct Value(IReadOnlyDictionary<IdKey, IPersistentValue> Data) : IPersistentValue, IReadOnlyDictionary<IdKey, IPersistentValue> {

		/// <summary>
		/// Creates a new instance of <see cref="Value"/> from a <see cref="PersistentMap"/>'s current state.
		/// </summary>
		/// <param name="state">The state to save the data from.</param>
		public Value(IReadOnlyDictionary<IdKey, PersistentState> state) : this(
			state.ToDictionary(
				kv => kv.Key,
				kv => kv.Value.Save()
			)
		) { }


		/// <inheritdoc/>
		public IPersistentValue this[IdKey key] => Data[key];
		/// <inheritdoc/>
		public int Count => Data.Count;

		/// <inheritdoc/>
		public IEnumerable<IdKey> Keys => Data.Keys;
		/// <inheritdoc/>
		public IEnumerable<IPersistentValue> Values => Data.Values;

		/// <inheritdoc/>
		public bool ContainsKey(IdKey key) {
			return Data.ContainsKey(key);
		}

		/// <inheritdoc/>
		public bool TryGetValue(IdKey key, [MaybeNullWhen(false)] out IPersistentValue value) {
			return Data.TryGetValue(key, out value);
		}

		/// <inheritdoc/>
		public IEnumerator<KeyValuePair<IdKey, IPersistentValue>> GetEnumerator() => Data.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}