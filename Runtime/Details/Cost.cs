using System;
using ArgEx = System.ArgumentException;
using static Activ.GOAP.Strings;

namespace Activ.GOAP{
// Note: in general cost values not strictly positive are unsafe;
// however this isn't checked here since it depends on the solver
public readonly struct Cost{

    public readonly bool done;
    public readonly float cost;

    Cost(bool flag, float c){ done = flag; cost = c; }

    public static implicit operator Cost(bool flag)
    => new Cost(flag, 1);

    public static implicit operator Cost(float cost)
    => new Cost(true, cost);

    public static implicit operator Cost((object, float cost) t)
    => new Cost(true, t.cost);

}}
