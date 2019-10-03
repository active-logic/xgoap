using System;
using NullRef = System.NullReferenceException;
using Ex      = System.Exception;
using UnityEngine;

public class Solver{

    public int maxNodes = 1000;
    public int maxIter  = 1000;

    public const string INIT = "%init", NOT_FOUND = "%not_found";

    public bool brfs = false;

    public string Eval<T>(T x, Func<T, bool> goal,
                               Func<T, float> h = null)
                                                      where T: Agent
    => Eval(x, new Goal<T>(goal, h));

    public string Eval<T>(T x, in Goal<T> g) where T: Agent
    {
        if(x == null) throw new NullRef("Agent is null");
        var avail = new NodeSet<T>(x, g.h, !brfs, maxNodes);
        int i = 0;
        while(avail && i++ < maxIter){
            var current = avail.Pop();
            if(g.goal(current.state)) return current.Head();
            var result = Expand(current, avail);
            if(result != null) return result.ToString();
        }
        return NOT_FOUND;
    }

    object Expand<T>(Node<T> x, NodeSet<T> @out) where T: Agent{
        for(int i = 0; i < x.state.actions.Length; i++){
            var y = Clone(x.state);
            if(y.actions[i]()){
                var name = x.state.actions[i].Method.Name;
                if(!brfs && (y.cost == x.state.cost))
                    throw new Ex("Zero cost op is not allowed");
                @out.Insert(new Node<T>(name, y, x));
            }
        } return null;
    }

    internal static T Clone<T>(T x) => CloneUtil.DeepClone(x);

}
