using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Reg2Yaml.Core.Services;
using Reg2Yaml.Core.Texts;

namespace Reg2Yaml.ViewModels
{
    public class BatchProcessPageViewModel : BindableBase
    {
        private readonly TextProcessingService textProcessingService = new();

        public BatchProcessPageViewModel(ObservableCollection<TextProcessorContainer> textProcessorContainers)
        {
            TextProcessorContainers = textProcessorContainers;
        }

        public ObservableCollection<TextProcessorContainer> TextProcessorContainers { get; set; }

        public ObservableCollection<FileListItem> Files { get; set; } = new ();

        public ObservableCollection<FileListItem> ProcessedFiles { get; set; } = new ();

        public DelegateCommand RunBatchProcessCommand => new DelegateCommand(() =>
        {
            if (TextProcessorContainers == null || !TextProcessorContainers.Any())
            {
                return;
            }

            if (Files == null || !Files.Any())
            {
                return;
            }

            var resultDirPath = Path.Combine(AppContext.BaseDirectory, "result_files");
            if (!Directory.Exists(resultDirPath))
            {
                Directory.CreateDirectory(resultDirPath);
            }

            foreach (var fileItem in Files)
            {
                var baseText = File.ReadAllText(fileItem.FileInfo.FullName);

                foreach (var container in TextProcessorContainers)
                {
                    if (!container.IsEnabled)
                    {
                        continue;
                    }

                    var yamlResult = textProcessingService.ExecuteAndExportToYaml(container, baseText);
                    if (string.IsNullOrEmpty(yamlResult))
                    {
                        continue;
                    }

                    var fileName = $"{Path.GetFileNameWithoutExtension(fileItem.FileInfo.Name)}_{container.DisplayName}.yaml";
                    var filePath = Path.Combine(resultDirPath, fileName);

                    File.WriteAllText(filePath, yamlResult);

                    ProcessedFiles.Add(new FileListItem(filePath));
                }
            }
        });
    }
}