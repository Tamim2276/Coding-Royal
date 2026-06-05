using System.Text.Json;
using ClashOfCodes.API.Data;
using ClashOfCodes.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClashOfCodes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class SubmitController : ControllerBase
{
    private readonly JudgeService _judgeService;
    private readonly AppDbContext _dbContext;

    public SubmitController(JudgeService judgeService, AppDbContext dbContext)
    {
        _judgeService = judgeService;
        _dbContext = dbContext;
    }

    [HttpPost]//[Route("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitRequest request)
    {
        //find the problem in the database
        var problem = await _dbContext.Problems.FindAsync(request.ProblemId);
        if (problem == null)
        {
            return NotFound("Problem not found");
        }
        //get the test cases for the problem
        string jsonDeserialize = problem.TestCasesJson;
        if (jsonDeserialize == null)
        {
            jsonDeserialize = "[]";
        }
        //deserialize the test cases to a list of test cases
        List<TestCase>? deserializedList = JsonSerializer.Deserialize<List<TestCase>>(jsonDeserialize);

        List<TestCase> testCases;
        if (deserializedList == null)
        {
            testCases = new List<TestCase>();
        }
        else
        {
            testCases = deserializedList;
        }
        if (testCases.Count == 0)
        {
            return BadRequest("No test cases found for this problem");
        }

        //run each test case through Judge0
        var results = new List<TestCaseResult>();
        foreach (var testCase in testCases)
        {
            var result = await _judgeService.RunTestCaseAsync(
                request.Code,
                request.Language,
                testCase.Input,
                testCase.ExpectedOutput
            );
            results.Add(result);
        }

        int passed = results.Count(r => r.Passed);
        int total = results.Count;

        // Calculate BE bonus
        // +1 BE for any successful compilation (at least one test ran without error)
        // +3 BE for flawless first submission (all tests passed)

        bool compiled = results.Any(r => string.IsNullOrEmpty(r.Error) || r.Error.Contains("Wrong Answer"));

        bool allPassed = passed == total;

        int beBonus = 0;
        if (compiled) beBonus += 1;
        if (allPassed) beBonus += 3;

        return Ok(new SubmitResponse
        {
            Passed = passed,
            Total = total,
            BeBonus = beBonus,
            AllPassed = allPassed,
            Results = results.Select((r, index) => new TestCaseResultDto
            {
                TestCaseNumber = index + 1,
                Passed = r.Passed,
                ActualOutput = r.ActualOutput,
                Error = r.Error,
                StatusDescription = r.StatusDescription
            }).ToList()
        });
    }
}

public class TestCaseResultDto
{
    public int TestCaseNumber { get; set; }
    public bool Passed { get; set; }
    public string ActualOutput { get; set; } = "";
    public string Error { get; set; } = "";
    public string StatusDescription { get; set; } = "";
}

public class SubmitResponse
{
    public int Passed { get; set; }
    public int Total { get; set; }
    public int BeBonus { get; set; }
    public bool AllPassed { get; set; }
    public List<TestCaseResultDto> Results { get; set; } = [];
}

public class TestCase
{
    public string Input { get; set; } = "";
    public string ExpectedOutput { get; set; } = "";

}

public class SubmitRequest
{
    public int ProblemId { get; set; }
    public string Code { get; set; } = "";
    public string Language { get; set; } = "csharp";
}