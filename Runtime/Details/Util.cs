#if UNITY_2018_1_OR_NEWER
using UnityEngine;
#endif

namespace Activ.GOAP{
public static class Util{

    #if UNITY_2018_1_OR_NEWER
    public static string ObjectName(Component c)
    => c.gameObject.name;

    #else
    public static string ObjectName(object c)
    => c.GetType().Name;

    #endif

}}
