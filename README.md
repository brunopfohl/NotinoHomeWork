# NotinoHomeWork

## EndPoints
 - (POST) Documents - The service accepts document in JSON format and saves it in to the storage.
 - (PUT) Documents - Mentioned document can also be updated afterwards.
 - (GET) Documents API consumer can retrive document in different formats if 'Accept' header is specified (application/json, application/xml, application/msgpack)
 
## Project structure
- Project is divided in to 4 projects
-- API
-- Applicatioon (Core)
-- Infrastructure (Implements interfaces from Application)
-- Tests (xUnit test project)

## Summary
API saves documents on to a local directory. Documents can then be retrieved in various formats,
which is implemented via `IOutputFormatter` services (see https://learn.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/content-negotiation for reference).
