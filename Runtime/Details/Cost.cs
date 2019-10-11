using System;
using ArgEx = System.ArgumentException;

namespace Activ.GOAP{
// Note: in general cost values not strictly positive are unsafe;
// however this isn't checked here since it depends on the solver
public readonly struct Cost{

    const string CostRangeErr = "Cost must be strictly positive";

    public readonly bool done;
    public readonly float cost;

    Cost(bool flag, float c){ done = flag; cost = c; }

    public static implicit operator Cost(bool flag)
    => new Cost(flag, 1);

    public static implicit operator Cost(float cost)
    => new Cost(true, cost);

    public static implicit operator Cost(ValueTuple<object, float> t){
        return new Cost(true, t.Item2);
    }

}}
