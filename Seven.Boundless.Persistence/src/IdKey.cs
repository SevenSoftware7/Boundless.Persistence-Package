namespace Seven.Boundless.Persistence;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// A key type used for identifying persistent data entries. It wraps a string and ensures it is valid.
/// The reason this is used instead of plain strings is to avoid easily misreadable keys and overall confusion.
/// </summary>
[Serializable]
public readonly partial record struct IdKey {
	// private static readonly Dictionary<int, string> CachedStringRepresentations = [];

	[System.Text.RegularExpressions.GeneratedRegex(@"[^a-z0-9\-_=\|!\?#~]")]
	private static partial System.Text.RegularExpressions.Regex SanitizeIdKeyRegex();

	/// <summary>
	/// Checks if the given string is a valid IdKey (not null or empty).
	/// </summary>
	/// <param name="string">The string to check.</param>
	/// <returns>True if the string is a valid IdKey; otherwise, false.</returns>
	public static bool IsValid([NotNullWhen(true)] string? @string) => !string.IsNullOrWhiteSpace(@string);
	/// <summary>
	/// Sanitizes the given string to be a valid IdKey by converting it to lowercase and replacing invalid characters with underscores.
	/// </summary>
	public static string SanitizeKey(string key) {
		return SanitizeIdKeyRegex().Replace(key.ToLowerInvariant(), "_");
	}

	/// <summary>
	/// The string representation of the IdKey.
	/// </summary>
	public readonly string String;
	/// <summary>
	/// The hash value of the IdKey.
	/// </summary>
	public readonly int HashCode;

	/// <summary>
	/// Initializes a new instance of the <see cref="IdKey"/> struct with the specified string.
	/// </summary>
	private IdKey(string @string) {
		String = SanitizeKey(@string);
		HashCode = String.GetHashCode(StringComparison.OrdinalIgnoreCase);
		// CachedStringRepresentations[HashCode] = String;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="IdKey"/> struct with the specified hash code.
	/// </summary>
	/// <param name="hash">The hash code of the IdKey.</param>
	public IdKey(int hash) {
		HashCode = hash;
		String = hash.ToString();
		// String = CachedStringRepresentations.GetValueOrDefault(hash) ?? hash.ToString();
	}

	/// <summary>
	/// Creates a new <see cref="IdKey"/> from the given string. If the string is null or empty, an <see cref="ArgumentException"/> is thrown.
	/// </summary>
	/// <param name="string">The string to create the IdKey from.</param>
	/// <exception cref="ArgumentException">Thrown when the string is null or empty.</exception>
	public static IdKey Create(string @string) => new(
		IsValid(@string)
			? SanitizeKey(@string)
			: throw new ArgumentException("Key cannot be null or empty.", nameof(@string))
	);

	/// <summary>
	/// Attempts to create the given string into an <see cref="IdKey"/>. If the string is null or empty, null is returned.
	/// </summary>
	/// <param name="string">The string to create the IdKey from.</param>
	/// <returns>The parsed IdKey, or null if the string is invalid.</returns>
	public static IdKey? TryCreate(string? @string) {
		if (!IsValid(@string)) return null;
		return new IdKey(@string);
	}

	/// <summary>
	/// Generates a random <see cref="IdKey"/> using a new GUID.
	/// </summary>
	/// <returns>A new random IdKey.</returns>
	public static IdKey Random() => new(Guid.NewGuid().ToString("N"));

	/// <summary>
	/// Creates a new <see cref="IdKey"/> from the given string, or returns the provided default key if the string is null or empty.
	/// </summary>
	/// <param name="string">The string to create the IdKey from.</param>
	/// <param name="defaultKey">The default IdKey to return if the string is invalid.</param>
	/// <returns>The created IdKey, or the default key if the string is invalid.</returns>
	public static IdKey CreateOrDefault(string? @string, IdKey defaultKey) => TryCreate(@string) ?? defaultKey;

	/// <summary>
	/// Creates a new <see cref="IdKey"/> from the given string, or generates a random key if the string is null or empty.
	/// </summary>
	/// <param name="string">The string to create the IdKey from.</param>
	/// <returns>The created IdKey, or a random IdKey if the string is invalid.</returns>
	public static IdKey CreateOrRandom(string? @string) => TryCreate(@string) ?? Random();


	/// <inheritdoc/>
	public override string ToString() => $"\"{String}\"";
}