namespace Seven.Boundless.Persistence;

using System;

/// <summary>
/// A singleton implementation of <see cref="IPersistentValue"/> representing a null value.
/// </summary>
[Serializable]
public sealed class PersistentNull : IPersistentValue {
	/// <summary>
	/// The singleton instance of <see cref="PersistentNull"/>.
	/// </summary>
	public static readonly PersistentNull Instance = new();

	private PersistentNull() { }
}