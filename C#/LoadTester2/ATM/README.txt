As this package is not incorporated on the Visual Studio project tree, 
instructions are as follows:

BUILD/CLEAN INSTRUCTIONS:
These two batch files have been written to build and clean the application respectively:
- build.bat
- clean.bat

build.bat
 - Will build 3 targets - ATMServer.exe to be run as a local server
			- ATMClient.dll assembly behaving as the client to this server
			- object.dll a supporting library
clean.bat
 - will remove all the 3 targets above in addition to it respective program 
debug files (.pdb) 


RUN INSTRUCTIONS:
To be run in conjunction with the testing framework, the 3 targets need to be built then 
ATMClient.dll and object.dll will need to be manually transferred into the 
"<direc>/LoadTester2/LoadTester/bin/Debug" folder, where <direc> is the name of the 
directory unzipped to. 

While runnning ATMServer.exe in a separate terminal window, edit config.xml in the 
"<direc>/LoadTester2/LoadTester/bin/Debug" folder to accept ATMClient.dll as the test 
assembly. LoadTester.exe -f config.xml to run.