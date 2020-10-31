
Photon Unity Client SDK - Readme
Exit Games GmbH - www.photonengine.com - forum.photonengine.com



Documentation
----------------------------------------------------------------------------------------------------
    The API reference is included in this package as CHM file.
    Find the manuals, tutorials, API reference and more online.

    http://doc.photonengine.com



Running the Demos
----------------------------------------------------------------------------------------------------
    Our demos are built for the Photon Cloud for convenience.
    The service is free for development. Signing up is instant and without obligation.

    https://dashboard.photonengine.com

    Each application type has it's own AppId (e.g Realtime and Chat).
    This must be copied into the clients: Update the property "AppId" in the source files of the demos.

    You will find specific sections of the Dashboard per application type.


    Alternatively host the Photon Server yourself. The AppId is not needed in that case.
    https://www.photonengine.com/sdks#server

    How to start the server:
    https://doc.photonengine.com/en-us/onpremise/current/getting-started/photon-server-in-5min



Chat Documentation
----------------------------------------------------------------------------------------------------
    http://doc.photonengine.com/en/chat
    http://forum.photonengine.com



Implementing Photon Chat
----------------------------------------------------------------------------------------------------
    Photon Chat is separated from other Photon Applications, so it needs it's own AppId.
    Our demos usually don't have an AppId set.
    In code, find "<your appid>" to copy yours in. In Unity, we usually use a component
    to set the AppId via the Inspector. Look for the "Scripts" GameObject in the scenes.

    Register your Chat Application in the Dashboard:
    https://dashboard.photonengine.com

    The class ChatClient and the interface IChatClientListener wrap up most important parts
    of the Chat API. Please refer to their documentation on how to use this API.
    More documentation will follow.

    If you use Unity, copy the source from the ChatApi folder into your project.



Unity Notes
----------------------------------------------------------------------------------------------------
    If you don't use Unity, skip this chapter.


    We assume you're using Unity 2017.4.7 or newer.
    The demos are prepared to export for Standalone only.

    The SDK contains an "Assets" folder. To import Photon into your project, copy the content
    into your project's Asset folder. You may need to setup the DLLs in the Inspector to export
    for your platform(s).


    Currently supported export platforms are:
        Standalone (Windows, OSx and Linux)
        WebGL
        iOS
        Android
        Windows 10 UWP
        Consoles			(see: https://doc.photonengine.com/en-us/pun/current/consoles)

    About UWP: 
        .Net Runtime needs the assembly in the Metro folder (configured to export). 
        Il2CPP could use the assembly in netstandard2.0 folder, which does not export by default.
        Both include their own WebSocket implementation right in the dll. No external WebSocket 
        dlls are needed.

    All Unity projects must use ExitGames.Client.Photon.Hashtable!
    This provides compatibility across all exports.
    Add this to your code (at the beginning), to resolve the "ambiguous Hashtable" declaration:
    using Hashtable = ExitGames.Client.Photon.Hashtable;


    How to add Photon to your Unity project:
    1) The Unity SDK contains an "Assets" folder.
       Copy the content into your project's Assets folder.
    2) Make sure to have the following line of code in your scripts to make it run in background:
       Application.runInBackground = true; //without this Photon will loose connection if not focussed
    3) Add "using Hashtable = ExitGames.Client.Photon.Hashtable;" to your scripts. Without quotation.
    5) If you host a Photon Server change the server address in the client. "localhost:5055" won't work on device.
    6) Implement OnApplicationQuit() and disconnect the client when the app stops.
