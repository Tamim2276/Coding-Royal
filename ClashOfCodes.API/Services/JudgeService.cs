namespace ClashOfCodes.API.Services;

// Piston API — free, no account, no API key, no card needed
// Public endpoint: https://emkc.org/api/v2/piston
// Docs: https://github.com/engineer-man/piston
public class JudgeService
{
    private readonly HttpClient _httpClient;

    public JudgeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// A dictionary mapping our language names to Piston's language name and version.
    /// Piston uses language name + version to identify the runtime.
    private static readonly Dictionary<string, (string language, string version)>
        LanguageMap = new()
        {
            { "csharp",     ("csharp",     "6.12.0")  },
            { "python",     ("python",     "3.10.0")  },
            { "java",       ("java",       "15.0.2")  },
            { "cpp",        ("c++",        "10.2.0")  },
            { "javascript", ("javascript", "18.15.0") }
        };

    /// Submits code to the Piston API for execution and returns the result.
    /// Unlike Judge0, Piston is synchronous — it runs the code and returns the
    /// result immediately in a single request, so no polling is needed.
    public async Task<TestCaseResult> RunTestCaseAsync(
        string code, string language, string input, string expectedOutput)
    {
        // Check if the language is supported
        if (!LanguageMap.TryGetValue(language.ToLower(), out var lang))
        {
            return new TestCaseResult
            {
                Passed = false,
                Error = $"Unsupported language: {language}"
            };
        }

        // Build the Piston request body
        // files: the source code files to run (we only need one file)
        // stdin: the input to pass to the program (same as test case input)
        var request = new
        {
            language = lang.language,
            version = lang.version,
            files = new[] { new { content = code } },
            stdin = input
        };

        // Send the request to Piston — no API key headers needed
        var response = await _httpClient.PostAsJsonAsync("execute", request);

        if (!response.IsSuccessStatusCode)
        {
            return new TestCaseResult
            {
                Passed = false,
                Error = $"Piston API returned an error: {response.StatusCode}"
            };
        }

        // Deserialize the Piston response
        var result = await response.Content
            .ReadFromJsonAsync<PistonResponse>();

        if (result == null)
        {
            return new TestCaseResult
            {
                Passed = false,
                Error = "No response received from Piston"
            };
        }

        // Check for compile errors — if the code failed to compile,
        // Stderr on the Compile output will contain the error message
        if (!string.IsNullOrEmpty(result.Compile?.Stderr))
        {
            return new TestCaseResult
            {
                Passed = false,
                Error = "Compile error: " + result.Compile.Stderr
            };
        }

        // Get the actual program output and trim whitespace for comparison
        // Piston puts the program's printed output in Run.Stdout
        var actualOutput = result.Run?.Stdout?.Trim() ?? "";
        var expected = expectedOutput.Trim();

        // Compare actual output with expected output to determine pass/fail
        var passed = actualOutput == expected;

        return new TestCaseResult
        {
            Passed = passed,
            ActualOutput = actualOutput,
            // If the run failed, Stderr contains the runtime error message
            Error = passed ? "" : (result.Run?.Stderr ?? ""),
            StatusDescription = passed ? "Accepted" : "Wrong Answer"
        };
    }
}

/// Represents the result of a single test case execution.
/// Passed: whether the output matched the expected output.
/// ActualOutput: what the program actually printed.
/// Error: compile error or runtime error message if it failed.
/// StatusDescription: human-readable verdict (Accepted / Wrong Answer).
public class TestCaseResult
{
    public bool Passed { get; set; }
    public string ActualOutput { get; set; } = "";
    public string Error { get; set; } = "";
    public string StatusDescription { get; set; } = "";
}

/// Represents the full response from the Piston API.
/// Compile: present only for compiled languages (C#, Java, C++).
///          Contains compile errors if compilation failed.
/// Run: always present. Contains the program's stdout and stderr.
file class PistonResponse
{
    public PistonOutput? Compile { get; set; }
    public PistonOutput? Run { get; set; }
}

/// Represents one stage of the Piston execution (compile or run).
/// Stdout: what the program printed to standard output.
/// Stderr: any error output (compile errors or runtime exceptions).
file class PistonOutput
{
    public string? Stdout { get; set; }
    public string? Stderr { get; set; }
}