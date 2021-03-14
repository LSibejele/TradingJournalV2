using Microsoft.Win32;
using Newtonsoft.Json;
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
using TradingJournalV2.Models;

namespace TradingJournalV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string DirectoryPath = @"C:\Journal\";
        public byte[] AttachedFile;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRetrieveHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fromDate = dpFromDate.SelectedDate.Value.ToShortDateString();
                string toDate = dpToDate.SelectedDate.Value.ToShortDateString();

                var files = Directory.GetFiles(DirectoryPath);
                string[] filesBetweenDates = GetFilesBetweenDates(fromDate, toDate, files);

                List<History> histories = MapFilesToHistories(files);
                lvHistory.ItemsSource = histories;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<History> MapFilesToHistories(string [] filesBetweenDates)
        {
            List<History> HistoryList = new List<History>();
            for (int i = 0; i < filesBetweenDates.Length; i++)
            {
                using (StreamReader reader = File.OpenText(filesBetweenDates[i]))
                {
                    string content = "";
                    while ((content = reader.ReadLine()) != null)
                    {
                        History history = JsonConvert.DeserializeObject<History>(content);
                        HistoryList.Add(history);
                    }
                }
            }
            return HistoryList;
        }

        private string [] GetFilesBetweenDates(string fromDate, string toDate, string[] files)
        {
            int fromDateNumber = Convert.ToInt32(fromDate.Replace("/", ""));
            int toDateNumber = Convert.ToInt32(toDate.Replace("/", ""));
            List<string> results = new List<string>();

            foreach (string file in files)
            {
                if (fromDateNumber >= GetDateNumberForFile(file) && toDateNumber <= GetDateNumberForFile(file))
                {
                    results.Add(file);
                }
            }
            return results.ToArray();
        }

        private int GetDateNumberForFile(string file)
        {
            string date = file.Substring(file.Length - 13).Replace(".json", "");
            return Convert.ToInt32(date);
        }

        private void btnAttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                AttachedFile = File.ReadAllBytes(openFileDialog.FileName);
                tbAttachedFile.Text = openFileDialog.FileName;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Save(GetHistory());
                MessageBox.Show("Entry Saved Successfully!");
            } catch (Exception ex)
            {
                MessageBox.Show($"Unable to save entry: {ex.Message}");
            }
        }

        private void Save(History history)
        {
            if (Directory.Exists(DirectoryPath))
            {
                CreateJsonFile(history);
            } else 
            {
                Directory.CreateDirectory(DirectoryPath);
                CreateJsonFile(history);
            }
        }

        private string GetFilePath(string historyDate)
        {
            string filePath = System.IO.Path.Combine(DirectoryPath, $"{historyDate}.json");
            if (!File.Exists(filePath))
            {
                return filePath;
            }
            int fileCount = Directory.GetFiles(DirectoryPath).Where((file) =>
            {
                return file.Contains(historyDate);
            }).Count();
            
            filePath = System.IO.Path.Combine(DirectoryPath, $"{fileCount}_{historyDate}.json");
            return filePath;
        }

        private void CreateJsonFile(History history)
        {
            string filePath = GetFilePath(history.Date.ToShortDateString().Replace("/", ""));
            try 
            {
                using (FileStream fileStream = File.Create(filePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(JsonConvert.SerializeObject(history));
                    fileStream.Write(info, 0, info.Length);
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private History GetHistory()
        {
            History history = new History();
            history.Trade = cbTrade.Text;
            history.Date = DateTime.Now;
            history.Comments = tbComments.Text.Trim();
            history.Result = cbResult.Text;
            history.ChartPattern = cbChartPattern.Text;
            history.StopLoss = Convert.ToDouble(tbStopLoss.Text);
            history.TakeProfit = Convert.ToDouble(tbTakeProfit.Text);
            history.CurrencyPair = cbCurrencyPair.Text;
            history.Price = Convert.ToDouble(tbPrice.Text);
            history.Lots = Convert.ToDouble(tbLots.Text);
            history.Screenshot = AttachedFile;
            return history;
        }

        private void btnViewScreenshot_Click(object sender, RoutedEventArgs e)
        {
            History history = (sender as Button).DataContext as History;
            ViewScreenshotWindow screenshotWindow = new ViewScreenshotWindow(history.Screenshot);
            screenshotWindow.Show();
        }

        private void btnViewComments_Click(object sender, RoutedEventArgs e)
        {
            History history = (sender as Button).DataContext as History;
            ViewCommentsWindow commentsWindow = new ViewCommentsWindow(history.Comments);
            commentsWindow.Show();
        }
    }
}
