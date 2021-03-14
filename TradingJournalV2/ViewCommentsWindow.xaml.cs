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
using TradingJournalV2.Models;

namespace TradingJournalV2
{
    /// <summary>
    /// Interaction logic for ViewCommentsWindow.xaml
    /// </summary>
    public partial class ViewCommentsWindow : Window
    {
        public History CurrentHistory { get; set; }
        public ViewCommentsWindow(History history)
        {
            InitializeComponent();
            CurrentHistory = history;
            tbComments.Text = CurrentHistory.Comments;
        }

        private void btnSaveComments_Click(object sender, RoutedEventArgs e)
        {
            CurrentHistory.Comments = tbComments.Text;
        }
    }
}
