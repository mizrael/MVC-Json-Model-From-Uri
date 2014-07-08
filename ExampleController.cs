public class ExampleController : ApiController
{
  public string Get([FromUriJson(typeof(MyModel))]MyModel model){
    return (null != model) ? model.ToString() : "[empty model]";  
  }
}

public class MyModel
{
  public string Foo {get;set;}
}
