@echo off
echo Uninstalling previous version
echo Appdata configs is save
call npm uninstall -g lanchat-npm
echo Downloading new version
call npm install -g lanchat-npm
echo Done
pause