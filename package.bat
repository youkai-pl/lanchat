@echo off
echo run
cd bin
del *.* /F /Q
cd ..
call pkg ./src/main.js --target win-x64 --output ./bin/lanchat-win64
call pkg ./src/main.js --target win-x86 --output ./bin/lanchat-win32
call pkg ./src/main.js --target linux-x86 --output ./bin/lanchat-linux32
call pkg ./src/main.js --target linux-x64 --output ./bin/lanchat-linux64
echo done