using NUnit.Framework;
using UnityEngine;

public class Z_WoodChopper : TestBase{

    [Test] public void Test(){
        var chopper = new WoodChopper();
        var plan = new Solver<WoodChopper>();
        var sel = plan.Eval(chopper, x => x.hasFirewood);
        o(sel, "GetAxe");
    }

}
