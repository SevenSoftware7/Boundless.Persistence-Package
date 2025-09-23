namespace Seven.Boundless.Persistence;

using System;

[Serializable]
public readonly partial record struct IdKey {
	[System.Text.RegularExpressions.GeneratedRegex(@"[^a-z0-9\-_=\|!#~]")]
	private static partial System.Text.RegularExpressions.Regex SanitizeIdKeyRegex();

	public static string SanitizeKey(string key) {
		return SanitizeIdKeyRegex().Replace(key.ToLowerInvariant(), "_");
	}

	public readonly string String;

	public IdKey(string @string) {
		String = !string.IsNullOrWhiteSpace(@string)
			? SanitizeKey(@string)
			: throw new ArgumentException("Key cannot be null or empty.", nameof(@string));
	}
	public static IdKey? TryParse(string? str) {
		if (string.IsNullOrWhiteSpace(str)) return null;
		return new IdKey(str);
	}

	/// <inheritdoc/>
	public override string ToString() => $"\"{String}\"";

	public static implicit operator string(IdKey key) => key.String;
}