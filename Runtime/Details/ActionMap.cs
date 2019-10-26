using System;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_2018_1_OR_NEWER
using UnityEngine;
#endif

namespace Activ.GOAP{
public class ActionMap : ActionHandler{

    static readonly object[] NoArg = new object[0];
    public bool verbose;
    Dictionary<string, MethodInfo> map;

    int frameCount{get{
        #if UNITY_2018_1_OR_NEWER
        return Time.frameCount;
        #else
        return 0;
        #endif
    }}

    void ActionHandler.Effect<T>(object action, GameAI<T> client){
        switch(action){
        case string noArg:
            // TODO should be Solver.INIT but move that constant
            // somewhere else first
            if(noArg == "%init") return;
            Print($"No-arg: {noArg} @{frameCount}");
            Map(noArg, client).Invoke(client, NoArg);
            return;
        case System.Action method:
            Print($"Delegate: {method.Method.Name} @{frameCount}");
            method();
            return;
        case null:
            client.Idle();
            return;
        default:
            throw new ArgumentException($"Unknown arg: " + action);
        }
    }

    MethodInfo Map<T>(string name, GameAI<T> client) where T: class{
        map = map ?? new Dictionary<string, MethodInfo>();
        MethodInfo method;
        map.TryGetValue(name, out method);
        if(method == null)
            map[name] = method = client.GetType().GetMethod(name);
        return method;
    }

    internal void Print(object arg){
        if(!verbose) return;
        #if UNITY_2018_1_OR_NEWER
        Debug.Log(arg);
        #else
        System.Console.WriteLine(arg);
        #endif
    }

}
}
