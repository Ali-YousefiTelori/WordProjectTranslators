using CodeReviewer.Engine;
using EasyMicroservices.TranslatorsMicroservice.Contracts.Common;
using EasyMicroservices.TranslatorsMicroservice.Database.Entities;
using EasyMicroservices.TranslatorsMicroservice.WebApi.Controllers;
using EasyMicroservices.Tests;

namespace EasyMicroservices.TranslatorsMicroservice.Tests
{
    public class CodeReviewerCheckTests : CodeReviewerTests
    {
        static CodeReviewerCheckTests()
        {
            //types to check (this will check all of types in assembly so no need to add all of types of assembly)
            AssemblyManager.AddAssemblyToReview(
                typeof(DatabaseBuilder),
                //typeof(CompileTimeClassesMappers),
                typeof(CategoryEntity),
                typeof(CategoryContract),
                typeof(CategoryController));
        }
    }
}