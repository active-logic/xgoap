using NUnit.Framework;
using UnityEngine;

public class Z_WoodChopper : TestBase{

    [Test] public void Test(){
        var chopper = new WoodChopper();
        var solver = new Solver<WoodChopper>();
        var action = (string)solver.Next(chopper, x => x.hasFirewood);
        o(action, "GetAxe");
    }

}
