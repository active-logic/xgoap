using System;

namespace Activ.GOAP{
public class Node<T> : Base{

    public readonly Node<T> prev;
    public readonly object  action;
    public readonly T       state;
    public float            value;
    public float cost{ get; private set; }

    public Node(object action, T result, Node<T> prev = null,
                                         float   cost = 0f){
        this.action = Assert(action, "action");
        this.state  = Assert(result, "result");
        this.prev   = prev;
        this.cost   = cost + (prev?.cost ?? 0f);
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

    public string PathToString(){
        var path = Path();
        var s = "";
        foreach(var k in path) s += k + '\n';
        return s;
    }

    override public string ToString()
    => $"[{value:0.0} :: {action} => {state}]"
                                .Replace("System.Object", "object");

}}
