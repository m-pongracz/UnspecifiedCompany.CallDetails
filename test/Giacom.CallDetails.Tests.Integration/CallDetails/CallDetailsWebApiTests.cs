using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Giacom.CallDetails.CsvGenerator;
using Giacom.CallDetails.WebApi.Client;
using Xunit.Abstractions;
using CallType = Giacom.CallDetails.Domain.CallDetails.CallType;

namespace Giacom.CallDetails.Tests.Integration.CallDetails;

public class CallDetailsWebApiTests : IntegrationTestsBase
{
    public CallDetailsWebApiTests(TestingWebApplicationFactory factory, ITestOutputHelper outputHelper) : base(factory,
        outputHelper)
    {
    }

    [Fact]
    public async Task Upload_CheckResult()
    {
        // Arrange
        var rows = CallDetailsGenerator.GenerateRows(1).ToArray();

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
        var rows = CallDetailsGenerator.GenerateRows(10).ToArray();
        await WebApiClient.UploadAsync(new FileParameter(CallDetailsGenerator.GenerateStream(rows.Take(3))));

        // Act -- upload the same three rows again
        var assertTask = () => WebApiClient.UploadAsync(new FileParameter(CallDetailsGenerator.GenerateStream(rows)));

        // Assert
        await assertTask.Should().NotThrowAsync("upload retry should be allowed in case of failure");
    }

    [Fact]
    public async Task GetCallDetailCountAndDuration_ByDateOnly()
    {
        // Arrange
        var rows = new[]
        {
            CallDetailsGenerator.GenerateRow(new(duration: 1, callDate: new DateTime(2000, 1, 1))),
            CallDetailsGenerator.GenerateRow(new(duration: 1, callDate: new DateTime(2000, 1, 5))),
            CallDetailsGenerator.GenerateRow(new(duration: 1, callDate: new DateTime(2000, 1, 6))),
            CallDetailsGenerator.GenerateRow(new(duration: 1, callDate: new DateTime(2000, 1, 7))),
            CallDetailsGenerator.GenerateRow(new(duration: 1, callDate: new DateTime(2000, 1, 30))),
        };

        await WebApiClient.UploadAsync(new FileParameter(CallDetailsGenerator.GenerateStream(rows)));

        // Act
        var result = await WebApiClient.CountAndDurationAsync(new DateTimeOffset(new DateTime(2000, 1, 5)),
            new DateTimeOffset(new DateTime(2000, 1, 7)));

        result.TotalDuration.Should()
            .Be(3, "that is the sum of durations of the three records that fit into our request period");
        result.Count.Should()
            .Be(3, "that is the count of records that fit into our request period");
    }
    
    [Fact]
    public async Task GetCallDetailCountAndDuration_ByCallType()
    {
        // Arrange
        var rows = new[]
        {
            CallDetailsGenerator.GenerateRow(new(duration: 1, callDate: new DateTime(2000, 1, 1), callType: CallType.International)),
            CallDetailsGenerator.GenerateRow(new(duration: 1, callDate: new DateTime(2000, 1, 1), callType: CallType.Domestic)),
            CallDetailsGenerator.GenerateRow(new(duration: 1, callDate: new DateTime(2000, 1, 1), callType: CallType.Domestic)),
        };

        await WebApiClient.UploadAsync(new FileParameter(CallDetailsGenerator.GenerateStream(rows)));

        // Act
        var result = await WebApiClient.CountAndDurationAsync(new DateTimeOffset(new DateTime(2000, 1, 1)),
            new DateTimeOffset(new DateTime(2000, 1, 2)), WebApi.Client.CallType._1);

        result.TotalDuration.Should().Be(2, "that is the sum of durations of domestic records");
        result.Count.Should().Be(2, "that is the count of domestic records");
    }

    
    [Fact]
    public async Task GetCallDetailCountAndDuration_PeriodValidation()
    {
        // Arrange
        var rows = CallDetailsGenerator.GenerateRows(10).ToArray();
        await WebApiClient.UploadAsync(new FileParameter(CallDetailsGenerator.GenerateStream(rows.Take(3))));

        // Act -- upload the same three rows again
        var assertTask = () => WebApiClient.CountAndDurationAsync(new DateTimeOffset(new DateTime(2000, 1, 1)),
            new DateTimeOffset(new DateTime(2000, 2, 1)));

        // Assert
        await assertTask.Should().ThrowAsync<ValidationException>("period is larger than 30 days"); // TODO status code should be asserted instead
    }   
    
    [Fact]
    public async Task GetCallDetailCountAndDuration_NegativePeriodValidation()
    {
        // Arrange
        var rows = CallDetailsGenerator.GenerateRows(10).ToArray();
        await WebApiClient.UploadAsync(new FileParameter(CallDetailsGenerator.GenerateStream(rows.Take(3))));

        // Act -- upload the same three rows again
        var assertTask = () => WebApiClient.CountAndDurationAsync(new DateTimeOffset(new DateTime(2000, 1, 2)),
            new DateTimeOffset(new DateTime(2000, 1, 1)));

        // Assert
        await assertTask.Should().ThrowAsync<ValidationException>("period is negative"); // TODO status code should be asserted instead
    }    
    
    [Fact]
    public async Task GetAllForCaller()
    {
        // Arrange
        const string callerId = "1";

        var callDate = new DateTime(2000, 1, 1);
        
        var rows = CallDetailsGenerator.GenerateRows(10, new GeneratorRequest(callerId: callerId, callDate: callDate))
            .Union(CallDetailsGenerator.GenerateRows(10, new GeneratorRequest(callerId: "2", callDate: callDate))).ToArray();
        
        await WebApiClient.UploadAsync(new FileParameter(CallDetailsGenerator.GenerateStream(rows)));

        // Act 1
        var page1 = await WebApiClient.CallerAsync(callerId, new DateTimeOffset(callDate), new DateTimeOffset(callDate), pageNumber: 1, pageSize: 7);

        // Assert 1
        page1.PageNumber.Should().Be(1);
        page1.PageSize.Should().Be(7);
        page1.Data.Should().HaveCount(7);
        page1.Data.Should().AllSatisfy(x => x.CallerId.Should().Be(callerId));
        
        // Act 1
        var page2 = await WebApiClient.CallerAsync(callerId, new DateTimeOffset(callDate), new DateTimeOffset(callDate), pageNumber: 2, pageSize: 7);

        // Assert 1
        page2.PageNumber.Should().Be(2);
        page2.PageSize.Should().Be(7);
        page2.Data.Should().HaveCount(3);
        page2.Data.Should().AllSatisfy(x => x.CallerId.Should().Be(callerId));
        page2.Data.Select(x => x.Reference).Should().NotIntersectWith(page1.Data.Select(x => x.Reference));
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