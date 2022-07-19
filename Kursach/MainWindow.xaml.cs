using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Globalization;
//using System.Windows.Forms.HorizontalAlignment

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public static class Ext
    {

        public static void SetText(this RichTextBox richTextBox, string text)
        {
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart,
                richTextBox.Document.ContentEnd).Text;
        }
    }

    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            FontSizecomboBox.ItemsSource = new List<double>() { 8, 9, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            FontSizecomboBox.SelectedIndex = 3;
        }


        public void UpdateStatusBar()
        {
            string words;
            string lines;

            words = Ext.GetText(richTextBox);
            lines = Ext.GetText(richTextBox);

            int i = 0;
            int c = 0;
            int myWords = 1;
            int myLines = 1;

            while (i <= words.Length - 2)
            {
                if (words[i] == ' ' || words[i] == '\n' || words[i] == '\t')
                {
                    myWords++;
                }
                i++;
            }
            while (c <= lines.Length - 2)
            {
                if (lines[c] == '\n')
                {
                    myLines++;
                }
                c++;
            }

            this.words.Text = myWords.ToString();
            int ch;
            ch = words.Length - 2;
            this.ch.Text = ch.ToString();
            this.lines.Text = myLines.ToString();
        }
        //private void richTextBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //    UpdateStatusBar();
        //}
        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateStatusBar();
        }

        private void BoldCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (chkbxBold.IsChecked == true)
                richTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
                richTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
        }
        private void ItalicCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (chkbxItalic.IsChecked == true)
                richTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                richTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
        }
        private void UnderlineCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (chkbxUnderline.IsChecked == true)
                richTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
                richTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, forDefaultTextDecorations.TextDecorations);

        }
        private void Save_Handler(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog()
            {
                Filter = "Rich Text Format (*.rtf)|*.rtf|Text file (*.txt)|*.txt|PDF (*.pdf)|*.pdf|All Files (*.*)|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }
        }
        private void Open_Handler(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
            //UpdateStatusBar();
        }

        private void New_Handler(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you want to save your existing document first?", "Text Redactor", MessageBoxButton.YesNoCancel);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.Save_Handler(sender, e);
                this.richTextBox.Document.Blocks.Clear();
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
                this.richTextBox.Document.Blocks.Clear();
            }
        }

        private void SaveAs_Handler(object sender, ExecutedRoutedEventArgs e)
        {
            Save_Handler(sender, e);
        }

        private void Exit_Handler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.Undo();
        }

        private void LeftAlignment_RadioButtonClick(object sender, RoutedEventArgs e)
        {
            richTextBox.Selection.Text = ContentAlignment.MiddleLeft.ToString();
        }

        private void CutButton_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox.Selection.Text != "")
                richTextBox.Cut();

        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.Redo();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox.Selection.Text.Length > 0)
            {
                richTextBox.Copy();
            }
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.Paste();
        }
        private void MiddleAlignment_RadioButtonClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            richTextBox.Selection.ApplyPropertyValue(FontSizeProperty, (double)(FontSizecomboBox.SelectedItem));
            
            
        }

        private void FontsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
