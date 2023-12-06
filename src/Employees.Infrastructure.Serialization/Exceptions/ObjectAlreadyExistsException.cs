namespace Employees.Infrastructure.Exceptions;

public class ObjectAlreadyExistsException : Exception
{
    public ObjectAlreadyExistsException(string keyName, object keyValue, Type type)
        :base($"Object of '{type}' is already exists with {keyName} = {keyValue}")
    {
        Data.Add(keyName, keyValue);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
