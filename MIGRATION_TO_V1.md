# Migration Guide to v1.0

The change to the v1.0 version introduced some breaking changes.

## MnoHelper

Direct calls to MnoHelper are deprecated. Use `MnoHelper.With(marketplaceName)` instead.

## Connec.Client

### Without preset/marketplace
```csharp
var client = new Maestrano.Connec.Client("cld-f7f5g4");
```
should be replaced by

```csharp
var client = Maestrano.Connec.Client.New("cld-f7f5g4");
```

### With preset/marketplace

```csharp
 var client = Maestrano.Connec.Client.With("somePreset").New("cld-f7f5g4");
```
should be replaced by
```csharp
 var client = Maestrano.Connec.Client.New("cld-f7f5g4", "somePreset");
```

## Maestrano.Sso.Session

All the `Session.With` and `Session.New` methods have been removed and should be replaced by the appropriate constructor.

For example

```csharp
var session = Session.With(marketplace).New(httpSessionObj, user);
```

```csharp
var session = new Maestrano.Sso.Session(marketplace, httpSession);
```

