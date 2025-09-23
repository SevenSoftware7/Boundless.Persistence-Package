namespace Seven.Boundless.Persistence;

public interface IItem {
	public IItemData? ItemData { get; }
}

public interface IItem<T> : IItem where T : IItem<T> {
	IItemData? IItem.ItemData => ItemData;
	public new IItemData<T>? ItemData { get; }
}