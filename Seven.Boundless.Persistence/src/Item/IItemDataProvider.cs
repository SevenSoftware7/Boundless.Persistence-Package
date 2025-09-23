namespace Seven.Boundless.Persistence;

public interface IItemDataProvider {
	IItemData? GetData(ItemKey key);
	IItemData<T>? GetData<T>(ItemKey key) where T : IItem;
}