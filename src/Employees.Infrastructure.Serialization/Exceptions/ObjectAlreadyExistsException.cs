namespace Employees.Infrastructure.Exceptions;

public class ObjectAlreadyExistsException : Exception
{
    public ObjectAlreadyExistsException(string keyName, object keyValue, Type type)
        :base($"Object of  '{type}' is already exists")
    {
        Data.Add(keyName, keyValue);
    }
}
