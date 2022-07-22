using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Chunk
{
    public const int SIZE = 32;
    public Block[,,] Blocks { get; } = new Block[SIZE, SIZE, SIZE];
}
