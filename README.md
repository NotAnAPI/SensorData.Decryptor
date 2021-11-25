### CLI Usage
The application tries to extract the `sensor_data` from `sensor.txt` file located in the application root directory. To decrypt the `sensor_data`, 2 keys are required. The keys are extracted from the `bm_sz` cookie. The first one is used to encrypt and the second one is used to change the orders. You simply provide the `bm_sz` cookie and the application will extract the keys within it.

You can provide the keys too within the `sensor.txt` file in two ways so you will not ever need to execute application with arguments:
1. `sensor_data` at first line, then `bm_sz` cookie value as the second line! or even the 
2. `sensor_data` at first line, then `first encryption key` as the second line and `second encryption key` as the third line!

Use `--help` to get the usage.

###  Found an Issue?

If you find a bug in the source code or a mistake in the documentation, you can help by submitting an issue to the [GitHub Repository](https://github.com/NotAnAPI/SensorData.Decryptor "GitHub Repository"). Even better you can submit a Pull Request with a fix.

When submitting an issue please include the following information:
- A description of the issue
- The exception message and stacktrace if an error was thrown
- If possible, please include code that reproduces the issue. DropBox or GitHub's Gist can be used to share large code samples, or you could submit a pull request with the issue reproduced in a new test.


The more information you include about the issue, the more likely it is to be fixed!

###  Submitting a Pull Request

When submitting a pull request to the [GitHub Repository](https://github.com/NotAnAPI/SensorData.Decryptor "GitHub Repository") make sure to do the following:

- Check that new and updated code follows Decryptor's existing code formatting and naming standard

### Discord
[Join the Discord server](https://discord.com/invite/MkzEUqEAbD "Join the Discord server")
