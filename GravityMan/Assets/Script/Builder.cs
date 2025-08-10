using System;

public class Builder<T> where T : new()
{
    private T instance;
    public Builder() => instance = new T();

    public Builder<T> SetVar(Action<T> action)
    {
        action(instance);
        return this;
    }

    public T Build()
    {
        return instance;
    }
}
