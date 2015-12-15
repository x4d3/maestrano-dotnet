<p align="center">
<img src="https://raw.github.com/maestrano/maestrano-dotnet/master/maestrano.png" alt="Maestrano Logo">
<br/>
<br/>
</p>

Maestrano Cloud Integration is currently in closed beta. Want to know more? Send us an email to <contact@maestrano.com>.

<img src="https://ci.appveyor.com/api/projects/status/github/maestrano/maestrano-dotnet?branch=master&amp;svg=true" alt="maestrano-dotnet status">

- - -

1.  [Getting Setup](#getting-setup)
2. [Getting Started](#getting-started)
  * [Installation](#installation)
  * [Configuration](#configuration)
  * [Metadata Endpoint](#metadata-endpoint)
3. [Single Sign-On Setup](#single-sign-on-setup)
  * [User Setup](#user-setup)
  * [Group Setup](#group-setup)
  * [Controller Setup](#controller-setup)
  * [Other Controllers](#other-controllers)
  * [Redirecting on logout](#redirecting-on-logout)
  * [Redirecting on error](#redirecting-on-error)
4. [Account Webhooks](#account-webhooks)
  * [Groups Controller](#groups-controller-service-cancellation)
  * [Group Users Controller](#group-users-controller-business-member-removal)
5. [API](#api)
  * [Payment API](#payment-api)
    * [Bill](#bill)
    * [Recurring Bill](#recurring-bill)
  * [Membership API](#membership-api)
    * [User](#user)
    * [Group](#group)
6. [Connec!™ Data Sharing](#connec-data-sharing)
  * [Making Requests](#making-requests)
  * [Webhook Notifications](#webhook-notifications)

- - -

## Getting Setup
Before integrating with us you will need an App ID and API Key. Maestrano Cloud Integration being still in closed beta you will need to contact us beforehand to gain production access.

For testing purpose we provide an API Sandbox where you can freely obtain an App ID and API Key. The sandbox is great to test single sign-on and API integration (e.g: billing API).

To get started just go to: http://api-sandbox.maestrano.io

A **.NET demo application** is also available here: https://github.com/maestrano/demoapp-dotnet

## Getting Started

### Installation

To install Maestrano, run the following command in the Package Manager Console
```console
PM> Install-Package Maestrano
```


### Configuration
The best way to configure the Maestrano api is to add a section in your config file (Web.config) as
shown below.

You can add configuration presets by putting additional "sectionGroup" blocks in your Web.config. These additional presets can then be specified when doing particular action, such as initializing a Connec!™ client or triggering a SSO handshake. These presets are particularly useful if you are dealing with multiple Maestrano-style marketplaces (multi-enterprise integration).

If this is the first time you integrate with Maestrano, we recommend adopting a multi-tenant approach. All code samples in this documentation provide examples on how to handle multi-tenancy by scoping method calls to a specific configuration preset.

More information about multi-tenant integration can be found on [Our Multi-Tenant Integration Guide](https://maestrano.atlassian.net/wiki/display/CONNECAPIV2/Multi-Tenant+Integration)

Your Web.config may look like this:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>

  ...

    <configSections>

      ...

      <sectionGroup name="maestrano">
        <section name="app" type="Maestrano.Configuration.App, Maestrano" />
        <section name="sso" type="Maestrano.Configuration.Sso, Maestrano" />
        <section name="api" type="Maestrano.Configuration.Api, Maestrano" />
        <sectionGroup name="webhook">
          <section name="account" type="Maestrano.Configuration.WebhookAccount, Maestrano" />
          <section name="connec" type="Maestrano.Configuration.WebhookConnec, Maestrano" />
          <section name="connecSubscriptions" type="Maestrano.Configuration.WebhookConnecSubscriptions, Maestrano" />
        </sectionGroup>
      </sectionGroup>

      ...

    </configSections>

  ...

  <maestrano>
    <!--
      ===> App Configuration

      => environment
      The environment to connect to.
      Accepted values are:
      - production: actual Maestrano production environment
      - production-sandbox: production-like environment to use in your UAT/staging environment
      - development: direct requests to our development sandbox (http://api-sandbox.maestrano.io)
      - test: same as development (deprecated)

      => host
      This is your application host (e.g: my-app.com) which is ultimately
      used to redirect users to the right SAML url during SSO handshake.
    -->
    <app
      environment="development"
      host="http://localhost"
      />

    <!--
      ===> Api Configuration

      => id and key
      Your application App ID and API key which you can retrieve on http://maestrano.com
      via your cloud partner dashboard.
      For testing you can retrieve/generate an api.id and api.key from the API Sandbox directly
      on http://api-sandbox.maestrano.io
    -->
    <api
      id="prod_or_sandbox_app_id"
      key="prod_or_sandbox_api_key"
      />

    <!--
      ===> SSO Configuration

      => enabled
      Enable/Disable single sign-on. When troubleshooting authentication issues
      you might want to disable SSO temporarily

      => sloEnabled
      Enable/Disable single logout. When troubleshooting authentication issues
      you might want to disable SLO temporarily.
      If set to false then Maestrano.Sso.Session#IsValid - which should be
      used in a controller action filter to check user session - always return true

      => idm
      By default we consider that the domain managing user identification
      is the same as your application host (see above config.app.host parameter)
      If you have a dedicated domain managing user identification and therefore
      responsible for the single sign-on handshake (e.g: https://idp.my-app.com)
      then you can specify it below
      
      
      => idp
      This is the URL of the identity provider to use when triggering a SSO handshake. With a multi-tenant integration, each tenant would have its own URL. Defaults to https://maestrano.com

      => initPath
      This is your application path to the SAML endpoint that allows users to
      initialize SSO authentication. Upon reaching this endpoint users your
      application will automatically create a SAML request and redirect the user
      to Maestrano. Maestrano will then authenticate and authorize the user. Upon
      authorization the user gets redirected to your application consumer endpoint
      (see below) for initial setup and/or login.

      => consumePath
      This is your application path to the SAML endpoint that allows users to
      finalize SSO authentication. During the 'consume' action your application
      sets users (and associated group) up and/or log them in.

      => creationMode
      !IMPORTANT
      On Maestrano users can take several "instances" of your service. You can consider
      each "instance" as 1) a billing entity and 2) a collaboration group (this is
      equivalent to a 'customer account' in a commercial world). When users login to
      your application via single sign-on they actually login via a specific group which
      is then supposed to determine which data they have access to inside your application.

      E.g: John and Jack are part of group 1. They should see the same data when they login to
      your application (employee info, analytics, sales etc..). John is also part of group 2
      but not Jack. Therefore only John should be able to see the data belonging to group 2.

      In most application this is done via collaboration/sharing/permission groups which is
      why a group is required to be created when a new user logs in via a new group (and
      also for billing purpose - you charge a group, not a user directly).

      - mode: 'real'
      In an ideal world a user should be able to belong to several groups in your application.
      In this case you would set the 'sso.creation_mode' to 'real' which means that the uid
      and email we pass to you are the actual user email and maestrano universal id.

      - mode: 'virtual'
      Now let's say that due to technical constraints your application cannot authorize a user
      to belong to several groups. Well next time John logs in via a different group there will
      be a problem: the user already exists (based on uid or email) and cannot be assigned
      to a second group. To fix this you can set the 'sso.creation_mode' to 'virtual'. In this
      mode users get assigned a truly unique uid and email across groups. So next time John logs
      in a whole new user account can be created for him without any validation problem. In this
      mode the email we assign to him looks like "usr-sdf54.cld-45aa2@mail.maestrano.com". But don't
      worry we take care of forwarding any email you would send to this address
    -->
    <!--
    <sso
      enabled="true"
      idm="https://idp.myapp.com"
      idp="https://maestrano.com"
      initPath="/maestrano/auth/saml/init.aspx"
      consumePath="/maestrano/auth/saml/consume"
      creationMode="virtual"
      />
     -->

    <!--
      ===> Account Webhooks
      Here you can configure various notification endpoints related to service cancellation  
      (account/user deletion) as well as Connec!™ entities updates.

    -->
    <webhook>
      <!--
      Single sign on has been setup into your app and Maestrano users are now able
      to use your service. Great! Wait what happens when a business (group) decides to
      stop using your service? Also what happens when a user gets removed from a business?
      Well the endpoints below are for Maestrano to be able to notify you of such
      events.

      Even if the routes look restful we issue only issue DELETE requests for the moment
      to notify you of any service cancellation (group deletion) or any user being
      removed from a group.
      <account
        groupsPath="/maestrano/account/groups/:id"
        groupUsersPath="/maestrano/account/groups/:group_id/users/:id"
        />
       -->

      <!--
      This is the path were Connec!™ should post notifications
      <connec
        notificationsPath="/maestrano/connec/notifications"
        />
       -->

      <!--
      This is the list of Connec!™ entities for which you want to
      receive updates
      <connecSubscriptions
        accounts=false
        company=false
        invoices=false
        salesOrders=false
        purchaseOrders=false
        quotes=false
        payments=false
        journals=false
        items=false
        organizations=false
        people=false
        projects=false
        taxCodes=false
        taxRates=false
        events=false
        venues=false
        eventOrders=false
        workLocations=false
        payItems=false
        employees=false
        paySchedules=false
        timeSheets=false
        timeActivities=false
        payRuns=false
        payStubs=false
        />
       -->

    </webhook>
  </maestrano>

  ...

</configuration>


Your Web.config in a multi-tenant context may look like this:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>

  ...

    <configSections>

      ...

      <sectionGroup name="maestrano">
        <section name="app" type="Maestrano.Configuration.App, Maestrano" />
        <section name="sso" type="Maestrano.Configuration.Sso, Maestrano" />
        <section name="api" type="Maestrano.Configuration.Api, Maestrano" />
        <sectionGroup name="webhook">
          <section name="account" type="Maestrano.Configuration.WebhookAccount, Maestrano" />
          <section name="connec" type="Maestrano.Configuration.WebhookConnec, Maestrano" />
          <section name="connecSubscriptions" type="Maestrano.Configuration.WebhookConnecSubscriptions, Maestrano" />
        </sectionGroup>
      </sectionGroup>

      <sectionGroup name="anotherTenant">
        <section name="app" type="Maestrano.Configuration.App, Maestrano" />
        <section name="sso" type="Maestrano.Configuration.Sso, Maestrano" />
        <section name="api" type="Maestrano.Configuration.Api, Maestrano" />
        <sectionGroup name="webhook">
          <section name="account" type="Maestrano.Configuration.WebhookAccount, Maestrano" />
          <section name="connec" type="Maestrano.Configuration.WebhookConnec, Maestrano" />
          <section name="connecSubscriptions" type="Maestrano.Configuration.WebhookConnecSubscriptions, Maestrano" />
        </sectionGroup>
      </sectionGroup>

      ...

    </configSections>

  ...

  <maestrano>
    ... Configuration for "maestrano" ...
  </maestrano>

  <anotherTenant>
    ... Configuration for "anotherTenant" ...
  </anotherTenant>

  ...

</configuration>

```

### Metadata Endpoint
Your configuration initializer is now all setup and shiny. Great! But need to know about it. Of course
we could propose a long and boring form on maestrano.com for you to fill all these details (especially the webhooks) but we thought it would be more convenient to fetch that automatically.

For that we expect you to create a metadata endpoint that we can fetch regularly (or when you press 'refresh metadata' in your maestrano cloud partner dashboard). By default we assume that it will be located at
YOUR_WEBSITE/maestrano/metadata(.json)

Of course if you prefer a different url you can always change that endpoint in your maestrano cloud partner dashboard.

What would the controller action look like? First let's talk about authentication. You don't want that endpoint to be visible to anyone. Maestrano always uses http basic authentication to contact your service remotely. The login/password used for this authentication are your actual api.id and api.key.

So here is an example of page to adapt depending on the framework you're using:

```csharp
using Maestrano;

...

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/json";

        // Authentication
        var request = HttpContext.Current.Request;

        // Authenticate
        var authenticated = MnoHelper.With("somePreset").Authenticate(request)
        // Using a specific Configuration Preset
        // var authenticated = MnoHelper.With("somePreset").Authenticate(request);

        if (authenticated)
        {
          Response.Write(MnoHelper.ToMetadata().ToString());
          // Using a specific Configuration Preset
          // Response.Write(MnoHelper.With("somePreset").ToMetadata().ToString());
        }
        else
        {
          Response.Write("Failed");
        }
    }
}
```

## Single Sign-On Setup

> **Heads up!** Prefer to use OpenID rather than our SAML implementation? Just look at our [OpenID Guide](https://maestrano.atlassian.net/wiki/display/CONNECAPIV2/SSO+via+OpenID) to get started!

In order to get setup with single sign-on you will need a user model and a group model. It will also require you to write a controller for the init phase and consume phase of the single sign-on handshake.

You might wonder why we need a 'group' on top of a user. Well Maestrano works with businesses and as such expects your service to be able to manage groups of users. A group represents 1) a billing entity 2) a collaboration group. During the first single sign-on handshake both a user and a group should be created. Additional users logging in via the same group should then be added to this existing group (see controller setup below)

### User Setup
Let's assume that your user model is called 'User'. The best way to get started with SSO is to define a class method on this model called 'findOrCreateForMaestrano' accepting a Maestrano.Sso.User and aiming at either finding an existing maestrano user in your database or creating a new one. Your user model should also have a 'Provider' property and a 'Uid' property used to identify the source of the user - Maestrano, LinkedIn, AngelList etc..

### Group Setup
The group setup is similar to the user one. The mapping is a little easier though. Your model should also have the 'Provider' property and a 'Uid' properties. Also your group model could have a AddMember method and also a hasMember method (see controller below)

### Controller Setup
You will need two controller action init and consume. The init action will initiate the single sign-on request and redirect the user to Maestrano. The consume action will receive the single sign-on response, process it and match/create the user and the group.

The init action is all handled via Maestrano methods and should look like this:
```csharp
using Maestrano;

...

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var request = HttpContext.Current.Request;

        var ssoUrl = MnoHelper.Sso.BuildRequest(request.QueryString).RedirectUrl();
        // Using a specific Configuration Preset
        // var ssoUrl = MnoHelper.With("somePreset").Sso.BuildRequest(request.QueryString).RedirectUrl();

        Response.Redirect(ssoUrl);
    }
}
```

Based on your application requirements the consume action might look like this:
```csharp
using Maestrano;

...

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var request = HttpContext.Current.Request;

        // Get SAML response, build maestrano user and group objects
        var samlResp = MnoHelper.Sso.BuildResponse(request.Params["SAMLResponse"]);
        // Using a specific Configuration Preset
        // var ssoUrl = MnoHelper.With("somePreset").Sso.BuildResponse(request.Params["SAMLResponse"]);

        // Check response validity
        if (samlResp.IsValid()) {
          var mnoUser = new Maestrano.Sso.User(samlResp);
          var mnoGroup = new Maestrano.Sso.Group(samlResp);
          // Using a specific Configuration Preset
          // var mnoUser = Maestrano.Sso.User.With("somePreset").New(samlResp);
          // var mnoGroup = Maestrano.Sso.Group.With("somePreset").New(samlResp);


          // Build/Map local entities
          var localGroup = MyGroup.FindOrCreateForMaestrano(mnoGroup);
          var localUser = MyUser.FindOrCreateForMaestrano(mnoUser);

          // Add localUser to the localGroup if not already part
          // of it
          if (!localGroup.HasMember(localUser)){
            localGroup.AddMember(localUser);
          }

          // Set Maestrano session
          MnoHelper.Sso.SetSession(Session,mnoUser);
          // Using a specific Configuration Preset
          // MnoHelper.With("somePreset").Sso.SetSession(Session,mnoUser)

          Response.Redirect("/");
        } else {
          Response.Write("Invalid SAML Response");
        }
    }
}
```
Note that for the consume action you should disable CSRF authenticity if your framework is using it by default. If CSRF authenticity is enabled then your app will complain on the fact that it is receiving a form without CSRF token.

### Other Controllers
If you want your users to benefit from single logout then you should define the following filter in a module and include it in all your controllers except the one handling single sign-on authentication.

```csharp
var mnoSession = new Maestrano.Sso.Session(httpContext.Session);
// Using a specific Configuration Preset
// var mnoSession = Maestrano.Sso.Session.With("somePreset").New(httpContext.Session);

if (!mnoSession.IsValid()) {
  Response.Redirect(MnoHelper.Sso.InitUrl());
  // Using a specific Configuration Preset
  // Response.Redirect(MnoHelper.With("somePreset").Sso.InitUrl());
}
```

The above piece of code makes at most one request every 3 minutes (standard session duration) to the Maestrano website to check whether the user is still logged in Maestrano. Therefore it should not impact your application from a performance point of view.

If you start seing session check requests on every page load it means something is going wrong at the http session level. In this case feel free to send us an email and we'll have a look with you.

### Redirecting on logout
When Maestrano users sign out of your application you can redirect them to the Maestrano logout page. You can get the url of this page by calling:

```csharp
MnoHelper.Sso.LogoutUrl()
```

### Redirecting on error
If any error happens during the SSO handshake, you can redirect users to the following URL:

```csharp
MnoHelper.Sso.UnauthorizedUrl()
```

## Account Webhooks
Single sign on has been setup into your app and Maestrano users are now able to use your service. Great! Wait what happens when a business (group) decides to stop using your service? Also what happens when a user gets removed from a business? Well the controllers describes in this section are for Maestrano to be able to notify you of such events.

### Groups Controller (service cancellation)
Sad as it is a business might decide to stop using your service at some point. On Maestrano billing entities are represented by groups (used for collaboration & billing). So when a business decides to stop using your service we will issue a DELETE request to the webhook.account.groups_path endpoint (typically /maestrano/account/groups/:id).

Maestrano only uses this controller for service cancellation so there is no need to implement any other type of action - ie: GET, PUT/PATCH or POST. The use of other http verbs might come in the future to improve the communication between Maestrano and your service but as of now it is not required.

The controller example below reimplements the authenticate_maestrano! method seen in the [metadata section](#metadata) for completeness. Utimately you should move this method to a helper if you can.

The example below needs to be adapted depending on your application:
```csharp
public HttpResponseMessage DisableGroup(string groupId)
{
    // Authentication
    var request = HttpContext.Current.Request;
    var authenticated = MnoHelper.With("somePreset").Authenticate(request);
    // Using a specific Configuration Preset
    // var authenticated = MnoHelper.With("somePreset").Authenticate(request);


    if (authenticated) {
      var mnoGroup = MyGroupModel.findByMnoId(groupId);
      mnoGroup.disableAccess();
    }


    ...
}
```

### Group Users Controller (business member removal)
A business might decide at some point to revoke access to your services for one of its member. In such case we will issue a DELETE request to the webhook.account.group_users_path endpoint (typically /maestrano/account/groups/:group_id/users/:id).

Maestrano only uses this controller for user membership cancellation so there is no need to implement any other type of action - ie: GET, PUT/PATCH or POST. The use of other http verbs might come in the future to improve the communication between Maestrano and your service but as of now it is not required.

The controller example below reimplements the authenticate_maestrano! method seen in the [metadata section](#metadata) for completeness. Utimately you should move this method to a helper if you can.

The example below needs to be adapted depending on your application:

```csharp
public HttpResponseMessage DisableGroup(string groupId, string userId)
{
    // Authentication
    var request = HttpContext.Current.Request;
    var authenticated = MnoHelper.With("somePreset").Authenticate(request);
    // Using a specific Configuration Preset
    // var authenticated = MnoHelper.With("somePreset").Authenticate(request);

    if (authenticated) {
      var mnoGroup = MyGroupModel.findByMnoId(mnoId);
      mnoGroup.removeUserById(userId);
    }

    ...
}
```

## API
The maestrano package also provides bindings to its REST API allowing you to access, create, update or delete various entities under your account (e.g: billing).

### Payment API

#### Bill
A bill represents a single charge on a given group.

```csharp
Maestrano.Account.Bill
```

##### Attributes

<table>
<tr>
<th>Field</th>
<th>Mode</th>
<th>Type</th>
<th>Required</th>
<th>Default</th>
<th>Description</th>
<tr>

<tr>
<td><b>Id</b></td>
<td>readonly</td>
<td>string</td>
<td>-</td>
<td>-</td>
<td>The id of the bill</td>
<tr>

<tr>
<td><b>GroupId</b></td>
<td>read/write</td>
<td>string</td>
<td><b>Yes</b></td>
<td>-</td>
<td>The id of the group you are charging</td>
<tr>

<tr>
<td><b>PriceCents</b></td>
<td>read/write</td>
<td>Integer</td>
<td><b>Yes</b></td>
<td>-</td>
<td>The amount in cents to charge to the customer</td>
<tr>

<tr>
<td><b>Description</b></td>
<td>read/write</td>
<td>String</td>
<td><b>Yes</b></td>
<td>-</td>
<td>A description of the product billed as it should appear on customer invoice</td>
<tr>

<tr>
<td><b>CreatedAt</b></td>
<td>readonly</td>
<td>DateTime</td>
<td>-</td>
<td>-</td>
<td>When the the bill was created</td>
<tr>

<tr>
<td><b>UpdatedAt</b></td>
<td>readonly</td>
<td>DateTime</td>
<td>-</td>
<td>-</td>
<td>When the bill was last updated</td>
<tr>

<tr>
<td><b>Status</b></td>
<td>readonly</td>
<td>String</td>
<td>-</td>
<td>-</td>
<td>Status of the bill. Either 'submitted', 'invoiced' or 'cancelled'.</td>
<tr>

<tr>
<td><b>Currency</b></td>
<td>read/write</td>
<td>String</td>
<td>-</td>
<td>AUD</td>
<td>The currency of the amount charged in <a href="http://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO 4217 format</a> (3 letter code)</td>
<tr>

<tr>
<td><b>Units</b></td>
<td>read/write</td>
<td>Decimal(10,2)</td>
<td>-</td>
<td>1.0</td>
<td>How many units are billed for the amount charged</td>
<tr>

<tr>
<td><b>PeriodStarted_at</b></td>
<td>read/write</td>
<td>DateTime</td>
<td>-</td>
<td>-</td>
<td>If the bill relates to a specific period then specifies when the period started. Both period_started_at and period_ended_at need to be filled in order to appear on customer invoice.</td>
<tr>

<tr>
<td><b>PeriodEndedAt</b></td>
<td>read/write</td>
<td>DateTime</td>
<td>-</td>
<td>-</td>
<td>If the bill relates to a specific period then specifies when the period ended. Both period_started_at and period_ended_at need to be filled in order to appear on customer invoice.</td>
<tr>

</table>

##### Actions

List all bills you have created and iterate through the list
```csharp
var bills = Maestrano.Account.Bill.All();
// Using a specific Configuration Preset
// var bills = Maestrano.Account.Bill.With("somePreset").All();
```

Access a single bill by id
```csharp
var bill = Maestrano.Account.Bill.Retrieve("bill-f1d2s54");
// Using a specific Configuration Preset
// var bill = Maestrano.Account.Bill.With("somePreset").Retrieve("bill-f1d2s54");
```

Create a new bill
```csharp
var bill = Maestrano.Account.Bill.Create(groupId: "cld-3", priceCents: 2000, description: "Product purchase");
// Using a specific Configuration Preset
// var bill = Maestrano.Account.Bill.With("somePreset").Create(groupId: "cld-3", priceCents: 2000, description: "Product purchase");
```

Cancel a bill
```csharp
var bill = Maestrano.Account.Bill.Retrieve("bill-f1d2s54");
bill.Cancel();
// Using a specific Configuration Preset
// var bill = Maestrano.Account.Bill.With("somePreset").Retrieve("bill-f1d2s54");
// bill.Cancel();
```

#### Recurring Bill
A recurring bill charges a given customer at a regular interval without you having to do anything.

```csharp
Maestrano.Account.RecurringBill
```

##### Attributes

<table>
<tr>
<th>Field</th>
<th>Mode</th>
<th>Type</th>
<th>Required</th>
<th>Default</th>
<th>Description</th>
<tr>

<tr>
<td><b>Id</b></td>
<td>readonly</td>
<td>string</td>
<td>-</td>
<td>-</td>
<td>The id of the recurring bill</td>
<tr>

<tr>
<td><b>GroupId</b></td>
<td>read/write</td>
<td>string</td>
<td><b>Yes</b></td>
<td>-</td>
<td>The id of the group you are charging</td>
<tr>

<tr>
<td><b>PriceCents</b></td>
<td>read/write</td>
<td>Integer</td>
<td><b>Yes</b></td>
<td>-</td>
<td>The amount in cents to charge to the customer</td>
<tr>

<tr>
<td><b>Description</b></td>
<td>read/write</td>
<td>String</td>
<td><b>Yes</b></td>
<td>-</td>
<td>A description of the product billed as it should appear on customer invoice</td>
<tr>

<tr>
<td><b>Period</b></td>
<td>read/write</td>
<td>String</td>
<td>-</td>
<td>Month</td>
<td>The unit of measure for the billing cycle. Must be one of the following: 'Day', 'Week', 'SemiMonth', 'Month', 'Year'</td>
<tr>

<tr>
<td><b>Frequency</b></td>
<td>read/write</td>
<td>Integer</td>
<td>-</td>
<td>1</td>
<td>The number of billing periods that make up one billing cycle. The combination of billing frequency and billing period must be less than or equal to one year. If the billing period is SemiMonth, the billing frequency must be 1.</td>
<tr>

<tr>
<td><b>Cycles</b></td>
<td>read/write</td>
<td>Integer</td>
<td>-</td>
<td>nil</td>
<td>The number of cycles this bill should be active for. In other words it's the number of times this recurring bill should charge the customer.</td>
<tr>

<tr>
<td><b>StartDate</b></td>
<td>read/write</td>
<td>DateTime</td>
<td>-</td>
<td>Now</td>
<td>The date when this recurring bill should start billing the customer</td>
<tr>

<tr>
<td><b>CreatedAt</b></td>
<td>readonly</td>
<td>DateTime</td>
<td>-</td>
<td>-</td>
<td>When the the bill was created</td>
<tr>

<tr>
<td><b>UpdatedAt</b></td>
<td>readonly</td>
<td>DateTime</td>
<td>-</td>
<td>-</td>
<td>When the recurring bill was last updated</td>
<tr>

<tr>
<td><b>Currency</b></td>
<td>read/write</td>
<td>String</td>
<td>-</td>
<td>AUD</td>
<td>The currency of the amount charged in <a href="http://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO 4217 format</a> (3 letter code)</td>
<tr>

<tr>
<td><b>Status</b></td>
<td>readonly</td>
<td>String</td>
<td>-</td>
<td>-</td>
<td>Status of the recurring bill. Either 'submitted', 'active', 'expired' or 'cancelled'.</td>
<tr>

<tr>
<td><b>InitialCents</b></td>
<td>read/write</td>
<td>Integer</td>
<td><b>-</b></td>
<td>0</td>
<td>Initial non-recurring payment amount - in cents - due immediately upon creating the recurring bill</td>
<tr>

</table>

##### Actions

List all recurring bills you have created and iterate through the list
```csharp
var rec_bills = Maestrano.Account.RecurringBill.All();
// Using a specific Configuration Preset
// var rec_bills = Maestrano.Account.RecurringBill.With("somePreset").All();
```

Access a single recurring bill by id
```csharp
var rec_bill = Maestrano.Account.RecurringBill.Retrieve("rbill-f1d2s54");
// Using a specific Configuration Preset
// var rec_bill = Maestrano.Account.RecurringBill.With("somePreset").Retrieve("rbill-f1d2s54");
```

Create a new recurring bill
```csharp
var rec_bill = Maestrano.Account.RecurringBill.Create(groupId: "cld-3", priceCents: 2000, description: "Product purchase", period: 'Month', startDate: DateTime.UtcNow);
// Using a specific Configuration Preset
// var rec_bill = Maestrano.Account.RecurringBill.With("somePreset").Create(groupId: "cld-3", priceCents: 2000, description: "Product purchase", period: 'Month', startDate: DateTime.UtcNow);
```

Cancel a recurring bill
```csharp
var rec_bill = Maestrano.Account.RecurringBill.Retrieve("rbill-f1d2s54");
rec_bill.Cancel();
// Using a specific Configuration Preset
// var rec_bill = Maestrano.Account.RecurringBill.With("somePreset").Retrieve("rbill-f1d2s54");
// rec_bill.Cancel();
```

### Membership API

#### User
A user is a member of a group having access to your application. Users are currently readonly.

```csharp
Maestrano.Account.User
```

##### Attributes

<table>
<tr>
<th>Field</th>
<th>Mode</th>
<th>Type</th>
<th>Required</th>
<th>Default</th>
<th>Description</th>
<tr>

<tr>
<td><b>Id</b></td>
<td>readonly</td>
<td>string</td>
<td>-</td>
<td>-</td>
<td>The id of the user</td>
<tr>

<tr>
<td><b>FirstName</b></td>
<td>readonly</td>
<td>string</td>
<td><b>-</b></td>
<td>-</td>
<td>The user first name</td>
<tr>

<tr>
<td><b>LastName</b></td>
<td>readonly</td>
<td>string</td>
<td><b>-</b></td>
<td>-</td>
<td>The user last name</td>
<tr>

<tr>
<td><b>Email</b></td>
<td>readonly</td>
<td>string</td>
<td><b>-</b></td>
<td>-</td>
<td>The user real email address</td>
<tr>

<tr>
<td><b>CompanyName</b></td>
<td>readonly</td>
<td>string</td>
<td><b>-</b></td>
<td>-</td>
<td>The user company name as it was entered when they signed up. Nothing related to the user group name.</td>
<tr>

<tr>
<td><b>Country</b></td>
<td>readonly</td>
<td>string</td>
<td><b>-</b></td>
<td>-</td>
<td>The country of the user in <a href="http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 alpha-2 format</a> (2 letter code). E.g: 'US' for USA, 'AU' for Australia.</td>
<tr>

<tr>
<td><b>CreatedAt</b></td>
<td>readonly</td>
<td>DateTime</td>
<td>-</td>
<td>-</td>
<td>When the user was created</td>
<tr>

<tr>
<td><b>UpdatedAt</b></td>
<td>readonly</td>
<td>DateTime</td>
<td>-</td>
<td>-</td>
<td>When the user was last updated</td>
<tr>

</table>

##### Actions

List all users having access to your application
```csharp
var users = Maestrano.Account.User.All();
// Using a specific Configuration Preset
// var users = Maestrano.Account.User.With("somePreset").All();
```

Access a single user by id
```csharp
var user = Maestrano.Account.User.Retrieve("usr-f1d2s54");
// Using a specific Configuration Preset
// var user = Maestrano.Account.User.With("somePreset").Retrieve("usr-f1d2s54");
```

#### Group
A group represents a customer account and is composed of members (users) having access to your application. A group also represents a chargeable account (see Bill/RecurringBill). Typically you can remotely check if a group has entered a credit card on Maestrano.

Groups are currently readonly.


```csharp
Maestrano.Account.Group
```

##### Attributes

<table>
<tr>
<th>Field</th>
<th>Mode</th>
<th>Type</th>
<th>Required</th>
<th>Default</th>
<th>Description</th>
<tr>

<tr>
<td><b>Id</b></td>
<td>readonly</td>
<td>string</td>
<td>-</td>
<td>-</td>
<td>The id of the group</td>
<tr>

<tr>
<td><b>Name</b></td>
<td>readonly</td>
<td>string</td>
<td><b>-</b></td>
<td>-</td>
<td>The group name</td>
<tr>

<tr>
<td><b>Email</b></td>
<td>readonly</td>
<td>string</td>
<td><b>-</b></td>
<td>-</td>
<td>The principal email address for this group (admin email address)</td>
<tr>

<tr>
<td><b>HasCreditCard</b></td>
<td>readonly</td>
<td>bool</td>
<td><b>-</b></td>
<td>-</td>
<td>Whether the group has entered a credit card on Maestrano or not</td>
<tr>

<tr>
<td><b>FreeTrialEndAt</b></td>
<td>readonly</td>
<td>DateTime</td>
<td><b>-</b></td>
<td>-</td>
<td>When the group free trial will be finishing on Maestrano. You may optionally consider this date for your own free trial (optional)</td>
<tr>

<tr>
<td><b>Currency</b></td>
<td>readonly</td>
<td>String</td>
<td>-</td>
<td>-</td>
<td>The currency used by this Group in <a href="http://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO 4217 format</a> (3 letter code)</td>
<tr>

<tr>
<td><b>Country</b></td>
<td>readonly</td>
<td>string</td>
<td><b>-</b></td>
<td>-</td>
<td>The country of the group in <a href="http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">ISO 3166-1 alpha-2 format</a> (2 letter code). E.g: 'US' for USA, 'AU' for Australia.</td>
<tr>

<tr>
<td><b>City</b></td>
<td>readonly</td>
<td>string</td>
<td><b>-</b></td>
<td>-</td>
<td>The city of the group</td>
<tr>

<tr>
<td><b>TimeZone</b></td>
<td>readonly</td>
<td>TimeZone</td>
<td><b>-</b></td>
<td>-</td>
<td>The group timezone</td>
<tr>

<tr>
<td><b>CreatedAt</b></td>
<td>readonly</td>
<td>DateTime</td>
<td>-</td>
<td>-</td>
<td>When the group was created</td>
<tr>

<tr>
<td><b>UpdatedAt</b></td>
<td>readonly</td>
<td>DateTime</td>
<td>-</td>
<td>-</td>
<td>When the group was last updated</td>
<tr>

</table>

##### Actions

List all groups having access to your application
```csharp
var groups = Maestrano.Account.Group.All();
// Using a specific Configuration Preset
// var groups = Maestrano.Account.Group.With("somePreset").All();
```

Access a single group by id
```csharp
var group = Maestrano.Account.Group.Retrieve("usr-f1d2s54");
// Using a specific Configuration Preset
// var groups = Maestrano.Account.Group.With("somePreset").Retrieve("usr-f1d2s54");
```


## Connec!™ Data Sharing
Maestrano offers the capability to share actual business data between applications via its data sharing platform Connec!™.

The platform exposes a set of RESTful JSON APIs allowing your application to receive data generated by other applications and update data in other applications as well!

Connec!™ also offers the ability to create webhooks on your side to get automatically notified of changes happening in other systems.

Connec!™ enables seamless data sharing between the Maestrano applications as well as popular apps such as QuickBooks and Xero. One connector - tens of integrations!

### Making Requests

Connec!™ REST API documentation can be found here: http://maestrano.github.io/connec

The Maestrano API provides a built-in client - based on Restsharp - for connecting to Connec!™. Things like connection and authentication are automatically managed by the Connec!™ client.


```csharp
// Pass the customer group id as argument
var client = new Maestrano.Connec.Client("cld-f7f5g4");
// Using a specific Configuration Preset
// var client = Maestrano.Connec.Client.With("somePreset").New("cld-f7f5g4");

// Retrieve all organizations (customers and suppliers) created in other applications
var resp = client.Get('/organizations');
resp.Content; // returns the raw response "{\"organizations\":[ ... ]}"

// Retrieve a parsed response (assuming model Organization exists in your application)
RestResponse<Organization> resp = client.Get<Organization>('/organizations');
resp.Data // returns a native object

// Create a new organization
var body = new Dictionary<string, Dictionary<string, string>>();
var entity = new Dictionary<string, string>();
entity.Add("name", "Jazz Corp Inc.");
body.Add("organizations", entity);
client.Post('/organizations', JsonConvert.SerializeObject(body))

// Update an organization
var updBody = new Dictionary<string, Dictionary<string, bool>>();
var update = new Dictionary<string, bool>();
entity.Add("is_customer", true);
updBody.Add("organizations", entity);
client.Put('/organizations/e32303c1-5102-0132-661e-600308937d74', JsonConvert.SerializeObject(updBody));

// If you prefer you can also get a Restsharp client (configured for Connec!™)
var restClient = Maestrano.Connec.Client.RestClient("cld-f7f5g4");
```


### Webhook Notifications
If you have configured the Maestrano API to receive update notifications (see 'connecSubscriptions' configuration at the top) from Connec!™ then you can expect to receive regular POST requests on the notification_path you have configured.

Notifications are JSON messages containing the list of entities that have recently changed in other systems. You will only receive notifications for entities you have subscribed to.

Example of notification message:
```ruby
{
  "organizations": [
    { "id": "e32303c1-5102-0132-661e-600308937d74", name: "DoeCorp Inc.", ... }
  ],
  "people": [
    { "id": "a34303d1-4142-0152-362e-610408337d74", first_name: "John", last_name: "Doe", ... }
  ]
}
```

Entities sent via notifications follow the same data structure as the one described in our REST API documentation (available at http://maestrano.github.io/connec)


## Support
This README is still in the process of being written and improved. As such it might not cover some of the questions you might have.

So if you have any question or need help integrating with us just let us know at support@maestrano.com

## License

MIT License. Copyright 2014 Maestrano Pty Ltd. https://maestrano.com

You are not granted rights or licenses to the trademarks of Maestrano.
