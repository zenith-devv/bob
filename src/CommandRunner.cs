using System.Diagnostics;

public static class CommandRunner
{
    public static int Run(string fileName, string arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        process.OutputDataReceived += (sender, e) => 
        {
            if (e.Data != null) Console.WriteLine(e.Data);
        };
        process.ErrorDataReceived += (sender, e) => 
        {
            if (e.Data != null) Console.WriteLine(e.Data); 
        };

        if (!process.Start()) return -1;
        
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();
        return process.ExitCode;
    }
}
