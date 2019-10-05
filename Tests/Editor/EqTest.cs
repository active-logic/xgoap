using NUnit.Framework;
using System.Runtime.Serialization;

public class EqTest : TestBase{

    [Test] public void TikTokEqTest(){
        var a = new TikTok();
        var b = new TikTok();
        o( a.Equals(b) );
    }

}
