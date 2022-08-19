using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OffsetArray2<T> : CustomArray<T>
{
    private readonly T[,] _array;
    private readonly IEqualityComparer<T> _comparer;

    protected override Array Array => _array;
    public int Length0 { get; }
    public int Length1 { get; }
    public int Start0 { get; }
    public int Start1 { get; }
    public int End0 { get; }
    public int End1 { get; }
    public T this[int index0, int inedex1]
    {
        get => _array[index0 - Start0, inedex1 - Start1];
        set => _array[index0 - Start0, inedex1 - Start1] = value;
    }

    public OffsetArray2(int start0, int length0, int start1, int length1)
    {
        Length0 = length0;
        Length1 = length1;
        _array = new T[Length0, Length1];
        Start0 = start0;
        Start1 = start1;
        End0 = start0 + length0 - 1;
        End1 = start1 + length1 - 1;
        _comparer = EqualityComparer<T>.Default;
    }

    public OffsetArray2(int start0, int length0, int start1, int length1, IEqualityComparer<T> comparer)
        : this(start0, length0, start1, length1) => _comparer = comparer;

    public override T SimpleGet(int index) =>
        this[Math.DivRem(index, Length0, out int index1), index1];

    public override void SimpleSet(int index, T value) =>
        this[Math.DivRem(index, Length0, out int index1), index1] = value;

    public override IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                yield return _array[i, y];
    }

    public override void CopyTo(T[] array, int arrayIndex)
    {
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                array[arrayIndex + i * Length0 + y] = _array[i, y];
    }

    public override bool Contains(T item)
    {
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                if (_comparer.Equals(_array[i, y], item)) 
                    return true;
        return false;
    }

    public override int IndexOf(T item)
    {
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                if (_comparer.Equals(_array[i, y], item)) 
                    return i * Length1 + y;
        return -1;
    }

    public bool TryGetValue(int index0, int index1, out T obj)
    {
        obj = default;
        if (index0 > End0 || index0 < Start0
            || index1 > End1 || index1 < Start1)
            return false;

        obj = this[index0, index1];
        return !typeof(T).IsValueType && !_comparer.Equals(obj, default);   
    }
}
