using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AsyncToken : IDisposable
{
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public CancellationToken GetToken()
    {
        return _cancellationTokenSource.Token;
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
    }
}
