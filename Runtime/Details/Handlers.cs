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

    public bool Block(S s){
        switch(s){
        case S.Failed:
            return OnFail == Policy.Err;
        case S.MaxIterExceeded:
            return OnMaxIterOverflow == Policy.Err;
        case S.CapacityExceeded:
            return OnCapacityOverflow == Policy.Err;
        default:
            return false;
        }
    }

    public void OnResult(S s, string c = null){
        switch(s){
        case S.Failed:
            Do(s, c, OnFail, warnOnFail);                   break;
        case S.MaxIterExceeded:
            Do(s, c, OnMaxIterOverflow, warnOnOverflow);    break;
        case S.CapacityExceeded:
            Do(s, c, OnCapacityOverflow, warnOnOverflow);   break;
        }
    }

    void Do(S s, string c, Policy policy, bool warn){
        switch(policy){
        case Policy.Stop:
            if(warn) Warn(s, c);                            break;
        case Policy.Restart:
            if(warn) Warn(s, c);                            break;
        default:
            Err(s, c);                                      break;
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
