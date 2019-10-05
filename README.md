# Beyond G.O.A.P

UPM package offering a fresh take on [Goal oriented action planning](http://alumni.media.mit.edu/~jorkin/goap.html).

In GOAP, actions have preconditions, effects, and a cost. Boilerplate? Have a look.

```cs
public bool ChopLog(){
    if(!hasAxe) return false; // Precondtion
    cost += 4;                // Pay cost
    hasFirewood = true;       // Effect
    return true;              // Success!
}
```

Use A\* or, if no cost function available, BFS.

Engine agnostic - Unit test without pain (for now you'll need to delete a few files since there *is* a Unity integration).

# Getting started [REVIEW]

For planning you need a model (Agent), a goal and, if available, a heuristic. I will borrow Brent Owens' wood chopper agent as an example.

In Owens example, a wood chopper has the *GetAxe*, *ChopLog* and *CollectBranches* actions. Here's our version of the wood chopper model

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
