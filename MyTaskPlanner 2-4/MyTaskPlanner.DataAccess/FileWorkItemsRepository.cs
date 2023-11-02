
using MyTaskPlanner.Domain.Models.Models;
using System.Text.Json;
using TaskPlanner.DataAccess.Abstractions;

public class FileWorkItemsRepository : IWorkItemsRepository
{
    private const string _path = @"c:\temp\work-items.json";
    private readonly Dictionary<Guid, WorkItem> _items;

    public FileWorkItemsRepository()
    {
        _items = LoadItemsFromFile();
    }

    private Dictionary<Guid, WorkItem> LoadItemsFromFile()
    {
        if (File.Exists(_path))
        {
            string text = File.ReadAllText(_path);
            List<WorkItem> itemsList = JsonSerializer.Deserialize<List<WorkItem>>(text) ?? new List<WorkItem>();
            return itemsList.ToDictionary(x => x.Id, x => x);
        }
        File.Create(_path);
        return new Dictionary<Guid, WorkItem>();
    }

    public Guid Add(WorkItem workItem)
    {
        WorkItem item = workItem.Clone() as WorkItem ?? new WorkItem();
        item.Id = Guid.NewGuid();
        _items.Add(item.Id, item);
        SaveChanges();
        return item.Id;
    }

    public WorkItem Get(Guid id)
    {
        return _items.GetValueOrDefault(id);
    }

    public WorkItem[] GetAll()
    {
        return _items.Values.ToArray();
    }

    public bool Remove(Guid id)
    {
        bool removed = _items.Remove(id);
        if (removed)
            SaveChanges();
        return removed;
    }

    public void SaveChanges()
    {
        string json = JsonSerializer.Serialize(_items.Values.ToList());
        File.WriteAllText(_path, json);
    }

    public bool Update(WorkItem workItem)
    {
        if (!_items.ContainsKey(workItem.Id))
            return false;

        _items[workItem.Id] = workItem.Clone() as WorkItem ?? new WorkItem();
        SaveChanges();
        return true;
    }
}
