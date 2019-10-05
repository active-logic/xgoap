public readonly struct OneArg{

    public readonly string name;
    public readonly object arg;

    public OneArg(string name, object arg){
        this.name = name;
        this.arg  = arg;
    }

    override public string ToString()=> $"{name}({arg})";

}
