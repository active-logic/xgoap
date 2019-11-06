using System;
using System.ComponentModel;
using static Activ.GOAP.Util;
using S = Activ.GOAP.PlanningState;

namespace Activ.GOAP{  // See also Runtime/Unity/GameAI.cs)
public abstract partial class GameAI<T>
             : SolverOwner, INotifyPropertyChanged where T : class {

    public event PropertyChangedEventHandler PropertyChanged;
    //
    public float         cooldown;
    public bool          verbose;
    public Solver<T>     solver;
    public SolverParams  config   = new SolverParams();
    public Handlers      policies = new Handlers();
    public ActionHandler handler  = new ActionMap();
    //
    int index = 0;
    Goal<T>[] goals;

    public SolverStats stats  => solver;
    public Goal<T>     goal   => goals[index];
    public S           status => solver.status;

    public abstract Goal<T>[] Goals();
    public abstract T         Model();
    virtual public void       Idle(){ }
    virtual public bool       IsActing() => false;

    public virtual void Update(){
        solver = solver ?? new Solver<T>();
        if(policies.Block(status) || IsActing()) return;
        var s    = status;
        var next = (solver.status != S.Running) ? StartSolving()
                   : solver.Iterate(config.frameBudget);
        if(next != null) handler.Effect(next.Head(), this);
        policies.OnResult(status, ObjectName(this));
        if(s != status) NotifyPropertyChanged(nameof(status));
    }

    Node<T> StartSolving(){
        if(handler is ActionMap m) m.verbose = verbose;
        var model = Model(); if(model == null) return null;
        var goal = NextGoal();
        config.Reset(solver);
        return solver.Next(model, goal, config.frameBudget);
    }

    Goal<T> NextGoal(){
        goals = Goals();
        switch(status){
        case S.Done:    index = 0; break;
        case S.Running: throw new System.Exception("Invalid");
        default:
            index = (index + 1);
            if(index >= goals.Length){
                index = 0;
                #if UNITY_2018_1_OR_NEWER
                Cooldown();
                #endif
            } break;
        }
        return goals[index];
    }

    void NotifyPropertyChanged(String p) => PropertyChanged?
        .Invoke(this, new PropertyChangedEventArgs(p));

}}
