using System;
using Seven.Boundless.Persistence;

namespace Seven.Boundless;

/// <summary>
/// Represents a state within the persistence system, holding a value that can be of any type implementing <see cref="IPersistentValue"/>.
/// In practice, implementations will only support a single type of <see cref="IPersistentValue"/>; for instance <see cref="PersistentOptions"/> only support <see cref="PersistentOptions.Value"/>s.
/// </summary>
public abstract class PersistentState : IDisposable {
	/// <summary>
	/// Applies the given <see cref="IPersistentValue"/> to this state.
	/// The exact behavior of this method depends on the implementation, but in general it should update the state to reflect the given value.
	/// Implementations should handle invalid or incompatible values gracefully, either by ignoring them or throwing an exception.
	/// </summary>
	/// <param name="value">The value to apply.</param>
	/// <param name="registry">The item data provider registry.</param>
	public abstract void Apply(IPersistentValue value, IItemDataProvider? registry);
	/// <summary>
	/// Saves the current state to an <see cref="IPersistentValue"/>.
	/// The exact type of <see cref="IPersistentValue"/> returned depends on the implementation and should be the same type that <see cref="Apply(IPersistentValue, IItemDataProvider)"/> accepts.
	/// </summary>
	/// <returns>The current state as an <see cref="IPersistentValue"/>.</returns>
	public abstract IPersistentValue Save();

	/// <inheritdoc/>
	~PersistentState() {
		Dispose(false);
	}

	/// <inheritdoc/>
	public void Dispose() {
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Disposes of the resources used by the <see cref="PersistentState"/>.
	/// </summary>
	/// <param name="disposing">true if both managed and unmanaged resources should be disposed, otherwise only unmanaged resources will be disposed.</param>
	protected virtual void Dispose(bool disposing) { }
}