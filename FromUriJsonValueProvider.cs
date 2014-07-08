public class FromUriJsonValueProvider<TModel> : System.Web.Http.ValueProviders.IValueProvider where TModel : class
{
    private System.Web.Http.Controllers.HttpActionContext _actionContext;

    public FromUriJsonValueProvider(System.Web.Http.Controllers.HttpActionContext actionContext)
    {
        if (null == actionContext)
            throw new ArgumentNullException("actionContext");

        _actionContext = actionContext;
    }

    public bool ContainsPrefix(string prefix)
    {
        var value = ExtractValue(prefix);
        return (!string.IsNullOrWhiteSpace(value));
    }

    public ValueProviderResult GetValue(string key)
    {
        var value = ExtractValue(key);
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var obj = JsonConvert.DeserializeObject<TModel>(value, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        return new ValueProviderResult(obj, value, CultureInfo.InvariantCulture);
    }

    private string ExtractValue(string key)
    {
        if (string.IsNullOrWhiteSpace(_actionContext.Request.RequestUri.Query))
            return null;
        var query = System.Web.HttpUtility.ParseQueryString(_actionContext.Request.RequestUri.Query);
        var value = query[key];
        return value;
    }
}
