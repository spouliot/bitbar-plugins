namespace Flite {

	public class FliteControllerInfo {
		public string Id { get; set; }
		public string Version { get; set; }

		public override string ToString ()
		{
			return $"Flite Id {Id} version {Version}";
		}
	}

	public class FliteValues {
		public string Level { get; set; }
		public string LevelUnits { get; set; }
		public string Temperature { get; set; }
		public string TemperatureUnits { get; set; }
		public string Pressure { get; set; }
		public string PressureUnits { get; set; }

		public override string ToString ()
		{
			return $"Level {Level} {LevelUnits}, Temperature {Temperature} {TemperatureUnits}, Pressure {Pressure} {PressureUnits}.";
		}
	}
}
