using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Airslip.Common.Services.MongoDb
{
    public static class MongoDbExtensions
    {
        public static async Task<(IReadOnlyList<TDocument> data, int totalCount)> AggregateByPage<TDocument>(
            this IMongoCollection<TDocument> collection,
            FilterDefinition<TDocument> filterDefinition,
            SortDefinition<TDocument> sortDefinition,
            int offset,
            int take,
            CancellationToken cancellationToken)
        {
            const string CountFacetName = "count";
            const string DataFacetName = "data";

            AggregateFacet<TDocument, AggregateCountResult>? countFacet = AggregateFacet.Create(CountFacetName,
                PipelineDefinition<TDocument, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<TDocument>()
                }));

            AggregateFacet<TDocument, TDocument>? dataFacet = AggregateFacet.Create(DataFacetName,
                PipelineDefinition<TDocument, TDocument>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(sortDefinition),
                    PipelineStageDefinitionBuilder.Skip<TDocument>(offset),
                    PipelineStageDefinitionBuilder.Limit<TDocument>(take)
                }));

            List<AggregateFacetResults>? aggregation = await collection.Aggregate()
                .Match(filterDefinition)
                .Facet(countFacet, dataFacet)
                .ToListAsync(cancellationToken);

            AggregateCountResult? aggregateCountResult = aggregation.First()
                .Facets.First(x => x.Name == CountFacetName)
                .Output<AggregateCountResult>()
                .FirstOrDefault();

            if (aggregateCountResult == null)
                return (new List<TDocument>(0), 0);

            long count = aggregateCountResult.Count;

            IReadOnlyList<TDocument>? data = aggregation.First()
                .Facets.First(x => x.Name == DataFacetName)
                .Output<TDocument>();

            return (data, (int)count);
        }

        public static BsonRegularExpression SearchPartially(string value)
        {
            return new BsonRegularExpression(Regex.Escape(value), "i"); //RegexOptions.IgnoreCase
        }
        
        public static BsonRegularExpression SearchCaseInsensitive(string value)
        {
            return new BsonRegularExpression("/^" + value + "$/i");
        }
    }
}