using QuickType;

var bf_auth_file = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.UserProfile), ".brewfather");
var auth = Convert.ToBase64String (File.ReadAllBytes (bf_auth_file));

using HttpClient client = new ();
client.DefaultRequestHeaders.Add ("Authorization", $"Basic {auth}");

Console.WriteLine (":beer:");
Console.WriteLine ("---");

var complete = await client.GetAsync ("https://api.brewfather.app/v1/batches?status=Conditioning&complete=True");
File.WriteAllText ("conditioning.json", await complete.Content.ReadAsStringAsync ());

// fermenting beers
Console.WriteLine ("Fermenting");

var result = await client.GetAsync ("https://api.brewfather.app/v1/batches?status=Fermenting&include=batchNo,name,_id,measuredOg");
// File.WriteAllText ("fermenting.json", await result.Content.ReadAsStringAsync ());

var fermenting = Fermenting.FromJson (await result.Content.ReadAsStringAsync ());
foreach (var brew in fermenting) {
	Console.WriteLine ($"#{brew.BatchNo} {brew.Recipe.Name} | href=https://web.brewfather.app/tabs/batches/batch/{brew.Id}");

	result = await client.GetAsync ($"https://api.brewfather.app/v1/batches/{brew.Id}/readings/last");
	// File.WriteAllText ("readings-last.json", await result.Content.ReadAsStringAsync ());

	LastReading? last_reading = LastReading.FromJson (await result.Content.ReadAsStringAsync ());
	// no readings yet ?
	if (last_reading is null)
		continue;

	var aa = (brew.MeasuredOg - 1.0f - (last_reading.Sg - 1.0d)) / (brew.MeasuredOg - 1.0f);
	var abv = (brew.MeasuredOg - last_reading.Sg) * 1.3125f;

	Console.WriteLine ($"  SG {last_reading.Sg:F3}  Temp {last_reading.Temp:F1}°C  Att {aa:P0}  ABV {abv:P1} | trim=false");
	Console.WriteLine ($"  Last update @ {DateTimeOffset.FromUnixTimeMilliseconds (last_reading.Time).ToLocalTime ()} | trim=false");
}

// conditioning beers
Console.WriteLine ("Conditioning");

result = await client.GetAsync ("https://api.brewfather.app/v1/batches?status=Conditioning&include=batchNo,name,_id,measuredAbv,notes");
//File.WriteAllText ("conditioning.json", await result.Content.ReadAsStringAsync ());

var conditioning = Conditioning.FromJson (await result.Content.ReadAsStringAsync ());
foreach (var brew in conditioning) {
	double days = 0d;
	foreach (var note in brew.Notes) {
		if (note.Type != "statusChanged")
			continue;
		if (note.Status != "Conditioning")
			continue;
		days = (DateTimeOffset.UtcNow - DateTimeOffset.FromUnixTimeMilliseconds (note.Timestamp)).TotalDays;
	}
	Console.WriteLine ($"#{brew.BatchNo} {brew.Recipe.Name} ABV {brew.MeasuredAbv:N1}% ({days:N0} days) | href=https://web.brewfather.app/tabs/batches/batch/{brew.Id}");
}
