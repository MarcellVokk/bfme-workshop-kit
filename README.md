# WorkshopKit by Bfme Foundation
#### This project is part of the Bfme Foundation Project!

 ## Welcome
 Welcome to the official github repository of WorkshopKit!
 This package is a C# wrapper for the public Bfme Foundation Workshop API, and also provides realy powerful tools for patch switching, modding and more!

 ## Usage
 - `BfmeWorkshopQueryManager` exposes several methods that can be used to query or search the workshop.
 - `BfmeWorkshopSyncManager` is the workshops built in "patch switcher". It is highly recommended that you use this instead of trying to build your own (trust me, it's not worth it)
 - `BfmeWorkshopLibraryManager` is used to access the users library, and query, add or remove workshop entries from it.
 - `BfmeWorkshopAdminManager` is for uploading or deleting your own entries to the workshop.
 - `BfmeWorkshopAuthManager` is used for authentication. This is the way you can get a `BfmeWorkshopAuthInfo` to use in the methods exposed by `BfmeWorkshopAdminManager`.

## Examples
A very simple workshop browser example made in WinForms is available in this repository.

###### Developed by: Gazdag Marcell (*@marcellvokk*)<br> Founder & Owner: Beterwell (*@Beterwell*)
