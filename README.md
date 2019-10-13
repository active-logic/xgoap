**Pre-release** - *First release planned: Nov. 2019; APIs are more likely to change during the pre-release period.*

[![Build Status](https://travis-ci.com/active-logic/xgoap.svg?branch=master)](https://travis-ci.com/active-logic/xgoap)
[![codecov](https://codecov.io/gh/active-logic/xgoap/branch/master/graph/badge.svg)](https://codecov.io/gh/active-logic/xgoap)

# Beyond G.O.A.P

A fresh take on [Goal oriented action planning](http://alumni.media.mit.edu/~jorkin/goap.html).

In GOAP, actions have preconditions, effects, and a cost. Boilerplate? Have a look.

```cs
public Cost ChopLog(){
    if(!hasAxe) return false;  // Precondtion
    hasFirewood = true;        // Effect
    return 4;                  // Cost
}
```

Use A\* or, if no cost function available, BFS.

- Engine agnostic models - test without pain.
- .NET Core compatible
- Unity integration with UPM support
- \[COMING SOON] integration with the BT Framework of Awesome, [Active Logic](https://github.com/active-logic/activelogic-cs) 🚀

## Install

Clone the repository and add the package to your project as normal.

For Unity 3D:
- Add `xgoap/package.json` via *package manager > + > add package from disk.*
- Alternatively, add `"com.activ.goap": "https://github.com/active-logic/xgoap.git"` to *Packages/manifest.json*

## Getting started

For planning you need a model (Agent), a goal and, if available, a heuristic. I will use [Brent Owens' woodcutter](https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793) agent as an example.

In this case, a woodcutter has the *GetAxe*, *ChopLog* and *CollectBranches* actions. Here's our version of the wood chopper model

```cs
using Activ.GOAP;

[Serializable]  // Helps cloning model state
public class WoodChopper : Agent{

    public bool hasAxe, hasFirewood;

    // Available actions may change as the model is modified
    public Func<Cost>[] actions => new Func<Cost>[]{
        ChopLog, GetAxe, CollectBranches
    };

    public Cost GetAxe(){
        if(hasAxe) return false;
        hasAxe = true;
        return 2;
    }

    public bool ChopLog(){
        if(!hasAxe) return false;
        hasFirewood = true;
        return 4;
    }

    // Expression-bodied shorthands are supported
    public bool CollectBranches() => (hasFirewood = true, 8);

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
var solver  = new Solver<WoodChopper>();
var next    = solver.Next(chopper, new Goal<WoodChopper>(x => x.hasFirewood));
```

Parametric actions are supported; they are concise and type safe. Check the [Baker](Tests/Models/Baker.cs) example.

The goal argument (here, `x => x.hasFirewood`) returns a `bool` to indicate whether the goal has been reached.

Quick and simple Unity integration via [GameAI.cs](Runtime/GameAI.cs) - check here for a [quick example](Documentation/BakerUnity.md).

## Getting involved

If you'd like to get involved, consider opening (or fixing) an issue.
Your support is appreciated!

<a href='https://ko-fi.com/A0114I97' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://az743702.vo.msecnd.net/cdn/kofi1.png?v=2' border='0' alt='Send a tip' /></a>
