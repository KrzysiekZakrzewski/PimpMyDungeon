namespace Inject
{
    public interface IInjectParam
    {

    }

    public interface IInjectParam<in TParam1> : IInjectParam
    {
        void Inject(TParam1 param);
    }

    public interface IInjectParam<in TParam1, in TParam2> : IInjectParam
    {
        void Inject(TParam1 param, TParam1 param2);
    }
}