@echo off
@rem ===============================================================
@rem This batch file is to set environment vairobles for the project
@rem
@rem Usage: setenv
@rem 
@rem ===============================================================

@rem
echo Reset path and classpath
@rem
set PATH=.
set CLASSPATH=.

@rem
echo Set Java
@rem
set JAVA_HOME=C:\app\Java\jdk1.7.0_71
set PATH=%PATH%;%JAVA_HOME%\bin

@rem
echo Set Ant
@rem
set ANT_HOME=C:\app\apache-ant\apache-ant-1.9.4
set PATH=%PATH%;%ANT_HOME%\bin

@rem
echo Set home
@rem 
set WORK_HOME=C:\Test\iTester\Java
set classpath=%CLASSPATH%;%WORK_HOME%\build\classes



