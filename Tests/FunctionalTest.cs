using NUnit.Framework;
using UnityEngine;

public class FunctionalTest{

    [Test] public void Test(){
        var x = new Eradicator();
        var p = new Planner();
        var s = p.Eval(x, z => z.rats == 0);
        Debug.Log(s);
    }

}
