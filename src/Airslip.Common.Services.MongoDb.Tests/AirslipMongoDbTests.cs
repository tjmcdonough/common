using Airslip.Common.Repository.Types.Models;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Services.MongoDb.Tests
{
    public class AirslipMongoDbTests : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;

        public AirslipMongoDbTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
        
        // [Fact]
        // public async Task Can_query_all_data_using_search()
        // {
        //     List<MyEntity> results = await fixture.SearchContext
        //         .SearchEntities<MyEntity>(new List<SearchFilterModel>());
        //
        //     results.Should().NotBeNull();
        //     results.Should().NotBeEmpty();
        //     results.Count.Should().Be(4);
        // }
        //
        // [Fact]
        // public async Task Can_query_data_using_search()
        // {
        //     List<MyEntity> results = await fixture.SearchContext
        //         .SearchEntities<MyEntity>(new List<SearchFilterModel>
        //         {
        //             new(nameof(MyEntity.Name), "Some Name 1")
        //         });
        //
        //     results.Should().NotBeNull();
        //     results.Should().NotBeEmpty();
        //     results.Count.Should().Be(1);
        // }
    }
}