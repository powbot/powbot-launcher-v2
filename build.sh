rm -rf dist/
mkdir dist/

dotnet warp -r osx-x64 -o dist/PowBotLauncherOSX
dotnet warp -r win10-x64 -o dist/PowBotLauncher.exe
dotnet warp -r linux-x64 -o dist/PowBotLauncherLinux
