namespace Seven.Boundless.Persistence;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A collection of persistent data, arranged as an array-like structure of <see cref="IPersistentValue"/>s.
/// </summary>
public abstract class PersistentCollection : PersistentState, IReadOnlyList<PersistentState> {
	/// <summary>
	/// An event invoked when deserializing data with an index that does not exist in the current collection.
	/// </summary>
	public event Action<int, IPersistentValue, IItemDataProvider?>? OnDeserializeMissingIndex;

	/// <inheritdoc/>
	public override void Apply(IPersistentValue data, IItemDataProvider? registry) {
		if (data is not IValue collectionData) return;

		int count = Math.Min(Count, collectionData.Count);
		for (int i = 0; i < count; i++) {
			GetState(i).Apply(collectionData[i], registry);
		}
		for (int i = count; i < collectionData.Count; i++) {
			OnDeserializeMissingIndex?.Invoke(i, collectionData[i], registry);
		}
	}
	/// <inheritdoc/>
	public override IPersistentValue Save() => new Value(this);


	/// <summary>
	/// Gets the <see cref="PersistentState"/> associated with the given index.
	/// </summary>
	/// <param name="index">The index to look up.</param>
	/// <returns>The <see cref="PersistentState"/> associated with the given index.</returns>
	public abstract PersistentState GetState(int index);
	/// <inheritdoc/>
	PersistentState IReadOnlyList<PersistentState>.this[int index] { get => GetState(index); }


	/// <inheritdoc/>
	public abstract int Count { get; }

	/// <inheritdoc/>
	public abstract IEnumerator<PersistentState> GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	/// A value representing a collection of persistent data, arranged as an array of <see cref="IPersistentValue"/>.
	/// </summary>
	public interface IValue : IPersistentValue, IReadOnlyList<IPersistentValue> {
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	/// <summary>
	/// A collection of persistent data, arranged as an array of <see cref="IPersistentValue"/>.
	/// </summary>
	/// <param name="Values">The persistent values.</param>
	[Serializable]
	public readonly record struct Value(IReadOnlyList<IPersistentValue> Values) : IValue {
		/// <summary>
		/// Creates a new <see cref="Value"/> from the given enumerable of <see cref="IPersistentValue"/>.
		/// </summary>
		/// <param name="values">The enumerable of <see cref="IPersistentValue"/>.</param>
		public Value(IEnumerable<PersistentState> values) : this([.. values.Select(v => v.Save())]) { }

		/// <inheritdoc/>
		public IPersistentValue this[int key] => Values[key];

		/// <inheritdoc/>
		public int Count => Values.Count;

		/// <inheritdoc/>
		public IEnumerator<IPersistentValue> GetEnumerator() => Values.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}