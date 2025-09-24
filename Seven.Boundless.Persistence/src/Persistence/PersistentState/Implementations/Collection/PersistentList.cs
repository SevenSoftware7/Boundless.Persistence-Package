namespace Seven.Boundless.Persistence;

using System;
using System.Collections.Generic;

/// <summary>
/// A <see cref="PersistentCollection"/> implementation backed by an IList.
/// </summary>
public class PersistentList : PersistentCollection, IList<PersistentState> {
	/// <summary>
	/// An event invoked when instantiating data with an index that did not exist in the current list.
	/// </summary>
	public event Action<object>? OnInstantiateMissingIndex;

	/// <summary>
	/// The current state of the persistent data, arranged as a list.
	/// </summary>
	public IList<PersistentState> State { get; private set; }

	/// <inheritdoc/>
	public PersistentList(IList<PersistentState>? initial = null) {
		State = initial ?? [];
		OnDeserializeMissingIndex += (index, value, registry) => {
			if (registry is null) return;
			if (value is not IInstantiablePersistentValue instantiable) return;

			object? instance = instantiable.Instantiate(registry);
			if (instance is null) return;

			if (instance is IPersistent persistent) {
				if (index >= 0 && index <= State.Count) {
					Insert(index, persistent.Persistence);
				}
				else {
					Add(persistent.Persistence);
				}
			}

			OnInstantiateMissingIndex?.Invoke(instance);
		};
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing) {
		base.Dispose(disposing);
		if (disposing) {
			foreach (PersistentState state in State) {
				state.Dispose();
			}
		}
		State.Clear();
		State = null!;
	}


	/// <inheritdoc/>
	public override PersistentState GetState(int index) => State[index];
	/// <inheritdoc/>
	public PersistentState this[int index] {
		get => GetState(index);
		set => State[index] = value;
	}

	/// <inheritdoc/>
	public override int Count => State.Count;
	/// <inheritdoc/>
	public bool IsReadOnly => State.IsReadOnly;


	/// <inheritdoc/>
	public void Add(PersistentState item) => State.Add(item);
	/// <inheritdoc/>
	public void Clear() => State.Clear();
	/// <inheritdoc/>
	public bool Contains(PersistentState item) => State.Contains(item);
	/// <inheritdoc/>
	public void CopyTo(PersistentState[] array, int arrayIndex) => State.CopyTo(array, arrayIndex);
	/// <inheritdoc/>
	public int IndexOf(PersistentState item) => State.IndexOf(item);
	/// <inheritdoc/>
	public void Insert(int index, PersistentState item) => State.Insert(index, item);
	/// <inheritdoc/>
	public bool Remove(PersistentState item) => State.Remove(item);
	/// <inheritdoc/>
	public void RemoveAt(int index) => State.RemoveAt(index);

	/// <inheritdoc/>
	public override IEnumerator<PersistentState> GetEnumerator() => State.GetEnumerator();
}