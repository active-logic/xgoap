using NUnit.Framework;

namespace Activ.GOAP{
public class CostTest : TestBase{

    Cost r;

    [Test] public void FromFloat([Values(-1, 0, 1)] float cost){
        r = cost;
        o(r.cost, cost);
        o(r.done, true);
    }

    [Test] public void FromTrue(){
        r = true;
        o(r.cost, 1);
        o(r.done, true);
    }

    [Test] public void FromFalse(){
        r = true;
        o(r.cost, 1);
        o(r.done, true);
    }

}}
