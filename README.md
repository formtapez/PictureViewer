# PictureViewer (Working title: Peakture)
Peakture is the worlds fastest picture viewer. The secret to this speed is its sensational background pre-caching technology.


### Usage
Peakture takes one command-line argument (absolute or relative path to an image or directory). Integration in the operating systems image file associations ("Open with" context menu) is highly recommended.
It will display the image and pre-cache other images in that directory. When executed without argument, an FileOpen dialog will be shown.
Example:
```sh
Peakture.exe "D:\Pictures\Cat.png"
```

### Key mapping

Peakture can be controlled with the following keys:

| Function | Key(s) |
| ------ | ------ |
| Open file dialog | O |
| Toggle fullscreen | F |
| Quit | Escape, Enter |
| Next Pic | PageDown, Space, Down, Right, LeftMouseButton, MouseWheelDown |
| Prev Pic | PageUp, Up, Left, RightMouseButton, MouseWheelUp |
| First Pic | Home |
| Last Pic | End |
| Zoom in | + |
| Zoom out | - |

In zoomed mode, some keys change its behaviour:

| Function | Key(s) |
| ------ | ------ |
| Zoom in | MouseWheelUp |
| Zoom out | MouseWheelDown |
| Move image section | Up, Down, Left, Right, LeftMouseButton+Drag |

### Supported file formats
* JPEG
* PNG
* GIF
* TIFF
* BMP

The binary was compiled with **Visual Studio Community 2015** using **.NET Framework 4.6**.