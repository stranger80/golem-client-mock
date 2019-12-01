using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Security
{
    public class GolemNetHubAuthorizationFilter : Attribute, IActionFilter
    {
        public GolemNetHubAuthorizationFilter()
        {

        }

        public string DefaultNodeId { get; set; }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // resolve the Bearer header here
            // and inject default if no header

            var config = context.HttpContext.RequestServices.GetService(typeof(IConfigProvider)) as IConfigProvider;

            if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var clientContext = new ClientContext();

                var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

                token = token.Replace("Bearer ", "");

                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    var jwt = tokenHandler.ReadJwtToken(token);

                    // Validate the token

                    var jwtBuilder = context.HttpContext.RequestServices.GetService(typeof(IJwtBuilder)) as IJwtBuilder;

                    var isValid = jwtBuilder.ValidateToken(token, new Dictionary<string, string>()
                        {
                            { "aud", "GolemNetHub" },
                            { "iss", "GolemNetHub" }
                        }, config.PublicKey);


                    if (isValid)
                    {
                        clientContext.NodeId = jwt.Subject;

                        if (clientContext.NodeId != null)
                        {
                            context.HttpContext.Items["ClientContext"] = clientContext;
                            return;
                        }
                    }
                }
                catch (Exception exc)
                {
                    // TODO Log the invalid token exception
                }

            }

            // if we are here - there was no proper authorization token in request
            if (this.DefaultNodeId != null)
            {
                // context.Result = new StatusCodeResult(401); // short circuit to return status 401

                var clientContext = new ClientContext()
                {
                    NodeId = DefaultNodeId
                };

                context.HttpContext.Items["ClientContext"] = clientContext;
            }
            else
            {
                context.Result = new StatusCodeResult(401);
            }

        }
    }
}
