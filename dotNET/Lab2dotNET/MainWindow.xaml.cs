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
using System.Diagnostics;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            MenuItem item = sender as MenuItem;
            string path = (string)item.Tag;
            string readText = File.ReadAllText(path);
            textBlock1.Text = readText;
        }

        private void ContextFileDeleteClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            string path = (string)item.Tag;
            FileAttributes attributes = System.IO.File.GetAttributes(path);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                //remove read only attribute
                File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
            }

            //delete
            File.Delete(path);
            var selected = TreeView1.SelectedItem;
            TreeViewItem f = (TreeViewItem)TreeView1.SelectedItem;
            if (f != null)
            {
                ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(f);
                if (parent != null)
                {
                    parent.Items.Remove(f);
                }
            }
        }


        private void ContextFolderDeleteClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            string path = (string)item.Tag;
            FileAttributes attributes = System.IO.File.GetAttributes(path);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                //remove read only attribute
                File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
            }

            //delete
            Directory.Delete(path);
            var selected = TreeView1.SelectedItem;
            TreeViewItem f = (TreeViewItem)TreeView1.SelectedItem;
            if (f != null)
            {
                ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(f);
                if (parent != null)
                {
                    parent.Items.Remove(f);
                }
            }
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

                //add context menu
                ChildItem.ContextMenu = new System.Windows.Controls.ContextMenu();
                var menuItem1 = new MenuItem
                {
                    Header = "Open",
                    Tag = dir.FullName
                };
                menuItem1.Click += new RoutedEventHandler(ContextOpenClick);
                var menuItem2 = new MenuItem
                {
                    Header = "Delete",
                    Tag = dir.FullName
                };
                menuItem2.Click += new RoutedEventHandler(ContextFolderDeleteClick);
                ChildItem.ContextMenu.Items.Add(menuItem1);
                ChildItem.ContextMenu.Items.Add(menuItem2);

                Task2_catalog_tree(ChildItem, dir.FullName, tabs);
            }

            foreach (FileInfo file in Files)
            {
                TreeViewItem ChildItem = new TreeViewItem
                {
                    Header = file.Name,
                    Tag = file.FullName
                };
                ParentItem.Items.Add(ChildItem);


                ChildItem.ContextMenu = new System.Windows.Controls.ContextMenu();
                var menuItem1 = new MenuItem {
                    Header = "Open",
                    Tag = file.FullName
                };
                menuItem1.Click += new RoutedEventHandler(ContextOpenClick);
                var menuItem2 = new MenuItem
                {
                    Header = "Delete",
                    Tag = file.FullName
                };
                menuItem2.Click += new RoutedEventHandler(ContextFileDeleteClick);
                ChildItem.ContextMenu.Items.Add(menuItem1);
                ChildItem.ContextMenu.Items.Add(menuItem2);
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
