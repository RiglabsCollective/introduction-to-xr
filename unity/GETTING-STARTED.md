<p>
  <img src="logos/unity.png" alt="Unity" width="72">
  &nbsp;
  <img src="logos/unity-hub.png" alt="Unity Hub" width="72">
  &nbsp;
  <img src="logos/vscode.png" alt="Visual Studio Code" width="72">
</p>

# Getting started with Unity

Do this once, before the first Unity session. It takes about **60 to 90 minutes**, mostly waiting for downloads, so start early on a good internet connection.

By the end you will have:

- [ ] a free **Unity account**
- [ ] **Unity Hub** with a free **Personal licence**
- [ ] **Unity 6** installed, with the Android module
- [ ] **Visual Studio Code** set up as your code editor
- [ ] (optional) a **phone** ready for testing: Android or iPhone

You can do every project on your computer without a phone. The phone steps are for anyone who wants to see the AR on a real device.

## What you need

| Item | Details |
|------|---------|
| Computer | Windows 10 or 11 (64-bit), or a recent Mac. 16 GB of memory is best, 8 GB is the least |
| Free disk space | About 25 GB for Unity and its modules |
| Internet | A fast connection. The downloads are several gigabytes |
| Email address | For your Unity account |
| Phone (optional) | An Android phone that supports ARCore, or an iPhone 6s or newer |

> Check the system requirements on Unity's website before you start, since they change between versions.

## Step 1 – Create your Unity account

1. Go to **id.unity.com** and choose **Create a Unity ID**.
2. Fill in your email, a password and your name, and accept the terms.
3. Open the email from Unity and click the link to **verify** your address.

Keep your login handy. You will use it in Unity Hub.

## Step 2 – Install Unity Hub and get a free licence

Unity Hub is the launcher. It installs the Editor and keeps your projects in one place.

1. Go to **unity.com/download** and download **Unity Hub** for Windows or Mac.
2. Run the installer and open Unity Hub.
3. Sign in with your Unity ID.
4. Hub asks you to add a licence. Choose **Unity Personal** (the free one) and accept the terms.

If Hub doesn't ask, open your account menu in the top corner, choose **Manage licenses**, then **Add**, and pick **Get a free personal license**.

## Step 3 – Install the Unity Editor

These guides were made with **Unity 6000.6.4f1**. Use that version if you can. If it is not offered, ask your trainer which Unity 6 version to use, because the menus can move a little between versions.

1. In Unity Hub, open **Installs** and choose **Install Editor**.
2. Pick **Unity 6000.6.4f1**. If it isn't in the list, open the **Archive** tab or go to the Unity download archive, find the version, and choose **Install**.
3. When Hub asks about modules, tick the ones for your computer and phone:

| Module | Who needs it |
|--------|--------------|
| **Android Build Support**, with **OpenJDK** and **Android SDK & NDK Tools** | Anyone who wants to test on an Android phone |
| **iOS Build Support** | **Mac only**, for anyone who wants to test on an iPhone |
| **Documentation** | Optional |

4. Click **Install** and wait. It is a large download.

You can add modules later: in **Installs**, click the gear beside your version and choose **Add modules**.

## Step 4 – Install Visual Studio Code

We use Visual Studio Code (VS Code) to read and edit any scripts you add later. It is free.

1. Go to **code.visualstudio.com** and download it for your system. Install it and open it.
2. Open the **Extensions** panel (the four-squares icon on the left, or press **Ctrl+Shift+X**, **Cmd+Shift+X** on a Mac).
3. Search for **Unity** (the one published by **Microsoft**) and click **Install**. It also adds the **C# Dev Kit** and the **C#** support it needs. VS Code may ask you to sign in with a Microsoft or GitHub account for C# Dev Kit. This is free.

Now tell Unity to use VS Code. You will do this inside Unity, so keep it for after Step 5:

1. In Unity, open **Edit > Preferences** (on a Mac, **Unity > Settings**).
2. Choose **External Tools**.
3. Set **External Script Editor** to **Visual Studio Code**.

From now on, double-clicking a script in Unity opens it in VS Code.

## Step 5 – Check that it all works

1. In Unity Hub, open **Projects** and click **New project**.
2. Choose the **Universal 3D** template, name the project `Test`, and choose a folder for it. Click **Create project**.
3. Wait for the Editor to open. The first start takes a few minutes.
4. Press the **Play** button at the top. You should see an empty scene. Press Play again to stop.
5. Do the External Tools step above, then close Unity.

If the Editor opens and Play works, Unity is ready. You can delete the `Test` project.

## Step 6 (optional) – Get a phone ready

Skip this if you will only use the computer.

### Android phone

Works on Windows and on a Mac.

1. **Check your phone supports AR.** Look for it on the **ARCore supported devices** list (search for it on Google's developer site). Most Android phones from the last few years do.
2. Make sure the **Google Play Store** is on the phone. Apps that use AR will ask to install **Google Play Services for AR** from there. This is free.
3. **Turn on Developer options.** Open **Settings > About phone** and tap **Build number** seven times.
4. Open **Settings > System > Developer options** and switch on **USB debugging**.
5. Connect the phone to the computer with a **USB cable that carries data** (some charging cables don't). Choose **File transfer** on the phone if it asks.
6. When the phone shows **Allow USB debugging?**, tick **Always allow** and tap **Allow**.

On Windows, if the computer does not recognise the phone, install your phone maker's **USB driver**, or Google's **USB driver** for Pixel phones.

### iPhone

You need a **Mac**. Unity cannot build for iPhone on Windows.

1. Use an **iPhone 6s or newer** on a recent version of iOS.
2. On the Mac, install **Xcode** from the **Mac App Store**. It is a very large download. Open it once and accept the licence.
3. In Xcode, open **Settings > Accounts**, click **+** and add your **Apple ID**. A free Apple ID is enough for testing on your own phone.
4. On the iPhone, switch on **Developer Mode**: **Settings > Privacy & Security > Developer Mode**. The phone restarts.
5. Connect the iPhone to the Mac with a cable and tap **Trust This Computer** on the phone.
6. After your first build, trust yourself as a developer on the phone: **Settings > General > VPN & Device Management**, choose your Apple ID, and tap **Trust**.

With a free Apple ID, an app you install on your phone stops working after **7 days**. Just build it again.

### Which phone steps do I need?

| I have | Do this |
|--------|---------|
| Windows and an Android phone | The Android steps |
| A Mac and an Android phone | The Android steps |
| A Mac and an iPhone | The iPhone steps |
| Windows and an iPhone | Use the computer (Editor) for testing. You can borrow a Mac for the iPhone steps |

## Problems?

| Problem | Fix |
|---------|-----|
| The download is very slow or stops | Pause and resume it in Hub, or try again later |
| Hub says my licence is missing | Account menu > **Manage licenses** > **Add** > **Get a free personal license** |
| The version I want isn't listed | Open the **Archive** tab in the Install Editor window, or use the download archive on Unity's site |
| Modules didn't install | **Installs** > gear beside the version > **Add modules** |
| Unity won't open scripts in VS Code | **Edit > Preferences > External Tools** and set the External Script Editor again |
| My Android phone isn't found | Try another USB cable, choose **File transfer** on the phone, and check **USB debugging** is on |

## Next

Open the [Unity projects](README.md) and start with the first one.
