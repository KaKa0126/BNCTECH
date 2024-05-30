using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Npgsql;

namespace SBK_TEST
{
    public partial class Form1 : Form
    {
        #region 선언
        private bool isButtonClicked = false;

        // 클릭된 버튼의 번호를 저장할 변수
        public List<int> clickedButtonNumbers = new List<int>();

        public List<int> now_ai_position = new List<int>();

        string code = "MAE1624ST01";
        string robot_mode = "ALL";
        string robot_act = "WAIT";

        private List<Button> buttons; // 버튼 리스트

        // 데이터베이스 연결 문자열을 클래스 변수로 선언
        public readonly string connString = "Host=localhost;Port=5432;Username=hsha;Password=6109;Database=sbk";

        string meta_Path = @"C:\JUDGE\ai_output\Meta";
        string name_model_Path = @"C:\JUDGE\Classifier\Name_Model.txt";
        string name_classifier_Path = @"C:\JUDGE\Classifier\Name_Classifier.txt";

        private FileSystemWatcher watcher;

        private TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();

        #endregion

        public Form1()
        {
            InitializeComponent();

            //SaveMETA(); // Form 로드 시, meta 텍스트 파일 저장
            //SaveNameModel(); //Form 로드 시, Name_Model 텍스트 파일 저장
            //SaveNameClassifier(); //Form 로드 시, Name_Classifier 텍스트 파일 저장
        }

        private async void btn_foldermove_Click(object sender, EventArgs e)
        {
            await MonitorFoldersContinuously();
            /*
            btn_foldermove.BackColor = Color.Red;
            Delay(2000);
            btn_foldermove.BackColor = Color.White;
            */
        }

        private async Task MonitorFoldersContinuously()
        {
            while (true)
            {
                await MoveFolderWhenReady();
            }
        }

