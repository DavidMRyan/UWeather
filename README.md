<!-- TODO: Add build qualtiy badges from the following websites. -->
<!-- [![AppVeyor]() -->
<!-- [![CodeFactor]() -->
<!-- [![License: GPL v3]() -->

# UWeather
![](https://i.imgur.com/IoS5bEg.png)

## Description
This project is mainly used to showcase a few different software development skillsets to potential employers. This app reads weather information from the open source API hosted on https://weather.gov/. The program will request access to the end-user's location data, and pass their latitude and longitude to the API to find the first endpoint. This endpoint houses the grid coordinates the NWS uses to locate weather forecast offices around the nation! These grid coordinates are then passed to a different endpoint in the API, which will give us a bunch of relevant information about the end-user's local forecast. 

**NOTE:** This program is *not* meant to be a production-ready product, it is purely to showcase skillsets I have acquired over time, therefore it doesn't have much thought put into the UI design nor does it look very visually appealing in my own opinion.

## Prerequisites
- Windows 10, version 1809 (build 17763) or later (including Windows 11)
- Windows Application SDK
- Visual Studio 2022, version 17.0 or later [with required workloads and components](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/set-up-your-development-environment?tabs=cs-vs-community%2Ccpp-vs-community%2Cvs-2022-17-1-a%2Cvs-2022-17-1-b#required-workloads-and-components)
    - (Alternative) Visual Studio 2019, version 16.9 or later

## Information About Pull-Requests
I am accepting pull-requests to this repository, however if you decide to contribute make sure to stick to the following procedure:
- Clone the repository to your local machine
- Install the prerequisites
- Modify or create the code you wish to contribute to this repository
- Document each and every feature you change or add in full detail
- Make sure to test your features thoroughly, do not send buggy or untested code!

## Credits
- https://weather.gov/ -> For their API.
