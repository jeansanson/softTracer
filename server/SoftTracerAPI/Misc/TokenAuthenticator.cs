using MySql.Data.MySqlClient;
using SoftTracerAPI.Misc;
using SoftTracerAPI.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ExtensionMethods
{
    public class TokenAuthenticator : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (IsAuthorizationHeaderEmpty(actionContext))
            {
                SetUnauthorizedRepsponse(actionContext);
            }
            else
            {
                string token = GetAuthorizationHeader(actionContext);
                if (IsFormatValid(actionContext, token))
                {
                    Validate(actionContext, token);
                }
            }
            base.OnAuthorization(actionContext);
        }

        private static void Validate(HttpActionContext actionContext, string token)
        {
            MySqlConnection connection = new SoftTracerConnection().connection;
            UsersRepository repository = new UsersRepository(connection);
            string username = repository.FindUsernameByToken(new Guid(token));
            if (!string.IsNullOrEmpty(username))
            {
                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(username), null);
            }
            else
            {
                SetUnauthorizedRepsponse(actionContext);
            }
            connection.Close();
        }

        private static bool IsAuthorizationHeaderEmpty(HttpActionContext actionContext)
        {
            return actionContext.Request.Headers.Authorization == null || actionContext.Request.Headers.Authorization.ToString() == String.Empty;
        }

        private static bool IsFormatValid(HttpActionContext actionContext, string token)
        {
            if (token.Length != 36 && !(token.Split(new char[] { '-' }).Length == 4))
            {
                SetUnauthorizedRepsponse(actionContext);
                return false;
            }
            return true;
        }

        private static void SetUnauthorizedRepsponse(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        private static string GetAuthorizationHeader(HttpActionContext actionContext)
        {
            var scheme = actionContext.Request.Headers.Authorization.Scheme;
            var parameter = actionContext.Request.Headers.Authorization.Parameter;
            if (scheme != null) { return scheme; }
            if (parameter != null) { return parameter; }
            return null;
        }
    }
}