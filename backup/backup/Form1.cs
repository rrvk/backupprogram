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
        private String rootFolder;
        delegate void SetTextCallback(string text);
        delegate void ToolStripPrograssDelegate(int value);
        private Boolean stop = false;
        private Thread workerThread;
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

        public void progressBarStep(int steps)
        {
            if (progressBar1.InvokeRequired)
            {
                ToolStripPrograssDelegate del = new ToolStripPrograssDelegate(progressBarStep);
                progressBar1.Invoke(del, new object[] { steps });
            }
            else
            {
                // voor het geval dat er teveel steps worden gedaan
                if (progressBar1.Value != progressBar1.Maximum)
                {
                    progressBar1.Value += steps;
                }
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
            // tellen hoeveel items er zijn
            int temp = 0;
            foreach (String path in lstbackup.Items)
            {
                try
                {
                    temp += Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;
                }
                catch (UnauthorizedAccessException ex) { Console.WriteLine(ex.Message); }
            }
            progressBar1.Maximum = temp;
            workerThread = new Thread(checkBackupSettings);
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
                    // for the thread stop
                    if (stop) { return; }
                    if (Directory.Exists(path))
                    {
                        // dit zodat orignele path kunnen opvragen
                        // todo misschien netter??
                        DirectoryInfo dir = new DirectoryInfo(path);
                        lastFolder = dir.Parent.FullName;
                        rootFolder = dir.Name;
                        // TODO DIT NOG CROSS THREAD
                        //btnStart.Enabled = false;
                        backUp(path);
                    }
                    else
                    {
                        var ant = MessageBox.Show(path + " does not exist do you want to continue?", "Continue?", MessageBoxButtons.YesNo);
                        if (ant == DialogResult.No) { /*// TODO DIT NOG CROSS THREADbtnStart.Enabled = true; */setProgressLabel("Error"); return; }
                    }
                }
                setProgressLabel("finished");
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
            DirectoryInfo[] folders = new DirectoryInfo[1];
            folders[0] = new DirectoryInfo(path);
            checkDirectorys(folders, "");
        }
        

        private void checkDirectorys(DirectoryInfo[] folders, String path)
        {
            
            // door alle folders heen gaan
            foreach (DirectoryInfo dir in folders)
            {
                // for the thread stop
                if (stop) { return; }
                // controleer of het bestaad zo niet aanmaken
                checkFolderAndCreate(Path.Combine(savePath,path, dir.Name));
                // kijken of er supfolders zijn
                DirectoryInfo[] supFolders = new DirectoryInfo(dir.FullName).GetDirectories();
                if (supFolders.Length > 0)
                {
                    // zo ja roepen we deze functies weer lekker aan
                    checkDirectorys(supFolders, Path.Combine(path,dir.Name));
                }                
                //de bestanden controleren
                String[] filePaths = Directory.GetFiles(Path.Combine(lastFolder, path,dir.Name));
                if (filePaths.Length>0){
                    foreach(String file in filePaths){
                        try{
                            // for the thread stop
                            if (stop) { return;  }
                            FileInfo fN = new FileInfo(file);
                            FileInfo fB = new FileInfo(Path.Combine(savePath,path,dir.Name,fN.Name));
                            setProgressLabel(fN.FullName);
                            if (fB.Exists){
                                // todo misschien aanpassen omdat iemand de LastWriteTime kan aanpassen
                                // TODO sha256 hash maken
                                if (fN.LastWriteTime != fB.LastWriteTime) {
                                    String d = DateTime.Now.ToString("mm") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yyyy");
                                    File.Move(fB.FullName, Path.Combine(fB.DirectoryName,fB.Name+"BK"+d));
                                    //File.Delete(fB.FullName);
                                    //fB.Name = "test";
                                    fN.CopyTo(fB.FullName);
                                }
                            }
                            else
                            {
                                fN.CopyTo(fB.FullName);
                            }
                        }
                        catch (UnauthorizedAccessException ex) { Console.WriteLine(ex.Message); }
                        progressBarStep(1);
                    }
                }                
            }
        }

        private void checkFolderAndCreate(String path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void Form1_FormClosing(object sender2, FormClosingEventArgs e)
        {
            if (workerThread!=null && !workerThread.Join(0))
            {
                e.Cancel = true; // Cancel the shutdown of the form.
                stop = true; // Signal worker thread that it should gracefully shutdown.
                var timer = new System.Timers.Timer();
                timer.AutoReset = false;
                timer.SynchronizingObject = this;
                timer.Interval = 500;
                timer.Elapsed +=
                  (sender, args) =>
                  {
                      // Do a fast check to see if the worker thread is still running.
                      if (workerThread.Join(0))
                      {
                          // Reissue the form closing event.
                          Close();
                      }
                      else
                      {
                          // Keep restarting the timer until the worker thread ends.
                          timer.Start();
                      }
                  };
                timer.Start();
            }
        }
    }
}
