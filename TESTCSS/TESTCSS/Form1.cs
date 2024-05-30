using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using System.IO;
using System.Text;

namespace TESTCSS
{
    public partial class Form1 : Form
    {
        // 데이터베이스 연결 문자열을 클래스 변수로 선언
        public readonly string connString = "Host=localhost;Port=5432;Username=hsha;Password=6109;Database=sbk";
        string metaPath = @"C:\Users\user\Desktop\정부 과제\SBK Engineering\07) SBK (소스코드)\Process\meta_text_file_test\TGA.txt";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.Black;
            // 로드 시 필요한 작업이 있다면 여기서 처리
            LoadDataFromDatabase(); // Form 로드 시 데이터 로딩
        }

        private void Registration_btn_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this); // this 키워드를 사용하여 Form1의 인스턴스를 Form2에 전달합니다.
            form2.Show(); // Form2를 비모달 창으로 표시
        }

        private void Delete_btn_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(this); // this 키워드를 사용하여 Form1의 인스턴스를 Form3에 전달합니다.
            form3.Show(); // Form3를 비모달 창으로 표시
        }

        public void LoadDataFromDatabase()
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    // 데이터베이스 연결
                    conn.Open();

                    // 'product_name'과 'robot_point'을 기준으로 오름차순으로 데이터를 정렬하여 불러오는 SQL 쿼리
                    string sql = "SELECT * FROM public.list ORDER BY product_name ASC, robot_point ASC;";

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt); // DataTable 객체에 데이터 채우기

                        // DataGridView에 데이터 표시
                        dataGridViewList.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 로딩 중 오류가 발생했습니다: " + ex.Message);
            }
        }

        private void SavaMETA_btn_Click(object sender, EventArgs e)
        {
            // 데이터 조회 SQL 쿼리
            string query = @"
                SELECT point, label, count 
                FROM meta 
                WHERE product_name = 'TGA' AND valid_yn = 'Y'
                ORDER BY point ASC, label ASC
            ";

            StringBuilder sb = new StringBuilder();

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 탭으로 구분된 값을 StringBuilder 객체에 추가
                                sb.AppendLine($"{reader["point"]}\t{reader["label"]}\t{reader["count"]}");
                            }
                        }
                    }
                }

                // StringBuilder의 내용을 파일에 쓰기
                File.WriteAllText(metaPath, sb.ToString());
                MessageBox.Show("Data has been successfully saved to the file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
