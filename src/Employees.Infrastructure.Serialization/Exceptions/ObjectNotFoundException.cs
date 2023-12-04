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
        foreach(var pair in data)
        {
            Data.Add(pair.Key, pair.Value);
        }
        
    }
}
