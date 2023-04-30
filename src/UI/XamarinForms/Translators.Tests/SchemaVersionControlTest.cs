using EasyMicroservices.FileManager.Providers.DirectoryProviders;
using EasyMicroservices.FileManager.Providers.FileProviders;
using EasyMicroservices.FileManager.Providers.PathProviders;
using Translators.Schemas;
using Translators.Shared.FileVersionControl;

namespace Translators.Tests
{
    public class SchemaVersionControlTest
    {
        [Fact]
        public async Task TestSave()
        {
            var pathProvider = new SystemPathProvider();
            var directoryProvider = new MemoryDirectoryProvider("Bin", pathProvider);
            SchemaVersionControl.Current = new SchemaVersionControl(pathProvider, new MemoryFileProvider(directoryProvider), directoryProvider);
            await SchemaVersionControl.Current.LoadAll();

            List<LanguageSchema> data = new List<LanguageSchema>
            {
                new LanguageSchema()
                {
                    Code = "en",
                    Id = 1,
                    Name = "English",
                }
            };
            await SchemaVersionControl.Current.SaveSchema(data);

            var listLoaded = await SchemaVersionControl.Current.LoadSchema<List<LanguageSchema>>();
            Assert.True(listLoaded);
            Assert.Single(listLoaded.Result);
            Assert.Equal(data[0].Name, listLoaded.Result[0].Name);
            Assert.Equal(data[0].Id, listLoaded.Result[0].Id);
            Assert.Equal(data[0].Code, listLoaded.Result[0].Code);

            var single = new LanguageSchema()
            {
                Code = "fa",
                Id = 2,
                Name = "Persian"
            };
            await SchemaVersionControl.Current.SaveSchema(single);

            var singleLoaded = await SchemaVersionControl.Current.LoadSchema<LanguageSchema>();
            Assert.Equal(single.Name, singleLoaded.Result.Name);
            Assert.Equal(single.Id, singleLoaded.Result.Id);
            Assert.Equal(single.Code, singleLoaded.Result.Code);
        }
    }
}