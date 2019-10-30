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
- [Demo project](https://github.com/active-logic/xgoap-demos) to help you get started
- \[COMING SOON] integration with the BT Framework of Awesome, [Active Logic](https://github.com/active-logic/activelogic-cs) 🚀

## Install

Clone the repository and add the package to your project as normal.

For Unity 3D:
- Add `xgoap/package.json` via *package manager > + > add package from disk.*
- Alternatively, add `"com.activ.goap": "https://github.com/active-logic/xgoap.git"` to *Packages/manifest.json*

## Getting started

Planning requires a *model* and a *goal*; if available, also provide a *heuristic*.
I will borrow [Brent Owens' woodcutter](https://gamedevelopment.tutsplus.com/tutorials/goal-oriented-action-planning-for-a-smarter-ai--cms-20793) example.

A woodcutter has the *GetAxe*, *ChopLog* and *CollectBranches* actions. Here is our implementation of the woodcutter planning model:

```cs
using Activ.GOAP;

public class WoodChopper : Agent, Clonable<WoodChopper>{

    public bool hasAxe, hasFirewood;
    Option[] opt;  // Caching reduces array alloc overheads

    public Option[] Options()
    => opt = opt ?? new Option[]{ ChopLog, GetAxe, CollectBranches };

    public Cost GetAxe(){
        if(hasAxe) return false;
        hasAxe = true;
        return 2;
    }

    public Cost ChopLog() => hasAxe ? (Cost)(hasFirewood = true, 4f) : (Cost)false;

    public Cost CollectBranches() => (hasFirewood = true, 8);

    // Clonable<WoodChopper>

    public WoodChopper Allocate() => new WoodChopper();

    public WoodChopper Clone(WoodChopper x){
        x.hasAxe      = hasAxe;
        x.hasFirewood = hasFirewood;
        return x;
    }

    // Override for correctness (don't compare refs) and faster hashes

    override public bool Equals(object other) => other is WoodChopper that
        && hasAxe == that.hasAxe && hasFirewood == that.hasFirewood;

    override public int GetHashCode() => (hasAxe ? 1 : 0)
                                       + (hasFirewood ? 2 : 0);

}
```

Run the model and get the next planned action:

```cs
var chopper = new WoodChopper();
var solver  = new Solver<WoodChopper>();
var next    = solver.Next(chopper, goal: (x => x.hasFirewood, null));
```

Parametric actions are supported; they are concise and type safe. Check the [Baker](Tests/Models/Baker.cs) example.

Quick and simple Unity integration via [GameAI.cs](Runtime/GameAI.cs) - for details, [read here](Documentation/BakerUnity.md).

Ready to GOAP? [follow the guide](Documentation/Overview.md).

## Getting involved

If you'd like to get involved, consider opening (or fixing) an issue.
Your support is appreciated!

<a href='https://ko-fi.com/A0114I97' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://az743702.vo.msecnd.net/cdn/kofi1.png?v=2' border='0' alt='Send a tip' /></a>
