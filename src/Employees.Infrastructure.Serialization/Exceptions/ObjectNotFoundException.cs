namespace Employees.Infrastructure.Exceptions;

public class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException(int key, Type type)
        :base($"Object of type '{type}' is not found with id = {key}")
    {
        Data.Add("id", key);
    }

    public ObjectNotFoundException(IList<(string Key, object Value)> data, Type type)
        : base($"Object of type '{type}' is not found with keys: {string.Join(',', data.Select(x => $"{x.Key} = {x.Value}"))}")
    {
        foreach(var (Key, Value) in data)
        {
            Data.Add(Key, Value);
        }
        
    }
}
