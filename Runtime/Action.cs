using System;

public readonly struct Action{

    public readonly Func<bool> action;
    public readonly System.Action effect;

    public Action(Func<bool> method, System.Action effect){
        this.action = method;
        this.effect = effect;
    }

}
