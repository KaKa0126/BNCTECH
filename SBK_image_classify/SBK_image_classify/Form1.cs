using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBK_image_classify
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sourceFolderPath = textBox1.Text;
            string destinationFolderPath = textBox2.Text;
            //파일 취합은 2024년 3월 6일 12시 이후의 이미지를 대상으로 함
            DateTime generalDateTime = new DateTime(2024, 3, 6, 12, 0, 0);
            //41번 포인트(기구물 수정)에 대하여 파일 취합은 2024년 3월 7일 12시 이후의 이미지를 대상으로 함
            DateTime specificDateTime = new DateTime(2024, 3, 7, 12, 0, 0);

            ProcessDirectory(sourceFolderPath, destinationFolderPath, generalDateTime, specificDateTime);

            MessageBox.Show("파일 분류 및 복사가 완료되었습니다.");
        }

        private void ProcessDirectory(string sourceFolderPath, string destinationFolderPath, DateTime generalDateTime, DateTime specificDateTime)
        {
            ProcessFilesInDirectory(sourceFolderPath, destinationFolderPath, generalDateTime, specificDateTime);
            foreach (var directory in Directory.GetDirectories(sourceFolderPath))
            {
                ProcessDirectory(directory, destinationFolderPath, generalDateTime, specificDateTime);
            }
        }

        private void ProcessFilesInDirectory(string folderPath, string destinationFolderPath, DateTime generalDateTime, DateTime specificDateTime)
        {
            Regex pattern = new Regex(@"_(\d+)_");
            foreach (var file in Directory.GetFiles(folderPath))
            {
                FileInfo fileInfo = new FileInfo(file);
                string fileName = Path.GetFileName(file);
                Match match = pattern.Match(fileName);

                if (match.Success)
                {
                    string folderNumber = match.Groups[1].Value;
                    string targetFolder = Path.Combine(destinationFolderPath, folderNumber + "P");
                    Directory.CreateDirectory(targetFolder);

                    DateTime targetDateTime = fileInfo.CreationTime;
                    if (folderNumber.Equals("41"))
                    {
                        // '_41_' 파일에 대해 특별한 날짜 조건을 적용
                        if (targetDateTime > specificDateTime)
                        {
                            string destFile = Path.Combine(targetFolder, fileName);
                            File.Copy(file, destFile, true);
                        }
                    }
                    else
                    {
                        // 나머지 파일에 대해 일반적인 날짜 조건을 적용
                        if (targetDateTime > generalDateTime)
                        {
                            string destFile = Path.Combine(targetFolder, fileName);
                            File.Copy(file, destFile, true);
                        }
                    }
                }
            }
        }
    }
}
