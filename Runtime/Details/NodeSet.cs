using System;
using System.Collections.Generic;

namespace Activ.GOAP{
public class NodeSet<T> : Base{

    float          precision = 0;
    internal bool  sorted;
    int            capacity;
    Func<T, float> h;
    HashSet<T>     states = new HashSet<T>();
    List<Node<T>>  list   = new List<Node<T>>();

    public NodeSet(T x, Func<T, float> h, bool sorted  = true,
                                          int  capacity = 128,
                                          float precision = 0){
        this.h = h;
        this.sorted    = sorted;
        this.capacity  = capacity;
        this.precision = precision;
        states.Add(Assert(x, "Initial state"));
        list.Add(new Node<T>(Solver<T>.INIT, x));
    }

    public bool capacityExceeded => count > capacity;

    public static implicit operator bool(NodeSet<T> self)
    => self.count > 0 && self.count <= self.capacity;

    internal int count => list.Count;

    public void Insert(Node<T> n){
        if(!states.Add(n.state)) return;
        if(sorted){
            n.value = n.cost + (h != null ? h(n.state) : 0);
            if(precision > 0) n.value = (int)(n.value / precision);
            // NOTE: In actual use, tested 4x faster than SortedSet;
            //  Likely bottleneck with SortedSet is the API, not the
            // algorithm; SortedSet needs total, ordering:
            // - a.CompareTo(a) == 0
            // - a.CompareTo(b) must positive if b.CompareTo(a) is
            // negative.
            // Superficially these rules are sound. In practice to
            // just order elements by cost, this is overkill.
            // Also getting Min/Max item is cheap, but there's no way
            // to combine this with a 'pop'
            for(int i = list.Count-1; i >= 0; i--){
                if(n.value <= list[i].value){
                    list.Insert(i + 1, n);
                    return;
                }
            }
        } list.Insert(0, n);
    }

    public Node<T> Pop(){
        int i = list.Count-1; var n = list[i]; list.RemoveAt(i);
        return n;
    }

}}
