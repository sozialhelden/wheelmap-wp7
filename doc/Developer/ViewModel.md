Developer Documentation for ViewModel
=====================================

This Document contains informations for developers of open source project **wheelmap-wp7**

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
History
-------
Version: 		1.0

Last Changes: 	23.04.2012

Author(s):		Daniel Bedarf

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

##General

If you need to display data it is nessesary to use the viemodel. The Entry Point for the ViewModel is the Class _MainViewModel_.

###MainViewModel

The MainViewModel create a connection to the DataAccess and receive Data from the Wheelmap Server automatically. 

For this reason you have to set the Wheelmap API Key on the DataAccess - Layer first.
If you did not have an API key, gi to wheelmap.org and register. You will find your Key in the Profile - Page.
To set the API - Key please use this Code:
`Wheelmap.Lib.DataAccess.DataManager.Instance.APIKey = "thisIsYourKey";`

You can set the API Key at any time you like. It will instantly be used for the next API call.

**IsDataLoaded**
If it is true, some data was previusly loaded from the cache and from the server.
It will improve the ViewModel - Perfomance if you call _LoadData_ if _IsDataLoaded_ is false.

**LoadData**
Start loading of data from the cache and the server


- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

##Location Service
If you try to get the data from the property _MainViewModel.CurrentPosition_ for the first time, the location service will be started.
We use all ways to find the current location. That is cell positions, WiFi and GPS.
All Erros where posted to the _MainViewModel.Error_ - Property

**CurrentPosition**
The _CurrentPosition_ will automaticaly be updated every 60 seconds.
If there is a new GeoPosition available you Receive a NotifyPropertyChanged Event, so you can bind this Property to your View.

**CurrentPositionChanged**
If you connect to this event you will receive an call each time y new position is located

**CurrentPositionWatcherState**
This bindable property will notify you about the LocationService State. An enum shows you the current state. So it is possible to change the viewstate if there is no geo informations available  

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
##Categories

Categories will not be automaticaly loaded.

**Categories**
This collection contains all available categories. The collection will be filled and updated by some different actions. e.g. loading nodes will fill the categories.

**actionLoadCategories**
This function will start a async server call to get or update all available categories. 

**GetCategorie**
this function allows you to select an category by her identifier. If there is no entry in the collelction, an new entry will be generated. This entry can be updated by call _actionLoadCategories_