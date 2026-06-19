using System;
using System.Collections.Generic;
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

        public void Save(string filePath, IEnumerable<TextProcessorContainer> containers)
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
        /// JSONファイルからデータを復元（読み込み）します。ファイルがない場合は空のリストを返します。
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <returns>復元されたデータのリスト</returns>
        public List<TextProcessorContainer> Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // ファイルが存在しない場合は空のリストを返す
                return new List<TextProcessorContainer>();
            }

            try
            {
                var jsonString = File.ReadAllText(filePath);
                var containers = JsonSerializer.Deserialize<List<TextProcessorContainer>>(jsonString, options);

                return containers ?? new List<TextProcessorContainer>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"データの読み込みに失敗しました: {filePath}", ex);
            }
        }
    }
}