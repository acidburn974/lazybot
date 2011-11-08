/*
This file is part of LazyBot - Copyright (C) 2011 Arutha

    LazyBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LazyBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with LazyBot.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar;
using LazyLib;

namespace LazyEvo.LFlyingEngine
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public partial class FlyingProfiles : Office2007Form
    {
        public const string Url = "http://profiles.wow-lazybot.com/listflying.php?username={0}&password={1}";

        public FlyingProfiles()
        {
            InitializeComponent();
        }

        private void BtnDownloadClick(object sender, EventArgs e)
        {
            if (ProfileList.SelectedRows[0] != null)
            {
                DataGridViewRow data = ProfileList.SelectedRows[0];
                var pro = new OnlineProfile(data.Cells[0].Value.ToString(), data.Cells[1].Value.ToString(),
                                            data.Cells[2].Value.ToString(), data.Cells[3].Value.ToString(),
                                            data.Cells[4].Value.ToString(), data.Cells[5].Value.ToString());
                var client = new WebClient();
                string dir = LazySettings.OurDirectory + @"\FlyingProfiles";
                string fileName = dir + @"\" + pro.Name + ".xml";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (File.Exists(fileName))
                {
                    DialogResult dr = MessageBox.Show(pro.Name + " already exist - not saving. Overwrite?", "Overwrite",
                                                      MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.No:
                            return;
                    }
                }
                try
                {
                    string setDowunload =
                        string.Format("http://profiles.wow-lazybot.com/getflying.php?username={0}&password={1}&id={2}",
                                      LazySettings.UserName, LazySettings.Password, pro.Id);
                    WebRequest.Create(setDowunload).GetResponse();
                    string url = string.Format("http://profiles.wow-lazybot.com/flyingprofiles/{0}.xml", pro.Id);
                    client.DownloadFile(url, fileName);
                    InvokeProfileDownload(new EProfileDownloaded(fileName));
                    MessageBox.Show("Profile downloaded and loaded");
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Download error !\n" + exception.Message);
                }
            }
            else
            {
                MessageBox.Show("Selected a profile before downloading");
            }
        }

        private void BtnRefreshClick(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            try
            {
                BtnRefresh.Enabled = false;
                Refresh();
                string url = string.Format(Url, LazySettings.UserName, LazySettings.Password);
                var reader = new StreamReader(WebRequest.Create(url).GetResponse().GetResponseStream(), Encoding.UTF8);
                var doc = new XmlDocument();
                string test = reader.ReadToEnd().Replace("\r", "");
                doc.LoadXml(test);
                reader.Close();
                XmlNodeList profiles = doc.GetElementsByTagName("Profile");
                var list = new List<OnlineProfile>();
                foreach (XmlNode profile in profiles)
                {
                    string id = "";
                    string creator = "";
                    string name = "";
                    string zone = "";
                    string comment = "";
                    string downloads = "";
                    foreach (XmlNode childNode in profile.ChildNodes)
                    {
                        switch (childNode.Name)
                        {
                            case "id":
                                id = childNode.InnerText;
                                break;
                            case "creator":
                                creator = childNode.InnerText;
                                break;
                            case "name":
                                name = childNode.InnerText;
                                break;
                            case "zone":
                                zone = childNode.InnerText;
                                break;
                            case "comment":
                                comment = childNode.InnerText;
                                break;
                            case "downloads":
                                downloads = childNode.InnerText;
                                break;
                        }
                    }
                    list.Add(new OnlineProfile(id, name, creator, zone, comment, downloads));
                }
                ProfileList.DataSource = list;
                ProfileList.Columns[0].Visible = false;
                BtnRefresh.Enabled = true;
            }
            catch (Exception e)
            {
                BtnRefresh.Enabled = false;
                MessageBox.Show("Could not refresh the list !\n" + e);
            }
        }

        public event EventHandler<EProfileDownloaded> ProfileDownload;

        private void InvokeProfileDownload(EProfileDownloaded e)
        {
            EventHandler<EProfileDownloaded> handler = ProfileDownload;
            if (handler != null)
                handler(null, e);
        }

        private void BtnBrowseClick(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
                          {
                              InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                              Filter = @"Profile (*.xml)|*.xml"
                          };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(dlg.FileName) == ".xml")
                {
                    if (ValidateProfile(dlg.FileName))
                    {
                        TBProfile.Text = dlg.FileName;
                    }
                    else
                    {
                        MessageBox.Show("Invalid profile loaded");
                        TBProfile.Text = string.Empty;
                    }
                }
            }
        }

        private void BtnBrowseImageClick(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
                          {
                              InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                              Filter = @"Image (*.jpg)|*.jpg;(*.gif)|*.gif;(*.png)|*.png"
                          };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(dlg.FileName) == ".jpg" || Path.GetExtension(dlg.FileName) == ".gif" ||
                    Path.GetExtension(dlg.FileName) == ".png")
                {
                    TBImage.Text = dlg.FileName;
                }
            }
        }

        private void BtnUploadClick(object sender, EventArgs e)
        {
            if (TBProfile.Text != string.Empty && TBName.Text != string.Empty && TBZone.Text != string.Empty)
            {
                var client = new WebClient();
                client.Headers.Add("Content-Type", "binary/octet-stream");
                string url =
                    string.Format(
                        "http://profiles.wow-lazybot.com/sendflying.php?username={0}&password={1}&name={2}&zone={3}&comment={4}",
                        LazySettings.UserName, LazySettings.Password, TBName.Text, TBZone.Text, TBComment.Text);
                byte[] result = client.UploadFile(url, "POST", TBProfile.Text);
                string s = Encoding.UTF8.GetString(result, 0, result.Length);
                if (s.Contains("Key:"))
                {
                    TBProfile.Text = string.Empty;
                    TBImage.Text = string.Empty;
                    TBZone.Text = string.Empty;
                    TBComment.Text = string.Empty;
                    TBName.Text = string.Empty;
                    MessageBox.Show("Upload ok");
                }
                else
                {
                    MessageBox.Show("Could not upload: " + s);
                }
            }
            else
            {
                MessageBox.Show("Profile, Name, Zone are required");
            }
        }

        private bool ValidateProfile(string fileName)
        {
            var doc = new XmlDocument();
            try
            {
                doc.Load(fileName);
            }
            catch (Exception e)
            {
                return false;
            }
            if (doc.GetElementsByTagName("Profile")[0] == null)
                return false;
            if (doc.GetElementsByTagName("Waypoint")[0] == null)
                return false;
            return true;
        }
    }

    public class OnlineProfile
    {
        public OnlineProfile(string id, string name, string creator, string zone, string comment, string downloads)
        {
            Name = name;
            Creator = creator;
            Zone = zone;
            Comment = comment;
            Downloads = downloads;
            Id = id;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Creator { get; private set; }
        public string Zone { get; private set; }
        public string Comment { get; private set; }
        public string Downloads { get; private set; }
    }

    public class EProfileDownloaded : EventArgs
    {
        public EProfileDownloaded(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }
    }
}