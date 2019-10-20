using System;
using NullRef = System.NullReferenceException;
using Ex      = System.Exception;
using InvOp   = System.InvalidOperationException;
using S       = Activ.GOAP.PlanningState;

namespace Activ.GOAP{
public class Solver<T> : SolverStats where T : Agent{

    public const string INIT   = "%init";
    const string ZERO_COST_ERR = "Zero cost op is not allowed",
                 NO_INIT       = "Init state is null";
    //
    public int  maxNodes = 1000,
                maxIter  = 1000;
    public bool brfs     = false;
    public PlanningState state { get; private set; }
    public int  fxMaxNodes     { get; private set; }
    public int  I              { get; private set; }
    //
    T          initialState;
    Goal<T>    goal;
    NodeSet<T> avail = null;

    public bool isRunning => state == S.Running;

    public Node<T> Next(T s, in Goal<T> g, int iter=-1){
        if(s == null) throw new NullRef(NO_INIT);
        initialState = s;
        goal         = g;
        avail        = new NodeSet<T>(s, g.h, !brfs, maxNodes);
        I            = 0;
        return Iterate(iter);
    }

    public Node<T> Iterate(int iter=-1){
        if(initialState == null) throw new InvOp(NO_INIT);
        if(state == S.Stalled) return null;
        if(iter == -1) iter = maxIter;
        int i = 0;
        while(avail && i++ < iter && I++ < maxIter){
            var current = avail.Pop();
            if(goal.match(current.state)){
                state = S.Done;
                return current;
            }
            ExpandActions(current, avail);
            ExpandMethods(current, avail);
            if(avail.count > fxMaxNodes) fxMaxNodes = avail.count;
        }
        state = avail ? (I < maxIter ? S.Running : S.Stalled)
                      : S.Failed;
        return null;
    }

    void ExpandActions(Node<T> x, NodeSet<T> @out){
        if(x.state.actions == null) return;
        for(int i = 0; i < x.state.actions.Length; i++){
            var y = Clone(x.state);
            var r = y.actions[i]();
            if(r.done){
                if(!brfs && (r.cost <= 0))
                    throw new Ex(ZERO_COST_ERR);
                var name = x.state.actions[i].Method.Name;
                @out.Insert(new Node<T>(name, y, x, r.cost));
            }
        }
    }

    void ExpandMethods(Node<T> x, NodeSet<T> @out){
        if(!(x.state is Parametric p)) return;
        if(p.methods == null) return;
        for(int i = 0; i < p.methods.Length; i++){
            var y = Clone(x.state);
            var q = y as Parametric;
            var r = q.methods[i].action();
            if(r.done){
                if(!brfs && r.cost <= 0)
                    throw new Ex(ZERO_COST_ERR);
                var effect = p.methods[i].effect;
                @out.Insert(new Node<T>(effect, y, x, r.cost));
            }
        }
    }

    internal static T Clone(T x) => CloneUtil.DeepClone(x);

}}
