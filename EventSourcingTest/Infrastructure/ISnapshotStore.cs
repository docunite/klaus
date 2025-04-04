using System.Threading.Tasks;

public interface ISnapshotStore<TSnapShot>
{
    TSnapShot Get(string id, out int version);
    Task Save(TSnapShot snapshot, string id, int version);
}