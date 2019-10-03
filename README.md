# Extended GOAP planner

Concise, adaptive GOAP (goal oriented action planning).

xGOAP is an object oriented planner. An action is a function of the model.

```cs
public bool ChopLog(){
    if(!hasAxe) return false; // Precondtion
    cost += 4;                // Pay cost
    hasFirewood = true;       // Effect
    return true;              // Success!
}
```

Use A\* or, if no cost function available, BFS.

Engine agnostic - unit test without pain.

# Getting started

For planning you need a model (Agent), a goal and, if available a heuristic. I will use Brent Owens' wood chopper agent as an example.

In GOAP an agent uses actions to update its model; each action has:
- Preconditions, used to decide whether the action can be performed.
- Effects, which determine the result(s) of an action
- A cost

In Owens example, a wood chopper has the *GetAxe*, *ChopLog* and *CollectBranches* actions. Here is the complete wood chopper model

```cs
[System.Serializable]
public class WoodChopper : Agent{

    public bool hasAxe = false;
    public bool hasFirewood = false;

    public float cost { get; set; }

    public bool GetAxe(){
        if(hasAxe) return false;
        cost += 2;
        return hasAxe = true;
    }

    public bool ChopLog(){
        if(!hasAxe) return false;
        cost += 4;
        return hasFirewood = true;
    }

    public bool CollectBranches(){
        cost += 8;
        return hasFirewood = true;
    }

    public Func<bool>[] actions => new Func<bool>[]{
        ChopLog, GetAxe, CollectBranches
    };

    override public string ToString()
    => $"WoodChopper[axe:{hasAxe} f.wood:{hasFirewood} ]";
}
```

Let's run the model and get the next planned action:

```cs
var chopper = new WoodChooper();
var plan = new Planner().Eval(chopper, x => x.hasFirewood);
```

The goal argument (here, `x => x.hasFirewood`) is just a function or lambda returning a bool to indicate whether the goal has been reached (true) or not (false).
