using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class CustomArray<T> : IList<T>, IReadOnlyList<T>
{
    private const string EXCEPTION = "Collection was of a fixed size.";

    protected abstract Array Array { get; }
    public int Length => Array.Length;
    int IReadOnlyCollection<T>.Count => Length;
    int ICollection<T>.Count => Length;
    bool ICollection<T>.IsReadOnly => false;

    T IReadOnlyList<T>.this[int index] => SingleIndexGet(index);
    T IList<T>.this[int index] 
    { 
        get => SingleIndexGet(index);
        set => SingleIndexSet(index, value); 
    }

    public void Clear() => Array.Clear(Array, 0, Length);
    public abstract int IndexOf(T item);
    public abstract bool Contains(T item);
    public abstract void CopyTo(T[] array, int arrayIndex);
    public abstract IEnumerator<T> GetEnumerator();

    protected abstract T SingleIndexGet(int index);
    protected abstract void SingleIndexSet(int index, T value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    void ICollection<T>.Add(T item) => throw new NotSupportedException(EXCEPTION);
    bool ICollection<T>.Remove(T item) => throw new NotSupportedException(EXCEPTION);
    void IList<T>.Insert(int index, T item) => throw new NotSupportedException(EXCEPTION);
    void IList<T>.RemoveAt(int index) => throw new NotSupportedException(EXCEPTION);
}
