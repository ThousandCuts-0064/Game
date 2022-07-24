using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ChunkAlreadyExistsException : ArgumentException
{
    public ChunkAlreadyExistsException() : base() { }
    public ChunkAlreadyExistsException(string messege) : base(messege) { }
}
