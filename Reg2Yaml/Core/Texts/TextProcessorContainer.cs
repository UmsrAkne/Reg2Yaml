using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace Reg2Yaml.Core.Texts
{
    public class TextProcessorContainer : BindableBase
    {
        private string displayName = string.Empty;

        public ObservableCollection<TextProcessor> TextProcessors { get; set; }

        /// <summary>
        /// ビューに表示するための名前。プログラム上では使わない。
        /// </summary>
        public string DisplayName { get => displayName; set => SetProperty(ref displayName, value); }
    }
}