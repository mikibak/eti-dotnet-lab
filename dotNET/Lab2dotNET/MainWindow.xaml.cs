using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
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
using Microsoft.Win32;

namespace Lab2dotNET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ContextOpenClick(object sender, RoutedEventArgs e)
        {
            
        }

        public void Task2_catalog_tree(TreeViewItem ParentItem, string arg, string tabs = "")
        {
            tabs += "     ";

            DirectoryInfo root = new DirectoryInfo(arg);
            DirectoryInfo[] Dirs = root.GetDirectories();
            FileInfo[] Files = root.GetFiles("*");

            foreach (DirectoryInfo dir in Dirs)
            {
                TreeViewItem ChildItem = new TreeViewItem
                {
                    Header = dir.Name,
                    Tag = dir.FullName
                };
                ParentItem.Items.Add(ChildItem);

                ChildItem.ContextMenu = new System.Windows.Controls.ContextMenu();
                var menuItem1 = new MenuItem { Header = "Open" };
                menuItem1.Click += new RoutedEventHandler(ContextOpenClick);
                var menuItem2 = new MenuItem { Header = "Delete" };
                menuItem2.Click += new RoutedEventHandler(ContextOpenClick);
                ChildItem.ContextMenu.Items.Add(menuItem1);
                ChildItem.ContextMenu.Items.Add(menuItem2);
                ChildItem.Selected += new RoutedEventHandler(ContextOpenClick);

                Task2_catalog_tree(ChildItem, dir.FullName, tabs);
            }

            foreach (FileInfo file in Files)
            {
                TreeViewItem Child1Item = new TreeViewItem
                {
                    Header = file.Name,
                    Tag = file.FullName
                };
                ParentItem.Items.Add(Child1Item);
            }
        }


        private void OpenClick(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog() { Description = "Select directory to open" };
            DialogResult result = dlg.ShowDialog();
            var folderName = System.IO.Path.GetFileName(dlg.SelectedPath);

            TreeViewItem ParentItem = new TreeViewItem
            {
                Header = folderName,
                Tag = dlg.SelectedPath
            };
            TreeView1.Items.Add(ParentItem);

            Task2_catalog_tree(ParentItem, dlg.SelectedPath);
        }
    }

}
