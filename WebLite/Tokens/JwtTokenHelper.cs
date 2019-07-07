using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebLite.Configurations;

namespace WebLite.Tokens
{
    public class JwtTokenHelper
    {
        private static readonly string issuer = ConfigurationManager.GetSection("JwtToken:issuer");
        public static readonly SymmetricSecurityKey credit = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.GetSection("JwtToken:credit")));
        private static readonly TimeSpan expires = TimeSpan.FromSeconds(double.Parse(ConfigurationManager.GetSection("JwtToken:expires")));
        private static readonly JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private static readonly SigningCredentials creditial = new SigningCredentials(credit, SecurityAlgorithms.HmacSha256);

        public static string SerializeToken(JwtToken token)
        {
            DateTime now = DateTime.UtcNow;
            var claims = new List<Claim>
                {
                //加入uid信息
                new Claim(JwtRegisteredClaimNames.Jti, token.Uid.ToString()),
                //颁发时间
                new Claim(JwtRegisteredClaimNames.Iat, now.ToString(), ClaimValueTypes.Integer64)
            };
            //加入角色信息
            if (token.Role != null && token.Role.Count > 0)
            {
                claims.AddRange(token.Role.Select(s => new Claim(JwtRegisteredClaimNames.Sub, s)));
            }

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: token.Name,
                claims: claims,
                expires: now.Add(expires),
                signingCredentials: creditial
                );

            return tokenHandler.WriteToken(jwt);
        }

        /// <summary>
        /// 解析Jwt
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static JwtToken DeserializeToken(string jwtStr)
        {
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(jwtStr);

            try
            {
                object role = null;
                jwtToken.Payload.TryGetValue(JwtRegisteredClaimNames.Sub, out role);

                var tm = new JwtToken
                {
                    Uid = jwtToken.Id,
                    Role = role != null ? (List<string>)role : null,
                    Name = jwtToken.Audiences.FirstOrDefault()
                };
                return tm;
            }
            catch
            {
                return default(JwtToken);
            }

        }
    }
}
