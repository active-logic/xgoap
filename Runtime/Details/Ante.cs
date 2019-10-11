using System;

namespace Activ.GOAP{
// Note: In general cost values not strictly positive are unsafe;
// however this isn't checked here since it depends on the solver.
public readonly struct ante{

    const string CostRangeErr = "Cost must be strictly positive";
    public readonly bool done;
    public readonly float cost;

    ante(bool flag, float c){ done = flag; cost = c; }

    public static implicit operator ante(bool flag)
    => new ante(flag, 1);

    public static implicit operator ante(float cost)
    => new ante(true, cost);

    public static implicit operator ante(ValueTuple<object, float> t)
    => new ante(true, t.Item2);

}}
