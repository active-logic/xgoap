using System;

namespace Activ.GOAP{
public class Node<T> : Base{

    public readonly Node<T> prev;
    public readonly object  action;
    public readonly T       state;
    public float            value;

    public Node(object action, T result, Node<T> prev = null){
        this.action = Assert(action, "action");
        this.state  = Assert(result, "result");
        this.prev   = prev;
    }

    public static implicit operator string(Node<T> x)
    => (string)(x.Head());

    public static implicit operator Delegate(Node<T> x)
    => (Delegate)(x.Head());

    // Regress to the next action; root (init state) does not count.
    public object Head()
    => prev?.prev == null ? action : prev.Head();

    public Node<T>[] Path(int n = 1){
        Node<T>[] @out;
        if(prev == null){
            @out = new Node<T>[n];
        }else{
            @out = prev.Path(n + 1);
        }
        @out[@out.Length - n] = this;
        return @out;
    }

    override public string ToString()
    => $"[{value} :: {action} => {state}]"
                                .Replace("System.Object", "object");

}}
