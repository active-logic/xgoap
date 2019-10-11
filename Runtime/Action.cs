using System;

namespace Activ.GOAP{
public readonly struct Action{

    public readonly Func<Cost> action;
    public readonly System.Action effect;

    public Action(Func<Cost> method, System.Action effect){
        this.action = method;
        this.effect = effect;
    }

}}
