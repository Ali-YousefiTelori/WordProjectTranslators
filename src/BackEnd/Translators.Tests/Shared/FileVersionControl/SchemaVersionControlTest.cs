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
                var directory = new EasyMicroservices.FileManager.Providers.DirectoryProviders.DiskDirectoryProvider("Root", pathyProvider);
                var file = new EasyMicroservices.FileManager.Providers.FileProviders.MemoryFileProvider(directory);
                SchemaVersionControl.Current = new SchemaVersionControl(pathyProvider, file, directory);
            }
        }

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
            SchemaVersionControl.Current.LoadSchema
        }
    }
}