        private async Task MoveFolderWhenReady()
        {
            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string date = DateTime.Now.ToString("dd");
            string output_dir = $"C:\\JUDGE\\ai_output\\{year}\\{month}\\{date}";
            string wait_dir = $"C:\\JUDGE\\wait";

            // outputDir 하위에 폴더가 없을 때까지 기다립니다.
            while (Directory.GetDirectories(output_dir).Length != 0)
            {
            }

            // waitDir 하위에 폴더가 생길 때까지 기다립니다.
            while (Directory.GetDirectories(wait_dir).Length == 0)
            {
            }
            Delay(100);  // 폴더뿐만 아니라, 이미지가 저장되는 것도 기다린다.

            // 이름순으로 가장 첫 번째 폴더를 이동시킵니다.
            var directoryToMove = Directory.GetDirectories(wait_dir)
                .OrderBy(d => d)
                .FirstOrDefault();

            if (directoryToMove != null)
            {
                string destPath = Path.Combine(output_dir, Path.GetFileName(directoryToMove));
                Directory.Move(directoryToMove, destPath);
                //Console.WriteLine($"Moved {directoryToMove} to {destPath}");
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            isButtonClicked = true;
            Button clickedButton = sender as Button;

            RemoveAllNumbers();

            if (clickedButton != null)
            {
                string buttonNumberStr = clickedButton.Name.Replace("button", "");
                if (int.TryParse(buttonNumberStr, out int buttonNumber))
                {
                    clickedButtonNumbers.Add(buttonNumber);
                    Delay(1);
                }
            }

            string number = (sender as Control).Text.ToString();
            if (number.Substring(0, 1) == "0")
                number = number.Substring(1);
            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string date = DateTime.Now.ToString("dd");
            string baseDir = $"C:/JUDGE/ai_result/{year}/{month}/{date}";
            string searchPattern = $"{code}_{number}.jpg";

            try
            {
                // 폴더가 존재하지 않으면 생성
                if (!Directory.Exists(baseDir))
                {
                    Directory.CreateDirectory(baseDir);
                }

                // 모든 하위 폴더를 검색하고 파일 찾기
                var directories = Directory.GetDirectories(baseDir);
                List<string> foundFiles = new List<string>();

                foreach (var dir in directories)
                {
                    foundFiles.AddRange(Directory.GetFiles(dir, searchPattern));
                }

                // 찾아진 이미지가 n개면, n분할로 화면에 표시
                if (foundFiles.Count > 0)
                {
                    tableLayoutPanel1.Controls.Clear();
                    tableLayoutPanel1.ColumnStyles.Clear();
                    tableLayoutPanel1.RowStyles.Clear();

                    tableLayoutPanel1.ColumnCount = foundFiles.Count;  // 찾아진 파일 수만큼 열 설정
                    tableLayoutPanel1.RowCount = 1;  // 하나의 행
                    tableLayoutPanel1.AutoSize = false;  // 자동 크기 조정 비활성화
                    tableLayoutPanel1.Dock = DockStyle.None;  // Dock을 None으로 설정
                    tableLayoutPanel1.Size = new Size(818, 596);  // 특정 크기 설정

                    // 모든 열에 동일한 비율 적용
                    for (int i = 0; i < foundFiles.Count; i++)
                    {
                        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / foundFiles.Count));
                    }

                    // 'for' 루프를 사용하여 각 파일에 대한 PictureBox를 추가
                    for (int i = 0; i < foundFiles.Count; i++)
                    {
                        var file = foundFiles[i];
                        PictureBox pictureBox = new PictureBox
                        {
                            SizeMode = PictureBoxSizeMode.StretchImage, // 이미지를 PictureBox에 맞게 스트레치
                            Dock = DockStyle.Fill, // PictureBox를 해당 셀에 꽉 차게 설정
                            Image = LoadImageAndReleaseFile(file)
                        };
                        tableLayoutPanel1.Controls.Add(pictureBox, i, 0); // PictureBox를 적절한 위치(i, 0)에 추가
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string sampleDir = $"C:/JUDGE/sample";
            string imagePattern = $"{code}_{number}_*.jpg";
        }
        private void RemoveAllNumbers()
        {
            // 리스트에 요소가 있을 경우만 삭제를 진행
            if (clickedButtonNumbers.Count > 0)
            {
                // 리스트의 모든 요소 삭제
                clickedButtonNumbers.Clear();

                // 배열 업데이트: 빈 배열로 설정
                now_ai_position = new List<int>(clickedButtonNumbers);
            }
        }

        public void Delay(int ms)
        {
            DateTime dateTimeNow = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime dateTimeAdd = dateTimeNow.Add(duration);
            while (dateTimeAdd >= dateTimeNow)
            {
                System.Windows.Forms.Application.DoEvents();
                dateTimeNow = DateTime.Now;
            }
            return;
        }
       

        private void get_init()
        {
            try
            {
                string year = DateTime.Now.ToString("yyyy");
                string month = DateTime.Now.ToString("MM");
                string date = DateTime.Now.ToString("dd");

                string output_dir = $"C:\\JUDGE\\ai_output\\{year}\\{month}\\{date}";

                // 'output_dir'에 있는 모든 하위 폴더를 가져옵니다.
                var directories = Directory.GetDirectories(output_dir);

                foreach (var directory in directories)
                {
                    // 폴더를 삭제합니다. true는 모든 내용(하위 폴더와 파일 포함)을 재귀적으로 삭제합니다.
                    Directory.Delete(directory, true);
                }
            }
            catch (Exception ex)
            {
                // 오류 발생시 처리
                Console.WriteLine("An error occurred while deleting directories: " + ex.Message);
            }

            try
            {
                string year = DateTime.Now.ToString("yyyy");
                string month = DateTime.Now.ToString("MM");
                string date = DateTime.Now.ToString("dd");

                string output_dir = $"C:\\JUDGE\\ai_result\\{year}\\{month}\\{date}";

                // 'output_dir'에 있는 모든 하위 폴더를 가져옵니다.
                var directories = Directory.GetDirectories(output_dir);

                foreach (var directory in directories)
                {
                    // 폴더를 삭제합니다. true는 모든 내용(하위 폴더와 파일 포함)을 재귀적으로 삭제합니다.
                    Directory.Delete(directory, true);
                }
            }
            catch (Exception ex)
            {
                // 오류 발생시 처리
                Console.WriteLine("An error occurred while deleting directories: " + ex.Message);
            }
        }

        private void one_btn_Click_Click(object sender, EventArgs e)
        {
            // 사용자에게 경고 메시지를 표시하는 메서드
            if (!isButtonClicked)
            {
                MessageBox.Show("부분검사할 버튼을 선택해 주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // 메서드 실행을 여기서 중단
            }

            get_init();

            // 클릭된 버튼에 따라 다른 작업 수행
            switch (clickedButtonNumbers[0].ToString())
            {
                case "3":
                    var task1 = set_img_async(code, "1", 1);
                    break;
                case "4":
                    var task2 = set_img_async(code, "2", 2);
                    break;
                case "5":
                    var task3 = set_img_async(code, "3", 3);
                    break;
                case "6":
                    var task4 = set_img_async(code, "4", 4);
                    break;
                case "7":
                    var task5 = set_img_async(code, "5", 5);
                    break;
                case "8":
                    var task6 = set_img_async(code, "6", 6);
                    break;
            }

            Delay(1);
        }

        private void all_btn_Click(object sender, EventArgs e)
        {
            get_init();
            var task1 = set_img_async(code, "1", 1);
            var task2 = set_img_async(code, "2", 2);
            var task3 = set_img_async(code, "3", 3);
            var task4 = set_img_async(code, "4", 4);
            var task5 = set_img_async(code, "5", 5);
            var task6 = set_img_async(code, "6", 6);

            var task0 = end_async();
        }

        private async Task set_img_async(string barcode, string now_count, int picture_num)
        {
            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string date = DateTime.Now.ToString("dd");
            string result_dir = $"C:\\JUDGE\\ai_result\\{year}\\{month}\\{date}";
            string output_dir = $"C:\\JUDGE\\ai_output\\{year}\\{month}\\{date}";
            string sample_dir = $"C:\\JUDGE\\sample";

            // 데이터베이스에서 now_count와 일치하는 point를 조회
            List<int> validPoints = new List<int>();
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();
                string query = $"SELECT point FROM meta WHERE point = {now_count}";
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        validPoints.Add(reader.GetInt32(0));  // point 값을 리스트에 추가
                    }
                }
            }

            // 감시할 경로 설정
            using (var watcher = new FileSystemWatcher(result_dir, $"{barcode}_{now_count}_*.jpg"))
            {
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime;
                watcher.IncludeSubdirectories = true;
                watcher.Created += async (sender, args) =>
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        Delay(500); // 파일이 완전히 사용 가능하도록 500ms 지연, 100ms 지연으로 바꿔도 되는 지 확인하기
                        //LoadAndDisplayImage(args.FullPath, picture_num);
                        //LoadAndDisplayImage2(args.FullPath, picture_num);
                        LoadAndDisplayImage3(args.FullPath, picture_num);
                        //SampleImageLoad();
                        string folderName = new DirectoryInfo(Path.GetDirectoryName(args.FullPath)).Name;
                        DeleteFolderInOutput(folderName, output_dir);

                        // DirectoryInfo 객체 생성
                        DirectoryInfo dirInfo = new DirectoryInfo(output_dir);

                        // GetDirectories를 사용하여 하위 폴더 목록을 가져옵니다.
                        DirectoryInfo[] subdirs = dirInfo.GetDirectories();

                        // 하위 폴더가 없다면 end_count를 1로 설정
                        if (picture_num == 6 && subdirs.Length == 0)
                        {
                            completionSource.SetResult(true);
                        }
                    });
                };
                watcher.EnableRaisingEvents = true;

