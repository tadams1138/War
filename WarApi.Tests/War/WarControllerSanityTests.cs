﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WarApi
{
    [TestClass]
    public class WarControllerSanityTests
    {
        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenAuthenticatedUser_Contestants_ReturnsOK()
        {
            var principal = TestHelper.CreateStubClaimsPrincipal("test Name", "test AuthenticationType", "test nameIdentifier");
            var request = CreateContestantsRequest();
            var responseVerification = CreateResponseVerification(HttpStatusCode.OK);

            await TestHelper.TestEndpoint(request, principal, responseVerification);
        }
        
        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenUnauthenticatedUser_Contestants_ReturnsUnauthorized()
        {
            var principal = TestHelper.CreateStubUnauthenticatedClaimsPrincipal();
            var request = CreateContestantsRequest();
            var responseVerification = CreateResponseVerification(HttpStatusCode.Unauthorized);

            await TestHelper.TestEndpoint(request, principal, responseVerification);
        }

        private static HttpRequestMessage CreateContestantsRequest()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = TestHelper.CreateRelativeUri("api/War/2/Contestants"),
                Method = HttpMethod.Get
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return request;
        }

        private static Func<HttpResponseMessage, Task> CreateResponseVerification(HttpStatusCode expectedStatusCode)
        {
            return (response) =>
            {
                response.StatusCode.Should().Be(expectedStatusCode);
                return Task.FromResult(0);
            };
        }
    }
}
