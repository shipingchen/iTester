
csc /debug+ /target:library /out:object.dll object.cs

csc /debug+ /r:object.dll /r:System.Runtime.Remoting.dll ATMserver.cs

csc /debug+ /target:library /out:ATMclient.dll /r:com.csiro.xml.datatype.dll /r:Interfaces.dll /r:object.dll /r:ATMserver.exe /r:System.Runtime.Remoting.dll ATMclient.cs