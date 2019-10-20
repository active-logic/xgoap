using NUnit.Framework;

public class TestBase{

    protected void o (bool arg)
    => Assert.That(arg);

    protected void o (object x, object y)
    => Assert.That(x, Is.EqualTo(y));

    protected void print(string msg){
        #if UNITY_EDITOR
        UnityEngine.Debug.Log(msg);
        #else
        System.Console.WriteLine(msg);
        #endif
    }

}
