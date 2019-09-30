using System;
using System.Collections.Generic;
using NullRef = System.NullReferenceException;
using Ex      = System.Exception;
using UnityEngine;

public class ASPlanner : Planner{

    public bool brfs = false;

    public string Eval<T>(T x, Func<T, bool> goal,
                               Func<T, float> h = null)
                                                      where T: Agent
    => Eval(x, new Goal<T>(goal, h));

    public string Eval<T>(T x, in Goal<T> g) where T: Agent
    {
        if(x == null) throw new NullRef("Agent is null");
        var avail = new List<T>(){ x };
        int i = 0;
        while(avail.Count > 0 && avail.Count < maxNodes
                              && i++         < maxIter  )
        {
            var result = Expand(Pop(avail), avail, g);
            if(result != null) return ""+result;
        }
        return null;
    }

    object Expand<T>(T x, List<T> @out, in Goal<T> g)
                                                     where T: Agent{
        for(int i = 0; i < x.actions.Length; i++){
            Debug.Log(x.actions[i]);
            var y = Clone(x);
            if(y.actions[i]()){
                if(g.goal(y)) return x.actions[i].Method.Name;
                if(!brfs && (y.cost == x.cost))
                    throw new Ex("Zero cost op is not allowed");
                Insert(y, @out, g.h);
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

    //class Node<T>{
    //    string action;
    //    T state;
    //}

}
