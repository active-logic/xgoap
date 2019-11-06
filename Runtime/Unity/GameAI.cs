#if UNITY_2018_1_OR_NEWER

using UnityEngine;

namespace Activ.GOAP{
public abstract partial class GameAI<T> : MonoBehaviour{

    void Cooldown(){
        if(cooldown <= 0) return;
        enabled = false;
        Invoke("Wake", cooldown);
    }
    
    void Wake() => enabled = true;

}}

#endif  // UNITY_2018_1_OR_NEWER
