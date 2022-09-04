using System;
using System.Text;
using System.Security.Cryptography;
using StoreLib.Exceptions;
using StoreLib.Models;
using StoreLib.Services;

namespace StoreLib.Utilities
{
    public static class TypeHelpers
    {
        /// <summary>
        /// Convert the provided DcatEndpoint enum to its matching Uri.
        /// </summary>
        /// <param name="EnumEndpoint"></param>
        /// <returns></returns>
        public static Uri EnumToUri(DCatEndpoint EnumEndpoint)
        {
            switch (EnumEndpoint)
            {
                case DCatEndpoint.Production:
                    return Endpoints.DCATProd;
                case DCatEndpoint.Dev:
                    return Endpoints.DCATDev;
                case DCatEndpoint.Int:
                    return Endpoints.DCATInt;
                case DCatEndpoint.Xbox:
                    return Endpoints.DCATXbox;
                case DCatEndpoint.XboxInt:
                    return Endpoints.DCATXboxInt;
                case DCatEndpoint.OneP:
                    return Endpoints.DCATOneP;
                case DCatEndpoint.OnePInt:
                    return Endpoints.DCATOnePInt;
                default:
                    return Endpoints.DCATProd;
            }
        }

        public static Uri EnumToSearchUri(DCatEndpoint Endpoint)
        {
            switch (Endpoint)
            {
                case DCatEndpoint.Production:
                    return Endpoints.DisplayCatalogSearch;
                case DCatEndpoint.Int:
                    return Endpoints.DisplayCatalogSearchInt;
                default:
                    return Endpoints.DisplayCatalogSearch;
            }
        }

        public static String EnumToPlatformDependencyName(DeviceFamily deviceFamily)
        {
             switch (deviceFamily)
            {
                case DeviceFamily.Desktop:
                    return "Windows.Desktop";
                case DeviceFamily.Xbox:
                    return "Windows.Xbox";
                case DeviceFamily.Universal:
                    return "Windows.Universal";
                case DeviceFamily.Mobile:
                    return "Windows.Mobile";
                case DeviceFamily.HoloLens:
                    return "Windows.Holographic";
                case DeviceFamily.IotCore:
                    return "Windows.Iot";
                case DeviceFamily.ServerCore:
                    return "Windows.Server";
                case DeviceFamily.Andromeda:
                    return "Windows.8828080";
                case DeviceFamily.WCOS:
                    return "Windows.Core";
                default:
                    throw new NotFoundException($"Undefined DeviceFamily {deviceFamily.ToString()}");
            }
        }

        public static PackageType StringToPackageType(string raw)
        {
            switch (raw)
            {
                case "XAP":
                    return PackageType.XAP;
                case "AppX":
                    return PackageType.AppX;
                case "UAP":
                    return PackageType.UAP;
                default:
                    return PackageType.Unknown;
            }
        }
    }

    internal static class HashHelpers //These are used for the image caching function. The input uri will be hashed and used as the downloaded image file name. 
    {

        internal static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        internal static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }


    }

    public static class UriHelpers
    {
        /// <summary>
        /// Create and format the DCat request uri based on the provided endpoint, id, id type and locale. 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="ID"></param>
        /// <param name="IDType"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static Uri CreateAlternateDCatUri(DCatEndpoint endpoint, string ID, IdentiferType IDType, Services.Locale locale)
        {
            switch (IDType)
            {
                case IdentiferType.ContentID:
                    return new Uri($"{TypeHelpers.EnumToUri(endpoint)}lookup?alternateId=CONTENTID&Value={ID}&{locale.DCatTrail}&fieldsTemplate=Details");
                case IdentiferType.LegacyWindowsPhoneProductID:
                    return new Uri($"{TypeHelpers.EnumToUri(endpoint)}lookup?alternateId=LegacyWindowsPhoneProductID&Value={ID}&{locale.DCatTrail}&fieldsTemplate=Details");
                case IdentiferType.LegacyWindowsStoreProductID:
                    return new Uri($"{TypeHelpers.EnumToUri(endpoint)}lookup?alternateId=LegacyWindowsStoreProductID&Value={ID}&{locale.DCatTrail}&fieldsTemplate=Details");
                case IdentiferType.LegacyXboxProductID:
                    return new Uri($"{TypeHelpers.EnumToUri(endpoint)}lookup?alternateId=LegacyXboxProductID&Value={ID}&{locale.DCatTrail}&fieldsTemplate=Details");
                case IdentiferType.PackageFamilyName:
                    return new Uri($"{TypeHelpers.EnumToUri(endpoint)}lookup?alternateId=PackageFamilyName&Value={ID}&{locale.DCatTrail}&fieldsTemplate=Details");
                case IdentiferType.XboxTitleID:
                    return new Uri($"{TypeHelpers.EnumToUri(endpoint)}lookup?alternateId=XboxTitleID&Value={ID}&{locale.DCatTrail}&fieldsTemplate=Details");
                case IdentiferType.ProductID:
                    return new Uri($"{TypeHelpers.EnumToUri(endpoint)}{ID}?{locale.DCatTrail}");
                default:
                    throw new Exception("CreateAlternateDCatUri: Unknown IdentifierType was passed, an update is likely required, please report this issue.");

            }

        }

        public static Uri CreateDCatUri(DCatEndpoint endpoint, DeviceFamily deviceFamily, Locale locale, string query)
        {
            return new Uri($"{TypeHelpers.EnumToSearchUri(endpoint)}{query}&productFamilyNames=apps,games&market={locale.Market}&languages={locale.Language}&platformDependencyName={TypeHelpers.EnumToPlatformDependencyName(deviceFamily)}");
        }
        
        public static Uri CreateSearchUrl(Locale locale, string query)
        {
            return new Uri($"{Endpoints.AdvancedSearchProducts}?hl={locale.Language}&gl={locale.Market}&icid=CNavAppsWindowsApps&FilteredCategories=AllProducts&Query={query}");
        }
    }
}
