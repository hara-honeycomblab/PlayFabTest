using System;
using UniRx;

public class PlayFabPresenter
{
    public Subject<Unit> _customIdLogin = new Subject<Unit>();

    public void CustomIdLogin()
    {
        _customIdLogin.OnNext(Unit.Default);
    }

    public IObservable<Unit> CustomIdLoginObservable()
    {
        return _customIdLogin.AsObservable();
    }
}
