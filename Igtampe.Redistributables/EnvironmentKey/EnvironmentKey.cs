namespace Igtampe.EnvironmentKey {

    /// <summary>Environment Key Utilities</summary>
    public static class EnvironmentKey {

        /// <summary>Attempts to get the key either from the environment, or from a text file with the name of the key. If it is not present, it will generate a GUID</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string? Get(string key) => Get(key,()=> new Guid().ToString());

        /// <summary>Attempts to get the key either from the environment, or from a text file with the name of the key. If it is not present, it will generate based on the provided generator function</summary>
        /// <param name="key"></param>
        /// <param name="generator"></param>
        /// <returns></returns>
        public static string? Get(string key, Func<string?> generator) {
            
            //Get it from the environment
            var val = Environment.GetEnvironmentVariable(key);

            //If we found it return that
            if (val != null) { return val; }

            //try to get it from the file
            if (File.Exists(key + ".txt")) {
                val = File.ReadAllText(key + ".txt");
                return val;
            }

            //If we're here, the file doesn't exist and we don't have a value.

            //Generate one with the generator
            val = generator();

            if (val == null) {  //If the value is STILL null
                return null; //we give up
            }

            Console.WriteLine($"{key} could not be found, so it was generated: '" + val + "'");
            Console.WriteLine("It was written to " + key + ".txt");

            //Write it to disk
            File.WriteAllText(key + ".txt", val);

            //And return it;
            return val;

        }

        /// <summary>Attempts to get the key either from the environment, or from a text file with the name of the key. If it is not present, it will return a blank string</summary>
        /// <returns></returns>
        public static string? UngeneratableGet(string key) => Get(key, () => null);
    }
}
