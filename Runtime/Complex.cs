using System;

namespace Activ.GOAP{
public readonly struct Complex{

    public readonly Func<Cost> action;
    public readonly System.Action effect;

    public Complex(Func<Cost> method, System.Action effect){
        this.action = method;
        this.effect = effect;
    }

}}
