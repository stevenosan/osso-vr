using FluentAssertions;
using Moq;
using osso_vr_api;
using osso_vr_models;
using System.Text.Json;

namespace osso_vr_api_tests
{
    public class DataStoreTests
    {
        private Mock<IDataStore> _dataStoreMock;
        private DataStore _dataStore;

        [SetUp]
        public void Setup()
        {
            var result = new Result
            {
                TotalRuns = 10,
                CompletedRunsPercentage = 80,
                PassedRunsPercentage = 50,
                MedianCompletedRunTime = 30.5,
                Runs = new List<RunResult>
                {
                    new RunResult { Duration = 10 },
                    new RunResult { Duration = 20 },
                    new RunResult { Duration = 30 },
                    new RunResult { Duration = 40 },
                    new RunResult { Duration = 50 }
                }
            };

            string resultJson = JsonSerializer.Serialize(result);
            File.WriteAllText("c:\\data\\result.json", resultJson);

            _dataStore = new DataStore("c:\\data\\result.json");
        }

        [Test]
        public void GetInsights_SetsCompletedPercentage_PassedPercentage_MedianTime()
        {
            int count = 3;
            string order = "asc";

            var insights = _dataStore.GetInsights(count, order);

            insights.RunsCount.Should().Be(10);
            insights.CompletedRunsPercentage.Should().Be(80);
            insights.PassedRunsPercentage.Should().Be(50);
            insights.MedianTimeCompletedRuns.Should().Be(30.5);
        }

        [Test]
        public void GetInsights_ReturnsTopRunsInAscendingOrder()
        {
            int count = 3;
            string order = "asc";

            var insights = _dataStore.GetInsights(count, order);

            insights.TopRuns.Count.Should().Be(3);
            insights.TopRuns.Should().BeEquivalentTo(insights.TopRuns.OrderBy(r => r.Duration));
        }

        [Test]
        public void GetInsights_ReturnsTopRunsInDescendingOrder()
        {
            int count = 3;
            string order = "desc";

            var insights = _dataStore.GetInsights(count, order);

            insights.TopRuns.Count.Should().Be(3);
            insights.TopRuns.Should().BeEquivalentTo(insights.TopRuns.OrderByDescending(r => r.Duration));
        }
    }
}