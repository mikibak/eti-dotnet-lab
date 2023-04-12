﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using System.Runtime.Intrinsics.X86;
using System.Net;
//using System.Windows.Forms;

namespace Lab5dotNet
{
    public partial class MainWindow : Window
    {
        public int N { get; set; }
        public int K { get; set; }

        string[] hostNames = { "www.microsoft.com", "www.apple.com",
            "www.google.com", "www.ibm.com", "cisco.netacad.net",
            "www.oracle.com", "www.nokia.com", "www.hp.com", "www.dell.com",
            "www.samsung.com", "www.toshiba.com", "www.siemens.com",
            "www.amazon.com", "www.sony.com", "www.canon.com", 
            "www.alcatel-lucent.com", "www.acer.com", "www.motorola.com" };

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

            var upper = upperDelegate.BeginInvoke(null, null);
            var lower = lowerDelegate.BeginInvoke(null, null);
            int result = upperDelegate.EndInvoke(upper) / lowerDelegate.EndInvoke(lower);
            calculateNewtonSymbolDelegatesTextBox.Text = result.ToString();
        }

        private static List<Tuple<string, string>> GetAdresses(string[] hostNames)
        {
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            hostNames.AsParallel()
                .ForAll(hostName =>
                {
                    lock (result)
                    {
                        result.Add(Tuple.Create(hostName, Dns.GetHostAddresses(hostName).First().ToString()));
                    }
                });

            return result;
        }

        public void ResolveDNS(object sender, RoutedEventArgs args)
        {
            var domainList = GetAdresses(hostNames);
            resolveDNSTextBox.Text = "";
            foreach (var domain in domainList)
            {
                resolveDNSTextBox.Text += $"{domain.Item1} => {domain.Item2}\n";
            }
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

        public void CalculateFibonacci(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (object sender, DoWorkEventArgs args) =>
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                long previouspreviousNumber;
                long previousNumber = 0;
                long progress = 0;
                long currentNumber = 1;

                for (int i = 1; i < N; i++) {
                    previouspreviousNumber = previousNumber;
                    previousNumber = currentNumber;
                    currentNumber = previouspreviousNumber + previousNumber;
                    progress = (i* 100) / N;
                    bw.ReportProgress((int)progress);
                    Thread.Sleep(20);
                }
                args.Result = currentNumber;
            };
            bw.ProgressChanged += ((object sender, ProgressChangedEventArgs args) =>
            {
                progressBar.Value = args.ProgressPercentage;
            });
            bw.RunWorkerCompleted += ((object sender, RunWorkerCompletedEventArgs args) =>
            {
                progressBar.Value = 100;
                MessageBox.Show("Result: " + args.Result);
                progressBar.Value = 0;
            });

            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync(100);

            //int result = 0;
            //calculateFibonacciTextBox.Text = result.ToString();
        }

        public void CompressFolder(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog() { Description = "Select directory to compress" };
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            if(dlg.SelectedPath != "") {
                DirectoryInfo root = new DirectoryInfo(dlg.SelectedPath);
                CompressFileAndItsChildren(root);
            }
        }

        public void DecompressFolder(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog() { Description = "Select directory to decompress" };
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            if (dlg.SelectedPath != null)
            {
                DirectoryInfo root = new DirectoryInfo(dlg.SelectedPath);
                DecompressFileAndItsChildren(root);
            }
        }

        private static void CompressFileAndItsChildren(DirectoryInfo root)
        {
            
            DirectoryInfo[] Dirs = root.GetDirectories();
            FileInfo[] Files = root.GetFiles("*");
            Parallel.For(0, Files.Length,
                   index => {
                       CompressFile(Files[index].FullName);
                       File.Delete(Files[index].FullName);
                   });
            foreach (var dir in Dirs)
            {
                CompressFileAndItsChildren(dir);
            }
        }

        private static void DecompressFileAndItsChildren(DirectoryInfo root)
        {

            DirectoryInfo[] Dirs = root.GetDirectories();
            FileInfo[] Files = root.GetFiles("*");
            Parallel.For(0, Files.Length,
                   index => {
                       DecompressFile(Files[index].FullName);
                       File.Delete(Files[index].FullName);
                   });
            foreach (var dir in Dirs)
            {
                DecompressFileAndItsChildren(dir);
            }
        }

        private static void CompressFile(string path)
        {
            using FileStream originalFileStream = File.Open(path, FileMode.Open);
            using FileStream compressedFileStream = File.Create(path + ".gz");
            using var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress);
            originalFileStream.CopyTo(compressor);
        }

        private static void DecompressFile(string path)
        {
            using FileStream compressedFileStream = File.Open(path, FileMode.Open);
            using FileStream outputFileStream = File.Create(path.Substring(0, path.Length - 3));
            using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
            decompressor.CopyTo(outputFileStream);
        }
    }
}
