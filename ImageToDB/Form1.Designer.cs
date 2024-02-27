namespace TEST2
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.Pic1 = new System.Windows.Forms.PictureBox();
            this.Pic2 = new System.Windows.Forms.PictureBox();
            this.BtnStart = new System.Windows.Forms.Button();
            this.BtnCapture = new System.Windows.Forms.Button();
            this.BtnDBinsert = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.Pic3 = new System.Windows.Forms.PictureBox();
            this.BtnSerachingDB = new System.Windows.Forms.Button();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.comboBoxTitles = new System.Windows.Forms.ComboBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.Pic4 = new System.Windows.Forms.PictureBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.TXT_Result = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Pic1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic4)).BeginInit();
            this.SuspendLayout();
            // 
            // Pic1
            // 
            this.Pic1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic1.Location = new System.Drawing.Point(39, 94);
            this.Pic1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Pic1.Name = "Pic1";
            this.Pic1.Size = new System.Drawing.Size(431, 358);
            this.Pic1.TabIndex = 0;
            this.Pic1.TabStop = false;
            // 
            // Pic2
            // 
            this.Pic2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic2.Location = new System.Drawing.Point(537, 94);
            this.Pic2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Pic2.Name = "Pic2";
            this.Pic2.Size = new System.Drawing.Size(431, 358);
            this.Pic2.TabIndex = 1;
            this.Pic2.TabStop = false;
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(39, 560);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(173, 130);
            this.BtnStart.TabIndex = 2;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // BtnCapture
            // 
            this.BtnCapture.Location = new System.Drawing.Point(537, 560);
            this.BtnCapture.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BtnCapture.Name = "BtnCapture";
            this.BtnCapture.Size = new System.Drawing.Size(173, 130);
            this.BtnCapture.TabIndex = 3;
            this.BtnCapture.Text = "Capture";
            this.BtnCapture.UseVisualStyleBackColor = true;
            this.BtnCapture.Click += new System.EventHandler(this.BtnCapture_Click);
            // 
            // BtnDBinsert
            // 
            this.BtnDBinsert.Location = new System.Drawing.Point(1027, 560);
            this.BtnDBinsert.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BtnDBinsert.Name = "BtnDBinsert";
            this.BtnDBinsert.Size = new System.Drawing.Size(173, 130);
            this.BtnDBinsert.TabIndex = 4;
            this.BtnDBinsert.Text = "DB Insert";
            this.BtnDBinsert.UseVisualStyleBackColor = true;
            this.BtnDBinsert.Click += new System.EventHandler(this.BtnDBinsert_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(39, 40);
            this.textBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(152, 28);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "Camera Live";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(537, 40);
            this.textBox2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(173, 28);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "Capture image";
            // 
            // Pic3
            // 
            this.Pic3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic3.Location = new System.Drawing.Point(537, 802);
            this.Pic3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Pic3.Name = "Pic3";
            this.Pic3.Size = new System.Drawing.Size(431, 358);
            this.Pic3.TabIndex = 7;
            this.Pic3.TabStop = false;
            // 
            // BtnSerachingDB
            // 
            this.BtnSerachingDB.Location = new System.Drawing.Point(39, 878);
            this.BtnSerachingDB.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.BtnSerachingDB.Name = "BtnSerachingDB";
            this.BtnSerachingDB.Size = new System.Drawing.Size(173, 130);
            this.BtnSerachingDB.TabIndex = 8;
            this.BtnSerachingDB.Text = "DB Searching";
            this.BtnSerachingDB.UseVisualStyleBackColor = true;
            this.BtnSerachingDB.Click += new System.EventHandler(this.BtnSerachingDB_Click);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(39, 802);
            this.dateTimePicker.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(368, 35);
            this.dateTimePicker.TabIndex = 9;
            // 
            // comboBoxTitles
            // 
            this.comboBoxTitles.FormattingEnabled = true;
            this.comboBoxTitles.Location = new System.Drawing.Point(39, 1020);
            this.comboBoxTitles.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBoxTitles.Name = "comboBoxTitles";
            this.comboBoxTitles.Size = new System.Drawing.Size(368, 32);
            this.comboBoxTitles.TabIndex = 10;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(1027, 40);
            this.textBox3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(173, 28);
            this.textBox3.TabIndex = 11;
            this.textBox3.Text = "AI Output image";
            // 
            // Pic4
            // 
            this.Pic4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic4.Location = new System.Drawing.Point(1027, 94);
            this.Pic4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Pic4.Name = "Pic4";
            this.Pic4.Size = new System.Drawing.Size(431, 358);
            this.Pic4.TabIndex = 12;
            this.Pic4.TabStop = false;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(1027, 494);
            this.textBox4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(173, 28);
            this.textBox4.TabIndex = 13;
            this.textBox4.Text = "Result";
            // 
            // TXT_Result
            // 
            this.TXT_Result.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TXT_Result.Location = new System.Drawing.Point(1116, 494);
            this.TXT_Result.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.TXT_Result.Name = "TXT_Result";
            this.TXT_Result.Size = new System.Drawing.Size(344, 28);
            this.TXT_Result.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1642, 1295);
            this.Controls.Add(this.TXT_Result);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.Pic4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.comboBoxTitles);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.BtnSerachingDB);
            this.Controls.Add(this.Pic3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.BtnDBinsert);
            this.Controls.Add(this.BtnCapture);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.Pic2);
            this.Controls.Add(this.Pic1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.Pic1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Pic1;
        private System.Windows.Forms.PictureBox Pic2;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Button BtnCapture;
        private System.Windows.Forms.Button BtnDBinsert;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.PictureBox Pic3;
        private System.Windows.Forms.Button BtnSerachingDB;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.ComboBox comboBoxTitles;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.PictureBox Pic4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox TXT_Result;
    }
}

