[System.AttributeUsage(System.AttributeTargets.Parameter)]
public class FromUriJsonAttribute : System.Web.Http.ModelBinding.ModelBinderAttribute
{
    private System.Type _modelType;

    public FromUriJsonAttribute(System.Type modelType)
    {
        _modelType = modelType;
    }

    public override System.Collections.Generic.IEnumerable<ValueProviderFactory> GetValueProviderFactories(HttpConfiguration configuration)
    {
        var factoryType = typeof(FromUriJsonValueProviderFactory<>);

        var genericFactoryType = factoryType.MakeGenericType(_modelType);

        var factory = (ValueProviderFactory)System.Activator.CreateInstance(genericFactoryType);
        return new[] { factory };
    }
}
