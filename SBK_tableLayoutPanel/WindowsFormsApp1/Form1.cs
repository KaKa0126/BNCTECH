using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            JudgeImageLoad();
            SampleImageLoad();
        }

        public void SampleImageLoad()
        {
            // pictureBox1 설정
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 이미지 로드 - 실제 경로로 변경 필요
            try
            {
                this.pictureBox1.Image = Image.FromFile("C:\\Users\\user\\Desktop\\TEST\\TGA_1P_1L\\MAE1624ST01_1_240417091822.jpg");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load image: " + ex.Message);
            }
        }

        public void JudgeImageLoad()
        {
            // pictureBox1 설정
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 이미지 로드 - 실제 경로로 변경 필요
            try
            {
                this.pictureBox2.Image = Image.FromFile("C:\\Users\\user\\Desktop\\TEST\\TGA_2P_2L\\MAE1624ST01_2_240306135342.jpg");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load image: " + ex.Message);
            }
            // PictureBox를 (1,0) 위치에 추가하고 RowSpan을 2로 설정하여 셀을 합침
            this.pictureBox2.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 0, 1);
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox2, 2); // (1,0)과 (1,1) 셀 합치기
        }

    }
}
