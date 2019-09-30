using NUnit.Framework;
using UnityEngine;

public class TestWoodChopper : TestBase{

    [Test] public void Test(){
        var chopper = new WoodChopper();
        var plan = new ASPlanner();
        var sel = plan.Eval(chopper, x => x.hasFirewood);
        Debug.Log(sel);
        o(sel, "ChopLog");
    }

}
