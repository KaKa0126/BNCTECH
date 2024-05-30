using System.Windows.Forms;

namespace TESTCSS
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
            this.Registration_btn = new System.Windows.Forms.Button();
            this.Delete_btn = new System.Windows.Forms.Button();
            this.dataGridViewList = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.SavaMETA_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewList)).BeginInit();
            this.SuspendLayout();
            // 
            // Registration_btn
            // 
            this.Registration_btn.Location = new System.Drawing.Point(1331, 94);
            this.Registration_btn.Name = "Registration_btn";
            this.Registration_btn.Size = new System.Drawing.Size(227, 85);
            this.Registration_btn.TabIndex = 0;
            this.Registration_btn.Text = "동작 등록";
            this.Registration_btn.UseVisualStyleBackColor = true;
            this.Registration_btn.Click += new System.EventHandler(this.Registration_btn_Click);
            // 
            // Delete_btn
            // 
            this.Delete_btn.Location = new System.Drawing.Point(1331, 228);
            this.Delete_btn.Name = "Delete_btn";
            this.Delete_btn.Size = new System.Drawing.Size(227, 85);
            this.Delete_btn.TabIndex = 1;
            this.Delete_btn.Text = "동작 삭제";
            this.Delete_btn.UseVisualStyleBackColor = true;
            this.Delete_btn.Click += new System.EventHandler(this.Delete_btn_Click);
            // 
            // dataGridViewList
            // 
            this.dataGridViewList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridViewList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewList.Location = new System.Drawing.Point(29, 94);
            this.dataGridViewList.Name = "dataGridViewList";
            this.dataGridViewList.RowHeadersWidth = 82;
            this.dataGridViewList.RowTemplate.Height = 37;
            this.dataGridViewList.Size = new System.Drawing.Size(1217, 648);
            this.dataGridViewList.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(194, 24);
            this.label6.TabIndex = 18;
            this.label6.Text = "로봇 동작 리스트";
            // 
            // SavaMETA_btn
            // 
            this.SavaMETA_btn.Location = new System.Drawing.Point(1331, 657);
            this.SavaMETA_btn.Name = "SavaMETA_btn";
            this.SavaMETA_btn.Size = new System.Drawing.Size(227, 85);
            this.SavaMETA_btn.TabIndex = 19;
            this.SavaMETA_btn.Text = "META 저장";
            this.SavaMETA_btn.UseVisualStyleBackColor = true;
            this.SavaMETA_btn.Click += new System.EventHandler(this.SavaMETA_btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1610, 802);
            this.Controls.Add(this.SavaMETA_btn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dataGridViewList);
            this.Controls.Add(this.Delete_btn);
            this.Controls.Add(this.Registration_btn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Registration_btn;
        private System.Windows.Forms.Button Delete_btn;
        private System.Windows.Forms.DataGridView dataGridViewList;
        private Label label6;
        private Button SavaMETA_btn;
    }
}

