using DotNetCore_MultipleJWTSchemas.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace DotNetCore_MultipleJWTSchemas.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SigningConfigurations _signingConfigurations;

        public AuthController(SigningConfigurations signingConfigurations)
        {
            _signingConfigurations = signingConfigurations;
        }

        #region Token 1

        [HttpGet("authToken1")]
        [AllowAnonymous]
        public ActionResult AuthToken1()
        {
            return Ok(gerarToken1());
        }

        private object gerarToken1()
        {
            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(300);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "IssuerToken1",
                Audience = "AudienceToken1",
                SigningCredentials = _signingConfigurations.SigningCredentials,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });

            return handler.WriteToken(securityToken);
        }

        [HttpGet("getToken1")]
        [Authorize("PolicyToken1")]
        public ActionResult<string> GetToken1()
        {
            return "OK";
        }

        #endregion Token 1

        #region Token 2

        [HttpGet("authToken2")]
        [AllowAnonymous]
        public ActionResult AuthToken2()
        {
            return Ok(gerarToken2());
        }

        private object gerarToken2()
        {
            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(300);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "IssuerToken2",
                Audience = "AudienceToken2",
                SigningCredentials = _signingConfigurations.SigningCredentials,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });

            return handler.WriteToken(securityToken);
        }

        [HttpGet("getToken2")]
        [Authorize("PolicyToken2")]
        public ActionResult<string> GetToken2()
        {
            return "OK";
        }

        #endregion Token 2
    }
}
