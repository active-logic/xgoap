using System;
using NullRef = System.NullReferenceException;
using Ex      = System.Exception;
using UnityEngine;
using static State;

public class Solver<T> where T : Agent{

    const string ZERO_COST_ERR = "Zero cost op is not allowed";
    public int maxNodes = 1000;
    public int maxIter  = 1000;

    public bool brfs = false;

    public Node<T>[] Path(T x, Func<T, bool> goal,
                            Func<T, float> h = null)
    => Path(x, new Goal<T>(goal, h));

    public Node<T>[] Path(T x, in Goal<T> g) {
        if(x == null) throw new NullRef("Agent is null");
        var avail = new NodeSet<T>(x, g.h, !brfs, maxNodes);
        int i = 0;
        while(avail && i++ < maxIter){
            var current = avail.Pop();
            if(g.goal(current.state)) return current.Path();
            ExpandActions(current, avail);
            ExpandMethods(current, avail);
        }
        return null;
    }

    public object Next(T x, Func<T, bool> goal,
                            Func<T, float> h = null)
    => Next(x, new Goal<T>(goal, h));

    public object Next(T x, in Goal<T> g) {
        if(x == null) throw new NullRef("Agent is null");
        var avail = new NodeSet<T>(x, g.h, !brfs, maxNodes);
        int i = 0;
        while(avail && i++ < maxIter){
            var current = avail.Pop();
            if(g.goal(current.state)) return current.Head();
            ExpandActions(current, avail);
            ExpandMethods(current, avail);
        }
        return State.NotFound;
    }

    void ExpandActions(Node<T> x, NodeSet<T> @out){
        if(x.state.actions == null) return;
        for(int i = 0; i < x.state.actions.Length; i++){
            var y = Clone(x.state);
            if(y.actions[i]()){
                if(!brfs && (y.cost == x.state.cost))
                    throw new Ex(ZERO_COST_ERR);
                var name = x.state.actions[i].Method.Name;
                @out.Insert(new Node<T>(name, y, x));
            }
        }
    }

    void ExpandMethods(Node<T> x, NodeSet<T> @out){
        if(!(x.state is Parametric p)) return;
        if(p.methods == null) return;
        for(int i = 0; i < p.methods.Length; i++){
            var y = Clone(x.state);
            var q = y as Parametric;
            if(q.methods[i].action()){
                if(!brfs && (y.cost == x.state.cost))
                    throw new Ex(ZERO_COST_ERR);
                var effect = p.methods[i].effect;
                @out.Insert(new Node<T>(effect, y, x));
            }
        }
    }

    internal static T Clone(T x) => CloneUtil.DeepClone(x);

}
