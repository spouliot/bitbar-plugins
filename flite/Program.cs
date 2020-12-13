using System;
using System.Net.Http;
using System.IO;
using System.Text.Json;
using IniParser;
using Flite;

var parser = new IniDataParser ();
var data = parser.Parse (new StreamReader (args [0]));
var client = new HttpClient ();
var json_options = new JsonSerializerOptions {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

Console.WriteLine (":beer: | dropdown = false");
foreach (var section in data.Sections) {
    var hostname = section.Name;
    Console.WriteLine ("---");
    try {
        var props = section.Properties;
        var streamTask = client.GetStreamAsync ($"http://{hostname}/getValuesBlack");
        var values = await JsonSerializer.DeserializeAsync<FliteValues> (await streamTask, json_options);
        Console.WriteLine ($"{props ["Name"]} @ {DateTime.Now} | color=white");
        Console.WriteLine ($"   Volume: {values.Level} {values.LevelUnits} Left | trim=false");
        Console.WriteLine ($"   Temperature: {values.Temperature} {values.TemperatureUnits} | trim=false");
        Console.WriteLine ($"   Pressure: {values.Pressure} {values.PressureUnits} | trim=false");
        Console.WriteLine ($"Configure '{hostname}'... | href=http://{hostname}/ trim=false");
    }
    catch {
        Console.WriteLine ($"{hostname}' is offline");
    }
}
return 0;