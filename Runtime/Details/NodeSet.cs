using System;
using UnityEngine;
using System.Collections.Generic;
using Ex = System.Exception;

namespace Activ.GOAP{
public class NodeSet<T> : Base where T : Agent{

    internal bool sorted;
    int capacity;
    Func<T, float> h;
    HashSet<T>         states = new HashSet<T>();
    SortedSet<Node<T>> list   = new SortedSet<Node<T>>();

    public NodeSet(T x, Func<T, float> h, bool sorted  = true,
                                          int  capacity = 128){
        this.h = h; this.sorted = sorted; this.capacity = capacity;
        states.Add(Assert(x, "Initial state"));
        list.Add(new Node<T>(Solver<T>.INIT, x));
    }

    public bool capacityExceeded => count > capacity;

    public static implicit operator bool(NodeSet<T> self)
    => self.count > 0 && self.count <= self.capacity;

    internal int count => list.Count;

    public void Insert(Node<T> n){
        if(!states.Add(n.state)) return;
        n.value = n.cost + (h != null ? h(n.state) : 0);
        list.Add(n);
    }

    public Node<T> Pop(){
        Node<T> e = list.Min;
        bool removed = list.Remove(e);
        // If Node does not implement CompareTo the way SortedSet is
        // expecting, this may happen.
        // - If CompareTo does not return 0 when an object is
        // compared to itself, then the object won't ever be found.
        // - If a.CompareTo(b) and b.CompareTo(a) are not consistent
        // errors also occur
        if(!removed){ throw new Ex("Not removed: " + e.ToString()); }
        return e;
    }

}}
