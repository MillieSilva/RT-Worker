set name="Remote Teller Worker"
set location=%cd%
sc create %name% binPath="%location%\Worker.exe"
sc config %name% start=auto
sc start %name%
