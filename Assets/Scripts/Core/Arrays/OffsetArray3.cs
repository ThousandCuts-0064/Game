using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OffsetArray3<T> : CustomArray<T>
{
    private readonly T[,,] _array;
    private readonly IEqualityComparer<T> _comparer;
    private readonly int _lengths1x2;
    protected override Array Array => _array;
    public int Length0 { get; }
    public int Length1 { get; }
    public int Length2 { get; }
    public int Start0 { get; }
    public int Start1 { get; }
    public int Start2 { get; }
    public int End0 { get; }
    public int End1 { get; }
    public int End2 { get; }
    public T this[int index0, int inedex1, int index2]
    {
        get => _array[index0 - Start0, inedex1 - Start1, index2 - Start2];
        set => _array[index0 - Start0, inedex1 - Start1, index2 - Start2] = value;
    }

    public OffsetArray3(int start0, int length0, int start1, int length1, int start2, int length2)
    {
        Length0 = length0;
        Length1 = length1;
        Length2 = length2;
        _lengths1x2 = Length1 * Length2;
        _array = new T[Length0, Length1, Length2];
        Start0 = start0;
        Start1 = start1;
        Start2 = start2;
        End0 = start0 + length0 - 1;
        End1 = start1 + length1 - 1;
        End2 = start2 + length2 - 1;
        _comparer = EqualityComparer<T>.Default;
    }

    public OffsetArray3(int start0, int length0, int start1, int length1, int start2, int length2, IEqualityComparer<T> comparer)
        : this(start0, length0, start1, length1, start2, length2) => _comparer = comparer;

    public override T SimpleGet(int index) =>
        this[Math.DivRem(index, _lengths1x2, out int indices1x2),
            Math.DivRem(indices1x2, Length1, out int index2),
            index2];

    public override void SimpleSet(int index, T value) =>
        this[Math.DivRem(index, _lengths1x2, out int indices1x2),
            Math.DivRem(indices1x2, Length1, out int index2),
            index2] = value;

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
                    array[arrayIndex + i * _lengths1x2 + y * Length2 + k] = _array[i, y, k];
    }

    public override bool Contains(T item)
    {
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                for (int k = 0; k < Length2; k++)
                    if (_comparer.Equals(_array[i, y, k], item)) 
                        return true;
        return false;
    }

    public override int IndexOf(T item)
    {
        for (int i = 0; i < Length0; i++)
            for (int y = 0; y < Length1; y++)
                for (int k = 0; k < Length2; k++)
                    if (_comparer.Equals(_array[i, y, k], item)) 
                        return i * _lengths1x2 + y * Length2 + k;
        return -1;
    }

    public bool TryGetValue(int index0, int index1, int index2, out T obj)
    {
        obj = default;
        if (index0 > End0 || index0 < Start0
            || index1 > End1 || index1 < Start1
            || index2 > End2 || index2 < Start2)
            return false;

        obj = this[index0, index1, index2];
        return !typeof(T).IsValueType && !_comparer.Equals(obj, default);
    }
}
