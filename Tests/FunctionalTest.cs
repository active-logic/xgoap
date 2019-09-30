using NUnit.Framework;
using UnityEngine;

public class FunctionalTest{

    [Test] public void Test(){
        var x = new Eradicator();
        var p = new BrPlanner();
        var s = p.Eval(x, z => z.rats == 0);
        Debug.Log(s);
    }

    [Test] public void TestLimit(){
        var x = new Eradicator();
        var p = new BrPlanner();
        var s = p.Eval(x, z => z.rats == -1);
        Debug.Log(s);
    }

}
