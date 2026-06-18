using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace Reg2Yaml.Core.Texts
{
    public class TextProcessorContainer : BindableBase
    {
        private string displayName = string.Empty;
        private bool isEnabled = true;

        public ObservableCollection<TextProcessor> TextProcessors { get; set; } = new();

        /// <summary>
        /// ビューに表示するための名前。プログラム上では使わない。
        /// </summary>
        public string DisplayName { get => displayName; set => SetProperty(ref displayName, value); }

        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }
    }
}