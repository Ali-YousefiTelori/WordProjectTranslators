using CodeReviewer.Engine;
using EasyMicroservices.Tests;
using Translators.Contracts.Common;
using Translators.Database.Entities;
using Translators.Services;

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
                typeof(ChapterController));
        }
    }
}