using Prism.Mvvm;

namespace Reg2Yaml.Core.Texts
{
    public class TextProcessingUnit : BindableBase
    {
        private string regexPattern = string.Empty;
        private TextProcessingType processingType;
        private bool isEnabled = true;

        public TextProcessingType ProcessingType
        {
            get => processingType;
            set => SetProperty(ref processingType, value);
        }

        public string RegexPattern { get => regexPattern; set => SetProperty(ref regexPattern, value); }

        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }
    }
}