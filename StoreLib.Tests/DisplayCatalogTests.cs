using System;
using System.Threading.Tasks;
using StoreLib.Services;
using StoreLib.Models;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace StoreLib.Tests
{
    public class DisplayCatalogTests
    {
        private readonly ITestOutputHelper _output;
        public DisplayCatalogTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task QueryNetflix()
        {
            DisplayCatalogHandler dcathandler = new DisplayCatalogHandler(DCatEndpoint.Production, new Locale(Market.US, Lang.en, true));
            var productListing = await dcathandler.QueryDcatAsync("9wzdncrfj3tj");

            Assert.NotNull(productListing.Product);
            Assert.Equal("Netflix", productListing.Product.LocalizedProperties[0].ProductTitle);
        }

        [Fact]
        public async Task QueryNetflixProdConfig()
        {
            DisplayCatalogHandler dcathandler = DisplayCatalogHandler.ProductionConfig();
            var productListing = await dcathandler.QueryDcatAsync("9wzdncrfj3tj");

            Assert.NotNull(productListing.Product);
            Assert.Equal("Netflix", productListing.Product.LocalizedProperties[0].ProductTitle);
        }

        [Fact]
        public async Task QueryNetflixUsingPackageFamilyName()
        {
            DisplayCatalogHandler dcathandler = new DisplayCatalogHandler(DCatEndpoint.Production, new Locale(Market.US, Lang.en, true));
            var productListing = await dcathandler.QueryDcatAsync("Microsoft.SoDTest_8wekyb3d8bbwe", IdentiferType.PackageFamilyName);

            Assert.NotNull(productListing.Product);
        }

        [Fact]
        public async Task QueryNetflixInt()
        {
            DisplayCatalogHandler dcathandler = new DisplayCatalogHandler(DCatEndpoint.Int, new Locale(Market.US, Lang.en, true));
            var productListing =await dcathandler.QueryDcatAsync("9wzdncrfj3tj");

            Assert.NotNull(productListing.Product);
            Assert.Equal("Netflix", productListing.Product.LocalizedProperties[0].ProductTitle);
        }

        [Fact]
        public async Task SearchXbox()
        {
            DisplayCatalogHandler dcathandler = new DisplayCatalogHandler(DCatEndpoint.Production, new Locale(Market.US, Lang.en, true));
            DCatSearch search = await dcathandler.SearchDcatAsync("Halo 5", DeviceFamily.Xbox);

            _output.WriteLine($"Halo 5: Guardians: Result Count: {search.TotalResultCount}");
            Assert.Equal("Halo 5: Guardians", search.Results[0].Products[0].Title);
        }

        [Fact]
        public async Task GetSuperHeroArtForNetflix()
        {
            DisplayCatalogHandler dcathandler = new DisplayCatalogHandler(DCatEndpoint.Production, new Locale(Market.US, Lang.en, true));
            var productListing = await dcathandler.QueryDcatAsync("9wzdncrfj3tj");

            Assert.NotNull(productListing.Product);
            Uri SuperHeroArt = StoreLib.Utilities.ImageHelpers.GetImageUri(ImagePurpose.SuperHeroArt, productListing);
            Assert.NotNull(SuperHeroArt);
        }

        [Fact]
        public async Task RandomLocale()
        {
            Array Markets = Enum.GetValues(typeof(Market));
            Array Langs = Enum.GetValues(typeof(Lang));
            Random ran = new Random();
            Market RandomMarket = (Market)Markets.GetValue(ran.Next(Markets.Length));
            Lang RandomLang = (Lang)Langs.GetValue(ran.Next(Langs.Length));
            _output.WriteLine($"RandomLocale: Testing with {RandomMarket}-{RandomLang}");
            DisplayCatalogHandler dcathandler = new DisplayCatalogHandler(DCatEndpoint.Production, new Locale(RandomMarket, RandomLang, true));
            var productListing = await dcathandler.QueryDcatAsync("9wzdncrfj3tj");

            Assert.NotNull(productListing.Product);
            Assert.Equal("Netflix", productListing.Product.LocalizedProperties[0].ProductTitle);
        }

        [Fact]
        public async Task CacheImage()
        {
            DisplayCatalogHandler dcathandler = new DisplayCatalogHandler(DCatEndpoint.Production, new Locale(Market.US, Lang.en, true));
            var productListing = await dcathandler.QueryDcatAsync("9wzdncrfj3tj");

            Assert.NotNull(productListing.Product);
            Uri SuperHeroArt = StoreLib.Utilities.ImageHelpers.GetImageUri(ImagePurpose.SuperHeroArt, productListing);
            byte[] imagetest = await Utilities.ImageHelpers.CacheImageAsync(SuperHeroArt, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), false); //The ExecutingAssembly path is only being used for this unit test, in an actual program, you would want to save to the temp.
            Assert.NotNull(imagetest);
        }

        /*
        [Fact]
        public async Task GetFiles()
        {
            DisplayCatalogHandler dcathandler = DisplayCatalogHandler.ProductionConfig();
            await dcathandler.QueryDCATAsync("9wzdncrfj3tj");

            foreach(Uri download in await dcathandler.GetPackagesForProductAsync())
            {
                _output.WriteLine(download.ToString());
            }
        }
        */
    }
}
