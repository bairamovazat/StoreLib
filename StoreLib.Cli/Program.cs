using System;
using System.Threading.Tasks;
using StoreLib.Services;
using StoreLib.Models;
using StoreLib.Exceptions;

namespace StoreLib.Cli
{
    
    class Options
    {
        // [Option('a', "authtoken", Required = false, HelpText = "Auth-token header value (e.g. \"XBL3.0=123;xyz\")")]
        public string AuthToken { get; set; }

        // [Option('m', "market", Required = false, Default = Market.US, HelpText = "Market (e.g. US)")]
        public Market Market { get; set; }

        // [Option('l', "lang", Required = false, Default=Lang.en, HelpText = "Language (e.g. EN)")]
        public Lang Language { get; set; }

        // [Option('e', "env", Required = false, Default=DCatEndpoint.Production, HelpText = "Environment (e.g. Production)")]
        public DCatEndpoint Environment { get; set; }

        // [Option('t', "idtype", Required = false, Default=IdentiferType.ProductID, HelpText = "IdentifierType")]
        public IdentiferType IdType { get; set; }

        // [Option('f', "devicefamily", Required = false, Default=DeviceFamily.Desktop, HelpText = "Device Family (used for search)")]
        public DeviceFamily DeviceFamily { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Run(new Options()
            {
                AuthToken = "",

                Market = Market.RU,

                Language = Lang.ru,

                Environment = DCatEndpoint.Production,

                IdType = IdentiferType.ProductID,

                DeviceFamily = DeviceFamily.Desktop,
            }).Wait();
        }

        private static async Task Run(Options opts)
        {
            String name = "Whatsapp";

            DisplayCatalogHandler dcatHandler = new DisplayCatalogHandler(
                opts.Environment,
                new Locale(opts.Market, opts.Language, true));

            if (!String.IsNullOrEmpty(opts.AuthToken) &&
                !opts.AuthToken.StartsWith("Token") &&
                !opts.AuthToken.StartsWith("Bearer") &&
                !opts.AuthToken.StartsWith("XBL3.0="))
            {
                Console.WriteLine("Invalid token format, ignoring...");
            }
            else if (!String.IsNullOrEmpty(opts.AuthToken))
            {
                Console.WriteLine("Setting token...");
                CommandHandler.Token = opts.AuthToken;
            }

            
            
            // Base search
            DCatSearch results;
            try
            {
                results = await dcatHandler.SearchDcatAsync(name, opts.DeviceFamily);
            }
            catch (StoreLibException exception)
            {
                Console.WriteLine("Failed to search DisplayCatalog");
                Console.WriteLine(exception);
                return;
            }
            
            foreach (Result res in results.Results)
            {
                foreach (Product prod in res.Products)
                {
                    Console.WriteLine($"{prod.Title} {prod.Type}: {prod.ProductId}. image: {prod.Icon}");
                }
            }

            var firstResult = results.Results.Find(e => e.Products.Count != 0);
            
            if (firstResult == null)
            {
                Console.WriteLine("Results count 0.");
                return;
            }
            
            
            
            // Product full info
            DisplayCatalogModel displayCatalogModel;
            try
            {
                displayCatalogModel = await dcatHandler.QueryDcatAsync(firstResult.Products[0].ProductId, IdentiferType.ProductID);
            }
            catch (StoreLibException exception)
            {
                Console.WriteLine("Failed to search DisplayCatalog");
                Console.WriteLine(exception);
                return;
            }

            
            
            Product product = displayCatalogModel.Product;
            
            
            
            //download product
            var packages = await dcatHandler.GetPackagesForProductAsync(product);
            
            foreach (PackageInstance package in packages)
            {
                var url = package.PackageUri;
                Console.WriteLine($"URL: {url}");
            }
            
            

        }
        
    }
}
