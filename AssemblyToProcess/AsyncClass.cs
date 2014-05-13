using System;
using System.Threading.Tasks;

public class AsyncClass
{
    public async Task AsyncMethod()
    {
        await AsyncMethod2(InstanceMethod);
    }

// ReSharper disable once UnusedParameter.Local
    Task AsyncMethod2(Action func)
    {
        throw new NotImplementedException();
    }

    public void InstanceMethod()
    {
        throw new NotImplementedException();
    }
}