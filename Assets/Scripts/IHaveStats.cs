﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IHaveStats<out T> where T : Stats
{
    public T Stats { get; }
}