# Covid19Radar (English)

*This Application is UNDER DEVELOPEMNT!!*
This app uses Bluetooth LE to get the contact logs of each other.  

iOS Builstatus [![iOS Build status](https://build.appcenter.ms/v0.1/apps/9c268337-4db9-4bf4-be09-efaf16672c15/branches/master/badge)](https://appcenter.ms)

Android Build status [![Android Build status](https://build.appcenter.ms/v0.1/apps/3dcdf5b5-da95-4d03-96a6-e6ed42de7e16/branches/master/badge)](https://appcenter.ms)

## Thank you for Your Contribute !!!
Currently, we are doing intensive sprints, mainly by core contributors.
The repository is synced from Azure DevOps to master on Github.
For this reason, tasks are centrally managed with AzureDevOps.
If you want to be a core contributor, talk to me on Discord.

Discord Channel
https://discord.gg/e5hMm4h

![App Description](img/AppDescription-en.jpg)

## How to install the app for tester

Please install the app for test from below link:(Sorry, currently available only Android)

https://install.appcenter.ms/orgs/Covid19Radar/apps/Covid19RadarAndroid/releases

Device configuration guide for tester:
https://docs.microsoft.com/ja-jp/appcenter/distribution/testers/testing-android

## Worked
- Screen transition and each screen design.
- Proximity communication between Android.

## Under development

- iOS iBeacon implementation , testing (I don't have a Mac)
- Add user (via REST API)
- Push Notification
- SErver Side  API (Azure Function and CosmosDB)
- Arrangement of conditions of dense contact
- Graph DB(Cosmos DB) side Query , manage site（Server Side)

## Development environment

This application uses Xamarin Forms (iOS and Android) with C # and Prism (MVVM IoC).

You can develop with Visual Studio for Windows or Visual Studio for Mac.

https://visualstudio.microsoft.com/ja/xamarin/

![App settings](img/design00-en.jpg)

Permission to use the following functions of the device is required. 

1. Bluetooth
2. Push Notification

After the setup is complete, the contact log between the people who have installed this app is automatically recorded.

# About the design

We use [Adobe XD](https://www.adobe.com/jp/products/xd.html) to create our designs.

![Full screen view](img/design01-en.jpg)

If you want to check your design files, install Adobe XD. (available for free).

Refer [DataModels and API](doc/domain-model.md) for more details about the spec. (Japanese only)


## App Prototypes

You can check the screen transition by accessing the following URL.

[Prototypes(English)](https://xd.adobe.com/view/37f0cf1d-ed5d-4328-5700-9c3f7c075307-41c1/?fullscreen)
Password：Covid19Radar

## Licensing
Covid19Radar is licensed under the GNU Affero General Public License v3.0. See
[LICENSE](./LICENSE) for the full license text.
