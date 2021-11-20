using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageGuardAPI
{
    public class CognitiveSettings
    {
        public const string LUIS_SERVICE_URL = "https://australiaeast.api.cognitive.microsoft.com/luis/prediction/v3.0/apps/2b62d810-3763-47ca-8b42-3f7a827eef30/slots/production/predict?verbose=true&show-all-intents=true&log=true&subscription-key=a9ab34d9c8bd415e83842a75248515fd&query=";
    }
}
