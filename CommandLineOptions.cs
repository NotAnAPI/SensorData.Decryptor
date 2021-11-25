using CommandLine;

namespace NotAnAPI.SensorData.Decryptor
{
    public class CommandLineOptions
    {
        [Option('c', "bm_sz", Required = false,
            HelpText = "The bm_sz cookie value, used to extract sensor_data encryption keys, if any.", Default = "")]
        public string BmSz { get; set; } = "";

        [Value(index: 1, Required = false, HelpText = "First encryption key", Default = 8888888)]
        public int FirstEncryptionKey { get; set; }

        [Value(index: 2, Required = false, HelpText = "Second encryption key", Default = 7777777)]
        public int SecondEncryptionKey { get; set; }
    }
}
