
using StoreLib.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StoreLib.Exceptions;
using StoreLib.Utilities;

namespace StoreLib.Services
{
    public class DisplayCatalogHandler
    {
        private readonly MSHttpClient _httpClient;
        
        private readonly DCatEndpoint _selectedEndpoint;
        private readonly Locale _selectedLocale;


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
        public async Task<IList<PackageInstance>> GetPackagesForProductAsync(Product product, string msaToken = null)
        {
            string xml = await FE3Handler.SyncUpdatesAsync(product.DisplaySkuAvailabilities[0].Sku.Properties.FulfillmentData.WuCategoryId, msaToken);
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
        public async Task<DisplayCatalogModel> QueryDcatAsync(string id, IdentiferType idType = IdentiferType.ProductID, string authenticationToken = null) //Optional Authentication Token used for Sandbox and Flighting Queries.
        {
            var url = UriHelpers.CreateAlternateDCatUri(_selectedEndpoint, id, idType, _selectedLocale);
            
            var httpResponse = await SendRequest(url, HttpMethod.Get, authenticationToken);
            
            string content = await httpResponse.Content.ReadAsStringAsync();
            return DisplayCatalogModel.FromJson(content);

        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="query">The raw search query</param>
        /// <param name="deviceFamily">The wanted DeviceFamily, only supported apps/games will be returned</param>
        /// <returns>Instance of DCatSearch, containing the returned products.</returns>
        public async Task<DCatSearch> SearchDcatAsync(string query, DeviceFamily deviceFamily)
        {

            var url = UriHelpers.CreateDCatUri(_selectedEndpoint, deviceFamily, query);
            var httpResponse = await SendRequest(url, HttpMethod.Get);

            string content = await httpResponse.Content.ReadAsStringAsync();
            return DCatSearch.FromJson(content);
        }

        private async Task<HttpResponseMessage> SendRequest(Uri url, HttpMethod httpMethod, string authenticationToken = null)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, url);
            if (!String.IsNullOrEmpty(authenticationToken))
            {
                httpRequestMessage.Headers.TryAddWithoutValidation("Authentication", authenticationToken);
            }
            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await _httpClient.SendAsync(httpRequestMessage, new System.Threading.CancellationToken());
            }
            catch (TaskCanceledException e)
            {
                throw new TimeOutException(url.ToString(), 0, null);
            }
            
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new NotFoundException(url.ToString(), (int) httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync());
            }
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new NotFoundException(url.ToString(), (int) httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync());
            }
            
            return httpResponse;

        }

    }
    
}
