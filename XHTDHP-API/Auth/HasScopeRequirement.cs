using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Auth
{
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Value { get; }
        public string Scope { get; }

        public HasScopeRequirement(string scope, string value)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
