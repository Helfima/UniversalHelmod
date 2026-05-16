set executable=D:\Perso\repos\UniversalHelmod\src\UniversalHelmod\Data\umodel_win32\umodel_64.exe
set pathPak=C:\Steam\steamapps\common\StarRupture\StarRupture\Content\Paks
set pathOut=c:\Temp\StarRupture
set filter=

set cmd=%executable%
set cmd=%cmd% -path="%pathPak%"
set cmd=%cmd% -out="%pathOut%"
set cmd=%cmd% -png
REM set cmd=%cmd% -export {filter}
REM set cmd=%cmd% -game=ue4.25

%cmd%

pause