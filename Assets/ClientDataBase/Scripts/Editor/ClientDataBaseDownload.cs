using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace ClientDataBase
{
    public class UnsafeSecurityPolicy
    {
        public static bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            Debug.Log("Validation successful!");
            return true;
        }

        public static void Instate()
        {
            ServicePointManager.ServerCertificateValidationCallback = Validator;
        }
    }

    public class ClientDataBaseDownload : Singleton<ClientDataBaseDownload>
    {
        string CLIENT_ID = "52201423218-dve7903h9pbgid0n6ecd2nc3chq009qp.apps.googleusercontent.com";
        string CLIENT_SECRET = "uRBiC1j2EBDopO01GFDOQP1c";
        string SCOPE = "https://www.googleapis.com/auth/drive https://spreadsheets.google.com/feeds https://docs.google.com/feeds";
        string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";
        string ACCESS_CODE = "4/nbDug2aVjmBFwRP7QFCs0Y1TvCbNAUIS2H22p4XzQ2U";


        private OAuth2Parameters GetParameters()
        {
            UnsafeSecurityPolicy.Instate();
            OAuth2Parameters parameters = new OAuth2Parameters();
            parameters.ClientId = CLIENT_ID;
            parameters.ClientSecret = CLIENT_SECRET;
            parameters.RedirectUri = REDIRECT_URI;
            parameters.Scope = SCOPE;
            parameters.AccessType = "offline"; // IMPORTANT 
            parameters.TokenType = "refresh"; // IMPORTANT 
            parameters.AccessCode = ACCESS_CODE;

            return parameters;
        }

        public void InitAuthenticate()
        {
            OAuth2Parameters parameters = GetParameters();

            string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
            Debug.Log(authorizationUrl);
            Debug.Log("Please visit the URL above to authorize your OAuth "
              + "request token.  Once that is complete, type in your access code to "
              + "continue...");
            Application.OpenURL(authorizationUrl);
        }

        public void FinishAuthenticate()
        {
            OAuth2Parameters parameters = GetParameters();
            OAuthUtil.GetAccessToken(parameters);

            string accessToken = parameters.AccessToken;
            string refreshToken = parameters.RefreshToken;
            Debug.Log("OAuth Access Token: " + accessToken);
            Debug.Log("OAuth Refresh Token: " + refreshToken);
        }

    }

}