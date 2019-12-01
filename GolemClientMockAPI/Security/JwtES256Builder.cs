using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Sec;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Security
{
    public class JwtES256Builder : IJwtBuilder
    {

        public const string ISSUER_CLAIM = "iss";
        public const string SUBJECT_CLAIM = "sub";
        public const string AUDIENCE_CLAIM = "aud";
        public const string EXPIRATION_TIME_CLAIM = "exp";
        public const string NOT_BEFORE_CLAIM = "nbf";
        public const string ISSUED_AT_TIME_CLAIM = "sub";
        public const string JWTID_CLAIM = "jti";
        public const string NAME_CLAIM = "name";

        public string CreateToken(IDictionary<string, string> claims, string privateKey, string publicKey)
        {
            var privateECDsa = LoadPrivateKey(FromHexString(privateKey));
            var publicECDsa = LoadPublicKey(FromHexString(publicKey));

            var now = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.InboundClaimTypeMap[JwtRegisteredClaimNames.Sub] = ClaimTypes.Name;
            tokenHandler.OutboundClaimTypeMap[ClaimTypes.Name] = JwtRegisteredClaimNames.Sub;

            var jwtToken = tokenHandler.CreateJwtSecurityToken(
                issuer: ResolveClaim(claims, JwtRegisteredClaimNames.Iss),
                audience: ResolveClaim(claims, JwtRegisteredClaimNames.Aud),
                subject: new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, ResolveClaim(claims, JwtRegisteredClaimNames.Sub)),
                        new Claim("public_key", publicKey)
                    }),
                notBefore: now,
                expires: now.AddMonths(12),
                issuedAt: now,
                signingCredentials: new SigningCredentials(new ECDsaSecurityKey(privateECDsa), SecurityAlgorithms.EcdsaSha256));

            return tokenHandler.WriteToken(jwtToken);
        }

        private string ResolveClaim(IDictionary<string, string> claims, string claimName)
        {
            return claims.ContainsKey(claimName) ? claims[claimName] : null;
        }

        public bool ValidateToken(string token, IDictionary<string, string> expectedClaims, string publicKey)
        {
            var publicECDsa = LoadPublicKey(FromHexString(publicKey));

            var tokenHandler = new JwtSecurityTokenHandler();

            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidIssuer = ResolveClaim(expectedClaims, JwtRegisteredClaimNames.Iss),
                ValidAudience = ResolveClaim(expectedClaims, JwtRegisteredClaimNames.Aud),
                IssuerSigningKey = new ECDsaSecurityKey(publicECDsa)
            }, out var parsedToken);

            return claimsPrincipal.Identity.IsAuthenticated;
        }

        private static byte[] FromHexString(string hex)
        {
            var numberChars = hex.Length;
            var hexAsBytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
                hexAsBytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return hexAsBytes;
        }

        private static ECDsa LoadPrivateKey(byte[] key)
        {
            var privKeyInt = new Org.BouncyCastle.Math.BigInteger(+1, key);
            var parameters = SecNamedCurves.GetByName("secp256r1");
            var ecPoint = parameters.G.Multiply(privKeyInt);
            var privKeyX = ecPoint.Normalize().XCoord.ToBigInteger().ToByteArrayUnsigned();
            var privKeyY = ecPoint.Normalize().YCoord.ToBigInteger().ToByteArrayUnsigned();

            return ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                D = privKeyInt.ToByteArrayUnsigned(),
                Q = new ECPoint
                {
                    X = privKeyX,
                    Y = privKeyY
                }
            });
        }

        private static ECDsa LoadPublicKey(byte[] key)
        {
            var pubKeyX = key.Skip(1).Take(32).ToArray();
            var pubKeyY = key.Skip(33).ToArray();

            return ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                Q = new ECPoint
                {
                    X = pubKeyX,
                    Y = pubKeyY
                }
            });
        }
    }
}
