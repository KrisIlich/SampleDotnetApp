using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);     

var salesFiles = FindFiles(storesDirectory);
var salesTotal = CalculateSalesTotal(salesFiles); 

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");


IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles){
    double salesTotal = 0;

    //read files loop
    foreach (var file in salesFiles)
    {
        //read contents of sales file
        string salesJson = File.ReadAllText(file);

        //Parse the contents as Json
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // add the amount found in the Total field to the salesTotal
        salesTotal += data?.Total ?? 0;
    }
    return salesTotal;
}

record SalesData (double Total);

/* Notes 

This solution uses the Newtonsoft.Json library to parse the JSON files. 

The sales files are found using the FindFiles method, 
and the salesTotal is calculated using the CalculateSalesTotal method. 

The results are then written to a file named "totals.txt" in the salesTotalDir directory.

The SalesData record is used to define the structure of the JSON data.

The FindFiles method uses Directory.EnumerateFiles to find all JSON files in the specified directory and its subdirectories.


Directory.EnumerateDirectories and Directory.EnumerateFiles accept a parameter 
that allows you to secify a search condition for names to return and a parameter
to recursively traverse all children directories

*/