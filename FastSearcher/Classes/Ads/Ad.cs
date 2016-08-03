using System;
using System.Collections.Generic;
using System.Text;
using FastSearcher.Classes.Support;
using Windows.ApplicationModel.Store;
using System.Threading.Tasks;

namespace FastSearcher.Classes.Ads
{
    class Ad
    {
        public enum AdType { INAPP, WEBLINK, TEXTONLY}
        public string Text { get; private set; }

        AdType Type;

        string uriLink;
        string productId;

        public bool AdSuccess { get; private set; }

        public Ad(string text, AdType type, string uri)
        {
            AdSuccess = false;
            Text = text;
            Type = type;
            if (type == AdType.INAPP)
                productId = uri;
            else if(type == AdType.WEBLINK)
                uriLink = uri;
                            
        }

        public async Task AdTapped()
        {
            AdSuccess = false;
            if (Type == AdType.INAPP)
            {
                FulfillmentResult fr = await MarketplaceHelper.Instance.PurchaseProduct(productId);
                if (fr == FulfillmentResult.Succeeded)
                    AdSuccess = true;
            }
            else if(Type == AdType.WEBLINK)
            {
                AdSuccess = true;
                await Windows.System.Launcher.LaunchUriAsync(new Uri(uriLink));
            }
        }

    }
}
