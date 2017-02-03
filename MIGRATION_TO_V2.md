# Migration Guide to v2

The change to the v2.0 version introduced some breaking changes.
See [Migration guide to v1.0](MIGRATION_TO_V1.0.md)

## Configuration

Maestrano 2.0 only accept configuration retrieved from the Developer platform. See [Readme](README.md) for more details.

## MnoHelper

Direct static method from MnoHelper have been removed.
Use `MnoHelper.With(marketplaceName)` instead to retrieve a `Preset` instance, from which you can call all of the previous methods.

In order to be consistent accross the library, all Maestrano Object and Services are now expecting to be called with a preset given as an argument.

## Accounts

Direct static method from `User`, `Group`, `Bill` and `RecurringBill` have been removed.

All the call should be done following this model:

```csharp
Bill.All();
```
should be replaced by

```csharp
var preset = MnoHelper.With(marketplaceName);
Bill.With(preset).All();
// or 
preset.Bill.All()
```

## Connec.Client

```csharp
 var client = Maestrano.Connec.Client.New("cld-f7f5g4", marketplace);
```
is replaced by:
```csharp
var preset = MnoHelper.With(marketplace);
var client = Maestrano.Connec.Client.New(preset, "cld-f7f5g4");
// or
var preset = MnoHelper.With(marketplace);
preset.ConnecClient("cld-f7f5g4");
```

## Maestrano.Sso

All Sso `New` instances methods have been replace by the more natural constructor.

### Maestrano.Sso.Group

```csharp
Group.With(marketplace).New(response);
```
is replaced by:
```csharp
new Group(response);
```

### Maestrano.Sso.User

```csharp
User.With(marketplace).New(response);
```
is replaced by:
```csharp
new User(response);
```

The two methods `ToUid()` and `ToEmail()` have been removed and should be replaced by Uid and Email respectively.


### Maestrano.Sso.Session

Maestrano.Sso.Session constructor is now taking a instance of preset.

For example

```csharp
var session = new Maestrano.Sso.Session(marketplace, httpSession);

```
is replaced by:
```csharp
var preset = MnoHelper.With(marketplace);
var session = new Maestrano.Sso.Session(preset, httpSession);
```

## Maestrano.Saml

### Maestrano.Saml.Request

```csharp
Request.With(marketplace).New(parameters)

```
is replaced by:
```csharp
var preset = MnoHelper.With(marketplace);
var session = new Request(preset, httpSession);
```

### Maestrano.Saml.Response

a Maestrano.Saml.Response can be now created using the following 2 static methods:

* `Reponse.LoadFromXML(Preset preset, String response)` reponse is string representing an XML document
* `Reponse.LoadFromBase64XML(Preset preset, String response)`  response is string reprenting a base 64 encoded XML document