using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Attributes;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Processors;
using GolemClientMockAPI.Security;
using GolemMarketMockAPI.GolemNetHubAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace GolemClientMockAPI.Controllers
{
    [ApiController]
    [GolemNetHubAuthorizationFilter(DefaultNodeId = "DummyNodeId")]
    public class GolemNetHubMessageApiController : ControllerBase
    {
        public INetHubProcessor NetHubProcessor { get; set; }

        public GolemNetHubMessageApiController(INetHubProcessor netHubProcessor)
        {
            this.NetHubProcessor = netHubProcessor;
        }


        /// <summary>
        /// Receive a message
        /// </summary>
        /// <response code="200">Successful operation</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [Route("/net-api/v1/message")]
        [ValidateModelState]
        [SwaggerOperation("Poll")]
        [SwaggerResponse(statusCode: 200, type: typeof(Message), description: "Successful operation")]
        public virtual async Task<IActionResult> Poll()
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            if (this.NetHubProcessor.IsNodeRegistered(clientContext.NodeId))
            {
                var messages = await this.NetHubProcessor.CollectMessagesAsync(clientContext.NodeId, 1000);

                var result = messages.Select(msg => new Message()
                {
                    Destination = msg.DestinationAddress,
                    Payload = msg.Payload,
                    RequestId = msg.RequestId
                });

                return new ObjectResult(result);
            }
            else
            {
                return StatusCode(405);
            }


        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="body">Network message to send</param>
        /// <response code="200">Successful operation</response>
        /// <response code="403">Forbidden</response>
        /// <response code="405">Invalid input</response>
        [HttpPost]
        [Route("/net-api/v1/message")]
        [ValidateModelState]
        [SwaggerOperation("Send")]
        public virtual IActionResult Send([FromBody]Message body)
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            if (this.NetHubProcessor.IsNodeRegistered(clientContext.NodeId))
            {
                var message = new NetMessage()
                {
                    DestinationAddress = body.Destination,
                    Payload = body.Payload,
                    RequestId = body.RequestId,
                    SenderNodeId = clientContext.NodeId
                };

                this.NetHubProcessor.SendMessage(message);
                return StatusCode(200);
            }
            else
            {
                return StatusCode(405);
            }
        }

    }
}