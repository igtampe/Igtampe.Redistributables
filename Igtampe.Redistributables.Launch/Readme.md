# Launcher

The IDACRA launcher makes it easy and simple to launch your ASP.NET Web API with my common configurations. Maybe it works for you as well. Launching uses the static Launch method, with generic type parameters for your main/master DBContext, and your error handling middleware. Along with this, provide a set of Launcher Options for your Swagger page, and other metadata.

## Launch Options

|Field|Description|
|----|------------|
|App|App Details (see App Details)|
|Author|Author details (see Author Details)|
|AllowAllCORS|Allow any and all CORS requests (useful for APIs but may be a security risk)|
|AlwaysDev|Allways act as if this was a development environment, exposing the Swagger and Developer exceptions regardless of environment|
|ToManifest()|See Manifest|

### App Details
Details of the app you are launching

|Field|Description|
|-|-|
|Name|Name of the application|
|Description|Description of the application. Will be appended by "Launched using the IRED/IDACRA Launcher"|
|License|License name of URL|
|Version|Version of this app|
|ProducesXML|Whether or not this app produces XML documentation|
|XMLDocLoc|Location of the XML documentation (if ProducesXML is true)|
|Graphic|Optional BasicGraphic to override the IDACRA logo on the server startup|
|ToOAL()|Converts the License field to an OpenAPI License|

### Author Details
Details of *yourself*

|Field|Description|
|-|-|
|Name|Your Name|
|Email|Your Email|
|Url|Link to your site|
|ToOAC()|Converts to an OpenAPIContact|

## Manifest
In addition to all of your controllers and endpoints, the IDACRA launcher registers `/IDACRA`, which provides a manifest of the app currently launched. This is the object it returns:

|Field|Description|
|-|-|
|idacra_version|Version of IDACRA running this app|
|Name|Name of the application|
|Description|Description of the application|
|License|License of the application|
|Version|Version of the application|
|Author|Author Details (See Author Details)|
