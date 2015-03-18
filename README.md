# DesktopCleaner

#### What is it?
This is a tool which watch on the desktop and copies all files/folders to an configurated destination path.
You also can blacklist files and directories.
A blacklisted file supports a wildcard as a name. 

Exampls for blacklisted files:
```
text.txt
*.zip
```

#### Why?
Simple: A friend of me ask me for a tool like this and i liked his idea.
I also was looking for a project idea, so I could develop my first WPF application.

#### Architecture
I really wanted a clean and structured architecture, after i came from MvvmCross. 
So i tried to build my architecutrue similar like MvvmCroos, which means i wanted MVVM pattern,
a service layer, CDI and messaging.
In the following, you see the frameworks/libraries, which i'm using in this project:

* Db4o (as embedded database)
* log4net (as logger)
* GalaSof.MvvmLight (for MVVM pattern)
* Unity (as dependency injection container)
* WPF (for UI)

Here is a little sketch of the architecutre:
![architecutre sketch](https://github.com/wasteland540/DesktopCleaner/blob/master/architectureDiagramm.png)

If you have any suggestions feel free to contact me.

