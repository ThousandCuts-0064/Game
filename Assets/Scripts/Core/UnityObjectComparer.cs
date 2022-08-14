using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UnityObjectComparer<T> : Singleton<UnityObjectComparer<T>>, IEqualityComparer<T> where T : UnityEngine.Object
{
    public bool Equals(T x, T y) => x == y;
    public int GetHashCode(T obj) => obj.GetHashCode();
}
