using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BDKurs
{
    /// <summary>
    /// Логика взаимодействия для SqlWindow.xaml
    /// </summary>
    public partial class SqlWindow : Window
    {

        LibraryDbContext context;

        public SqlWindow(LibraryDbContext _context)
        {
            context = _context;
            InitializeComponent();
        }

        private string GetRichTextBoxText(RichTextBox richTextBox)
        {
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            return textRange.Text.Trim();
        }

        private void ExecuteSqlQuery()
        {
            string sqlQuery = GetRichTextBoxText(sqlRichTextBox); // Получаем текст из RichTextBox
            try
            {
                var result = context.Database.ExecuteSqlRaw(sqlQuery);
                outputRichTextBox.Document.Blocks.Clear(); // Очищаем предыдущие результаты
                new TextRange(outputRichTextBox.Document.ContentStart, outputRichTextBox.Document.ContentEnd)
                    .Text = $"Запрос выполнен успешно. Затронуто строк: {result}";
            }
            catch (Exception ex)
            {
                outputRichTextBox.Document.Blocks.Clear(); // Очищаем предыдущие результаты
                new TextRange(outputRichTextBox.Document.ContentStart, outputRichTextBox.Document.ContentEnd)
                    .Text = $"Ошибка: {ex.Message}";
            }
        }

        private void ExecuteSelectQuery()
        {
            string sqlQuery = GetRichTextBoxText(sqlRichTextBox);
            try
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    context.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        outputRichTextBox.Document.Blocks.Clear(); // Очищаем предыдущие результаты

                        // Получаем названия столбцов
                        for (int i = 0; i < result.FieldCount; i++)
                        {
                            new TextRange(outputRichTextBox.Document.ContentStart, outputRichTextBox.Document.ContentEnd)
                                .Text += result.GetName(i) + "\t";
                        }
                        new TextRange(outputRichTextBox.Document.ContentStart, outputRichTextBox.Document.ContentEnd)
                            .Text += "\n";

                        // Получаем строки данных
                        while (result.Read())
                        {
                            for (int i = 0; i < result.FieldCount; i++)
                            {
                                new TextRange(outputRichTextBox.Document.ContentStart, outputRichTextBox.Document.ContentEnd)
                                    .Text += result[i]?.ToString() + "\t";
                            }
                            new TextRange(outputRichTextBox.Document.ContentStart, outputRichTextBox.Document.ContentEnd)
                                .Text += "\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                outputRichTextBox.Document.Blocks.Clear(); // Очищаем предыдущие результаты
                new TextRange(outputRichTextBox.Document.ContentStart, outputRichTextBox.Document.ContentEnd)
                    .Text = $"Ошибка: {ex.Message}";
            }
            finally
            {
                context.Database.CloseConnection();
            }
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string sqlQuery = GetRichTextBoxText(sqlRichTextBox); // Получаем текст из RichTextBox

            if (sqlQuery.Trim().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                ExecuteSelectQuery();
            }
            else
            {
                ExecuteSqlQuery();
            }
        }
    }
}
