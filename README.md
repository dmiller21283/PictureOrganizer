# PictureOrganizer

## Description
A console application to copy or move a flat directory of images to a structured directory. The pictures are copied or moved to a structure of [year]/[month] based on the DateTaken attribute or DateCreated attribute.

## Usage
```
pictureorganizer.exe [copy | move | list] [source] [desitination]
```

## Example
```
pictureorganizer.exe move "c:\users\jdoe\camera roll" "c:\users\jdoe\pictures"

#Source
c:\users\jdoe\camera roll
  Name          Date Taken
  img_001.jpg   12/25/2020
  img_002.jpg   2/14/2021
  img_003.jpg   7/4/2021
  img_004.jpg   7/4/2021
  img_005.jpg   8/1/2021


#Destination
c:\users\jdoe\pictures
  Name          Date Taken
|_2020
| |_December
|   |_img_001.jpg   12/25/2020
|_2021
 |_Februrary
 |  |_img_002.jpg   2/14/2021
 |_July
 |  |_img_003.jpg   7/4/2021
 |  |_img_004.jpg   7/4/2021
 |_August
    |_img_005.jpg   8/1/2021
```
