using NUnit.Framework;
using S = Activ.GOAP.PlanningState;
#if UNITY_2018_1_OR_NEWER
using UnityEngine;
using UnityEngine.TestTools;
#endif

namespace Activ.GOAP{
public class HandlersTest : TestBase{

    Handlers x;

    [SetUp] public void Setup() => x = new Handlers();

    [Test] public void NoActionCases(
        [Values(S.Done, S.Running, S.MaxIterExceeded,
                                   S.CapacityExceeded)] S s,
        [Values(true, false)] bool warnOnFail,
        [Values(true, false)] bool warnOnOverflow)
    {
        x.warnOnFail = warnOnFail;
        x.warnOnOverflow = warnOnOverflow;
        #if UNITY_2018_1_OR_NEWER
        switch(s){
            case S.MaxIterExceeded:
                LogAssert.Expect(LogType.Error, ": MaxIterExceeded");
                break;
            case S.CapacityExceeded:
                LogAssert.Expect(LogType.Error, ": CapacityExceeded");
                break;
        }
        #endif
        o( x.OnResult(s), Handlers.NoAction );
    }

    [Test] public void RestartCases(
        [Values(S.Failed)] S s,
        [Values(true, false)] bool warnOnFail,
        [Values(true, false)] bool warnOnOverflow)
    {
        x.warnOnFail = warnOnFail;
        x.warnOnOverflow = warnOnOverflow;
        o( x.OnResult(s), Handlers.Restart );
    }

    [Test] public void StopOnFailPolicy(
        [Values(true, false)] bool warnOnFail){
        x.warnOnFail = warnOnFail;
        x.OnFail     = Handlers.Policy.Stop;
        o( x.OnResult(S.Failed), Handlers.NoAction );
    }

}}
