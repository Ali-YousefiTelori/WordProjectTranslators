using EasyMicroservices.TranslatorsMicroservice.Clients.Helpers;
using EasyMicroservices.TranslatorsMicroservice.Clients.Tests.Contracts.Common;
using EasyMicroservices.Laboratory.Engine;
using EasyMicroservices.Laboratory.Engine.Net.Http;

namespace EasyMicroservices.TranslatorsMicroservice.Clients.Tests
{
    public class ContentLanguageHelperTest
    {
        const int Port = 1202;
        string _routeAddress = "";
        public static HttpClient HttpClient { get; set; } = new HttpClient();
        public ContentLanguageHelperTest()
        {
            _routeAddress = $"http://localhost:{Port}";
        }

        static bool _isInitialized = false;
        static SemaphoreSlim Semaphore = new SemaphoreSlim(1);
        async Task OnInitialize()
        {
            if (_isInitialized)
                return;
            try
            {
                await Semaphore.WaitAsync();
                _isInitialized = true;

                ResourceManager resourceManager = new ResourceManager();
                HttpHandler httpHandler = new HttpHandler(resourceManager);
                await httpHandler.Start(Port);
                resourceManager.Append(@$"POST *RequestSkipBody* HTTP/1.1*RequestSkipBody*

{{""language"":""fa-IR"",""key"":""1-1-Title""}}"
    ,
    @"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: 0

{""result"":{""Data"": ""Hello My Title Language""},""isSuccess"":true,""error"":null}");

                resourceManager.Append(@$"POST *RequestSkipBody* HTTP/1.1*RequestSkipBody*

{{""language"":""fa-IR"",""key"":""1-1-Content""}}"
   ,
   @"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: 0

{""result"":{""Data"": ""Hello My Content Language""},""isSuccess"":true,""error"":null}");

                resourceManager.Append(@$"POST /api/Content/GetAllByKey HTTP/1.1*RequestSkipBody*

{{""key"":""1-1-Title""}}"
    ,
    @"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: 0

{
	""result"": [
		{
			""Data"": ""Hello My Title Language"",
			""Language"": {
				""Name"": ""en-US""
			}
		},
		{
			""Data"": ""persian hi"",
			""Language"": {
				""Name"": ""fa-IR""
			}
		}
	],
	""isSuccess"": true,
	""error"": null
}");

                resourceManager.Append(@$"POST /api/Content/GetByLanguage HTTP/1.1*RequestSkipBody*

{{""language"":""fa-IR"",""key"":""Title"",""uniqueIdentity"":""1-1""}}"
    ,
    @"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: 0

                {
                	""result"":
                		{
                			""Data"": ""Hello My Title Language"",
                			""Language"": {
                				""Name"": ""en-US""
                			}
                		},
                	""isSuccess"": true,
                	""error"": null
                }");
                resourceManager.Append(@$"POST /api/Content/GetByLanguage HTTP/1.1*RequestSkipBody*

{{""language"":""fa-IR"",""key"":""Content"",""uniqueIdentity"":""1-1""}}"
,
@"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: 0

                {
                	""result"":
                		{
                			""Data"": ""Hello My Content Language"",
                			""Language"": {
                				""Name"": ""en-US""
                			}
                		},
                	""isSuccess"": true,
                	""error"": null
                }");

                resourceManager.Append(@$"POST /api/Content/GetAllByKey HTTP/1.1*RequestSkipBody*

{{""key"":""Title"",""uniqueIdentity"":""1-1""}}"
,
@"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: 0

                {
	                ""result"": [
		                {
			                ""Data"": ""Hello My Title Language"",
			                ""Language"": {
				                ""Name"": ""en-US""
			                }
		                },
		                {
			                ""Data"": ""persian hi"",
			                ""Language"": {
				                ""Name"": ""fa-IR""
			                }
		                }
	                ],
                	""isSuccess"": true,
                	""error"": null
                }");
            }
            finally
            {
                Semaphore.Release();
            }
        }

        [Fact]
        public async Task ContentLanguageTest()
        {
            await OnInitialize();
            var microserviceClient = new Translators.GeneratedServices.ContentClient(_routeAddress, HttpClient);
            var microservices = await microserviceClient.GetByLanguageAsync(new Translators.GeneratedServices.GetByLanguageRequestContract()
            {
                Key = "Title",
                UniqueIdentity = "1-1",
                Language = "fa-IR"
            });
            Assert.True(microservices.IsSuccess);
            Assert.NotEmpty(microservices.Result.Data);
        }

        [Fact]
        public async Task ContentLanguagePersonTest()
        {
            await OnInitialize();

            PersonContract personContract = new PersonContract();
            personContract.UniqueIdentity = "1-1";
            personContract.Post = new PostContract()
            {
                Person = personContract,
                UniqueIdentity = "1-1"
            };
            personContract.Posts = new List<PostContract>()
            {
                 new PostContract()
                 {
                      Person = personContract,
                      UniqueIdentity = "1-1"
                 }
            };

            ContentLanguageHelper contentLanguageHelper = new ContentLanguageHelper(new Translators.GeneratedServices.ContentClient(_routeAddress, HttpClient));
            await contentLanguageHelper.ResolveContentLanguage(personContract, "fa-IR");

            Assert.NotEmpty(personContract.Title);
            Assert.NotEmpty(personContract.Post.Title);
            Assert.NotEmpty(personContract.Post.Content);
            Assert.NotEmpty(personContract.Posts[0].Title);
            Assert.NotEmpty(personContract.Posts[0].Content);
        }

        [Fact]
        public async Task ContentAllLanguagePersonTest()
        {
            await OnInitialize();

            PersonLanguageContract personContract = new PersonLanguageContract();
            personContract.UniqueIdentity = "1-1";

            ContentLanguageHelper contentLanguageHelper = new ContentLanguageHelper(new Translators.GeneratedServices.ContentClient(_routeAddress, HttpClient));
            await contentLanguageHelper.ResolveContentAllLanguage(personContract);

            Assert.NotEmpty(personContract.Titles);
            Assert.True(personContract.Titles.Count > 1);
        }
    }
}
