# [Redis Console Manager](http://redisconsole.com "Redis Console Manager Home")
![RedisConsole](https://img1.wsimg.com/isteam/ip/c44d55e4-8322-4577-ba80-be3a1c589268/android-chrome-512x512.png/:/rs=h:200/qt=q:95)

A .net Core 5.0 console app that offers Redis utils through a console UI or as a cross platform desktop app implemented on the ElectronNet framework.
RedisConsole is a console or desktop application intended to make it easier to manage and debug Redis instances and works on all terminals including monochrome terminals, as well as modern color terminals with mouse support.

> The project is in active development but main functionality is already implemented for most data types.

# Project URL
[Redis Console web site](https://redisconsole.com)

# Ready to download releases
[Take me to the downloads](https://redisconsole.com/#1d5d717a-fde5-4c23-b2c7-d562d20a9408)

## Projects is the solution
* ConsoleUI (class library)
  * [Terminal.GUI](https://github.com/migueldeicaza/gui.cs) implementation of the console UI
* Redis.Core (class library)
  * Redis abstraction layer
* RedisConsole (.net Core console app)
  * [Terminal.GUI](https://github.com/migueldeicaza/gui.cs) entrypoint for the console app
* RedisConsoleDesktop (asp.net Core web app)
  * [ElectronNet](https://github.com/ElectronNET/Electron.NET) implementation (can run also as a asp.net Core app, environment directives are used to implement support for output specific functionality)

## How to set up the [ElectronNet](https://github.com/ElectronNET/Electron.NET) environment
In order to run the electron command you need to have node and npm installed and configured correctly then run.
```
npm i -g electron
```

https://www.grapecity.com/blogs/building-cross-platform-desktop-apps-with-electron-dot-net
Install the Command Line Tool - install a .NET Core global tool that implements a command named electronize
```
dotnet tool install ElectronNET.CLI -g  
```

To see a list of tools/commands installed on your system
```
dotnet tool list -g
```

#### Run the Electronized Application
Windows/Linux
```
electronize init  
electronize start
```

MAC OS
```
~/.dotnet/tools/electronize start
```

Electron.NET supports a watch mode where it will monitor your changes and automatically rebuild and relaunch your application. To invoke the watch mode, run the following command:
```
 electronize start /watch  
```

The following are dependencies related to the Kendo UI controls used for the desktop ElectroNet app
```
npm install jquery --save
npm install --save @progress/kendo-ui@latest
npm install --save @progress/kendo-ui
```

Packager
-------------
https://www.christianengvall.se/electron-packager-tutorial/#windows
```
npm install electron-packager -g
npm install --save-dev electron
```


### Creeating a release
```
dotnet electronize build /target win /electron-params "--icon=path/to/your/icon.ico MyNewAppName"

electronize build /target win
electronize build /target osx
electronize build /target linux
```

To trust the certificate run 
```
dotnet dev-certs https --trust (Windows and macOS only).
```
Learn about HTTPS: https://aka.ms/dotnet-https


### Linux folder permissions
```
chmod -R 1744 foldername
chmod 777 ./foldername
chmod 777 ./appname
```

### Examples
#### Build for macOS, Windows and Linux
```
electron-builder -mwl
```

#### Build deb and tar.xz for Linux
```
electron-builder --linux deb tar.xz       
```

#### Build for Windows ia32
```
electron-builder --win --ia32             
```

#### Build systax
```
electron-builder -c.extraMetadata.foo=bar	set package.json property `foo` to `bar`
electron-builder                          configure unicode options for NSIS
```

### Where is the zshrc file on MacOs ?
https://superuser.com/questions/886132/where-is-the-zshrc-file-on-mac
```
nano ~/.zshrc
```

## Screenshots
![Linux](https://media-exp3.licdn.com/dms/image/C4E12AQHtyP2Cj02-MA/article-inline_image-shrink_1000_1488/0/1589279908317?e=1632355200&v=beta&t=KtMeDLzCHCMSylrVliC4YBzaIJDU5JBQ8v27jgXkE4Y)

![Windows](https://img1.wsimg.com/isteam/ip/c44d55e4-8322-4577-ba80-be3a1c589268/RC03-0001.PNG/:/cr=t:0%25,l:0%25,w:100%25,h:100%25/rs=w:1240,h:620,cg:true)

![MacOS](https://img1.wsimg.com/isteam/ip/c44d55e4-8322-4577-ba80-be3a1c589268/OSX2-0001.PNG/:/cr=t:0%25,l:0%25,w:100%25,h:100%25/rs=w:600,h:300,cg:true)

## Authors

* **Christos Christodoulidis** - *Initial work* - [wcmCloud](https://github.com/wcmCloud)
