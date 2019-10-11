using System;
using System.Collections.Generic;

namespace Activ.GOAP{
public class NodeSet<T> : Base where T : Agent{

    internal bool sorted;
    int capacity;
    Func<T, float> h;
    HashSet<T>     states = new HashSet<T>();
    List<Node<T>>  list   = new List<Node<T>>();

    public NodeSet(T x, Func<T, float> h, bool sorted  = true,
                                          int  capacity = 128){
        this.h = h; this.sorted = sorted; this.capacity = capacity;
        states.Add(Assert(x, "Initial state"));
        list.Add(new Node<T>(Solver<T>.INIT, x));
    }

    public static implicit operator bool(NodeSet<T> self)
    => self.count > 0 && self.count <= self.capacity;

    internal int count => list.Count;

    public void Insert(Node<T> n){
        if(states.Contains(n.state)) return;
        if(sorted){
            n.value = n.cost + (h != null ? h(n.state) : 0);
            for(int i = list.Count-1; i >= 0; i--){
                if(n.value < list[i].value){
                    list.Insert(i + 1, n);
                    states.Add(n.state); return;
                }
            }
        } list.Insert(0, n);
    }

    public Node<T> Pop(){
        int i = list.Count-1; var n = list[i]; list.RemoveAt(i);
        return n;
    }

}}
