using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Reg2Yaml.Core.Texts;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Reg2Yaml.Core.Services
{
    public class TextProcessingService
    {
        /// <summary>
        /// 指定されたコンテナ内の有効なプロセッサーを実行し、YAML形式の文字列を出力します。
        /// </summary>
        /// <param name="container">処理対象のコンテナ</param>
        /// <param name="baseText">初期入力テキスト</param>
        /// <returns>生成されたYAML文字列</returns>
        public string ExecuteAndExportToYaml(TextProcessorContainer container, string baseText)
        {
            if (container == null || !container.IsEnabled)
            {
                return string.Empty;
            }

            // YAMLのシリアライズ用に辞書形式（Caption -> 最終結果テキスト）で結果を蓄積
            var yamlResultMap = new Dictionary<string, string>();

            // 1. TextProcessors を順番に処理
            foreach (var processor in container.TextProcessors)
            {
                if (!processor.IsEnabled)
                {
                    continue;
                }

                // 2. 各プロセッサーの初期入力は「ベーステキスト」
                var currentText = baseText;

                // 3. ユニットを順番に実行（前のユニットの結果が次の入力になる）
                foreach (var unit in processor.Units)
                {
                    if (!unit.IsEnabled)
                    {
                        continue;
                    }

                    currentText = ExecuteUnit(unit, currentText);
                }

                // 4. 最終結果を Caption をキーにして保持（重複防止のため上書きか退避を考慮）
                if (!string.IsNullOrEmpty(processor.Caption))
                {
                    yamlResultMap[processor.Caption] = currentText;
                }
            }

            // 5. YamlDotNet を使ってシリアライズ
            var serializer = new SerializerBuilder()
                .WithNamingConvention(NullNamingConvention.Instance) // プロパティ名（キー）をそのまま出力
                .Build();

            return serializer.Serialize(yamlResultMap);
        }

        /// <summary>
        /// 1つのユニット（抽出または置換）を実行します。
        /// </summary>
        private string ExecuteUnit(TextProcessingUnit unit, string inputText)
        {
            if (string.IsNullOrEmpty(unit.RegexPattern))
            {
                return inputText;
            }

            // 複数行マッチ（^ $ が各行の先頭・末尾にマッチ）を設定
            // 必要に応じて . が改行にもマッチする RegexOptions.SingleLine も考慮してください
            var options = RegexOptions.Multiline | RegexOptions.Compiled;

            switch (unit.ProcessingType)
            {
                case TextProcessingType.Extract:
                    // マッチしたすべての箇所を抽出
                    var matches = Regex.Matches(inputText, unit.RegexPattern, options);

                    if (matches.Count == 0)
                    {
                        return string.Empty;
                    }

                    // すべてのマッチした文字列（またはキャプチャグループ全体）を改行で連結
                    var extractedValues = matches.Select(m => m.Value);
                    return string.Join(Environment.NewLine, extractedValues);

                case TextProcessingType.Replace:
                    return Regex.Replace(inputText, unit.RegexPattern, unit.Replacement, options);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}