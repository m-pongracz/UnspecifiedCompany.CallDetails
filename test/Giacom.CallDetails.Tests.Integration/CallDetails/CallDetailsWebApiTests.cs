using FluentAssertions;
using Giacom.CallDetails.CsvGenerator;
using Giacom.CallDetails.WebApi.Client;
using Xunit.Abstractions;

namespace Giacom.CallDetails.Tests.Integration.CallDetails;

public class CallDetailsWebApiTests : IntegrationTestsBase
{
    public CallDetailsWebApiTests(TestingWebApplicationFactory factory, ITestOutputHelper outputHelper) : base(factory, outputHelper)
    {
    }

    [Fact]
    public async Task Upload_CheckResult()
    {
        // Arrange
        var rows = CallDetailsGenerator.GenerateRows(new GeneratorRequest(1)).ToArray();
        
        // Act
        await WebApiClient.UploadAsync(new FileParameter(CallDetailsGenerator.GenerateStream(rows)));

        // Assert
        var expectedRow = rows.Single();
        
        var result = await WebApiClient.CdrAsync(expectedRow.Reference);

        AssertCallDetailDto(result, expectedRow);
    }
    
    [Fact]
    public async Task Upload_Retry()
    {
        // Arrange
        var rows = CallDetailsGenerator.GenerateRows(new GeneratorRequest(10)).ToArray();
        await WebApiClient.UploadAsync(new FileParameter(CallDetailsGenerator.GenerateStream(rows.Take(3))));
        
        // Act -- upload the same three rows again
        var assertTask = () => WebApiClient.UploadAsync(new FileParameter(CallDetailsGenerator.GenerateStream(rows)));

        // Assert
        await assertTask.Should().NotThrowAsync("upload retry should be allowed in case of failure");
    }
    
    private static void AssertCallDetailDto(CallDetailRecordDto asserted, WebApi.Dtos.CallDetailRecordDto expected)
    {
        DateOnly.FromDateTime(asserted.CallDate.DateTime).Should().Be(DateOnly.FromDateTime(expected.CallDate));
        asserted.CallerId.Should().Be(expected.CallerId);
        asserted.Recipient.Should().Be(expected.Recipient);
        asserted.EndTime.Should().Be(expected.EndTime);
        asserted.Duration.Should().Be(expected.Duration);
        asserted.Cost.Should().BeApproximately((double)expected.Cost, 0.1); // TODO value from the client should be decimal, but converter must be added first
        asserted.Currency.Should().Be(expected.Currency);
        ((int)asserted.CallType).Should().Be((int)expected.CallType);
    }
}