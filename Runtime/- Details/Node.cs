public class Node<T> : Base{

    public readonly Node<T> prev;
    public readonly string  action;
    public readonly T       state;
    public float            value;

    public Node(string action, T result, Node<T> prev = null){
        this.action = Assert(action, "action");
        this.state  = Assert(result, "result");
        this.prev   = prev;
    }

    // Regress to the next action; root (init state) does not count.
    public string Head()
    => prev?.prev == null ? action : prev.Head();

    override public string ToString()
    => $"[{value} :: {action} => {state}]";

}
