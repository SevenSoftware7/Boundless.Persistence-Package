namespace Seven.Boundless;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Seven.Boundless.Persistence;

/// <summary>
/// A persistence state that can take on one of a predefined set of options. Acts like a 'dropdown' selector.
/// </summary>
/// <param name="options">The available options for the state.</param>
/// <param name="defaultValue">The default value; will be used as the initial value and fallback if an invalid value is deserialized.</param>
public sealed class PersistentOptions(IDictionary<PersistentOptions.Value, string> options, PersistentOptions.Value? defaultValue = null) : PersistentState, IOrderedEnumerable<KeyValuePair<PersistentOptions.Value, string>> {
	/// <inheritdoc/>
	public event Action<Value>? OnUpdate;

	/// <summary>
	/// The available options for the state.
	/// </summary>
	public readonly IDictionary<Value, string> Options = options;

	/// <inheritdoc/>
	public override void Apply(IPersistentValue value, IItemDataProvider? registry) {
		if (value is not Value val || !Options.ContainsKey(val)) return;
		Key = val;
		OnUpdate?.Invoke(Key);
	}
	/// <inheritdoc/>
	public override IPersistentValue Save() => Key;

	/// <summary>
	/// The currently selected option key.
	/// </summary>
	public Value Key { get; private set; } = defaultValue is not null && options.ContainsKey(defaultValue.Value)
		? defaultValue.Value
		: options.Keys.OrderBy(k => k).First();
	/// <summary>
	/// The display name of the currently selected option.
	/// </summary>
	public string OptionName => Options[Key];

	private IOrderedEnumerable<KeyValuePair<Value, string>> OrderedOptions => Options.OrderBy(kv => kv.Value);


	/// <inheritdoc/>
	public IEnumerator<KeyValuePair<Value, string>> GetEnumerator() => OrderedOptions.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <inheritdoc/>
	public IOrderedEnumerable<KeyValuePair<Value, string>> CreateOrderedEnumerable<TKey>(Func<KeyValuePair<Value, string>, TKey> keySelector, IComparer<TKey>? comparer, bool descending) {
		return OrderedOptions.OrderBy(keySelector, comparer);
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing) {
		if (disposing) {
			OnUpdate = null;
			Options.Clear();
		}
		base.Dispose(disposing);
	}


	/// <summary>
	/// Represents the key of an option; will be serialized and used to identify the selected option upon deserialization.
	/// </summary>
	/// <param name="IdKey">The underlying id key value.</param>
	[Serializable]
	public readonly record struct Value(IdKey IdKey) : IPersistentValue {
		/// <inheritdoc/>
		public Value(string value) : this(IdKey.Create(value)) { }

		/// <inheritdoc/>
		public override string ToString() => IdKey.ToString();

		/// <inheritdoc/>
		public static implicit operator IdKey(Value key) => key.IdKey;
		/// <inheritdoc/>
		public static implicit operator Value(IdKey key) => new(key);
		/// <inheritdoc/>
		public static implicit operator string(Value key) => key.IdKey;
	}
}