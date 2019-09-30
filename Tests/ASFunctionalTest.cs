using NUnit.Framework;
using UnityEngine;

public class ASFunctionalTest{

    [Test] public void Test(){
        var x = new Eradicator();
        var p = new ASPlanner();
        var s = p.Eval(x, z => z.rats == 0);
        Debug.Log(s);
    }

    [Test] public void TestLimit(){
        var x = new Eradicator();
        var p = new ASPlanner();
        var s = p.Eval(x, z => z.rats == -1);
        Debug.Log(s);
    }

}
