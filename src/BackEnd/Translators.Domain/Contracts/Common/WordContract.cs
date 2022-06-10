namespace Translators.Contracts.Common
{
    /// <summary>
    /// word of paragraph
    /// </summary>
    public class WordContract
    {
        public long Id { get; set; }
        public int Index { get; set; }

        public List<ValueContract> Values { get; set; }

        public long ParagraphId { get; set; }

        public List<WordLetterContract> WordLetters { get; set; }
        public List<WordRootContract> WordRoots { get; set; }

        public override string ToString()
        {
            return string.Join(",", Values.Select(x => x.Value));
        }
    }
}
