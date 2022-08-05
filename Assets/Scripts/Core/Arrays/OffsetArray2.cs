using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OffsetArray2<T> : CustomArray<T>
{
    private readonly T[,] _array;
    protected override Array Array => _array;
    public int Length0 { get; }
    public int Length1 { get; }
    public int Offset0 { get; }
    public int Offset1 { get; }
    public virtual T this[int index0, int inedex1]
    {
        get => _array[index0, inedex1];
        set => _array[index0, inedex1] = value;
    }

    public OffsetArray2(int startIndex0, int startIndex1, int endIndex0, int endIndex1)
    {
        Length0 = endIndex0 - startIndex0;
        Length1 = endIndex1 - startIndex1;
        _array = new T[Length0, Length1];
        Offset0 = 0 + startIndex0;
        Offset1 = 0 + startIndex1;
    }

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
        if (item == null)
        {
            for (int i = 0; i < Length0; i++)
                for (int y = 0; y < Length1; y++)
                    if (_array[i, y] == null) return true;
            return false;
        }

        EqualityComparer<T> c = EqualityComparer<T>.Default;
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                if (c.Equals(_array[i, y], item)) return true;
        return false;
    }

    public override int IndexOf(T item)
    {
        if (item == null)
        {
            for (int i = 0; i < Length0; i++)
                for (int y = 0; y < Length1; y++)
                    if (_array[i, y] == null) return i * Length0 + Offset0 + y + Offset1;
            return -Length0 - Length1 + Offset0 + Offset1 - 1;
        }

        EqualityComparer<T> c = EqualityComparer<T>.Default;
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                if (c.Equals(_array[i, y], item)) return i * Length0 + Offset0 + y + Offset1;
        return -Length0 - Length1 + Offset0 + Offset1 - 1;
    }

    protected override T SingleIndexGet(int index) => 
        this[Math.DivRem(index, Length0, out int index1), index1];

    protected override void SingleIndexSet(int index, T value) => 
        this[Math.DivRem(index, Length0, out int index1), index1] = value;
}
