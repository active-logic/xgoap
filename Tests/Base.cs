using NUnit.Framework;

public class TestBase{

    protected void o (bool arg)
    => Assert.That(arg);

    protected void o (object x, object y)
    => Assert.That(x, Is.EqualTo(y));

    #if UNITY_EDITOR

    protected static void print(object msg)
    => UnityEngine.Debug.Log(msg);

    #endif

}
