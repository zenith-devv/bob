using static Logger;
using static Logger.MessageType;
using System.Diagnostics;

public class RustBuilder : IBuilder
{
    public string Name => "rust (cargo)";
    public bool CanHandle(string extension) => extension is ".rs" or ".toml";

    public void Build(ProjectConfig config)
    {
        Log(Default, $"{Name} project detected\n");
        Stopwatch timer = Stopwatch.StartNew();
        string extension = Path.GetExtension(config.MainFile).ToLower();
        string cmd;
        string args;
        string outPath = string.IsNullOrWhiteSpace(config.OutputFile) ? "target/release" : config.OutputFile;

        if (extension == ".toml")
        {
            cmd = "cargo";
            args = $"build {config.CompilerFlags}";
        }
        else
        {
            cmd = "rustc";
            string binaryName = string.IsNullOrWhiteSpace(config.OutputFile) ? "app.out" : config.OutputFile;
            args = $"{config.MainFile} -o {binaryName} {config.CompilerFlags}";
            Log(Warn, "single file build. for better performance and dependencies use Cargo.toml\n");
        }

        Log(Default, $"running \"{cmd} {args}\"\n");
        int result = CommandRunner.Run(cmd, args);
        timer.Stop();
        string elapsed = timer.Elapsed.TotalSeconds.ToString("0.0");

        if (result == 0)
            Log(Success, $"build finished successfully in {elapsed}s. output located in {outPath}\n");
        else
            Log(Err, $"project build failed. (exit code {result})\n");
    }
}