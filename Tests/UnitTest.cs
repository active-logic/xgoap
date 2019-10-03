using NUnit.Framework;

public class UnitTest<T> : TestBase where T : new() {

    protected T x;

    [SetUp] public void Setup() => x = new T();

}
