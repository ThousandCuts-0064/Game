using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OffsetArray3<T> : CustomArray<T>
{
    private readonly T[,,] _array;
    private readonly int _lengths0x1;
    protected override Array Array => _array;
    public int Length0 { get; }
    public int Length1 { get; }
    public int Length2 { get; }
    public int Offset0 { get; }
    public int Offset1 { get; }
    public int Offset2 { get; }
    public virtual T this[int index0, int inedex1, int index2]
    {
        get => _array[index0, inedex1, index2];
        set => _array[index0, inedex1, index2] = value;
    }

    public OffsetArray3(int startIndex0, int startIndex1, int startIndex2, int endIndex0, int endIndex1, int endIndex2)
    {
        Length0 = endIndex0 - startIndex0;
        Length1 = endIndex1 - startIndex1;
        Length2 = endIndex2 - startIndex2;
        _lengths0x1 = Length0 * Length1;
        _array = new T[Length0, Length1, Length2];
        Offset0 = 0 + startIndex0;
        Offset1 = 0 + startIndex1;
        Offset2 = 0 + startIndex2;
    }

    public override IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                for (int k = 0; k < Length2; k++)
                    yield return _array[i, y, k];
    }

    public override void CopyTo(T[] array, int arrayIndex)
    {
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                for (int k = 0; k < Length2; k++)
                    array[arrayIndex + i * Length0 + y] = _array[i, y, k];
    }

    public override bool Contains(T item)
    {
        if (item == null)
        {
            for (int i = 0; i < Length0; i++)
                for (int y = 0; y < Length1; y++)
                    for (int k = 0; k < Length2; k++)
                        if (_array[i, y, k] == null) return true;
            return false;
        }

        EqualityComparer<T> c = EqualityComparer<T>.Default;
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                for (int k = 0; k < Length2; k++)
                    if (c.Equals(_array[i, y, k], item)) return true;
        return false;
    }

    public override int IndexOf(T item)
    {
        if (item == null)
        {
            for (int i = 0; i < Length0; i++)
                for (int y = 0; y < Length1; y++)
                    for (int k = 0; k < Length2; k++)
                        if (_array[i, y, k] == null) return i * Offset0 + Length0 + y * Offset1 + Length1 + k + Offset2;
            return -Length0 - Length1 - Length2 + Offset0+ + Offset1 + Offset2 - 1;
        }

        EqualityComparer<T> c = EqualityComparer<T>.Default;
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                for (int k = 0; k < Length2; k++)
                    if (c.Equals(_array[i, y, k], item)) return i * Offset0 + Length0 + y * Offset1 + Length1 + k + Offset2;
        return -Length0 - Length1 - Length2 + Offset0 + +Offset1 + Offset2 - 1;
    }

    protected override T SingleIndexGet(int index) =>
        this[Math.DivRem(index, _lengths0x1, out int indices1x2),
            Math.DivRem(indices1x2, Length1, out int index0),
            index0];

    protected override void SingleIndexSet(int index, T value) =>
        this[Math.DivRem(index, _lengths0x1, out int indices1x2),
            Math.DivRem(indices1x2, Length1, out int index2),
            index2] = value;
}
