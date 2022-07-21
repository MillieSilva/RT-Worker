set name="Remote Teller Worker"
set location=%~dp0
sc create %name% binPath="%location%\Worker.exe"
sc config %name% obj="NT AUTHORITY\LocalService" start=auto
sc description %name% "Worker service for the Remote Teller application"
sc start %name%

pause
