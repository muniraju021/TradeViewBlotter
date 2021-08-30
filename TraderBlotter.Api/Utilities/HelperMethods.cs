using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace TraderBlotter.Api.Utilities
{
    public class HelperMethods
    {
        private const string secretKey = "3ad7bf9c-b91f-4f76-a91e-{0}-779df4795d03";
        private static string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText.Replace("-",string.Empty));
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string GetLicensekey()
        {
            var macAddress = GetMacAddress();
            var key = string.Format(secretKey, macAddress);            
            return Base64Encode(key);
        }
    }
}
