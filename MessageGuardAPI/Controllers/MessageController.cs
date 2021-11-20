using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageGuardAPI.Models;
using DomainCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;

namespace MessageGuardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpGet]
        [Route("CheckIntent")]
        public async Task<MessageResponse> CheckMessageIntent(string message)
        {
            LuisResponse luisResponse = null;
            HttpClient messageClient = new HttpClient();

            var httpResponse = await messageClient.GetAsync(CognitiveSettings.LUIS_SERVICE_URL+message);

            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                luisResponse = JsonConvert.DeserializeObject<LuisResponse>(content);
            }

            var safe = luisResponse?.prediction?.TopIntent != "Insult";

            return new MessageResponse { Safe = safe };
        }
    }
}