# Libra
The Libra project is inspired by the C64 game Libra - you can find material about it [here](http://www.gamebase64.com/game.php?h=0&id=4371) or [here](https://www.youtube.com/watch?v=8KfHnlf-Nso). It uses [MonoGame](http://www.monogame.net) and a fork of [Nez](https://github.com/KamiNeko/Nez) as underlying frameworks. 

## Development Requirements
* Windows (MonoGame / Nez supports multiple OS, but I only test on this one, so there is no guarantee it will work on Mac or Linux)
* MonoGame 3.5.1
* Visual Studio 2017
* OpenAL (download [here](https://www.openal.org/downloads/))
* [Aseprite](https://github.com/aseprite/aseprite), if you want to open / edit the sprite files

## Build
* Open the solution in Visual Studio and build it
* Your MonoGame installation should compile the assets of the LibraContent project. These are automatically copied into the output directory of LibraGame 
* If you get the nuget error message of *kamineko.nez* not found, try to activate the *include prerelease* option
* Copy the levels directory from the Assets directory to the output directory of LibraGame

## Playing
Navigate the spaceship through the level and try to avoid any collision with the environment and all obstacles. Move with the arrow keys on the keyboard or use a gamepad. You can shoot bullets with the space key or the X key on a gamepad. Furthermore, you can toggle fullscreen mode with the F key on the keyboard.

## Extending
You can manage the order of the levels in the levels.json file, this references further level files such as the level01.json. Take a look at the existing levels and [Level-Structure](https://github.com/KamiNeko/Libra/wiki/Level-Structure) as reference. You can toogle the level edit mode with the Q key, this deactivates the level restart and level switch logic.
