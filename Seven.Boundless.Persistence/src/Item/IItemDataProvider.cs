namespace Seven.Boundless.Persistence;

public interface IItemDataProvider {
	IItemData? Get(ItemKey key);
	IItemData<T>? Get<T>(ItemKey key) where T : IItem;
}