using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace FastSearcher.Classes.Support
{
    class MarketplaceHelper
    {
        static MarketplaceHelper instance;
        static public MarketplaceHelper Instance { get { if (instance == null) instance = new MarketplaceHelper(); return instance; } }

        private MarketplaceHelper() { }

        public void ReviewApp()
        {
            MarketplaceReviewTask mrktTask = new MarketplaceReviewTask();
            mrktTask.Show();
        }

        public async Task<FulfillmentResult> PurchaseProduct(string productId)
        {
            try
            {
                await CurrentApp.RequestProductPurchaseAsync(productId);

                try
                {
                    var licenses = CurrentApp.LicenseInformation.ProductLicenses;
                    if (licenses[productId].IsConsumable && licenses[productId].IsActive)
                    {
                        Guid transactionID = new Guid();
                        FulfillmentResult fr = await CurrentApp.ReportConsumableFulfillmentAsync(productId, transactionID);
                        return fr;
                    }
                }
                catch (Exception) { }
            }
            catch (Exception) { }

            return FulfillmentResult.ServerError;
        }
    }
}
