public class DataRowPoolingController : BaseObjectPoolingController<DataRow>
{
    public void Init()
    {
        Init(defaultCapacity: 5, maxSize: 5);
    }
}
