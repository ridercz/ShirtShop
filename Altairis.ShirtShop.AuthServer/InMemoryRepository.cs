using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Altairis.ShirtShop.AuthServer {
    public static class InMemoryRepository {

        public static IEnumerable<Client> GetClients() {
            yield return new Client {
                ClientId = "ShirtShopClientId",
                ClientSecrets = { new Secret("ShirtShopClientSecret") },
                ClientName = "ShirtShop",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowedScopes = {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
            },
                RedirectUris = { "http://localhost:5001/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:5001/" }
            };
        }

        public static IEnumerable<TestUser> GetUsers() {
            yield return new TestUser {
                SubjectId = "this_is_user_unique_id",
                Username = "user",
                Password = "password"
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources() {
            yield return new IdentityResources.OpenId();
            yield return new IdentityResources.Profile();
        }

    }
}
