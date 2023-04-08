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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab5dotNet
{
    public partial class MainWindow : Window
    {
        public int N { get; set; }
        public int K { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            N = 5;
            K = 4;
            //NewtonSymbol newtonSymbol = new NewtonSymbol(5, 4, calculateNewtonSymbolTasksTextBox);
        }

        private void nChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            TextBox t = (TextBox)sender;
            int i;
            if(int.TryParse(t.Text, out i) && i >= 0 && i <= 9999)
            {
                N = i;
            }
            else
            {
                statusBox.Text = "Write correct number!";
            }
        }

        private void kChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            TextBox t = (TextBox)sender;
            int i;
            if (int.TryParse(t.Text, out i) && i >= 0 && i <= 9999)
            {
                K = i;
            }
            else
            {
                statusBox.Text = "Write correct number!";
            }
        }

        public void CalculateNewtonSymbolTasks(object sender, RoutedEventArgs e)
        {
            (int, int) tuple = (N, K);
            Task<int> taskUpper = new Task<int>((object obj) =>
            {
                int result = N;
                for (int i = 1; i < K; i++)
                {
                    result *= (N - i);
                }
                return result;
            }, tuple);
            taskUpper.Start();
            taskUpper.Wait();

            Task<int> taskLower = new Task<int>((object obj) =>
            {
                int result = 1;
                for (int i = 1; i <= K; i++)
                {
                    result *= i;
                }
                return result;
            }, K);
            taskLower.Start();
            taskLower.Wait();
            int result = taskUpper.Result / taskLower.Result;
            calculateNewtonSymbolTasksTextBox.Text = result.ToString();
        }

        public void CalculateNewtonSymbolDelegates(object sender, RoutedEventArgs e)
        {
            Func<int> upperDelegate = CalculateUpper;
            Func<int> lowerDelegate = CalculateLower;

            var upper = upperDelegate.BeginInvoke(null, null);//await Task.Run(upperDelegate);
            var lower = lowerDelegate.BeginInvoke(null, null);
            int result = upperDelegate.EndInvoke(upper) / lowerDelegate.EndInvoke(lower);
            calculateNewtonSymbolDelegatesTextBox.Text = result.ToString();
        }

        public async void CalculateNewtonSymbolAsyncAwait(object sender, RoutedEventArgs e)
        {
            var upper = await Task.Run(() => CalculateUpper());
            var lower = await Task.Run(() => CalculateLower());
            int result = upper / lower;
            calculateNewtonSymbolAsyncAwaitTextBox.Text = result.ToString();
        }

        private int CalculateUpper()
        {
            int result = N;
            for (int i = 1; i < K; i++)
            {
                result *= (N - i);
            }
            return result;
        }

        private int CalculateLower()
        {
            int result = 1;
            for (int i = 1; i <= K; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}
