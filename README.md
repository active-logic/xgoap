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

## Getting started

For planning you need a model (Agent), a goal and, if available, a heuristic. I will use [Brent Owens' woodcutter](https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793) agent as an example.

In this case, a woodcutter has the *GetAxe*, *ChopLog* and *CollectBranches* actions. Here's our version of the wood chopper model

```cs
[Serializable]  // Helps cloning model state
public class WoodChopper : Agent{

    public bool hasAxe = false;
    public bool hasFirewood = false;

    public float cost { get; set; }

    // Available actions may change as the model is modified
    public Func<bool>[] actions => new Func<bool>[]{
        ChopLog, GetAxe, CollectBranches
    };

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

    // Needed to avoid reentering previously visited states while searching
    override public bool Equals(object other){
        if(other == null) return false;
        if(other is WoodChopper that){
            return this.hasAxe == that.hasAxe
                && this.hasFirewood == that.hasFirewood;
        } else return false;
    }

    // Helps quickly finding duplicate states
    override public int GetHashCode()
    => (hasAxe ? 1 : 0) + (hasFirewood ? 2 : 0);

    override public string ToString()
    => $"WoodChopper[axe:{hasAxe} f.wood:{hasFirewood} ]";

}
```

Run the model and get the next planned action:

```cs
var chopper = new WoodChopper();
var solver = new Solver<WoodChopper>();
var next = (string)solver.Next(chopper, x => x.hasFirewood);
```

In this example, `next` is a string because the action set consists in no-arg actions. Parametric actions are supported; they are concise and type safe. Check the [Baker](Tests/Models/Baker.cs) example.

The goal argument (here, `x => x.hasFirewood`) returns a `bool` to indicate whether the goal has been reached.

Quick and simple Unity integration via [GameAI.cs](Runtime/Unity/GameAI.cs)

## Getting involved

If you'd like to get involved, consider opening (or fixing) an issue.
Your support is appreciated!

<a href='https://ko-fi.com/A0114I97' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://az743702.vo.msecnd.net/cdn/kofi1.png?v=2' border='0' alt='Send a tip' /></a>
