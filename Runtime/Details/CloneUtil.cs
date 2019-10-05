using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Activ.GOAP{
public static class CloneUtil{

    public static T DeepClone<T>(T obj){
        using (var ms = new MemoryStream()){
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;
            return (T)formatter.Deserialize(ms);
        }
    }

}}
