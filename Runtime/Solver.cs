using System;
using NullRef = System.NullReferenceException;
using Ex      = System.Exception;
using S       = Activ.GOAP.PlanningState;
using static Activ.GOAP.State;

namespace Activ.GOAP{
public class Solver<T> where T : Agent{

    const string ZERO_COST_ERR = "Zero cost op is not allowed",
                 NO_INIT       = "Init state is null";
    //
    public int  maxNodes = 1000,
                maxIter  = 1000;
    public bool brfs     = false;
    public PlanningState state { get; private set; }
    //
    T          initialState;
    Goal<T>    goal;
    NodeSet<T> avail = null;
    int        I;

    public bool isRunning => state == S.Running;

    public object Next(T initState,
                       Func<T, bool>  goal, Func<T, float> h=null,
                       int iter=-1)
    => Next(initState, new Goal<T>(goal, h), iter);

    public object Next(T initState, in Goal<T> goal, int iter=-1)
    => Start(initState, goal, iter)?.Head();

    public object Path(T initState, in Goal<T> goal, int iter=-1)
    => Start(initState, goal, iter)?.Path();

    public object Iterate(int iter=-1) => DoIterate(iter)?.Head();

    public object IterateAndReturnPath(int iter=-1)
    => DoIterate(iter)?.Path();

    Node<T> Start(T s, in Goal<T> g, int iter=-1){
        if(s == null) throw new NullRef(NO_INIT);
        initialState = s;
        goal         = g;
        avail        = new NodeSet<T>(s, g.h, !brfs, maxNodes);
        I            = 0;
        return DoIterate(iter);
    }

    Node<T> DoIterate(int iter=-1){
        if(initialState == null) throw new NullRef(NO_INIT);
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
        }
        state = avail ? (I < maxIter ? S.Running : S.Stalled)
                      : S.Failed;
        return null;
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

}}
