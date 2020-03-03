using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Attributes;
using GolemClientMockAPI.Processors;
using GolemClientMockAPI.Repository;
using GolemClientMockAPI.Security;
using GolemMarketMockAPI.GolemNetHubAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace GolemClientMockAPI.Controllers
{
    [ApiController]
    [GolemClientAuthorizationFilter(DefaultNodeId = null)]
    public class GolemNetHubAuthApiController : ControllerBase
    {

        public INetHubProcessor NetHubProcessor { get; set; }
        public IJwtBuilder JwtBuilder { get; set; }
        public IConfigProvider ConfigProvider { get; set; }

        public GolemNetHubAuthApiController(INetHubProcessor netHubProcessor, IJwtBuilder jwtBuilder, IConfigProvider config)
        {
            this.NetHubProcessor = netHubProcessor;
            this.JwtBuilder = jwtBuilder;
            this.ConfigProvider = config;
        }

        /// <summary>
        /// Authenticate within the hub
        /// </summary>
        /// <response code="200">Successful operation</response>
        /// <response code="403">Forbidden</response>
        [HttpPost]
        [Route("/net-api/v1/auth")]
        [ValidateModelState]
        [SwaggerOperation("Authenticate")]
        [SwaggerResponse(statusCode: 200, type: typeof(JWT), description: "Successful operation")]
        public virtual IActionResult Authenticate()
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            var claims = new Dictionary<string, string>()
            {
                { "iss", "GolemNetHub" },
                { "aud", "GolemNetHub" },
                { "sub", clientContext.NodeId }
            };

            var newToken = this.JwtBuilder.CreateToken(claims, this.ConfigProvider.PrivateKey, this.ConfigProvider.PublicKey);

            if (!this.NetHubProcessor.IsNodeRegistered(clientContext.NodeId))
                this.NetHubProcessor.RegisterNode(clientContext.NodeId);

            return StatusCode(200, newToken.ToString());
        }


        /// <summary>
        /// Deactivate the authorization token within the hub
        /// </summary>
        /// <response code="200">Successful operation</response>
        /// <response code="403">Forbidden</response>
        /// <response code="405">Invalid input</response>
        [HttpDelete]
        [Route("/net-api/v1/auth")]
        [ValidateModelState]
        [Authorize(AuthenticationSchemes = BearerAuthenticationHandler.SchemeName)]
        [SwaggerOperation("AuthDelete")]
        public virtual IActionResult AuthDelete()
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            if (this.NetHubProcessor.IsNodeRegistered(clientContext.NodeId))
            {
                this.NetHubProcessor.DeregisterNode(clientContext.NodeId);
                return StatusCode(200);
            }
            else
            {
                return StatusCode(405);
            }
        }


    }
}