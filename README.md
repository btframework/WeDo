# Lego WeDo 2.0 Education Framework

This Framework expands [Bluetooth Framework](https://www.btframework.com/bluetoothframework.htm) with [Lego WeDo 2.0 Education](https://education.lego.com/en-us) support. Should you have any questions please contact us at support@btframework.com

## License Terms

**This project is free for education purpose only**. It can be used with free of charge for any education purposes: at home, in schools or in other education organizations.

All other terms and conditions except the usage for education are described in the [Wireless Communication Library EULA](https://www.btframework.com/eula.htm).

You **can not modify** the We Do Framework source code. You **can not distribute** the We Do Framework source code with your projects or by any other way. If your project was created during education process and you want to distribute it **you must distribute it with the We Do Framework assemblies** and **with links to this GitHub We Do Framework project page**. Your project **must be distributed as Open Source**.

For any other usage of the We Do Framework refer to the [Wireless Communication Library EULA](https://www.btframework.com/eula.htm).

##  System Requirements

### Common Requirements

- [Bluetooth Framework](https://www.btframework.com/bluetoothframework.htm) 7.18.2.0 or later
- Windows 7 and above (see remarks below)
- Bluetooth 4.0 (or high) hardware

### .NET Edition Requirements

- .NET Framework 4.0 or high
- Visual Studio 2019 or high
- [Bluetooth Framework .NET Edition](https://www.btframework.com/bluetoothframework.htm#download) (demo version is included)

### VCL Edition Requirements

- RAD Studio 10 Seattle
- [Bluetooth Framework VCL Edition](https://www.btframework.com/bluetoothframework.htm#download)

## Windows versions

### Windows 7, 8

To be able to run the Lego WeDo 2.0 Education Framework on Windows 7 you need to install [BlueSoleil](http://www.bluesoleil.com) Bluetooth drivers (version 10 and above required). Or use [BLED112](https://www.btframework.com/bled112.htm) Bluetooth dongle.

The **wclWeDoWatcher** is not supported on this platform with BlueSoleil drivers. Use **wclBluetoothManager** to discover (find) Lego WeDo 2.0 Hubs instead.

### Windows 8.1

On this Windows version the Framework can be used with Microsoft, [BlueSoleil](http://www.bluesoleil.com) Bluetooth drivers (version 10 and above required) or with [BLED112](https://www.btframework.com/bled112.htm) Bluetooth dongle. If yo use Microsoft Bluetooth drivers you must pair your Lego WeDo 2.0 Hub with Windows first with using **Add Bluetooth Device** dialog.

The **wclWeDoWatcher** is not supported on this platform with BlueSoleil and Microsoft Bluetooth drivers. Use **wclBluetoothManager** to discover (find) Lego WeDo 2.0 Hubs instead.

### Windows 10

On this Windows version the Framework can be used with Microsoft, [BlueSoleil](http://www.bluesoleil.com) Bluetooth drivers (version 10 and above required) or [BLED112](https://www.btframework.com/bled112.htm) Bluetooth dongle **Full features** of the Framework are supported on Windows 10 **with Microsoft Bluetooth drivers and with BLED112 Bluetooth dongle**.

With [BlueSoleil](http://www.bluesoleil.com) Bluetooth drivers (version 10 and above required). Bluetooth drivers the **wclWeDoWatcher** is not supported. Use **wclBluetoothManager** to discover (find) Lego WeDo 2.0 Hubs instead.
