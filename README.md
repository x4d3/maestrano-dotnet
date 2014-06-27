<p align="center">
<img src="https://raw.github.com/maestrano/maestrano-rails/master/maestrano.png" alt="Maestrano Logo">
</p>

Maestrano Cloud Integration is currently in closed beta. Want to know more? Send us an email to <contact@maestrano.com>.
  
  
  
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
4. [Account Webhooks](#account-webhooks)
  * [Groups Controller](#groups-controller-service-cancellation)
  * [Group Users Controller](#group-users-controller-business-member-removal)
5. [API](#api)
  * [Bill](#bill)
  * [Recurring Bill](#recurring-bill)

- - -

## Getting Setup
Before integrating with us you will need an App ID and API Key. Maestrano Cloud Integration being still in closed beta you will need to contact us beforehand to gain production access.

For testing purpose we provide an API Sandbox where you can freely obtain an App ID and API Key. The sandbox is great to test single sign-on and API integration (e.g: billing API).

To get started just go to: http://api-sandbox.maestrano.io

## Getting Started

### Installation

[TODO: Package not available on NuGet yet]
To install Maestrano, run the following command in the Package Manager Console
```console
PM> Install-Package Maestrano
```


### Configuration
The best way to configure the Maestrano api is to add a section in your config file (Web.config) as
shown below

The initializer should look like this:
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
      If set to 'production' then all Single Sign-On (SSO) and API requests
      will be made to maestrano.com
      If set to 'test' then requests will be made to api-sandbox.maestrano.io
      The api-sandbox allows you to easily test integration scenarios.
      More details on http://api-sandbox.maestrano.io
      
      => host
      This is your application host (e.g: my-app.com) which is ultimately
      used to redirect users to the right SAML url during SSO handshake.
    -->
    <app
      environment="test"
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
      If set to false then Maestrano.SSO.Session#IsValid - which should be
      used in a controller action filter to check user session - always return true
      
      => idm
      By default we consider that the domain managing user identification
      is the same as your application host (see above config.app.host parameter)
      If you have a dedicated domain managing user identification and therefore
      responsible for the single sign-on handshake (e.g: https://idp.my-app.com)
      then you can specify it below
      
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
      initPath="/maestrano/auth/saml/init.aspx"
      consumePath="/maestrano/auth/saml/consume"
      creationMode="virtual"
      />
     -->
    
    <!--
      ===> Account Webhooks
      Single sign on has been setup into your app and Maestrano users are now able
      to use your service. Great! Wait what happens when a business (group) decides to 
      stop using your service? Also what happens when a user gets removed from a business?
      Well the endpoints below are for Maestrano to be able to notify you of such
      events.
  
      Even if the routes look restful we issue only issue DELETE requests for the moment
      to notify you of any service cancellation (group deletion) or any user being
      removed from a group.
  
    -->
    <webhook>
      <!--
      <account
        groupsPath="/maestrano/account/groups/:id"
        groupUsersPath="/maestrano/account/groups/:group_id/users/:id"
        />
       -->
    </webhook>
  </maestrano>
  
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
public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/json";
      
        // Authentication
        var request = HttpContext.Current.Request;
        
        if (Maestrano.Authenticate(request)) 
        {
          Response.Write(Maestrano.ToMetadata().ToString());
        } 
        else 
        {
          Response.Write("Failed");
        }
    }
}
```

## Single Sign-On Setup
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
public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var request = HttpContext.Current.Request;
        
        var ssoUrl = Maestrano.Sso.BuildRequest(request.Params).RedirectUrl();
        Response.Redirect(ssoUrl);
    }
}
```

Based on your application requirements the consume action might look like this:
```csharp
public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var request = HttpContext.Current.Request;
        
        // Get SAML response, build maestrano user and group objects
        var samlResp = Maestrano.Sso.BuildResponse(request.Params["SAMLResponse"]) 
        var mnoUser = new Maestrano.Sso.User(samlResp);
        var mnoGroup = new Maestrano.Sso.Group(samlResp);
        
        // Build/Map local entities
        var localGroup = MyGroup.FindOrCreateForMaestrano(mnoGroup);
        var localUser = MyUser.FindOrCreateForMaestrano(mnoUser);
        
        // Add localUser to the localGroup if not already part
        // of it
        if (!localGroup.HasMember(localUser)){
          localGroup.AddMember(localUser)
        }
        
        // Set Maestrano session
        Maestrano.Sso.SetSession(Session,mnoUser)
        
        Response.Redirect("/");
    }
}
```
Note that for the consume action you should disable CSRF authenticity if your framework is using it by default. If CSRF authenticity is enabled then your app will complain on the fact that it is receiving a form without CSRF token.

### Other Controllers
If you want your users to benefit from single logout then you should define the following filter in a module and include it in all your controllers except the one handling single sign-on authentication.

```csharp
//TODO
```

## Account Webhooks
Single sign on has been setup into your app and Maestrano users are now able to use your service. Great! Wait what happens when a business (group) decides to stop using your service? Also what happens when a user gets removed from a business? Well the controllers describes in this section are for Maestrano to be able to notify you of such events.

### Groups Controller (service cancellation)
Sad as it is a business might decide to stop using your service at some point. On Maestrano billing entities are represented by groups (used for collaboration & billing). So when a business decides to stop using your service we will issue a DELETE request to the webhook.account.groups_path endpoint (typically /maestrano/account/groups/:id).

Maestrano only uses this controller for service cancellation so there is no need to implement any other type of action - ie: GET, PUT/PATCH or POST. The use of other http verbs might come in the future to improve the communication between Maestrano and your service but as of now it is not required.

The controller example below reimplements the authenticate_maestrano! method seen in the [metadata section](#metadata) for completeness. Utimately you should move this method to a helper if you can.

The example below needs to be adapted depending on your application:
```csharp
//TODO
```

### Group Users Controller (business member removal)
A business might decide at some point to revoke access to your services for one of its member. In such case we will issue a DELETE request to the webhook.account.group_users_path endpoint (typically /maestrano/account/groups/:group_id/users/:id).

Maestrano only uses this controller for user membership cancellation so there is no need to implement any other type of action - ie: GET, PUT/PATCH or POST. The use of other http verbs might come in the future to improve the communication between Maestrano and your service but as of now it is not required.

The controller example below reimplements the authenticate_maestrano! method seen in the [metadata section](#metadata) for completeness. Utimately you should move this method to a helper if you can.

The example below needs to be adapted depending on your application:
```csharp
//TODO
```

## API
The maestrano gem also provides bindings to its REST API allowing you to access, create, update or delete various entities under your account (e.g: billing).

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
<td>Time</td>
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
var bills = Maestrano.Account.Bill.All()
```

Access a single bill by id
```csharp
bill = Maestrano.Account.Bill.Retrieve("bill-f1d2s54")
```

Create a new bill
```csharp
bill = Maestrano.Account.Bill.Create(groupId: "cld-3", priceCents: 2000, description: "Product purchase")
```

Cancel a bill
```csharp
bill = Maestrano.Account.Bill.retrieve("bill-f1d2s54")
bill.Cancel()
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
<td>Time</td>
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
rec_bills = Maestrano.Account.RecurringBill.All()
```

Access a single recurring bill by id
```csharp
rec_bill = Maestrano.Account.RecurringBill.Retrieve("rbill-f1d2s54")
```

Create a new recurring bill
```csharp
rec_bill = Maestrano.Account.RecurringBill.Create(groupId: "cld-3", priceCents: 2000, description: "Product purchase", period: 'Month', startDate: DateTime.UtcNow)
```

Cancel a recurring bill
```csharp
rec_bill = Maestrano.Account.RecurringBill.Retrieve("rbill-f1d2s54")
rec_bill.Cancel()
```


## Support
This README is still in the process of being written and improved. As such it might not cover some of the questions you might have.

So if you have any question or need help integrating with us just let us know at support@maestrano.com

## License

MIT License. Copyright 2014 Maestrano Pty Ltd. https://maestrano.com

You are not granted rights or licenses to the trademarks of Maestrano.

