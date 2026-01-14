# Duplicate Files Remover
Simple tool to find and remove identical files of any type with max efficiency

<img width="1169" height="1041" alt="Image" src="https://github.com/user-attachments/assets/2661cea7-9a19-4174-89fa-06c7c78dd1e8" />

## Technology
 - Multithreaded search engine
 - Multithreaded compare processing
 - XXHash algorithm used, most speed efficient in the world

## Search methods
 - "Fast method" - compare hash of only last bytes
   - You can select how much last bytes to verify
 - "Slow method" - compare the whole file hash
   - Before calculating the hash of entire file, the partial "Fast method" hash check is applied

## Features
 - Removes file permanently or in Recycle Bin
 - Displays total size of duplicated files
 - No AI :)
 - No telemetry :)
 - No ads :)

## TODO
 - Add filters (by file type, directory exclusions)
 - Add option to speed up by compare only identical file extensions
 - USN Journal search engine to process the entire disk drive with max speed
 - Add sorting by size
 - Save settings
 - Show app version
 - Improve UI and add context menus
 - Silently skip file system read errors and log to file
 - "About" page
