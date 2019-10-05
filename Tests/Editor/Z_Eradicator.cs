using NUnit.Framework;
using UnityEngine;

public class Z_Eradicator : TestBase{

    [Test] public void Test(){
        var x = new Eradicator();
        var p = new Solver<Eradicator>();
        var s = p.Eval(x, z => z.rats == 0);
        o( s, "Eradicate");
    }

    [Test] public void TestLimit(){
        var x = new Eradicator();
        var p = new Solver<Eradicator>();
        p.maxIter = 10;
        var s = p.Eval(x, z => z.rats == -1);
        o( s, State.NotFound);
    }

}
