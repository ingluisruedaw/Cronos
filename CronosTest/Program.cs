// See https://aka.ms/new-console-template for more information
using Cronos.LogExtension;

Log l = new Log("Program", "Client", "CronosTest");

l.Create(new Exception("Prueba"),"Demo");



Console.WriteLine("writed log");
Console.ReadKey();