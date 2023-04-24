using System;

namespace VirtuosityAbstractClassLDFTNIssue;

public interface IChannelBase
{
    AsyncResultBase BeginCreateSession(AsyncCallback asyncCallback);
}

public class RegistrationChannel<TChannel>
    where TChannel : IChannelBase
{
    protected class WcfChannelAsyncResult : AsyncResultBase
    {
        public virtual void OnOperationCompleted(IAsyncResult ar)
        {
        }
    }

    public AsyncResultBase DoSomething(AsyncCallback callback)
    {
        var arb = new WcfChannelAsyncResult();

        arb.InnerResult = arb.Channel.BeginCreateSession(arb.OnOperationCompleted);

        return arb;
    }
}

public abstract class AsyncResultBase
{
    public AsyncResultBase InnerResult { get; set; }

    public IChannelBase Channel { get; set; }
}