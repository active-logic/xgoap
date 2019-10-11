using System;

namespace Activ.GOAP{
public readonly struct Action{

    public readonly Func<ante> action;
    public readonly System.Action effect;

    public Action(Func<ante> method, System.Action effect){
        this.action = method;
        this.effect = effect;
    }

}}
