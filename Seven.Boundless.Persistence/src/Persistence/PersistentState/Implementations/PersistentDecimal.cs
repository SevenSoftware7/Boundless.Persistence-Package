namespace Seven.Boundless.Persistence;

using System;

/// <summary>
/// A persistence state representing a decimal value within a specified range.
/// </summary>
/// <param name="min">The minimum value of the range.</param>
/// <param name="max">The maximum value of the range.</param>
/// <param name="defaultValue">The default value of the range; will be used as the initial value and fallback if an invalid value is deserialized.</param>
public sealed class PersistentDecimal(decimal min, decimal max, decimal defaultValue) : PersistentState {
	/// <inheritdoc/>
	public event Action<Value>? OnUpdate;

	private Value _value = defaultValue;

	/// <inheritdoc/>
	public override void Apply(IPersistentValue value, IItemDataProvider? registry) {
		_value = value switch {
			Value val => Math.Clamp(val, min, max),
			_ => defaultValue
		};
		OnUpdate?.Invoke(_value);
	}
	/// <inheritdoc/>
	public override IPersistentValue Save() => _value;

	/// <summary>
	/// Represents a decimal value that can be persisted.
	/// </summary>
	/// <param name="Val">The underlying decimal value.</param>
	[Serializable]
	public readonly record struct Value(decimal Val) : IPersistentValue {
		/// <inheritdoc/>
		public static implicit operator decimal(Value key) => key.Val;
		/// <inheritdoc/>
		public static implicit operator Value(decimal key) => new(key);
	}
}