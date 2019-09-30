using System;
using System.Collections.Generic;

/* A* planner */
public class ASPlanner : Planner{

    public string Eval<T>(T x, Func<T, bool> goal,
                               Func<T, float> h) where T: Agent{
        var avail = new List<T>(){ x };
        while(avail.Count > 0 && avail.Count < limit){
            var result = Expand(Pop(avail), avail, goal, h);
            if(result != null) return result.ToString();
        }
        return null;
    }

    object Expand<T>(T x, List<T> @out,
                     Func<T, bool> goal, Func<T, float> h)
                                                     where T: Agent{
        for(int i = 0; i < x.actions.Length; i++){
            var y = Clone(x);
            if(y.actions[i]()){
                if(goal(y)) return y;
                Insert(y, @out, h);
            }
        } return null;
    }

    void Insert<T>(T n, List<T> l, Func<T, float> h) where T: Agent{
        n.est = n.cost + h(n);
        for(int i = l.Count-1; i >= 0; i++){
            if(n.est < l[i].est) l.Insert(i, n);
        }
    }

}
