using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Reg2Yaml.Core.Services;
using Reg2Yaml.Core.Texts;
using Reg2Yaml.Utils;

namespace Reg2Yaml.ViewModels;

public class MainWindowViewModel : BindableBase
{
    #if DEBUG
    // ReSharper disable once UnusedMember.Local
    private readonly string testDirectoryPath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            @"myFiles\tests\RiderProjects\Reg2Yaml");
    #endif

    private readonly AppVersionInfo appVersionInfo = new();
    private readonly TextProcessingService textProcessingService = new ();
    private readonly TextProcessorStorageService textProcessorStorageService = new();
    private TextProcessorContainer selectedContainer;
    private TextProcessor selectedProcessor;
    private string inputText;
    private string resultText;
    private string message = string.Empty;

    public MainWindowViewModel()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "user_data", "textProcessorContainer.json");
        var loadedContainers = textProcessorStorageService.Load(path);
        BatchProcessPageViewModel = new BatchProcessPageViewModel(TextProcessorContainers);

        if (loadedContainers.Any())
        {
            foreach (var container in loadedContainers)
            {
                TextProcessorContainers.Add(container);
            }

            SelectedContainer = TextProcessorContainers.FirstOrDefault();
        }
        else
        {
            SetupDummyData();
        }
    }

    public string Title => appVersionInfo.Title;

    public ObservableCollection<TextProcessorContainer> TextProcessorContainers { get; set; } = new();

    public string InputText
    {
        get => inputText;
        set
        {
            if (SetProperty(ref inputText, value))
            {
                RaisePropertyChanged(nameof(ExecuteTextProcessCommand));
            }
        }
    }

    public string ResultText { get => resultText; set => SetProperty(ref resultText, value); }

    public string Message { get => message; set => SetProperty(ref message, value); }

    public TextProcessorContainer SelectedContainer
    {
        get => selectedContainer;
        set => SetProperty(ref selectedContainer, value);
    }

    public TextProcessor SelectedProcessor
    {
        get => selectedProcessor;
        set => SetProperty(ref selectedProcessor, value);
    }

    public BatchProcessPageViewModel BatchProcessPageViewModel { get; set; }

    public DelegateCommand AddContainerCommand => new DelegateCommand(() =>
    {
        TextProcessorContainers.Add(new TextProcessorContainer() { DisplayName = "new container", });
    });

    public DelegateCommand AddProcessorCommand => new DelegateCommand(() =>
    {
        SelectedContainer?.TextProcessors.Add(new TextProcessor() { Caption = "caption", });
    });

    public DelegateCommand AddUnitCommand => new DelegateCommand(() =>
    {
        SelectedProcessor?.Units.Add(new TextProcessingUnit());
    });

    public DelegateCommand ExecuteTextProcessCommand => new DelegateCommand(() =>
    {
        if (SelectedContainer == null)
        {
            return;
        }

        ResultText = textProcessingService.ExecuteAndExportToYaml(SelectedContainer, InputText);
    });

    public DelegateCommand SaveJsonCommand => new DelegateCommand(() =>
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "user_data", "textProcessorContainer.json");
            textProcessorStorageService.Save(path, TextProcessorContainers);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        const string msg = "Json saved successfully.";
        Console.WriteLine(msg);
        Message = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + $" {msg}";
    });

    public DelegateCommand CopyResultCommand => new DelegateCommand(() =>
    {
        if (string.IsNullOrWhiteSpace(ResultText))
        {
            Console.WriteLine("ResultText is empty.");
            return;
        }

        Clipboard.SetText(ResultText);
    });

    [Conditional("DEBUG")]
    private void SetupDummyData()
    {
        // 1つ目のコンテナ：ユーザー情報に関するテキスト処理
        var userProfileContainer = new TextProcessorContainer
        {
            DisplayName = "ユーザー情報パース設定",
            TextProcessors = new ObservableCollection<TextProcessor>
            {
                new()
                {
                    Caption = "UserNameExtractor",
                    Units = new ObservableCollection<TextProcessingUnit>
                    {
                        new()
                        {
                            ProcessingType = TextProcessingType.Extract, RegexPattern = @"ID:\d+\s+Name:(?<name>\w+)",
                        },
                        new() { ProcessingType = TextProcessingType.Replace, RegexPattern = @"\s+", },
                    },
                },
                new()
                {
                    Caption = "EmailMasker",
                    Units = new ObservableCollection<TextProcessingUnit>
                    {
                        new()
                        {
                            ProcessingType = TextProcessingType.Replace, RegexPattern = @"(?<=.)[^@](?=[^@]*?[^@].*?@)",
                        },
                    },
                },
            },
        };

        // 2つ目のコンテナ：ログ解析に関するテキスト処理
        var logAnalysisContainer = new TextProcessorContainer
        {
            DisplayName = "サーバーログ解析設定",
            TextProcessors = new ObservableCollection<TextProcessor>
            {
                new()
                {
                    Caption = "ErrorLogFilter",
                    Units = new ObservableCollection<TextProcessingUnit>
                    {
                        new()
                        {
                            ProcessingType = TextProcessingType.Extract, RegexPattern = @"\[ERROR\]\s+(?<message>.*)",
                        },
                    },
                },
            },
        };

        // コレクションに追加
        TextProcessorContainers.Add(userProfileContainer);
        TextProcessorContainers.Add(logAnalysisContainer);

        // 初期選択状態として1つ目のコンテナをセット
        SelectedContainer = userProfileContainer;
    }
}