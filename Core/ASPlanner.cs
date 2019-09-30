using System;
using System.Collections.Generic;
using NullRef = System.NullReferenceException;
using Ex      = System.Exception;

public class ASPlanner : Planner{

    public bool brfs = false;

    public string Eval<T>(T x,
                          Func<T, bool> goal,
                          Func<T, float> h = null) where T: Agent{
        if(x    == null) throw new NullRef("Agent is null");
        if(goal == null) throw new NullRef("Goal is null");
        var avail = new List<T>(){ x };
        int i = 0;
        while(avail.Count > 0 && avail.Count < maxNodes
                              && i++ < maxIter)
        {
            var result = Expand(Pop(avail), avail, goal, h);
            if(result != null) return result.ToString();
        }
        return null;
    }

    object Expand<T>(T x, List<T> @out,
                     Func<T, bool> goal, Func<T, float> h)
                                                     where T: Agent{
        for(int i = 0; i < x.actions.Length; i++){
            var y = Clone(x);
            if(y.actions[i]()){
                if(goal(y)) return y;
                if(!brfs && (y.cost == x.cost))
                    throw new Ex("Zero cost op is not allowed");
                Insert(y, @out, h);
            }
        } return null;
    }

    void Insert<T>(T n, List<T> l, Func<T, float> h) where T: Agent{
        if(brfs)
            l.Insert(0, n);
        else{
            n.est = n.cost + (h != null ? h(n) : 0);
            for(int i = l.Count-1; i >= 0; i++)
                if(n.est < l[i].est) l.Insert(i, n);
        }
    }

}
