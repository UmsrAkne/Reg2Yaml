using System;
using System.Collections;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Reg2Yaml.Core.Texts;

namespace Reg2Yaml.Core.Services
{
    public class TextProcessorStorageService
    {
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // 日本語が文字化け（\uXXXX）するのを防ぐ
        };

        public void Save(string filePath, IEnumerable containers)
        {
            try
            {
                // ディレクトリが存在しない場合は作成
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var jsonString = JsonSerializer.Serialize(containers, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"データの保存に失敗しました: {filePath}", ex);
            }
        }

        /// <summary>
        /// JSONファイルからデータを復元（読み込み）します。ファイルがない場合は空のコンテナを返します。
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <returns>復元されたデータのコンテナ</returns>
        public TextProcessorContainer Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // ファイルが存在しない場合は初期インスタンスを返す
                return new TextProcessorContainer();
            }

            try
            {
                var jsonString = File.ReadAllText(filePath);
                var container = JsonSerializer.Deserialize<TextProcessorContainer>(jsonString, options);

                return container ?? new TextProcessorContainer();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"データの読み込みに失敗しました: {filePath}", ex);
            }
        }
    }
}