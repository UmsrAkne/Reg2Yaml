using System;
using System.Diagnostics;
using System.IO;
using Prism.Mvvm;
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

    public MainWindowViewModel()
    {
        SetupDummyData();
    }

    public string Title => appVersionInfo.Title;

    [Conditional("DEBUG")]
    private void SetupDummyData()
    {
    }
}