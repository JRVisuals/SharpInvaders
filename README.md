# SharpInvaders
An open source Space Invaders clone written in C# with the Monogame framework.


## Rationale
This is a sandbox for me to learn C# and the Monogame (formerly Microsoft XNA) framework. 

> If you can code Pong, you can code Space Invaders.
> If you can code Space Invaders, you can code anything. 


## Running and Building
### Dev

* `dotnet watch run`

### Build

* `dotnet build`

### Bundle

* TBD


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
* These have been used to create a more robust and specific to my needs `AnimatedSprite.cs` 


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