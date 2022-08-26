
using StoreLib.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace StoreLib.Services
{
    public class DisplayCatalogHandler
    {
        private readonly MSHttpClient _httpClient;

        public DisplayCatalogModel ProductListing { get; private set; }
        private Uri ConstructedUri { get; set; }
        private DCatEndpoint _selectedEndpoint;
        private DisplayCatalogResult Result { get; set; }
        private readonly Locale _selectedLocale;
        public bool IsFound;


        public DisplayCatalogHandler(DCatEndpoint selectedEndpoint, Locale locale)
        {
            //Adds needed headers for MS related requests. See MS_httpClient.cs
            _httpClient = new MSHttpClient();

            _selectedEndpoint = selectedEndpoint;
            _selectedLocale = locale;
        }

        public static DisplayCatalogHandler ProductionConfig()
        {
            return new DisplayCatalogHandler(DCatEndpoint.Production, new Locale(Market.US, Lang.en, true));
        }


        /// <summary>
        /// Returns an IList of Uris containing the direct download links for the product's apps and dependacies. (if it has any). 
        /// </summary>
        /// <returns>IList of Direct File URLs</returns>
        public async Task<IList<PackageInstance>> GetPackagesForProductAsync(string msaToken = null)
        {
            string xml = await FE3Handler.SyncUpdatesAsync(ProductListing.Product.DisplaySkuAvailabilities[0].Sku.Properties.FulfillmentData.WuCategoryId, msaToken);
            IList<string> revisionIDs;
            IList<string> packageNames;
            IList<string> updateIDs;
            FE3Handler.ProcessUpdateIDs(xml, out revisionIDs, out packageNames, out updateIDs);
            IList<PackageInstance> packageInstances = await FE3Handler.GetPackageInstancesAsync(xml);
            IList<Uri> files = await FE3Handler.GetFileUrlsAsync(updateIDs, revisionIDs, msaToken);
            foreach(PackageInstance package in packageInstances)
            {
                int id = packageInstances.IndexOf(package);
                package.PackageUri = files[id];
                package.UpdateId = updateIDs[id];
            }
            return packageInstances;
        }

        /// <summary>
        /// Queries DisplayCatalog for the provided ID. The resulting possibly found product is reflected in DisplayCatalogHandlerInstance.ProductListing. If the product isn't found, that variable will be null, check IsFound and Result.
        /// The provided Auth Token is also sent allowing for flighted or sandboxed listings. The resulting possibly found product is reflected in DisplayCatalogHandlerInstance.ProductListing. If the product isn't found, that variable will be null, check IsFound and Res
        /// </summary>
        /// <param name="id">The ID, type specified in DCatHandler Instance.</param>
        /// <param name="idType">Type of ID being passed.</param>
        /// <param name="authenticationToken"></param>
        /// <returns></returns>
        public async Task QueryDcatAsync(string id, IdentiferType idType = IdentiferType.ProductID, string authenticationToken = null) //Optional Authentication Token used for Sandbox and Flighting Queries.
        {
            ConstructedUri = Utilities.UriHelpers.CreateAlternateDCatUri(_selectedEndpoint, id, idType, _selectedLocale);
            Result = new DisplayCatalogResult(); //We need to clear the result incase someone queries a product, then queries a not found one, the wrong product will be returned.
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            
            //We need to build the request URL based on the requested EndPoint;
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, ConstructedUri);

            if (!String.IsNullOrEmpty(authenticationToken))
            {
                httpRequestMessage.Headers.TryAddWithoutValidation("Authentication", authenticationToken);
            }

            try
            {
                httpResponse = await _httpClient.SendAsync(httpRequestMessage, new System.Threading.CancellationToken());
            }
            catch (TaskCanceledException)
            {
                Result = DisplayCatalogResult.TimedOut;
            }
            if (httpResponse.IsSuccessStatusCode)
            {
                string content = await httpResponse.Content.ReadAsStringAsync();
                Result = DisplayCatalogResult.Found;
                IsFound = true;
                ProductListing = DisplayCatalogModel.FromJson(content);
            }
            else if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Result = DisplayCatalogResult.NotFound;
            }
            else
            {
                throw new Exception($"Failed to query DisplayCatalog Endpoint: {_selectedEndpoint.ToString()} Status Code: {httpResponse.StatusCode} Returned Data: {await httpResponse.Content.ReadAsStringAsync()}");
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="query">The raw search query</param>
        /// <param name="deviceFamily">The wanted DeviceFamily, only supported apps/games will be returned</param>
        /// <returns>Instance of DCatSearch, containing the returned products.</returns>
        public async Task<DCatSearch> SearchDcatAsync(string query, DeviceFamily deviceFamily)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            HttpRequestMessage httpRequestMessage;
            switch (deviceFamily)
            {
                case DeviceFamily.Desktop:
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Utilities.TypeHelpers.EnumToSearchUri(_selectedEndpoint)}{query}&productFamilyNames=apps,games&platformDependencyName=Windows.Desktop");
                    httpResponse = await _httpClient.SendAsync(httpRequestMessage, new System.Threading.CancellationToken());
                    break;
                case DeviceFamily.Xbox:
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Utilities.TypeHelpers.EnumToSearchUri(_selectedEndpoint)}{query}&productFamilyNames=apps,games&platformDependencyName=Windows.Xbox");
                    httpResponse = await _httpClient.SendAsync(httpRequestMessage, new System.Threading.CancellationToken());
                    break;
                case DeviceFamily.Universal:
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Utilities.TypeHelpers.EnumToSearchUri(_selectedEndpoint)}{query}&productFamilyNames=apps,games&platformDependencyName=Windows.Universal");
                    httpResponse = await _httpClient.SendAsync(httpRequestMessage, new System.Threading.CancellationToken());
                    break;
                case DeviceFamily.Mobile:
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Utilities.TypeHelpers.EnumToSearchUri(_selectedEndpoint)}{query}&productFamilyNames=apps,games&platformDependencyName=Windows.Mobile");
                    httpResponse = await _httpClient.SendAsync(httpRequestMessage, new System.Threading.CancellationToken());
                    break;
                case DeviceFamily.HoloLens:
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Utilities.TypeHelpers.EnumToSearchUri(_selectedEndpoint)}{query}&productFamilyNames=apps,games&platformDependencyName=Windows.Holographic");
                    httpResponse = await _httpClient.SendAsync(httpRequestMessage, new System.Threading.CancellationToken());
                    break;
                case DeviceFamily.IotCore:
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Utilities.TypeHelpers.EnumToSearchUri(_selectedEndpoint)}{query}&productFamilyNames=apps,games&platformDependencyName=Windows.Iot");
                    httpResponse = await _httpClient.SendAsync(httpRequestMessage, new System.Threading.CancellationToken());
                    break;
                case DeviceFamily.ServerCore:
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Utilities.TypeHelpers.EnumToSearchUri(_selectedEndpoint)}{query}&productFamilyNames=apps,games&platformDependencyName=Windows.Server");
                    httpResponse = await _httpClient.SendAsync(httpRequestMessage, new System.Threading.CancellationToken());
                    break;
                case DeviceFamily.Andromeda:
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Utilities.TypeHelpers.EnumToSearchUri(_selectedEndpoint)}{query}&productFamilyNames=apps,games&platformDependencyName=Windows.8828080");
                    httpResponse = await _httpClient.SendAsync(httpRequestMessage, new System.Threading.CancellationToken());
                    break;
                case DeviceFamily.WCOS:
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Utilities.TypeHelpers.EnumToSearchUri(_selectedEndpoint)}{query}&productFamilyNames=apps,games&platformDependencyName=Windows.Core");
                    httpResponse = await _httpClient.SendAsync(httpRequestMessage, new System.Threading.CancellationToken());
                    break;
            }

            if (!httpResponse.IsSuccessStatusCode)
            {
                var message =
                    $"Failed to search DisplayCatalog: {deviceFamily.ToString()} Status Code: {httpResponse.StatusCode} Returned Data: {await httpResponse.Content.ReadAsStringAsync()}";
                throw new Exception(message);
            }

            string content = await httpResponse.Content.ReadAsStringAsync();
            Result = DisplayCatalogResult.Found;
            DCatSearch dcatSearch = DCatSearch.FromJson(content);
            return dcatSearch;

        }
        
    }
    
}
