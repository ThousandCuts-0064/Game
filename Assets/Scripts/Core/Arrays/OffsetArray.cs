using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OffsetArray<T> : CustomArray<T>
{
    private readonly T[] _array;
    protected override Array Array => _array;
    public int Offset { get; }
    public T this[int index]
    { 
        get => _array[index + Offset]; 
        set => _array[index + Offset] = value; 
    }

    public OffsetArray(int startIndex, int endIndex)
    {
        _array = new T[endIndex - startIndex];
        Offset = 0 + startIndex;
    }

    public override IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
    public override int IndexOf(T item) => ((IList<T>)_array).IndexOf(item) + Offset;
    public override bool Contains(T item) => _array.Contains(item);
    public override void CopyTo(T[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);

    protected override T SingleIndexGet(int index) => this[index];
    protected override void SingleIndexSet(int index, T value) => this[index] = value;
}
