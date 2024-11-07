using System.Collections.Generic;

public sealed class ListPool<T> : Pool<ListPool<T>>
{
    public readonly List<T> List = new List<T>();

    protected override void Deinitialize()
    {	
        List.Clear();
    }
}