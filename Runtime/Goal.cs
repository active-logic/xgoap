using System;
using NullRef = System.NullReferenceException;

namespace Activ.GOAP{

public delegate float Heuristic<T>(T model);
public delegate bool  Condition<T>(T model);

public readonly struct Goal<T>{

    public readonly Condition<T> match;
    public readonly Heuristic<T> h;

    public Goal(Condition<T> goal, Heuristic<T> h = null){
        if(goal == null) throw new NullRef("Goal is null");
        this.match = goal;
        this.h     = h;
    }

    public static implicit operator Goal<T>(
                       (Condition<T> condition, Heuristic<T> h) arg)
    => new Goal<T>(arg.condition, arg.h);


}}
