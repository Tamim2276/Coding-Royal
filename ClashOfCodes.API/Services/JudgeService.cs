namespace ClashOfCodes.API.Services;

public class JudgeService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    public JudgeService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    /// A dictionary mapping programming language names to their corresponding IDs used by the Judge0 API.
    public static readonly Dictionary<string, int> LanguageIds = new Dictionary<string, int>()
    {
        {"csharp",51},
        {"python",71},
        {"java",62},
        {"javascript",63},
        {"cpp",54},
    };


    /// Submits code to the Judge0 API for execution and returns the result.
    public async Task<TestCaseResult> RunTestCaseAsync(string code, string language, string input, string expectedOutput)
    {
        if (!LanguageIds.TryGetValue(language.ToLower(), out int languageId))
        {
            return new TestCaseResult
            {
                Passed = false,
                ActualOutput = "",
                Error = $"Unsupported language: {language}"
            };
        }
        var submission = new
        {
            source_code = code,
            language_id = languageId,
            stdin = input,
            expected_output = expectedOutput
        };

        var submitRequest = new HttpRequestMessage(HttpMethod.Post, "/submissions?base64_encoded=false&wait=false"); // Adjust the endpoint as needed

        submitRequest.Headers.Add("X-RapidAPI-Key", _configuration["Judge0:ApiKey"]); // Add the API key header
        submitRequest.Headers.Add("X-RapidAPI-Host", _configuration["Judge0:ApiHost"]);// Add the API host header

        var submitResponse = await _httpClient.SendAsync(submitRequest); // Send the submission request to Judge0

        if (!submitResponse.IsSuccessStatusCode)
        {
            return new TestCaseResult
            {
                Passed = false,
                Error = "Failed to submit code to Judge0"
            };
        }

        var submitResult = await submitResponse.Content.ReadFromJsonAsync<SubmissionToken>();

        if (submitResult?.Token == null)
        {
            return new TestCaseResult
            {
                Passed = false,
                Error = "No token received"
            };
        }

        return await PollResultAsync(submitResult.Token);
    }

    /// Polls the Judge0 API for the result of a code submission using the provided token.
    private async Task<TestCaseResult> PollResultAsync(string token)
    {
        for (int attempt = 0; attempt < 10; attempt++)
        {
            await Task.Delay(1000); // Wait for 10 seconds before polling

            var pollRequest = new HttpRequestMessage(HttpMethod.Get, $"/submissions/{token}?base64_encoded=false"); // Adjust the endpoint as needed

            pollRequest.Headers.Add("X-RapidAPI-Key", _configuration["Judge0:ApiKey"]); // Add the API key header
            pollRequest.Headers.Add("X-RapidAPI-Host", _configuration["Judge0:ApiHost"]);// Add the API host header

            var pollResponse = await _httpClient.SendAsync(pollRequest); // Send the polling request to Judge0

            var pollResult = await pollResponse.Content.ReadFromJsonAsync<Judge0Result>(); // Deserialize the response into a Judge0Result object

            if (pollResult == null) continue;

            // Check the status of the result and return the appropriate TestCaseResult
            // Status 1 = In Queue, 2 = Processing — keep waiting
            if (pollResult.Status?.Id <= 2) continue;
            // Status 3 = Accepted (Passed), other status codes indicate failure
            return new TestCaseResult
            {
                Passed = pollResult.Status?.Id == 3,
                ActualOutput = pollResult.Stdout?.Trim() ?? "",
                Error = pollResult.Stderr ?? pollResult.CompileOutput ?? "",
                StatusDescription = pollResult.Status?.Description ?? ""
            };
        }
        return new TestCaseResult
        {
            Passed = false,
            Error = "Timed out waiting for Judge0 result"
        };
    }
}

/// Represents the result of a test case execution, including whether it passed, the actual output, any errors, and a status description.
public class TestCaseResult
{
    public bool Passed { get; set; }
    public string ActualOutput { get; set; } = "";
    public string Error { get; set; } = "";
    public string StatusDescription { get; set; } = "";
}
// Represents the response from the Judge0 API for a code submission, including standard output, standard error, compile output, and status information.
internal class Judge0Result
{
    public string? Stdout { get; set; }
    public string? Stderr { get; set; }
    public string? CompileOutput { get; set; }
    public Judge0Status? Status { get; set; }
}
// Represents the status of a code submission in the Judge0 API, including an ID and a description.
public class Judge0Status
{
    public int Id { get; set; }
    public string? Description { get; set; }
}
// Represents the response from the Judge0 API when a code submission is made, containing a token that can be used to poll for results.
internal class SubmissionToken
{
    public string? Token { get; set; }
}