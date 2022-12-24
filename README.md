# OpenIDConnectServer
OpenID Connect code sample containing SSOn/Out, reference tokens, custom grants and multi-tenancy.

## Steps:
- cd to `src` directory.
- Run `dotnet new --install Duende.IdentityServer.Templates` command to install Duende project templates CLI.
-  Run `dotnet new isinmem -n IdentityServer.IDP` command to create new identity project.
- Run `dotnet new sln -n IdentityServer` command to create empty solution.
- Open solution file and add project into it.
