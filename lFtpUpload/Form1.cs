using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace lFtpUpload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async    void Form1_Load(object sender, EventArgs e)
        {
            await Upload();
        }
        public async Task Upload()
        {
            string FilePath = "uploadXml.xml";
            if (File.Exists(FilePath))
            {
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(FilePath);
                    XmlNodeList xmlNode = xml.SelectNodes("/file");
                    
                    foreach (XmlNode fileNode in xmlNode)
                    {
                        string Server = fileNode.SelectSingleNode("server").InnerText;
                        string Username = fileNode.SelectSingleNode("username").InnerText;
                        string Password = fileNode.SelectSingleNode("password").InnerText;
                        string RemoteFilePath = fileNode.SelectSingleNode("remotefilepath").InnerText;
                        string LocalFilePath = fileNode.SelectSingleNode("localfilepath").InnerText;

                        FtpEntity ftpEntity = new FtpEntity
                        {
                            Server = Server,
                            Username = Username,
                            Password = Password,
                            RemoteFilePath = RemoteFilePath,
                            LocalFilePath = LocalFilePath
                        };
                        
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{ftpEntity.Server}{ftpEntity.RemoteFilePath}");
                        request.Method = WebRequestMethods.Ftp.UploadFile;
                        request.Credentials = new NetworkCredential(ftpEntity.Username, ftpEntity.Password);



                        using (Stream fileStream = File.OpenRead(ftpEntity.LocalFilePath))
                        using (Stream ftpStream = await request.GetRequestStreamAsync())
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead;
                            while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                label1.Text = "Yükleniyor...";
                                await ftpStream.WriteAsync(buffer, 0, bytesRead);
                            }
                        }

                        this.Close();
                        MessageBox.Show("Dosya Başarıyla Yüklendi!");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata!" + ex.ToString());
                }

            }
        }
    }
}
