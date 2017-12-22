using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Threading;

namespace WebsiteCrawler
{
    public partial class frmMain : Form
    {
        private List<string> mlstPages;
        private bool mblnExiting = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartWebsiteCrawling();
        }

        private void txtURL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                StartWebsiteCrawling();
            }
        }

        private void StartWebsiteCrawling()
        {
            string strURL = txtURL.Text;
            string strFilePath;
            int intFileCount = 0;
                        
            mlstPages = new List<string>();
            
            // make sure it starts with http://
            if (!strURL.ToLower().StartsWith("http"))
            {
                strURL = "http://" + strURL;
            }

            mlstPages.Add(strURL);
            while (mlstPages.Count > 0 & !mblnExiting)
            {
                // get top url on the stack
                strURL = mlstPages[0];
                mlstPages.RemoveAt(0);
                strFilePath = GetLocalPathForUrl(strURL, false);
                if (!File.Exists(strFilePath) & !Directory.Exists(strFilePath))
                {                    
                    if (!mblnExiting)
                    {
                        byte[] arrContents = GetURLContents(strURL);
                        if (!strURL.EndsWith("/") & arrContents.Length > 0)  // not a folder
                        {
                            strFilePath = GetLocalPathForUrl(strURL, true);
                            SaveContentsToFile(strFilePath, ref arrContents);
                            if (intFileCount >= 5)   // sleep a few seconds after every few files downloaded
                            {
                                intFileCount = 0;
                                for (int intCount = 0; intCount < 20 & !mblnExiting; intCount++)  // 2 seconds
                                {
                                    Thread.Sleep(100);
                                    Application.DoEvents();
                                }
                            }
                            intFileCount++;

                        }
                        string strContents = Encoding.Default.GetString(arrContents);
                        AddLinkedPages(ref strContents, strURL);
                    }
                }
            }
            txtPages.Text += "Done." + "\r\n";
            txtPages.SelectionStart = txtPages.Text.Length - 1;
            txtPages.ScrollToCaret();
            Application.DoEvents();
        }

        private byte[] GetURLContents(string strURL)
        {
            byte[] arrBuffer ;
                byte[] arrAllContents;
            long lngCountBytes;
            long lngTotalBytes = 0;
            List<byte[]> lstBuffers;
            List<long> lstBufferSizes;

            txtPages.Text += strURL + "\r\n";
            txtPages.SelectionStart = txtPages.Text.Length - 1;
            txtPages.ScrollToCaret();
            Application.DoEvents();

            // make sure it starts with http://
            if (!strURL.ToLower().StartsWith("http"))
            {
                strURL = "http://" + strURL;
            }

            lstBuffers = new List<byte[]>();
            lstBufferSizes = new List<long>();

            try
            {
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(strURL);
                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                Stream objStream = objResponse.GetResponseStream();
                do
                {
                    arrBuffer = new byte[8192];
                    lngCountBytes = objStream.Read(arrBuffer, 0, arrBuffer.Length);
                    if (lngCountBytes != 0)
                    {
                        lstBuffers.Add(arrBuffer);
                        lstBufferSizes.Add(lngCountBytes);
                        lngTotalBytes += lngCountBytes;
                    }
                }
                while (lngCountBytes > 0);

                arrAllContents = new byte[lngTotalBytes];
                lngTotalBytes = 0;
                for (int intBufferCount = 0; intBufferCount < lstBuffers.Count; intBufferCount++)
                {
                    Array.Copy(lstBuffers[intBufferCount], 0, arrAllContents, lngTotalBytes, lstBufferSizes[intBufferCount]);
                    lngTotalBytes += lstBufferSizes[intBufferCount];
                }
                
                return arrAllContents;
            }
            catch 
            {
                return new byte[0];
            }
        }

        private void SaveContentsToFile(string pstrFilePath, ref byte[] parrContents)
        {
            FileStream objFile;

            if (!Directory.Exists(pstrFilePath))  //  a file should not overwrite a folder
            {
                if (pstrFilePath.Length < 248)
                {
                    objFile = new FileStream(pstrFilePath, FileMode.Create, FileAccess.Write);
                    objFile.Write(parrContents, 0, parrContents.Length);
                    objFile.Close();
                }
            }
        }

        private string GetLocalPathForUrl(string pstrURL, bool blnCreate)
        // get path where URL will be saved, and create it if it doesn't exist
        {
            string strFullPath;
            string strURLBase;
            int intStartPosittion;
            int intEndPosition;
            bool blnFinishedParsingPath = false;
            string strURLWithoutQuery;

            // create SavedWebsites folder
            strFullPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\SavedWebsites\\";
            if (!Directory.Exists(strFullPath) & blnCreate)
            {
                Directory.CreateDirectory(strFullPath);
            }

            // make sure it starts with http://
            if (!pstrURL.ToLower().StartsWith("http"))
            {
                pstrURL = "http://" + pstrURL;
            }

            strURLWithoutQuery = new Uri(pstrURL).GetLeftPart(UriPartial.Path);

            // remove http:// , https:// , ..
            strURLBase = GetURLBase (strURLWithoutQuery);
            if (strURLBase.Contains("//"))
            {
                strURLBase = strURLBase.Substring(strURLBase.IndexOf("//") + 2);
            }

            // remove www. and all site prefixes
            if (strURLBase.Length - strURLBase.Replace(".", "").Length >= 2)
            // 2 or more dots in url base
            {
                // cut at 2nd dot from the right
                strURLBase = strURLBase.Substring(strURLBase.Substring(0, strURLBase.LastIndexOf(".")).LastIndexOf(".") + 1);                
            }

            // create website folder
            strFullPath += strURLBase;
            if (!Directory.Exists(strFullPath) & blnCreate)
            {
                Directory.CreateDirectory(strFullPath);
            }

            // move position to the base URL
            intStartPosittion = strURLWithoutQuery.IndexOf(strURLBase);

            // move to end of base URL
            intStartPosittion += strURLBase.Length;

            // remove unwanted characters
            strURLWithoutQuery = strURLWithoutQuery.Replace(":", "_");

            while (!blnFinishedParsingPath)
            {
                // still more characters after base URL
                if (strURLWithoutQuery.Length > intStartPosittion)
                {
                    intEndPosition = strURLWithoutQuery.IndexOf("/", intStartPosittion + 1);
                    if (intEndPosition != -1) // found another '/'
                    {
                        strFullPath += "\\" + strURLWithoutQuery.Substring(intStartPosittion + 1, intEndPosition - intStartPosittion - 1);
                        if (!Directory.Exists(strFullPath) & blnCreate)
                        {
                            if (File.Exists(strFullPath))  // delete file if it exists with the same name as the folder
                            {
                                File.Delete(strFullPath);
                            }
                            Directory.CreateDirectory(strFullPath);
                        }
                        intStartPosittion = intEndPosition;
                    }
                    else // no more forwards slashes
                    {
                        if (strURLWithoutQuery.Length > intStartPosittion + 1)     // there is text after the last forward slash, treat it as a file
                        {
                            strFullPath += "\\" + strURLWithoutQuery.Substring(intStartPosittion + 1);
                        }
                        blnFinishedParsingPath = true;
                    }
                }
                else
                {
                    blnFinishedParsingPath = true;
                }
            }

            // add the query part
            if (pstrURL.Length > strURLWithoutQuery.Length)
            {
                strFullPath = strFullPath + pstrURL.Substring(strURLWithoutQuery.Length, pstrURL.Length - strURLWithoutQuery.Length).Replace("/", "_").Replace("?", "_").Replace(":", "_");
            }
            return strFullPath;
        }

        private void AddLinkedPages(ref string pstrContents, string pstrURL)
        {
            int intPosition1 = 0;
            int intPosition2;
            int intPosition3;
            int intPosition4;
            bool blnFinished = false;
            string strLink;

            while (!blnFinished)
            {
                intPosition1 = pstrContents.IndexOf("href", intPosition1, StringComparison.CurrentCultureIgnoreCase);
                if (intPosition1 != -1)
                {
                    intPosition2 = pstrContents.IndexOf("=", intPosition1);
                    if (intPosition2 != -1)
                    {
                        if (pstrContents.Substring(intPosition1 + 4, intPosition2 - intPosition1 - 4).Trim() == "")   // nothing between the href and the =
                        {
                            intPosition3 = pstrContents.IndexOf("\"", intPosition2);
                            if (intPosition3 != -1)
                            {
                                if (pstrContents.Substring(intPosition2 + 1, intPosition3 - intPosition2 - 1).Trim() == "") // nothing between the = and the "
                                {
                                    intPosition4 = pstrContents.IndexOf("\"", intPosition3 + 1);
                                    if (intPosition4 != -1)
                                    {
                                        strLink = pstrContents.Substring(intPosition3 + 1, intPosition4 - intPosition3 - 1); // link is between the 2 quotes
                                        if (strLink.StartsWith("/")) // relative link on same website
                                        {
                                            strLink = GetURLBase(pstrURL) + strLink;
                                            mlstPages.Add(strLink);
                                        }
                                        else
                                        {
                                            if (strLink.ToLower().StartsWith("http"))
                                            {
                                                // must be on the same website
                                                if (GetURLBase(pstrURL).Replace("//www.", "//") == GetURLBase(strLink).Replace("//www.", "//"))
                                                {
                                                    mlstPages.Add(strLink);
                                                }
                                            }
                                            else
                                            {
                                                // a file on the same folder as the current url
                                                int intPosition = new Uri(pstrURL).GetLeftPart(UriPartial.Path).LastIndexOf("/");
                                                if (intPosition != -1)
                                                {
                                                    // add link name to url folder name
                                                    strLink = new Uri(pstrURL).GetLeftPart(UriPartial.Path).Substring(0, intPosition + 1) + strLink;
                                                    mlstPages.Add(strLink);
                                                }
                                            }
                                        }
                                        intPosition1 = intPosition4;
                                    }
                                    else
                                    {
                                        blnFinished = true;  // no closing quote 
                                    }
                                }
                                else
                                {
                                    intPosition1 += 4;  // skip the word href
                                }
                            }
                            else
                            {
                                intPosition1 += 4;  // skip the word href
                            }
                        }
                        else
                        {
                            intPosition1 += 4;   // skip the word href
                        }
                    }
                    else
                    {
                        blnFinished = true;  // no equal sign
                    }
                }
                else
                {
                    blnFinished = true;   // no href
                }
            }
        }

        private string GetURLBase(string pstrURL)
        {
            try
            {
                return new Uri(pstrURL).GetLeftPart(UriPartial.Authority);
            }
            catch
            {
                return pstrURL;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            mblnExiting = true;
        }

    }
}
