using System;
using System.Diagnostics.Metrics;
using System.Net;
using System.Numerics;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

class NewtonSymbol
{
    private TextBox calculateNewtonSymbolTasksTextBox;
    public int N { get; set; }
    public int K { get; set; }

    public NewtonSymbol(int N, int K, TextBox calculateNewtonSymbolTasksTextBox)
    {
        this.N = N;
        this.K = K;
        this.calculateNewtonSymbolTasksTextBox = calculateNewtonSymbolTasksTextBox;
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
