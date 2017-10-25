﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SAHB.GraphQLClient.FieldBuilder;
using SAHB.GraphQLClient.QueryBuilder;
using Xunit;

namespace SAHB.GraphQLClient.Tests.GraphQLClient.IntegrationTests
{
    public class NestedIntegrationTests
    {
        private readonly GraphQLQueryBuilder _queryBuilder;

        public NestedIntegrationTests()
        {
            var fieldBuilder = new GraphQLFieldBuilder();
            _queryBuilder = new GraphQLQueryBuilder(fieldBuilder);
        }

        [Fact]
        public async Task TestGraphQLClient()
        {
            var responseContent = "{\"data\":{\"Me\":{\"Firstname\":\"Søren\", Age:\"24\", \"lastname\": \"Bjergmark\"}}}";
            var httpClient = new HttpClientMock.HttpClientMock(responseContent, "{\"Query\":\"query{Me:me{Firstname:firstname Age:age lastname}}\"}");
            var client = new SAHB.GraphQLClient.GraphQLClient(httpClient, _queryBuilder);

            // Act
            var response = await client.Get<QueryToTest>("");

            // Assert
            Assert.Equal("Søren", response.Me.Firstname);
        }

        public class QueryToTest
        {
            public Person Me { get; set; }
        }

        public class Person
        {
            public string Firstname { get; set; }
            public uint Age { get; set; }
            public string lastname { get; set; }
        }
    }
}
