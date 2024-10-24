using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SiyafundaApplication
{
    public class SiyafundaFunctions
    {
        private static string keyVaultUrl = "https://SiyafundVault.vault.azure.net/";

        public static async Task<string> GetSecretAsync(string secretName)
        {
            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            KeyVaultSecret secret = await client.GetSecretAsync(secretName);
            return secret.Value;
        }

        public static async Task<string> GetConnectionStringAsync()
        {
            return await GetSecretAsync("ConnectionStrings--SiyafundaDB");
        }

        public static void SafeRedirect(string url)
        {
            HttpContext.Current.Response.Redirect(url, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}