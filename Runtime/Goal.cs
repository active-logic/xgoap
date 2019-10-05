using System;
using NullRef = System.NullReferenceException;

namespace Activ.GOAP{
public readonly struct Goal<T>{

    public readonly Func<T, bool>  goal;
    public readonly Func<T, float> h;

    public Goal(Func<T, bool> goal, Func<T, float> h = null){
        if(goal == null) throw new NullRef("Goal is null");
        this.goal = goal;
        this.h    = h;
    }

}}
