# SharpInvaders
An open source Space Invaders clone written in C# with the Monogame framework.

[Watch a demo video on YouTube.](https://www.youtube.com/watch?v=u0PpvAS_lXM)

## Rationale
This is a sandbox for me to learn C# and the Monogame (formerly Microsoft XNA) framework. 

> If you can code Pong, you can code Space Invaders.
> If you can code Space Invaders, you can code anything. 


## Running and Building
### Dev

* `dotnet watch run`
* `dotnet watch run -p SharpInvaders`

### Build

* `dotnet build`

### Bundle

* [Packaging Games](https://docs.monogame.net/articles/packaging_games.html) on Monogame.net 
    * Mac
        * `dotnet publish -c Release -r osx-x64 /p:SingleFilePublish=true /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained`
    * Win
        * `dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained`


## Setup
Standard installation of `.Net Core`, `Monogame`, `Extended`
* See [Monogame.net Getting Started](https://docs.monogame.net/articles/getting_started/0_getting_started.html)
* See [Monogame.Extended Getting Started](https://www.monogameextended.net/docs/)


## IDE Tweaks
Started with Visual Studio but moved back to Visual Studio Code for comfort and less context switching with my day to day. There were some tricky parts to getting this environment set up.
* [C# Plugin](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
    * Note that this uses OmniSharp for code hinting and formatting
      This can sometimes lag behind and will need to be resarted if things feel wonky. `CMD Shift P` is your friend here.
* `.vscode/launch.json` manual configuration
    * Some things had to be updated by hand here to help VSC find the files for running from IDE and breakpoint debugging
    ```
        "name": ".NET Core Launch (console)",
        "type": "coreclr",
        "request": "launch",
        "preLaunchTask": "build",
        "program": "${workspaceFolder}/SharpInvaders/bin/Debug/netcoreapp3.1/SharpInvaders.dll",
    ```

* `.vscode/tasks.json` manual configuration
    * Some things had to be updated by hand here to help VSC find the files for running from IDE and breakpoint debugging
    ```
        "args": [
            "build",
            "${workspaceFolder}/SharpInvaders/SharpInvaders.csproj",
    ```


## Spritesheet Workflow
Using `TexturePackerLoader` to help keep my old Aseprite and TexturePacker workflow in place.
* See [TexturePackerLoader](https://github.com/CodeAndWeb/TexturePacker-MonoGame-Demo)
* This did not seem to work via `nuget` and I wound up manually creating a `TexturePackerLoader/` directory and copying over the requisite files:
    ```
        SpriteFrame.cs
        SpriteRenderer.cs
        SpriteSheet.cs
        SpriteSheetLoader.cs
    ```
* Sample code files were copied to `Utils/`
    ```
        Animation.cs
        AnimationManager.cs
    ```
* These have been used to create a more robust and specific to my needs `AnimatedEntity.cs` 

* Due to some issues with the `SpriteSheetLoader.cs` I'm bypassing the default behavior and workflow
  * Once a sheet is exported the `.png` file can be dropped into the `Content` directory and added to `Content.mgcb` manually
  ```
    #begin tpSpriteSheet.png
    /importer:TextureImporter
    /processor:TextureProcessor
    /processorParam:ColorKeyColor=255,0,255,255
    /processorParam:ColorKeyEnabled=True
    /processorParam:GenerateMipmaps=False
    /processorParam:PremultiplyAlpha=True
    /processorParam:ResizeToPowerOfTwo=False
    /processorParam:MakeSquare=False
    /processorParam:TextureFormat=Color
    /build:tpSpriteSheet.png
  ```
  * The `.txt` file is another thing altogether. I've created a `Constants.Assets.SPRITESHEET_DATA` and pulled the pertinent lines out as an array. This is what the `SpriteSheetLoader` ultimately wants from the text file. Will loop back another time make this more dynamic. The end results are something like:
  ```
    "EnemyEyes/idle/0;0;35;136;31;32;32;32;0;0",
    "EnemyEyes/idle/1;0;1;1;32;32;32;32;0;0",
    "EnemyEyes/idle/2;0;33;203;30;31;32;32;0;-0.03225806451612903",
  ```


### Using the Generated Spritesheeets

This may not be valid after all of the edits I made, will revisit.
* Single Sprite (not animated)
    ```
        tpSpriteRender.Draw(
            tpSpriteSheet.Sprite(
                tpSprites.EnemyEyes_idle1
            ),
            new Vector2(350, 530),
            Color.White
        );
    ```
