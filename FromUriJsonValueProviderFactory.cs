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
