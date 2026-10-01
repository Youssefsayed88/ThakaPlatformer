namespace Thaka.Platformer.Persistence
{
    public interface ISaveService
    {
        bool HasSave { get; }

        bool Save(SaveData data);

        bool TryLoad(out SaveData data);

        void Delete();
    }
}
