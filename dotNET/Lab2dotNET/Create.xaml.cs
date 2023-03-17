using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Lab2dotNET
{
    /// <summary>
    /// Interaction logic for Create.xaml
    /// </summary>
    public partial class Create : Window
    {
        private string path;
        private TreeViewItem treeViewItem;
        private MainWindow mainWindow;
        public Create(string path, TreeViewItem treeViewItem, MainWindow mainWindow)
        {
            this.path = path;
            this.treeViewItem = treeViewItem;
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void CreateOkClick(object sender, RoutedEventArgs e)
        {
            string name = CreatedName.Text;
            string newPath = path + "\\" + name;

            if (name == null)
            {
                this.Close();
            }

            FileAttributes attributes = FileAttributes.Normal;

            if ((bool)ReadOnlyBox.IsChecked)
            {
                attributes = attributes | FileAttributes.ReadOnly;
            }
            if ((bool)ArchiveBox.IsChecked)
            {
                attributes = attributes | FileAttributes.Archive;
            }
            if ((bool)HiddenBox.IsChecked)
            {
                attributes = attributes | FileAttributes.Hidden;
            }
            if ((bool)SystemBox.IsChecked)
            {
                attributes = attributes | FileAttributes.System;
            }


            if (FileRadio.IsChecked == true && Regex.IsMatch(name, "^[a-zA-Z0-9_~-]{1,8}\\.(txt|php|html)$")) 
            {
                File.SetAttributes(path, attributes);
                FileStream fs = File.Create(newPath);
                fs.Close();

                if (treeViewItem != null)
                {
                    TreeViewItem ChildItem = new TreeViewItem
                    {
                        Header = name,
                        Tag = newPath
                    };
                    treeViewItem.Items.Add(ChildItem);

                    //add context menu
                    ChildItem.ContextMenu = new System.Windows.Controls.ContextMenu();
                    var menuItem1 = new MenuItem
                    {
                        Header = "Open",
                        Tag = newPath
                    };
                    menuItem1.Click += new RoutedEventHandler(mainWindow.ContextOpenClick);
                    var menuItem2 = new MenuItem
                    {
                        Header = "Delete",
                        Tag = newPath
                    };
                    menuItem2.Click += new RoutedEventHandler(mainWindow.ContextFileDeleteClick);
                    ChildItem.ContextMenu.Items.Add(menuItem1);
                    ChildItem.ContextMenu.Items.Add(menuItem2);

                    ChildItem.Selected += new RoutedEventHandler(mainWindow.SelectedItem);
                }
                this.Close();
            } 
            else if(DirectoryRadio.IsChecked == true && Regex.IsMatch(name, "^[a-zA-Z0-9_~-]{1,8}")) 
            {
                Directory.CreateDirectory(newPath);
                var dirInfo = new DirectoryInfo(newPath);
                dirInfo.Attributes = attributes;
                if (treeViewItem != null)
                {
                    TreeViewItem ChildItem = new TreeViewItem
                    {
                        Header = name,
                        Tag = newPath
                    };
                    treeViewItem.Items.Add(ChildItem);

                    //add context menu
                    ChildItem.ContextMenu = new System.Windows.Controls.ContextMenu();
                    var menuItem1 = new MenuItem
                    {
                        Header = "Create",
                        Tag = newPath
                    };
                    menuItem1.Click += new RoutedEventHandler(mainWindow.ContextCreateClick);
                    var menuItem2 = new MenuItem
                    {
                        Header = "Delete",
                        Tag = newPath
                    };
                    menuItem2.Click += new RoutedEventHandler(mainWindow.ContextFolderDeleteClick);
                    ChildItem.ContextMenu.Items.Add(menuItem1);
                    ChildItem.ContextMenu.Items.Add(menuItem2);

                    ChildItem.Selected += new RoutedEventHandler(mainWindow.SelectedItem);
                }
                this.Close();
            }
            else
            {
                this.Close();
            }

        }

        private void CreateCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
