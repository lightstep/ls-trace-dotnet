using System.Collections.Generic;
using Datadog.Trace.TestHelpers;

namespace Datadog.Trace.ClrProfiler.IntegrationTests
{
    public class GraphQLSpanExpectation : WebServerSpanExpectation
    {
        public GraphQLSpanExpectation(string serviceName, string operationName)
            : base(serviceName, operationName, SpanTypes.GraphQL)
        {
            RegisterDelegateExpectation(ExpectErrorMatch);
            RegisterTagExpectation(nameof(Tags.GraphQLSource), expected: GraphQLSource, Always);
            RegisterTagExpectation(nameof(Tags.GraphQLOperationType), expected: GraphQLOperationType, Always);
        }

        public string GraphQLRequestBody { get; set; }

        public string GraphQLOperationType { get; set; }

        public string GraphQLOperationName { get; set; }

        public string GraphQLSource { get; set; }

        public bool IsGraphQLError { get; set; }

        private IEnumerable<string> ExpectErrorMatch(MockTracerAgent.Span span)
        {
            if (string.IsNullOrEmpty(GetTag(span, Tags.ErrorMsg)))
            {
                if (IsGraphQLError)
                {
                    yield return "Expected an error message.";
                }
            }
            else
            {
                if (!IsGraphQLError)
                {
                    yield return "Expected no error message.";
                }
            }
        }
    }
}
