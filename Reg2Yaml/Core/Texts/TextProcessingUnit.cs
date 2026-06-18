using Prism.Mvvm;

namespace Reg2Yaml.Core.Texts
{
    public class TextProcessingUnit : BindableBase
    {
        private string regexPattern = string.Empty;

        private TextProcessingType ProcessingType { get; set; }

        public string RegexPattern { get => regexPattern; set => SetProperty(ref regexPattern, value); }
    }
}