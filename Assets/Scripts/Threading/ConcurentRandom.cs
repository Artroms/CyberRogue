using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System;


public class ConcurentRandom : Random
{
    public override double NextDouble()
    {
        var id = Thread.CurrentThread.ManagedThreadId;
        return base.NextDouble();
    }

    public override int Next()
    {
        return base.Next();
    }

    public override int Next(int maxValue)
    {
        return base.Next(maxValue);
    }

    public override int Next(int minValue, int maxValue)
    {
        return base.Next(minValue, maxValue);
    }

    public override void NextBytes(byte[] buffer)
    {
        base.NextBytes(buffer);
    }
}
