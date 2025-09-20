namespace Seven.Boundless.Persistence;

using System;

[Serializable]
public readonly partial record struct ItemKey {
	[System.Text.RegularExpressions.GeneratedRegex(@"[^a-z0-9\-_=\|!#~]")]
	private static partial System.Text.RegularExpressions.Regex ItemKeySanitizeRegex();

	public static string ToItemKeyFormat(string key) {
		return ItemKeySanitizeRegex().Replace(key.ToLowerInvariant(), "_");
	}

	public readonly string String;

	public ItemKey(string @string) {
		String = !string.IsNullOrWhiteSpace(@string)
			? ToItemKeyFormat(@string)
			: throw new ArgumentException("Key cannot be null or empty.", nameof(@string));
	}

	public override string ToString() => String;

	public static implicit operator string(ItemKey key) => key.String;
}