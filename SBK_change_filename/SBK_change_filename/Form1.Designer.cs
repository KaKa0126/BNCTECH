namespace SBK_change_filename
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
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.btnChangeFileName = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFolderPath2 = new System.Windows.Forms.TextBox();
            this.btnChangeFileName2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Location = new System.Drawing.Point(55, 109);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(365, 35);
            this.txtFolderPath.TabIndex = 0;
            // 
            // btnChangeFileName
            // 
            this.btnChangeFileName.Location = new System.Drawing.Point(505, 109);
            this.btnChangeFileName.Name = "btnChangeFileName";
            this.btnChangeFileName.Size = new System.Drawing.Size(245, 103);
            this.btnChangeFileName.TabIndex = 1;
            this.btnChangeFileName.Text = "btnChangeFileName";
            this.btnChangeFileName.UseVisualStyleBackColor = true;
            this.btnChangeFileName.Click += new System.EventHandler(this.btnChangeFileName_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1270, 48);
            this.label1.TabIndex = 2;
            this.label1.Text = "파일명 변경할 상위 폴더 경로(파일명 : A_B_C >>> A)B_C)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 253);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(639, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "파일명 변경할 상위 폴더 경로(파일명 : A_B_C >>> A_B_X)";
            // 
            // txtFolderPath2
            // 
            this.txtFolderPath2.Location = new System.Drawing.Point(59, 315);
            this.txtFolderPath2.Name = "txtFolderPath2";
            this.txtFolderPath2.Size = new System.Drawing.Size(365, 35);
            this.txtFolderPath2.TabIndex = 4;
            // 
            // btnChangeFileName2
            // 
            this.btnChangeFileName2.Location = new System.Drawing.Point(505, 361);
            this.btnChangeFileName2.Name = "btnChangeFileName2";
            this.btnChangeFileName2.Size = new System.Drawing.Size(245, 103);
            this.btnChangeFileName2.TabIndex = 5;
            this.btnChangeFileName2.Text = "btnChangeFileName2";
            this.btnChangeFileName2.UseVisualStyleBackColor = true;
            this.btnChangeFileName2.Click += new System.EventHandler(this.btnChangeFileName2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 381);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "X(숫자)";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(59, 429);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(365, 35);
            this.textBox3.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 540);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnChangeFileName2);
            this.Controls.Add(this.txtFolderPath2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnChangeFileName);
            this.Controls.Add(this.txtFolderPath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Button btnChangeFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFolderPath2;
        private System.Windows.Forms.Button btnChangeFileName2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
    }
}

