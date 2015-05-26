using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace backup
{
    public partial class Form1 : Form
    {
        private String savePath;
        private readonly String addString = "Add new entry";
        private ContextMenuStrip listboxContextMenu;
        private String lastFolder;
        delegate void SetTextCallback(string text);
        public Form1()
        {
            InitializeComponent();
            // make the right click on the lstbox
            listboxContextMenu = new ContextMenuStrip();
            listboxContextMenu.Opening += new CancelEventHandler(listboxContextMenu_Opening);
            listboxContextMenu.ItemClicked += listboxContextMenu_ItemClicked;
            lstbackup.ContextMenuStrip = listboxContextMenu;
            savePath = lblLocation.Text.ToString();
        }

        private void setProgressLabel(String tekst)
        {
            if (this.lblProgress.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(setProgressLabel);
                this.Invoke(d, new object[] { tekst });
            }
            else
            {
                lblProgress.Text = tekst;
            }
        }

        private void listboxContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.ToString() == addString)
            {
                FolderBrowserDialog folder = new FolderBrowserDialog();
                if (folder.ShowDialog() == DialogResult.OK)
                {
                    lstbackup.Items.Add(folder.SelectedPath);
                }
            }
            else if (e.ClickedItem.ToString().Contains("Remove"))
            {
                lstbackup.Items.RemoveAt(lstbackup.SelectedIndex);
            }
        }

        private void lblLocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                savePath = folder.SelectedPath;
                lblLocation.Text = savePath;
            }
        }

        private void lstbackup_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                lstbackup.SelectedIndex = lstbackup.IndexFromPoint(e.Location);
                listboxContextMenu.Show();  
            }
        }

        private void listboxContextMenu_Opening(object sender, CancelEventArgs e)
        {
            //clear the menu and add custom items
            listboxContextMenu.Items.Clear();
            listboxContextMenu.Items.Add(addString);
            if (lstbackup.SelectedIndex != -1)
            {
                listboxContextMenu.Items.Add(string.Format("Remove - {0}", lstbackup.SelectedItem.ToString()));
            }
        }

        private void lstbackup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (lstbackup.SelectedIndex != -1)
                {
                    lstbackup.Items.RemoveAt(lstbackup.SelectedIndex);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Thread workerThread = new Thread(checkBackupSettings);
            workerThread.Start();
        }

        private void checkBackupSettings()
        {
            if (lstbackup.Items.Count == 0)
            {
                MessageBox.Show("Geen backup locaties");
                return;
            }
            if (Directory.Exists(savePath))
            {
                foreach (String path in lstbackup.Items)
                {
                    if (Directory.Exists(path))
                    {
                        // TODO DIT NOG CROSS THREAD
                        //btnStart.Enabled = false;
                        backUp(path);
                    }
                    else
                    {
                        var ant = MessageBox.Show(path + " does not exist do you want to continue?", "Continue?", MessageBoxButtons.YesNo);
                        if (ant == DialogResult.No) { /*// TODO DIT NOG CROSS THREADbtnStart.Enabled = true; */return; }
                    }
                }
            }
            else
            {
                MessageBox.Show("The save directory does not exist");
            }
            // TODO DIT NOG CROSS THREAD
            //btnStart.Enabled = true;
        }

        private void backUp(String path)
        {
            //String[] filePath = Directory.GetFiles(path,"*.*",SearchOption.AllDirectories);
            // first get the root folder
            string dirName = new DirectoryInfo(path).Name;
            // check if root directory exist in backup location
            checkFolderAndCreate(Path.Combine(savePath, dirName));
            // then every folder within
            // loop through all the maps
            lastFolder = path;
            checkDirectorys(new DirectoryInfo(path).GetDirectories());
            /*foreach(String file in filePath){
                dirName = new DirectoryInfo(file).Name;
                setProgressLabel(file);
                Console.WriteLine(file);
            }*/
        }
        

        private void checkDirectorys(DirectoryInfo[] folders)
        {
            foreach (DirectoryInfo dir in folders)
            {
                DirectoryInfo[] supFolders = new DirectoryInfo(dir.FullName).GetDirectories();
                if (supFolders.Length > 0)
                {
                    checkDirectorys(supFolders);
                }
                Console.WriteLine(dir.Name);
                //DirectoryInfo[] supFolders = new DirectoryInfo(dir.Name);
            }
        }

        private void checkFolderAndCreate(String path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
