using System;

public readonly struct Action{

    public readonly Func<bool> action;
    public readonly object     effect;

    public Action(Func<bool> method, object effect){
        this.action = method;
        this.effect = effect;
    }

}
