namespace Translators.Database.Entities
{
    public class LanguageEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<ValueEntity> Values { get; set; }
    }
}
