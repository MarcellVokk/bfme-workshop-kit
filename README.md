# WorkshopKit by Bfme Foundation
#### This project is part of the Bfme Foundation Project!
<a href="https://github.com/MarcellVokk/bfme-foundation-project">
    <img src="https://img.shields.io/badge/GitHub-Foundation Project-lime"/>
</a>

## Welcome
Welcome to the official github repository of WorkshopKit!
This package is a C# wrapper for the public Bfme Foundation Workshop API, and also provides realy powerful tools for patch switching, modding and more!

## Get on NuGet
<a href="https://www.nuget.org/packages/BfmeFoundationProject.WorkshopKit">
   <img src="https://img.shields.io/nuget/v/BfmeFoundationProject.WorkshopKit"/>
</a>

## Usage
- `BfmeWorkshopQueryManager` exposes several methods that can be used to query or search the workshop.
- `BfmeWorkshopDownloadManager` exposes several methods that allow you to download packages from the workshop. Downloading a package is required when syncing, as a package preview (obtained by a query) is not enough.
- `BfmeWorkshopSyncManager` is the workshops built in "patch switcher". It is highly recommended that you use this instead of trying to build your own (trust me, it's not worth it)
- `BfmeWorkshopLibraryManager` is used to access the users library, and query, add or remove workshop entries from it.
- `BfmeWorkshopAdminManager` is for uploading or deleting your own entries to the workshop.
- `BfmeWorkshopAuthManager` is used for authentication. This is the way you can get a `BfmeWorkshopAuthInfo` to use in the methods exposed by `BfmeWorkshopAdminManager`.

## Examples
A very simple workshop browser example made in WinForms is available in this repository.

Developed by The Online Battle Arena Team, in collaboration with the Patch 2.22 Team
