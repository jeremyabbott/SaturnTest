# SaturnTest

It looks like there might be a couple of bugs.

1. If you don't include an action of the type `ctx -> 'a -> System.Threading.Tasks.Task<HttpContext option>`, the controller can't build because ControllerBuilder can't find a type for `<'Key>`. If you specify `controller<type> {...}` the issues goes way. However, without an explicit type, something like this:

```fsharp
  let resource = controller {
    // show showAction
    create createAction
  }
```

results in this:

```text
Unhandled Exception: System.TypeInitializationException: The type initializer for '<StartupCode$SaturnTest>.$Server' threw an exception. ---> System.TypeInitializationException: The type initializer for '<StartupCode$SaturnTest>.$Router' threw an exception. ---> System.TypeInitializationException: The type initializer for '<StartupCode$SaturnTest>.$BooksController' threw an exception. ---> System.Exception: Couldn't create router for controller. Key type not supported.
   at Microsoft.FSharp.Core.PrintfModule.PrintFormatToStringThenFail@1645.Invoke(String message)
   at Saturn.Controller.ControllerBuilder`1.Run(ControllerState`1 state) in /home/chris/Programming/Saturn/Saturn/src/Saturn/Controller.fs:line 60
   at <StartupCode$SaturnTest>.$BooksController..cctor()
   --- End of inner exception stack trace ---
   at Books.Controller.get_resource()
   at <StartupCode$SaturnTest>.$Router..cctor()
   --- End of inner exception stack trace ---
   at Router.get_router()
   at <StartupCode$SaturnTest>.$Server..cctor()
   --- End of inner exception stack trace ---
   at Server.main(String[] _arg1)
```

1. It looks like AddGiraffe isn't being called in the ApplicationBuilder. I think this is because the ApplicationState record doesn't have any services, [so when `serviceConfigs` is called, it doesn't have anything to iterate over](https://github.com/SaturnFramework/Saturn/blob/4b6ad1b1ef609f0b8ab203421d5a1136fcf48403/src/Saturn/Application.fs#L61). As a result, trying to serialize/deserialize json fails.

If you try to get an IJsonSerializer from the IServiceProvider instance, it returns null.

`let x = app.Services.GetService(typedefof<Giraffe.Serialization.Json.IJsonSerializer>)`

In the above snippet x is null.