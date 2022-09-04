using System.Collections.Generic;
using Newtonsoft.Json;

namespace StoreLib.Models
{

    public class FilterOptions
    {
        [JsonProperty("mediaType")] 
        public List<string> MediaType { get; set; }

        [JsonProperty("Category")] 
        public List<string> Category { get; set; }
        
        [JsonProperty("UserAge")] 
        public List<string> UserAge { get; set; }
        
        [JsonProperty("PriceType")] 
        public List<string> PriceType { get; set; }
        
    }

    public class AlternateIds
    {
        [JsonProperty("type")] 
        public string Type { get; set; }
        
        [JsonProperty("alternateIdType")] 
        public string AlternateIdType { get; set; }
        
        [JsonProperty("alternateIdValue")] 
        public string AlternateIdValue { get; set; }
        
        [JsonProperty("alternatedIdTypeString")] 
        public string AlternatedIdTypeString { get; set; }
    }

    public class AdvancedImage
    {
        [JsonProperty("imageType")] 
        public string ImageType { get; set; }
        
        [JsonProperty("url")] 
        public string Url { get; set; }
        
        [JsonProperty("caption")] 
        public string Caption { get; set; }
        
        [JsonProperty("height")] 
        public int Height { get; set; }
        
        [JsonProperty("width")] 
        public int Width { get; set; }
        
        [JsonProperty("backgroundColor")] 
        public string BackgroundColor { get; set; }
        
        [JsonProperty("foregroundColor")] 
        public string ForegroundColor { get; set; }
        
        [JsonProperty("imagePositionInfo")] 
        public string ImagePositionInfo { get; set; }
    }

    public class AdvancedProduct
    {
        public AdvancedImage GetLogo()
        {
            return Images?.Find(e => e.ImageType == "logo");
        }

        [JsonProperty("categories")] 
        public List<string> Categories { get; set; }
        
        [JsonProperty("images")] 
        public List<AdvancedImage> Images { get; set; }
        
        [JsonProperty("productId")] 
        public string ProductId { get; set; }
        
        [JsonProperty("title")] 
        public string Title { get; set; }
        
        [JsonProperty("shortTitle")] 
        public string ShortTitle { get; set; }
        [JsonProperty("titleLayout")] 
        public string TitleLayout { get; set; }
        [JsonProperty("description")] 
        public string Description { get; set; }
        [JsonProperty("publisherName")] 
        public string PublisherName { get; set; }
        [JsonProperty("publisherId")] 
        public string PublisherId { get; set; }
        [JsonProperty("isUniversal")] 
        public string IsUniversal { get; set; }
        [JsonProperty("language")] 
        public string Language { get; set; }
        [JsonProperty("bgColor")] 
        public string BgColor { get; set; }
        [JsonProperty("fgColor")] 
        public string FgColor { get; set; }
        [JsonProperty("averageRating")] 
        public string AverageRating { get; set; }
        [JsonProperty("ratingCount")] 
        public string RatingCount { get; set; }
        [JsonProperty("hasFreeTrial")] 
        public string HasFreeTrial { get; set; }
        [JsonProperty("productType")] 
        public string ProductType { get; set; }
        [JsonProperty("price")] 
        public string Price { get; set; }
        [JsonProperty("currencySymbol")] 
        public string CurrencySymbol { get; set; }
        [JsonProperty("currencyCode")] 
        public string CurrencyCode { get; set; }
        [JsonProperty("displayPrice")] 
        public string DisplayPrice { get; set; }
        [JsonProperty("strikethroughPrice")] 
        public string StrikethroughPrice { get; set; }
        [JsonProperty("developerName")] 
        public string DeveloperName { get; set; }
        [JsonProperty("productFamilyName")] 
        public string ProductFamilyName { get; set; }
        [JsonProperty("mediaType")] 
        public string MediaType { get; set; }
        
        [JsonProperty("contentIds")] 
        public List<string> ContentIds { get; set; }
        
        [JsonProperty("packageFamilyNames")] 
        public List<string> PackageFamilyNames { get; set; }
        
        [JsonProperty("subcategoryName")] 
        public string SubcategoryName { get; set; }
        [JsonProperty("alternateIds")] 
        public List<AlternateIds> AlternateIds { get; set; }
        [JsonProperty("collectionItemType")] 
        public string CollectionItemType { get; set; }
        [JsonProperty("numberOfSeasons")] 
        public string NumberOfSeasons { get; set; }
        [JsonProperty("releaseDateUtc")] 
        public string ReleaseDateUtc { get; set; }
        [JsonProperty("durationInSeconds")] 
        public string DurationInSeconds { get; set; }
        [JsonProperty("isCompatible")] 
        public string IsCompatible { get; set; }
        [JsonProperty("isPurchaseEnabled")] 
        public string IsPurchaseEnabled { get; set; }
        [JsonProperty("developerOptOutOfSDCardInstall")] 
        public string DeveloperOptOutOfSDCardInstall { get; set; }
        [JsonProperty("hasAddOns")] 
        public string HasAddOns { get; set; }
        [JsonProperty("hasThirdPartyIAPs")] 
        public string HasThirdPartyIAPs { get; set; }
        [JsonProperty("voiceTitle")] 
        public string VoiceTitle { get; set; }
        [JsonProperty("hideFromCollections")] 
        public string HideFromCollections { get; set; }
        [JsonProperty("hideFromDownloadsAndUpdates")] 
        public string HideFromDownloadsAndUpdates { get; set; }
        [JsonProperty("gamingOptionsXboxLive")] 
        public bool GamingOptionsXboxLive { get; set; }
        [JsonProperty("availableDevicesDisplayText")] 
        public string AvailableDevicesDisplayText { get; set; }
        [JsonProperty("availableDevicesNarratorText")] 
        public string AvailableDevicesNarratorText { get; set; }
        [JsonProperty("isGamingAppOnly")] 
        public bool IsGamingAppOnly { get; set; }
        [JsonProperty("isSoftBlocked")] 
        public string IsSoftBlocked { get; set; }
        [JsonProperty("tileLayout")] 
        public string TileLayout { get; set; }
        [JsonProperty("typeTag")] 
        public string TypeTag { get; set; }
        [JsonProperty("longDescription")] 
        public string LongDescription { get; set; }
        [JsonProperty("schema")] 
        public string Schema { get; set; }
        [JsonProperty("iconUrl")] 
        public string IconUrl { get; set; }
   
    }

    public class AdvancedSearchResult
    {

        [JsonProperty("title")] 
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("curatedCollectionDetails")]
        public string CuratedCollectionDetails { get; set; }
        
        [JsonProperty("highlightedList")]
        public List<AdvancedProduct> HighlightedList { get; set; }
        
        [JsonProperty("productsList")]
        public List<AdvancedProduct> ProductsList { get; set; }

        [JsonProperty("Cursor")]
        public string Cursor { get; set; }
        
        [JsonProperty("hasCursorPaging")]
        public bool HasCursorPaging { get; set; }
        
        [JsonProperty("filterOptions")]
        public FilterOptions FilterOptions { get; set; }
        
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
        
        [JsonProperty("nextPageNo")]
        public int NextPageNo { get; set; }
        
        [JsonProperty("hasMorePages")]
        public bool HasMorePages { get; set; }
        
        [JsonProperty("continuationToken")]
        public string ContinuationToken { get; set; }
        
        public static AdvancedSearchResult FromJson(string json) => JsonConvert.DeserializeObject<AdvancedSearchResult>(json, Converter1.Settings);

    }

}