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
	var name = section.Name;
	var get_url = String.Empty;
	try {
		var props = section.Properties;
		get_url = props ["GetUrl"];
		if (String.IsNullOrEmpty (get_url))
			continue;
		Console.WriteLine ("---");
		var streamTask = client.GetStreamAsync (get_url);
		var values = await JsonSerializer.DeserializeAsync<FliteValues> (await streamTask, json_options);
		Console.WriteLine ($"{section.Name} @ {DateTime.Now} | color=white");
		Console.WriteLine ($"   Volume: {values.Level} {values.LevelUnits} Left | trim=false");
		Console.WriteLine ($"   Temperature: {values.Temperature} {values.TemperatureUnits} | trim=false");
		Console.WriteLine ($"   Pressure: {values.Pressure} {values.PressureUnits} | trim=false");
		var config_url = props ["ConfigUrl"];
		if (!String.IsNullOrEmpty (get_url))
			Console.WriteLine ($"Configure... | href={config_url} trim=false");
	}
	catch {
		if (String.IsNullOrEmpty (get_url)) {
			Console.WriteLine ($"No 'GetUrl=' found inside section {name} of the ini file");
		} else {
			var get_uri = new Uri (get_url);
			Console.WriteLine ($"'{get_uri.Host}' is offline");
		}
	}
}
return 0;