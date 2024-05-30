using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBK_change_filename
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnChangeFileName_Click(object sender, EventArgs e)
        {
            string folderPath = txtFolderPath.Text;
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Directory does not exist.");
                return;
            }

            // 지정된 경로 및 하위 경로에서 모든 이미지 파일과 텍스트 파일을 검색
            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(folderPath, "*.jpg", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(folderPath, "*.jpeg", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories));

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                // 파일명 분할: 'A_B_C' 형식 가정
                string[] parts = fileName.Split('_');
                if (parts.Length == 3) // A, B, C 3부분으로 나뉘어야 함
                {
                    // 파일명 재구성: 'A_C_B'
                    //string newFileName = $"{parts[0]}_{parts[2]}_{parts[1]}{extension}";
                    string newFileName = $"{parts[0]}){parts[1]}_{parts[2]}{extension}";
                    string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);

                    // 실제 파일명 변경
                    try
                    {
                        File.Move(filePath, newFilePath);
                        //MessageBox.Show($"Renamed {fileName}{extension} to {newFileName}");
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"Error renaming file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            MessageBox.Show($"Done");
        }

        private void btnChangeFileName2_Click(object sender, EventArgs e)
        {
            string folderPath2 = txtFolderPath2.Text;  // txtFolderPath2는 폴더 경로를 입력받는 텍스트 박스
            int renameNumber;

            if (!Directory.Exists(folderPath2))
            {
                MessageBox.Show("Directory does not exist.");
                return;
            }

            if (!int.TryParse(textBox3.Text, out renameNumber))  // textBox3는 숫자를 입력받는 텍스트 박스
            {
                MessageBox.Show("Invalid number.");
                return;
            }

            // 지정된 경로 및 하위 경로에서 모든 이미지 파일을 검색
            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(folderPath2, "*.jpg", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(folderPath2, "*.jpeg", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(folderPath2, "*.png", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(folderPath2, "*.txt", SearchOption.AllDirectories));

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                // 파일명 분할: 'A_B_C' 형식 가정
                string[] parts = fileName.Split('_');
                if (parts.Length >= 2)  // A, B, C (C는 변경될 부분)
                {
                    // 파일명 재구성: 'A_B_renameNumber'
                    string newFileName = $"{parts[0]}_{parts[1]}_{renameNumber}{extension}";
                    string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);

                    // 실제 파일명 변경
                    try
                    {
                        File.Move(filePath, newFilePath);
                        //MessageBox.Show($"Renamed {fileName}{extension} to {newFileName}");
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"Error renaming file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            MessageBox.Show($"Done");
        }
    }
}
