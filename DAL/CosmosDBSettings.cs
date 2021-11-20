using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class CosmosDBSettings
    {

        public static readonly string ENDPOINT_URI = "https://ms-ng-nosql-db-acct.documents.azure.com:443/";

        public static readonly string KEY = "13cPxx0d3o8s1fsTYauAHRv4jSeXv4dpium30OUWMHv73duYh9FC7OvDR8dI2WxAoHNGsLRTsfC6IYHZmlOEzg==";

        public static readonly string DB_ID = "ChatDatabase";
        public static readonly string GROUP_CONTAINER_ID = "GroupContainer";
        public static readonly string USER_CONTAINER_ID = "UserContainer";
        public static readonly string MESSAGE_CONTAINER_ID = "MsgContainer";
    }
}
