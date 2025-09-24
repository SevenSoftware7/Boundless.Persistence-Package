namespace Seven.Boundless;

using System;
using Seven.Boundless.Persistence;

/// <summary>
/// A persistence state representing a toggle (on/off) value.
/// </summary>
/// <param name="name">The name of the toggle.</param>
public sealed class PersistentToggle(string? name) : PersistentState {
	/// <inheritdoc/>
	public event Action<bool>? OnUpdate;
	/// <inheritdoc/>
	public override string? DisplayName { get; } = name;

	private Value _value = Value.False;

	/// <inheritdoc/>
	public override void Apply(IPersistentValue value, IItemDataProvider? registry) {
		_value = value switch {
			Value val => val,
			_ => Value.False
		};
		OnUpdate?.Invoke(_value.Val);
	}
	/// <inheritdoc/>
	public override IPersistentValue Save() => _value;

	/// <inheritdoc/>
	protected override void Dispose(bool disposing) {
		if (disposing) {
			OnUpdate = null;
		}
		base.Dispose(disposing);
	}


	/// <summary>
	/// Represents a toggle (boolean) value that can be persisted.
	/// </summary>
	[Serializable]
	public readonly record struct Value(bool Val) : IPersistentValue {
		/// <summary>
		/// A toggle value representing 'true'.
		/// </summary>
		public static readonly Value True = new(true);
		/// <summary>
		/// A toggle value representing 'false'.
		/// </summary>
		public static readonly Value False = new(false);

		/// <inheritdoc/>
		public static implicit operator bool(Value key) => key.Val;
		/// <inheritdoc/>
		public static implicit operator Value(bool key) => key ? True : False;
	}
}