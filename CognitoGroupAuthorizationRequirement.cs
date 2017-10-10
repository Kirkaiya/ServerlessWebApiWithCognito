using Microsoft.AspNetCore.Authorization;

namespace ServerlessSpaWithDotNet
{
    class CognitoGroupAuthorizationRequirement : IAuthorizationRequirement
    {
        public string CognitoGroup { get; private set; }

        public CognitoGroupAuthorizationRequirement(string cognitoGroup)
        {
            CognitoGroup = cognitoGroup;
        }
    }
}
