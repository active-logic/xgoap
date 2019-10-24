public class Pool<T> where T : new(){

    T[] elements;
    int index = 0;

    public Pool(int capacity){
        elements = new T[capacity];
        for(int i = 0; i < capacity; i++) elements[i] = new T();
    }

    public int count => index;

    public T next{get{
        if(index > elements.Length-1){
            UnityEngine.Debug.LogError(
                            $"Max size exceeded: "+elements.Length);
        }
        return elements[index++];
    }}

    public void Clear()   => index = 0;
    public void Reclaim(){ if(index > 0 ) index --; }


}
