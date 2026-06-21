using System.Collections.ObjectModel;
using Prism.Mvvm;
using Reg2Yaml.Core.Texts;

namespace Reg2Yaml.ViewModels
{
    public class BatchProcessPageViewModel : BindableBase
    {
        public BatchProcessPageViewModel(ObservableCollection<TextProcessorContainer> textProcessorContainers)
        {
            TextProcessorContainers = textProcessorContainers;
        }

        public ObservableCollection<TextProcessorContainer> TextProcessorContainers { get; set; }

        public ObservableCollection<FileListItem> Files { get; set; } = new ();

        public ObservableCollection<FileListItem> ProcessedFiles { get; set; } = new ();
    }
}