                // 하나 이상의 유효한 point가 있을 때만 대기
                if (validPoints.Count > 0)
                {
                    await Task.Delay(Timeout.Infinite);  // 파일이 생성될 때까지 무한 대기
                }
            }
        }

        public void SampleImageLoad()
        {
            // pictureBox1 설정
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 이미지 로드 - 실제 경로로 변경 필요
            try
            {
                this.pictureBox1.Image = Image.FromFile("C:\\Users\\user\\Desktop\\TEST\\TGA_2P_2L\\MAE1624ST01_2_240306135342.jpg");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load image: " + ex.Message);
            }
            this.pictureBox1.Location = new System.Drawing.Point(621, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(149, 116);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
        }


        private async Task end_async()
        {
            // 비동기 작업 대기
            await completionSource.Task;

            //button_Click(button3, EventArgs.Empty);
            Delay(1000);
            button3.PerformClick();
            button3.Focus();
        }

        private void LoadAndDisplayImage(string imagePath, int picture_num)
        {
            if (File.Exists(imagePath))
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.ColumnStyles.Clear();
                tableLayoutPanel1.RowStyles.Clear();

                tableLayoutPanel1.ColumnCount = 1;  // 하나의 열
                tableLayoutPanel1.RowCount = 1;  // 하나의 행
                tableLayoutPanel1.AutoSize = false;  // 자동 크기 조정 비활성화
                tableLayoutPanel1.Dock = DockStyle.None;  // Dock을 None으로 설정
                tableLayoutPanel1.Size = new Size(818, 596);  // 이미지를 꽉 채울 크기 설정(tableLayoutPanel1.Size의 가로, 세로 1/2로 입력해야 원하는 사이즈로 나옴)

                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); // 열 너비 100%
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // 행 높이 100%

                var pictureBox = new PictureBox
                {
                    Name = $"pictureBox{picture_num}",
                    SizeMode = PictureBoxSizeMode.StretchImage,  // 이미지가 PictureBox 내에서 꽉 차게 조정됨
                    Dock = DockStyle.Fill,  // PictureBox를 해당 셀에 꽉 차게 설정
                    Image = LoadImageAndReleaseFile(imagePath)  // 이미지 로드
                };

                tableLayoutPanel1.Controls.Add(pictureBox, 0, 0);  // PictureBox를 TableLayoutPanel에 추가
            }
        }

        private async void LoadAndDisplayImage2(string imagePath, int picture_num)
        {
            if (File.Exists(imagePath))
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.ColumnStyles.Clear();
                tableLayoutPanel1.RowStyles.Clear();

                tableLayoutPanel1.ColumnCount = 1;  // 하나의 열
                tableLayoutPanel1.RowCount = 1;  // 하나의 행
                tableLayoutPanel1.AutoSize = false;  // 자동 크기 조정 비활성화
                tableLayoutPanel1.Dock = DockStyle.None;  // Dock을 None으로 설정
                tableLayoutPanel1.Size = new Size(818, 596);  // 이미지를 꽉 채울 크기 설정(tableLayoutPanel1.Size의 가로, 세로 1/2로 입력해야 원하는 사이즈로 나옴)

                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); // 열 너비 100%
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // 행 높이 100%

                var pictureBox = new PictureBox
                {
                    Name = $"pictureBox{picture_num}",
                    SizeMode = PictureBoxSizeMode.StretchImage,  // 이미지가 PictureBox 내에서 꽉 차게 조정됨
                    Dock = DockStyle.Fill  // PictureBox를 해당 셀에 꽉 차게 설정
                };

                tableLayoutPanel1.Controls.Add(pictureBox, 0, 0);  // PictureBox를 TableLayoutPanel에 추가

                // 이미지를 Base64 문자열로 비동기적으로 로드하고 PictureBox에 설정
                string base64Image = await LoadImageAndEncodeBase64(imagePath);
                if (!string.IsNullOrEmpty(base64Image))
                {
                    // Base64 문자열을 Image 객체로 변환
                    var imageBytes = Convert.FromBase64String(base64Image);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                }
            }
        }

        private async Task LoadAndDisplayImage3(string imagePath, int picture_num)
        {
            if (File.Exists(imagePath))
            {
                string base64Image = await LoadImageAndEncodeBase64_2(imagePath);  // 비동기적으로 이미지를 Base64 문자열로 로드

                // UI 컨트롤 업데이트 실행
                this.BeginInvoke((MethodInvoker)delegate
                {
                    tableLayoutPanel1.Controls.Clear();
                    tableLayoutPanel1.ColumnStyles.Clear();
                    tableLayoutPanel1.RowStyles.Clear();

                    tableLayoutPanel1.ColumnCount = 1;
                    tableLayoutPanel1.RowCount = 1;
                    tableLayoutPanel1.AutoSize = true;  // AutoSize를 false로 설정하여 사이즈를 직접 제어합니다.

                    // 열의 너비를 50%로 설정
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

                    // 행의 높이를 50%로 설정
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));

                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        var imageBytes = Convert.FromBase64String(base64Image);
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            var image = Image.FromStream(ms);

                            var pictureBox = new PictureBox
                            {
                                Name = $"pictureBox{picture_num}",
                                SizeMode = PictureBoxSizeMode.StretchImage, // 이미지가 PictureBox에 맞게 확대/축소됩니다.
                                Dock = DockStyle.Fill, // PictureBox를 TableLayoutPanel의 셀에 꽉 차게 합니다.
                                Image = image
                            };

                            //tableLayoutPanel1.Controls.Add(pictureBox, 0, 0);
                            // (1,0)과 (1,1) 영역에 이미지를 띄우기 위해 RowSpan을 2로 설정
                            tableLayoutPanel1.Controls.Add(pictureBox, 0, 1); // 0번 열, 1번 행에 추가
                            tableLayoutPanel1.SetRowSpan(pictureBox, 2); // 1번 행과 2번 행에 걸쳐서 표시
                        }
                    }
                    else
                    {
                        // 이미지 로딩 실패 처리, 예를 들어 로깅 또는 사용자에게 메시지 표시
                        MessageBox.Show("Failed to load image.");
                    }
                    tableLayoutPanel1.Dock = DockStyle.None; // TableLayoutPanel을 부모 컨트롤에 꽉 차게 합니다.
                });
            }
            else
            {
                // 파일 존재하지 않음 로깅 또는 사용자 알림
                MessageBox.Show("File does not exist.");
            }
        }

        //이미지 로드 및 파일 핸들 해제
        private Image LoadImageAndReleaseFile(string imagePath)
        {
            int maxRetries = 3; // 최대 재시도 횟수
            int delay = 1000; // 재시도 사이의 지연 시간(밀리초)

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        Image loadedImage = Image.FromStream(stream);
                        return new Bitmap(loadedImage); // 이미지를 Bitmap으로 복사하여 파일과의 연결을 끊습니다.
                    }
                }
                catch (IOException)
                {
                    if (i == maxRetries - 1) // 마지막 시도에서도 실패하면 예외를 던집니다.
                        throw;
                    Thread.Sleep(delay); // 지정된 시간만큼 대기 후 재시도
                }
            }

            return null; // 모든 재시도가 실패하면 null 반환
        }

        private async Task<string> LoadImageAndEncodeBase64(string imagePath)
        {
            int maxRetries = 5; // 최대 재시도 횟수
            int delay = 1000; // 재시도 사이의 지연 시간(밀리초)

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        Image loadedImage = Image.FromStream(stream);
                        if (loadedImage != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                loadedImage.Save(ms, loadedImage.RawFormat); // 이미지를 메모리 스트림에 저장
                                byte[] imageBytes = ms.ToArray();
                                return Convert.ToBase64String(imageBytes); // 이미지를 베이스64 문자열로 변환
                            }
                        }
                    }
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    if (i == maxRetries - 1) throw; // 마지막 시도에서도 실패하면 예외를 던집니다.
                    await Task.Delay(delay); // 지정된 시간만큼 대기 후 재시도
                }
            }

            return null; // 모든 재시도가 실패하면 null 반환
        }

        private async Task<string> LoadImageAndEncodeBase64_2(string imagePath)
        {
            int maxRetries = 5; // 최대 재시도 횟수
            int delay = 1000; // 재시도 사이의 지연 시간(밀리초)

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    using (Bitmap loadedImage = new Bitmap(imagePath))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            loadedImage.Save(ms, loadedImage.RawFormat); // 이미지를 메모리 스트림에 저장
                            byte[] imageBytes = ms.ToArray();
                            return Convert.ToBase64String(imageBytes); // 이미지를 베이스64 문자열로 변환
                        }
                    }
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    if (i == maxRetries - 1) throw; // 마지막 시도에서도 실패하면 예외를 던집니다.
                    await Task.Delay(delay); // 지정된 시간만큼 대기 후 재시도
                }
            }

            return null; // 모든 재시도가 실패하면 null 반환
        }

        private void DeleteFolderInOutput(string folderName, string basePath)
        {
            string folderPath = Path.Combine(basePath, folderName);
            if (Directory.Exists(folderPath))
            {
                try
                {
                    Directory.Delete(folderPath, true);
                    //MessageBox.Show($"Folder deleted: {folderPath}", "Folder Deletion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveMETA()
        {
            // 데이터 조회 SQL 쿼리
            string query = @"
                SELECT point, label, count
                FROM meta 
                WHERE product_name = 'TGA' AND valid_yn = 'Y'
                ORDER BY point ASC, label ASC
            ";

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
                                int point = reader.GetInt32(reader.GetOrdinal("point"));
                                int label = reader.GetInt32(reader.GetOrdinal("label"));
                                int count = reader.GetInt32(reader.GetOrdinal("count"));
                                string fileName = $"TGA_{point}P_{label}L.txt";
                                string filePath = Path.Combine(meta_Path, fileName);
                                string content = $"{point}\t{label}\t{count}\n";

                                // 파일에 내용을 추가
                                File.WriteAllText(filePath, content);
                            }
                        }
                    }
                }
                //MessageBox.Show("Data has been successfully saved to the files.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //240424 수정
        private void SaveNameClassifier()
        {
            // 데이터 조회 SQL 쿼리: product_name, point, label 기준으로 오름차순 정렬
            string query = @"
                SELECT product_name, point, label, conversion_technique
                FROM meta
                WHERE valid_yn = 'Y'
                ORDER BY product_name ASC, point ASC, label ASC
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
                                string productName = reader["product_name"].ToString();
                                int point = Convert.ToInt32(reader["point"]);
                                int conversion_technique = Convert.ToInt32(reader["conversion_technique"]);
                                string label = reader["label"].ToString();

                                // 'product_name'_'point'P_'label'L:'product_name'_'point'P_'label'L 형식으로 내용 작성
                                sb.AppendLine($"{productName}_{point}P_{label}L;L;{point}:classifier_{productName}_{point}P_{label}L:L1;R1;{conversion_technique}");
                            }
                        }
                    }
                }

                // StringBuilder의 내용을 파일에 쓰기
                File.WriteAllText(name_classifier_Path, sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //240424 수정
        private void SaveNameModel()
        {
            // 데이터 조회 SQL 쿼리: product_name, point, label 기준으로 오름차순 정렬
            string query = @"
                SELECT product_name, point, label
                FROM meta
                WHERE valid_yn = 'Y'
                ORDER BY product_name ASC, point ASC, label ASC
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
                                string productName = reader["product_name"].ToString();
                                int point = Convert.ToInt32(reader["point"]);
                                string label = reader["label"].ToString();

                                // 'product_name'_'point'P_'label'L:'product_name'_'point'P_'label'L 형식으로 내용 작성
                                sb.AppendLine($"{productName}_{point}P_{label}L;L;{point}:{productName}_{point}P_{label}L");
                            }
                        }
                    }
                }

                // StringBuilder의 내용을 파일에 쓰기
                File.WriteAllText(name_model_Path, sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
