using System;
using System.Collections.Generic;

/* Best first planner (cost value) */
public class BePlanner : Planner{

    public string Eval<T>(T x, Func<T, bool> goal) where T: Agent{
        var avail = new List<T>(){ x };
        while(avail.Count > 0 && avail.Count < limit){
            var result = Expand(Pop(avail), avail, goal);
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
                Insert(y, @out);
            }
        } return null;
    }

    void Insert<T>(T n, List<T> l) where T: Agent{
        for(int i = l.Count-1; i >= 0; i++){
            if(n.cost < l[i].cost) l.Insert(i, n);
        }
    }

}
