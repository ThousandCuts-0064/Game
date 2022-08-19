using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OffsetArray<T> : CustomArray<T>
{
    private readonly T[] _array;
    private readonly IEqualityComparer<T> _comparer;
    protected override Array Array => _array;
    public int Start { get; }
    public int End { get; }
    public T this[int index]
    {
        get => _array[index - Start];
        set => _array[index - Start] = value;
    }

    public OffsetArray(int start, int length)
    {
        _array = new T[length];
        Start = start;
        End = start + length - 1;
        _comparer = EqualityComparer<T>.Default;
    }

    public OffsetArray(int start, int length, IEqualityComparer<T> comparer) 
        : this(start, length) => _comparer = comparer;

    public override T SimpleGet(int index) => this[index];
    public override void SimpleSet(int index, T value) => this[index] = value;
    public override IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
    public override bool Contains(T item) => _array.Contains(item, _comparer);
    public override void CopyTo(T[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);

    public override int IndexOf(T item)
    {
        for (int i = 0; i < Length; i++)
            if (_comparer.Equals(_array[i], item))
                return i;
        return -1;
    }

    public bool TryGetValue(int index, out T obj)
    {
        obj = default;
        if (index > End || index < Start)
            return false;

        obj = this[index];
        return !typeof(T).IsValueType && !_comparer.Equals(obj, default);
    }
}
