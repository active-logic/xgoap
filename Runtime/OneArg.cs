public class OneArg{

    string name;
    object arg;

    public OneArg(string name, object arg){
        this.name = name;
        this.arg  = arg;
        //nityEngine.Debug.Log("Created one arg: " + this.ToString());
    }

    override public string ToString()=> $"{name}({arg})";

}
