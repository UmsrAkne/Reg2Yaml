using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace Reg2Yaml.Core.Texts
{
    public class TextProcessor : BindableBase
    {
        private string caption = string.Empty;
        private bool isEnabled = true;

        public ObservableCollection<TextProcessingUnit> Units { get; set; } = new();

        /// <summary>
        /// yaml に出力する際に対象とする要素名。
        /// </summary>
        public string Caption { get => caption; set => SetProperty(ref caption, value); }

        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }
    }
}