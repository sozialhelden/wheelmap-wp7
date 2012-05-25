http://stackoverflow.com/questions/4724004/free-silverlight-mapping-with-bing-maps-and-openstreetmap

Following excerpted from the Microsoft Bing Maps Terms of Use (http://www.microsoft.com/maps/product/terms.html), with emphasis added:

"If you would like to develop or host an Application that is designed to access and use the service for commercial, non-commercial or government use... and your Application and content will be available publically without restriction (for example, login or password must not be required) you may do so without entering into a MWS/BM agreement or licensing the service through Microsoft Volume Licensing by complying with... all of the restrictions on educational and non-profit use... the following restriction also applies:

You may not exceed more than 125,000 sessions or 500,000 transactions, both as defined in the SDKs, in any twelve month period."

So, you do not need to have a licence, even for commercial projects, so long as you abide by the limits set out in the terms of service (there are other limits stated in the full ToS, including no more than 50,000 geocoding requests in a 24 hour period).

However, be aware that loading the Silverlight control itself (even with OSM data) counts as a transaction (from http://msdn.microsoft.com/en-us/library/ff859477.aspx), so you might need a licence if you have more than 125,000 sessions a year.

If you are unsure, try emailing maplic@microsoft.com
