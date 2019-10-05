using NUnit.Framework;
using System.Runtime.Serialization;

namespace Activ.GOAP{
public class CloneTest : TestBase{

    [Test] public void DeepClone()
    => CloneUtil.DeepClone(new Eradicator());

    [Test] public void DeepCloneThrows()
    => Assert.Throws<SerializationException>(
                             () => CloneUtil.DeepClone(new NotSerializable()));

    class NotSerializable{}

}}
