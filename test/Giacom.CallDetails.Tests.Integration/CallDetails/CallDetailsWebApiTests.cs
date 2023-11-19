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

    private static void AssertCallDetailDto(CallDetailRecordDto asserted, WebApi.Dtos.CallDetailRecordDto expected)
    {
        asserted.CallDate.Should().Be(expected.CallDate);
        asserted.CallerId.Should().Be(expected.CallerId);
        asserted.Recipient.Should().Be(expected.Recipient);
        asserted.EndTime.Should().Be(expected.EndTime);
        asserted.Duration.Should().Be(expected.Duration);
        asserted.Cost.Should().Be((double)expected.Cost);
        asserted.Currency.Should().Be(expected.Currency);
        ((int)asserted.CallType).Should().Be((int)expected.CallType);
    }
}