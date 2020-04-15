using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using DiffPlex.DiffBuilder.Model;

namespace DiffPlexDemo
{
    public class RichTextVmToFlowDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is DiffPaneModel diffVM))
                return Binding.DoNothing;

            var paragraph = new Paragraph();
            foreach (var lineVM in diffVM.Lines)
            {
                List<Run> lineView = ConvertLinveVmToRuns(lineVM);

                paragraph.Inlines.AddRange(lineView);

                //改行を追加
                paragraph.Inlines.Add(new LineBreak());
            }
            return new FlowDocument(paragraph);
        }

        private static List<Run> ConvertLinveVmToRuns(DiffPiece lineVM)
        {
            //差分タイプによって、行頭の文字列内容と背景色を決定
            var (color, preFix) = lineVM.Type switch
            {
                ChangeType.Deleted => (Colors.Pink, "💣| "),
                ChangeType.Inserted => (Colors.GreenYellow, "➕| "),
                ChangeType.Imaginary => (Colors.SkyBlue, "📌| "),
                ChangeType.Modified => (Colors.Yellow, "✏| "),
                _ => (Colors.Transparent, "🔏| "),
            };

            //見やすいように少し半透明にしておく
            color.A = 0xC0;
            var baseColorBrush = new SolidColorBrush(color);
            var modifiedPieceBrush = new SolidColorBrush(Colors.Orange);

            //ChangeType.Modified以外は行全体で同じ書式
            if (lineVM.Type != ChangeType.Modified)
            {
                var lineView = new Run()
                {
                    Text = preFix + lineVM.Text,
                    Background = baseColorBrush,
                };
                return new List<Run> { lineView };
            }

            //ChangeType.Modifiedだったら変更された部分だけハイライトしたいのでSubPieceからいろいろやる
            var prefixRun = new Run()
            {
                Text = preFix,
                Background = baseColorBrush,
            };
            var runs = new List<Run> { prefixRun };

            foreach (var piece in lineVM.SubPieces)
            {
                runs.Add(new Run
                {
                    Text = piece.Text,
                    Background = piece.Type == ChangeType.Unchanged
                        ? baseColorBrush
                        : modifiedPieceBrush,
                });
            }
            return runs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}