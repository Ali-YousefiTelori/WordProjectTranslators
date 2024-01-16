using EasyMicroservices.FileManager.Providers.PathProviders;
using Translators.Schemas;
using Translators.Shared.FileVersionControl;

namespace Translators.Tests.Shared.FileVersionControl
{
    public class SchemaVersionControlTest
    {
        static SchemaVersionControlTest()
        {
            if (SchemaVersionControl.Current == null)
            {
                var pathyProvider = new SystemPathProvider();
                var directory = new EasyMicroservices.FileManager.Providers.DirectoryProviders.MemoryDirectoryProvider("Root", pathyProvider);
                var file = new EasyMicroservices.FileManager.Providers.FileProviders.MemoryFileProvider(directory);
                var serializer = new EasyMicroservices.Serialization.Newtonsoft.Json.Providers.NewtonsoftJsonProvider();
                SchemaVersionControl.Current = new SchemaVersionControl(serializer, pathyProvider, file, directory);
            }
        }

        [Fact]
        public async Task Test()
        {
            await SchemaVersionControl.Current.RegisterTypes(false);
            List<LinkGroupSchema> items = new List<LinkGroupSchema>()
            {
                new LinkGroupSchema()
                {
                    Id = 1,
                    Title = "Test 1",
                },
                new LinkGroupSchema()
                {
                    Id = 2,
                    Title = "Test 2",
                }
            };
            await SchemaVersionControl.Current.SaveSchema(items);
            var loaded = await SchemaVersionControl.Current.LoadSchema<List<LinkGroupSchema>>();
            Assert.True(loaded);
            Assert.Equal(1, loaded.Result[0].Id);
            Assert.Equal("Test 1", loaded.Result[0].Title);
            Assert.Equal(2, loaded.Result[1].Id);
            Assert.Equal("Test 2", loaded.Result[1].Title);
        }
    }
}
