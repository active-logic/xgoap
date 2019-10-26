using System;
using NullRef = System.NullReferenceException;
using Ex      = System.Exception;
using InvOp   = System.InvalidOperationException;
using S       = Activ.GOAP.PlanningState;

namespace Activ.GOAP{
public class Solver<T> : SolverStats where T : class{

    public const string INIT   = "%init";
    const string ZERO_COST_ERR = "Zero cost op is not allowed",
                 NO_INIT       = "Init state is null";
    //
    public int   maxNodes  = 1000,
                 maxIter   = 1000;
    public float tolerance = 0f;
    public bool  brfs      = false;
    public PlanningState state { get; private set; }
    public int  fxMaxNodes     { get; private set; }
    public int  I              { get; private set; }
    //
    T          storage;
    T          initialState;
    Goal<T>    goal;
    NodeSet<T> avail = null;

    public bool isRunning => state == S.Running;

    public Node<T> Next(T s, in Goal<T> g, int iter=-1){
        if(s == null) throw new NullRef(NO_INIT);
        initialState = s;
        goal         = g;
        avail        = new NodeSet<T>(s, g.h, !brfs, maxNodes, tolerance);
        I            = 0;
        return Iterate(iter);
    }

    public Node<T> Iterate(int iter=-1){
        if(initialState == null) throw new InvOp(NO_INIT);
        if(state == S.MaxIterExceeded) return null;
        if(iter == -1) iter = maxIter;
        int i = 0;
        while(avail && i++ < iter && I++ < maxIter){
            //rint($"# {I} (avail: {avail.count}) --------------- ");
            var current = avail.Pop();
            if(goal.match(current.state)){
                state = S.Done;
                return current;
            }
            ExpandActions(current, avail);
            ExpandMethods(current, avail);
            if(avail.count > fxMaxNodes) fxMaxNodes = avail.count;
            //rint("Avail: " + avail.count);
        }
        if(avail.capacityExceeded){
            state = S.CapacityExceeded;
        }else{
            state = avail ? (I < maxIter ? S.Running : S.MaxIterExceeded)
                        : S.Failed;
        }
        return null;
    }

    void ExpandActions(Node<T> x, NodeSet<T> @out){
        if(!(x.state is Agent p)) return;
        if(p.Actions() == null) return;
        for(int i = 0; i < p.Actions().Length; i++){
            var y = Clone(x.state);
            var q = y as Agent;
            var r = q.Actions()[i]();
            if(r.done){
                if(!brfs && (r.cost <= 0))
                    throw new Ex(ZERO_COST_ERR);
                var name = p.Actions()[i].Method.Name;
                if(@out.Insert(new Node<T>(name, y, x, r.cost)))
                    storage = null;
            }
        }
    }

    void ExpandMethods(Node<T> x, NodeSet<T> @out){
        if(!(x.state is Parametric p)) return;
        if(p.Functions() == null) return;
        for(int i = 0; i < p.Functions().Length; i++){
            var y = Clone(x.state);
            var q = y as Parametric;
            var r = q.Functions()[i].action();
            if(r.done){
                if(!brfs && r.cost <= 0)
                    throw new Ex(ZERO_COST_ERR);
                var effect = p.Functions()[i].effect;
                if(@out.Insert(new Node<T>(effect, y, x, r.cost)))
                    storage = null;
            }
        }
    }

    internal T Clone(T x) => (x is Clonable<T> c)
        ? c.Clone(storage = storage ?? c.Allocate())
        : CloneUtil.DeepClone(x);

}}
