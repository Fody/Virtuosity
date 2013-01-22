using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Utilities;

public static class Verifier
{
    public static void Verify(string beforeAssemblyPath, string afterAssemblyPath)
    {
		var before = Validate(beforeAssemblyPath);
		var after = Validate(afterAssemblyPath);
        var trimmedBefore = TrimLineNumbers(before);
        var trimmedAfter = TrimLineNumbers(after);

        if (trimmedAfter != trimmedBefore)
        {
            var message = string.Format("Failed processing {0}\r\n{1}", Path.GetFileName(afterAssemblyPath), after);
            throw new Exception(message);
        }
    }

	public static string Validate(string assemblyPath2)
	{
		var exePath = GetPathToPeVerify();

		var process = Process.Start(new ProcessStartInfo(exePath, "\"" + assemblyPath2 + "\"")
			{
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true
			});

		process.WaitForExit(10000);
		return process.StandardOutput.ReadToEnd().Trim().Replace(assemblyPath2, "");
	}

    static string GetPathToPeVerify()
    {
        var pathToDotNetFrameworkSdk = ToolLocationHelper.GetPathToDotNetFrameworkSdk(TargetDotNetFrameworkVersion.Version40);
        return Path.Combine(pathToDotNetFrameworkSdk, @"bin\NETFX 4.0 Tools\peverify.exe");
    }

    static string TrimLineNumbers(string foo)
	{
		return Regex.Replace(foo, @"0x.*]", "");
	}
}