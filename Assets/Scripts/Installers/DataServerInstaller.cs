using Zenject;

public class DataServerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IDataServer>().To<DataServerMock>().AsSingle();
        Container.Bind<IDataServerWrapper>().To<DataServerWrapper>().AsSingle();
    }
}