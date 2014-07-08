using System;
using System.Globalization;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using Newtonsoft.Json;

namespace ValueProviders
{
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

    public sealed class FromUriJsonValueProviderFactory<TModel> : System.Web.Http.ValueProviders.ValueProviderFactory where TModel : class
    {
        public override IValueProvider GetValueProvider(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentNullException("actionContext");

            var provider = new FromUriJsonValueProvider<TModel>(actionContext);
            return provider;
        }
    }

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
}
