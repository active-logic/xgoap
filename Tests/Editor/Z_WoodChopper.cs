using NUnit.Framework;

namespace Activ.GOAP{
public class Z_WoodChopper : TestBase{

    [Test] public void Test(){
        var chopper = new WoodChopper();
        var solver  = new Solver<WoodChopper>();
        var action  = solver.Next(
                      chopper, goal: (x => x.hasFirewood, h: null));
        o((string)action, "GetAxe");
    }

}}
