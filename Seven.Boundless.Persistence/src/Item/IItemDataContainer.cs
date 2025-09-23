namespace Seven.Boundless.Persistence;

public interface IItemDataContainer : IItemDataProvider {
	bool Register(IItemData data, bool overwrite = false);
	bool Unregister(IItemData data);

	bool Clear();
}