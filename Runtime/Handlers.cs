#if UNITY_2018_1_OR_NEWER
using UnityEngine;
#else
using System;
#endif

using S = Activ.GOAP.PlanningState;

namespace Activ.GOAP{
[System.Serializable] public class Handlers{

    public const bool Restart = true, NoAction = false;

    public Policy OnFail             = Policy.Restart,
                  OnCapacityOverflow = Policy.Err,
                  OnMaxIterOverflow  = Policy.Err;
    public bool   warnOnFail,
                  warnOnOverflow;

    public enum Policy{ Stop, Restart, Err }

    public bool OnResult(S s, string c = null){
        switch(s){
        case S.Failed:
            return Do(s, c, OnFail, warnOnFail);
        case S.MaxIterExceeded:
            return Do(s, c, OnMaxIterOverflow, warnOnOverflow);
        case S.CapacityExceeded:
            return Do(s, c, OnCapacityOverflow, warnOnOverflow);
        default: return NoAction;
        }
    }

    bool Do(S s, string c, Policy policy, bool w){
        switch(policy){
        case Policy.Stop:
            if(warnOnFail) Warn(s, c); return NoAction;
        case Policy.Restart:
            if(warnOnFail) Warn(s, c); return Restart;
        default:
            Err(s, c); return NoAction;

        }
    }

    #if UNITY_2018_1_OR_NEWER
    void Warn(S s, string c) => Debug.LogWarning ($"{c}: {s}");
    void Err (S s, string c) => Debug.LogError   ($"{c}: {s}");
    #else
    void Warn(S s, string c) => Console.WriteLine($"{c}: {s}");
    void Err (S s, string c) => Console.WriteLine($"{c}: {s}");
    #endif

}}
