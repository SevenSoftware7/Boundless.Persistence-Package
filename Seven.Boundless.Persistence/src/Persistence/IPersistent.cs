namespace Seven.Boundless.Persistence;

/// <summary>
/// Defines an object that has persistence data.
/// </summary>
public interface IPersistent {
	/// <summary>
	/// The unique identifier key for the kind of object this is. Mostly used for nested persistent object serialization.
	/// Think of this as the name of the persistence category, inside of which all of the persistence data will be stored.
	/// Examples: "items", "costume", "preferences"
	/// </summary>
	public IdKey Identifier { get; }
	/// <summary>
	/// The persistence state of the object.
	/// </summary>
	public PersistentState Persistence { get; }
}