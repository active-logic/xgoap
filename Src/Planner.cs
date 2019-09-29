using System;
using System.Collections.Generic;

public class Planner{

    public string Eval<T>(T x, Func<T, bool> goal) where T: Agent{
        var avail = new List<T>();
        avail.Add(x);
        while(avail.Count > 0){
            var current = avail[0];
            avail.RemoveAt(0);
            var result = Expand(current, avail, goal);
            if(result != null) return result.ToString();
        }
        return null;
    }

    object Expand<T>(T x, List<T> @out,  Func<T, bool> goal)
                                                     where T: Agent{
        for(int i = 0; i < x.actions.Length; i++){
            var y = Clone(x);
            if(y.actions[i]()){
                if(goal(y)) return y;
                @out.Add(y);
            }
        } return null;
    }

    internal static T Clone<T>(T x) => CloneUtil.DeepClone(x);

}
