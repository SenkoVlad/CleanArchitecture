﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections;
using System.Collections.Generic;

namespace CleanArchitecture.Identity
{
    public static class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("NotesWebApi", "Web Api")
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("NotesWebApi", "Web Api", new[] {JwtClaimTypes.Name })
                {
                    Scopes = { "NotesWebApi" }
                }                    
            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "notes-web-app",
                    ClientName = "Notes Web",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris = {"http://localhost:3000/signin-oidc" },
                    AllowedCorsOrigins = {"http://localhost:3000"},
                    PostLogoutRedirectUris = { "http://localhost:3000" },
                    AllowedScopes = 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "NotesWebApi"
                    },
                    AllowAccessTokensViaBrowser = true
                }
            };
    }
}
