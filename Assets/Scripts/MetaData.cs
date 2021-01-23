using System;
using System.Collections.Generic;

public class MetaData
{
    private readonly Dictionary<Type, Object> data = new Dictionary<Type, Object>();

    public void Add<T>(T meta)
    {
        if (!data.ContainsKey(typeof(T)))
            data.Add(typeof(T), meta);
    }

    public T Get<T>()
    {
        return (T)data[typeof(T)];
    }

    public bool Remove<T>()
    {
        return data.Remove(typeof(T));
    }

    public void Clear()
    {
        data.Clear();
    }
}
