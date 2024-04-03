namespace FightingFeather
{
    partial class AdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_General = new System.Windows.Forms.Button();
            this.label_RegisterUsers = new System.Windows.Forms.Label();
            this.metroPanel4 = new MetroFramework.Controls.MetroPanel();
            this.Panel_Line = new MetroFramework.Controls.MetroPanel();
            this.postedMunton = new MetroFramework.Controls.MetroGrid();
            this.textBox_Search = new MetroFramework.Controls.MetroTextBox();
            this.button_FIND = new System.Windows.Forms.Button();
            this.button_CLEAR = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.button4 = new System.Windows.Forms.Button();
            this.button_License = new System.Windows.Forms.Button();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USERNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label_AdminSettings = new System.Windows.Forms.Label();
            this.panel_Indicator = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postedMunton)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(253)))));
            this.panel1.Controls.Add(this.button_License);
            this.panel1.Controls.Add(this.metroPanel4);
            this.panel1.Controls.Add(this.button_General);
            this.panel1.Location = new System.Drawing.Point(1, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(219, 684);
            this.panel1.TabIndex = 1;
            // 
            // button_General
            // 
            this.button_General.BackColor = System.Drawing.Color.Transparent;
            this.button_General.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_General.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button_General.FlatAppearance.BorderSize = 0;
            this.button_General.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_General.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_General.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(84)))), ((int)(((byte)(55)))));
            this.button_General.Image = global::FightingFeather.Properties.Resources.File_Configuration_24px;
            this.button_General.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_General.Location = new System.Drawing.Point(12, 37);
            this.button_General.Name = "button_General";
            this.button_General.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button_General.Size = new System.Drawing.Size(201, 31);
            this.button_General.TabIndex = 333;
            this.button_General.Text = "ACCESS MANAGEMENT";
            this.button_General.UseVisualStyleBackColor = true;
            // 
            // label_RegisterUsers
            // 
            this.label_RegisterUsers.AutoSize = true;
            this.label_RegisterUsers.Font = new System.Drawing.Font("Calibri", 11F);
            this.label_RegisterUsers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(84)))), ((int)(((byte)(55)))));
            this.label_RegisterUsers.Location = new System.Drawing.Point(256, 42);
            this.label_RegisterUsers.Name = "label_RegisterUsers";
            this.label_RegisterUsers.Size = new System.Drawing.Size(169, 18);
            this.label_RegisterUsers.TabIndex = 360;
            this.label_RegisterUsers.Text = "LIST OF REGISTERED USERS";
            this.label_RegisterUsers.Click += new System.EventHandler(this.label_RegisterUsers_Click);
            // 
            // metroPanel4
            // 
            this.metroPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanel4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.metroPanel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.metroPanel4.HorizontalScrollbarBarColor = true;
            this.metroPanel4.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel4.HorizontalScrollbarSize = 10;
            this.metroPanel4.Location = new System.Drawing.Point(29, 84);
            this.metroPanel4.Name = "metroPanel4";
            this.metroPanel4.Size = new System.Drawing.Size(160, 1);
            this.metroPanel4.TabIndex = 334;
            this.metroPanel4.UseCustomBackColor = true;
            this.metroPanel4.UseStyleColors = true;
            this.metroPanel4.VerticalScrollbarBarColor = true;
            this.metroPanel4.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel4.VerticalScrollbarSize = 10;
            // 
            // Panel_Line
            // 
            this.Panel_Line.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Line.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Panel_Line.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Panel_Line.HorizontalScrollbarBarColor = true;
            this.Panel_Line.HorizontalScrollbarHighlightOnWheel = false;
            this.Panel_Line.HorizontalScrollbarSize = 10;
            this.Panel_Line.Location = new System.Drawing.Point(259, 70);
            this.Panel_Line.Name = "Panel_Line";
            this.Panel_Line.Size = new System.Drawing.Size(1045, 1);
            this.Panel_Line.TabIndex = 363;
            this.Panel_Line.UseCustomBackColor = true;
            this.Panel_Line.UseStyleColors = true;
            this.Panel_Line.VerticalScrollbarBarColor = true;
            this.Panel_Line.VerticalScrollbarHighlightOnWheel = false;
            this.Panel_Line.VerticalScrollbarSize = 10;
            // 
            // postedMunton
            // 
            this.postedMunton.AllowUserToAddRows = false;
            this.postedMunton.AllowUserToResizeRows = false;
            dataGridViewCellStyle29.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle29.Font = new System.Drawing.Font("Bahnschrift SemiLight SemiConde", 9.75F);
            dataGridViewCellStyle29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(235)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.postedMunton.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle29;
            this.postedMunton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.postedMunton.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.postedMunton.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.postedMunton.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.postedMunton.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.postedMunton.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(251)))), ((int)(((byte)(253)))));
            dataGridViewCellStyle30.Font = new System.Drawing.Font("Calibri", 7.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle30.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(85)))), ((int)(((byte)(109)))));
            dataGridViewCellStyle30.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(251)))), ((int)(((byte)(253)))));
            dataGridViewCellStyle30.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(85)))), ((int)(((byte)(109)))));
            dataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.postedMunton.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle30;
            this.postedMunton.ColumnHeadersHeight = 35;
            this.postedMunton.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.NAME,
            this.USERNAME,
            this.PASSWORD});
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle33.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle33.Font = new System.Drawing.Font("Bahnschrift SemiLight SemiConde", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle33.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle33.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(235)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle33.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle33.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.postedMunton.DefaultCellStyle = dataGridViewCellStyle33;
            this.postedMunton.EnableHeadersVisualStyles = false;
            this.postedMunton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.postedMunton.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.postedMunton.Location = new System.Drawing.Point(253, 206);
            this.postedMunton.MultiSelect = false;
            this.postedMunton.Name = "postedMunton";
            this.postedMunton.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle34.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle34.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle34.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle34.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle34.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.postedMunton.RowHeadersDefaultCellStyle = dataGridViewCellStyle34;
            this.postedMunton.RowHeadersWidth = 20;
            this.postedMunton.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle35.Font = new System.Drawing.Font("Bahnschrift SemiLight SemiConde", 9.75F);
            this.postedMunton.RowsDefaultCellStyle = dataGridViewCellStyle35;
            this.postedMunton.RowTemplate.Height = 23;
            this.postedMunton.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.postedMunton.Size = new System.Drawing.Size(1057, 433);
            this.postedMunton.TabIndex = 364;
            this.postedMunton.UseCustomBackColor = true;
            this.postedMunton.UseCustomForeColor = true;
            this.postedMunton.UseStyleColors = true;
            // 
            // textBox_Search
            // 
            // 
            // 
            // 
            this.textBox_Search.CustomButton.Image = null;
            this.textBox_Search.CustomButton.Location = new System.Drawing.Point(345, 1);
            this.textBox_Search.CustomButton.Name = "";
            this.textBox_Search.CustomButton.Size = new System.Drawing.Size(25, 25);
            this.textBox_Search.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.textBox_Search.CustomButton.TabIndex = 1;
            this.textBox_Search.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textBox_Search.CustomButton.UseSelectable = true;
            this.textBox_Search.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.textBox_Search.Lines = new string[0];
            this.textBox_Search.Location = new System.Drawing.Point(258, 147);
            this.textBox_Search.MaxLength = 32767;
            this.textBox_Search.Name = "textBox_Search";
            this.textBox_Search.PasswordChar = '\0';
            this.textBox_Search.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_Search.SelectedText = "";
            this.textBox_Search.SelectionLength = 0;
            this.textBox_Search.SelectionStart = 0;
            this.textBox_Search.ShortcutsEnabled = true;
            this.textBox_Search.ShowButton = true;
            this.textBox_Search.ShowClearButton = true;
            this.textBox_Search.Size = new System.Drawing.Size(371, 27);
            this.textBox_Search.TabIndex = 365;
            this.textBox_Search.UseSelectable = true;
            this.textBox_Search.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.textBox_Search.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // button_FIND
            // 
            this.button_FIND.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.button_FIND.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_FIND.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(195)))), ((int)(((byte)(237)))));
            this.button_FIND.FlatAppearance.BorderSize = 2;
            this.button_FIND.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_FIND.Font = new System.Drawing.Font("Bahnschrift SemiBold", 8F, System.Drawing.FontStyle.Bold);
            this.button_FIND.ForeColor = System.Drawing.Color.MintCream;
            this.button_FIND.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_FIND.Location = new System.Drawing.Point(643, 147);
            this.button_FIND.Name = "button_FIND";
            this.button_FIND.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button_FIND.Size = new System.Drawing.Size(75, 27);
            this.button_FIND.TabIndex = 366;
            this.button_FIND.Text = "FIND";
            this.button_FIND.UseVisualStyleBackColor = false;
            // 
            // button_CLEAR
            // 
            this.button_CLEAR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(251)))), ((int)(((byte)(253)))));
            this.button_CLEAR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_CLEAR.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.button_CLEAR.FlatAppearance.BorderSize = 2;
            this.button_CLEAR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_CLEAR.Font = new System.Drawing.Font("Bahnschrift SemiBold", 8F, System.Drawing.FontStyle.Bold);
            this.button_CLEAR.ForeColor = System.Drawing.Color.DimGray;
            this.button_CLEAR.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_CLEAR.Location = new System.Drawing.Point(730, 147);
            this.button_CLEAR.Name = "button_CLEAR";
            this.button_CLEAR.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button_CLEAR.Size = new System.Drawing.Size(75, 27);
            this.button_CLEAR.TabIndex = 367;
            this.button_CLEAR.Text = "CLEAR";
            this.button_CLEAR.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(195)))), ((int)(((byte)(237)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Bahnschrift SemiBold", 8F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Image = global::FightingFeather.Properties.Resources.new_copy_24px;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.Location = new System.Drawing.Point(865, 107);
            this.button1.Name = "button1";
            this.button1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button1.Size = new System.Drawing.Size(77, 65);
            this.button1.TabIndex = 368;
            this.button1.Text = "ADD NEW USER";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(195)))), ((int)(((byte)(237)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Bahnschrift SemiBold", 8F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Image = global::FightingFeather.Properties.Resources.new_copy_24px;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.Location = new System.Drawing.Point(984, 107);
            this.button2.Name = "button2";
            this.button2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button2.Size = new System.Drawing.Size(73, 65);
            this.button2.TabIndex = 369;
            this.button2.Text = "CHECK PASSWORD";
            this.button2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(195)))), ((int)(((byte)(237)))));
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Bahnschrift SemiBold", 8F, System.Drawing.FontStyle.Bold);
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Image = global::FightingFeather.Properties.Resources.new_copy_24px;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button3.Location = new System.Drawing.Point(1100, 107);
            this.button3.Name = "button3";
            this.button3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button3.Size = new System.Drawing.Size(75, 65);
            this.button3.TabIndex = 370;
            this.button3.Text = "CHANGE PASSWORD";
            this.button3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button3.UseVisualStyleBackColor = false;
            // 
            // metroPanel1
            // 
            this.metroPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.metroPanel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(1200, 117);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(1, 47);
            this.metroPanel1.TabIndex = 371;
            this.metroPanel1.UseCustomBackColor = true;
            this.metroPanel1.UseStyleColors = true;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Transparent;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(195)))), ((int)(((byte)(237)))));
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Bahnschrift SemiBold", 8F, System.Drawing.FontStyle.Bold);
            this.button4.ForeColor = System.Drawing.Color.Black;
            this.button4.Image = global::FightingFeather.Properties.Resources.send_file_24px;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button4.Location = new System.Drawing.Point(1227, 113);
            this.button4.Name = "button4";
            this.button4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button4.Size = new System.Drawing.Size(54, 51);
            this.button4.TabIndex = 372;
            this.button4.Text = "EXPORT";
            this.button4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button_License
            // 
            this.button_License.BackColor = System.Drawing.Color.Transparent;
            this.button_License.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_License.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button_License.FlatAppearance.BorderSize = 0;
            this.button_License.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_License.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_License.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(42)))));
            this.button_License.Image = global::FightingFeather.Properties.Resources.Licence_24px;
            this.button_License.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_License.Location = new System.Drawing.Point(12, 107);
            this.button_License.Name = "button_License";
            this.button_License.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button_License.Size = new System.Drawing.Size(201, 31);
            this.button_License.TabIndex = 335;
            this.button_License.Text = "LICENSE MANAGEMENT";
            this.button_License.UseVisualStyleBackColor = true;
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ID.DividerWidth = 3;
            this.ID.FillWeight = 91.15936F;
            this.ID.HeaderText = "ID NO.";
            this.ID.Name = "ID";
            // 
            // NAME
            // 
            this.NAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle31.Format = "N2";
            dataGridViewCellStyle31.NullValue = null;
            this.NAME.DefaultCellStyle = dataGridViewCellStyle31;
            this.NAME.HeaderText = "NAME";
            this.NAME.Name = "NAME";
            // 
            // USERNAME
            // 
            this.USERNAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle32.Padding = new System.Windows.Forms.Padding(10, 4, 0, 4);
            this.USERNAME.DefaultCellStyle = dataGridViewCellStyle32;
            this.USERNAME.HeaderText = "USERNAME";
            this.USERNAME.Name = "USERNAME";
            // 
            // PASSWORD
            // 
            this.PASSWORD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PASSWORD.HeaderText = "PASSWORD";
            this.PASSWORD.Name = "PASSWORD";
            // 
            // label_AdminSettings
            // 
            this.label_AdminSettings.AutoSize = true;
            this.label_AdminSettings.Font = new System.Drawing.Font("Calibri", 11F);
            this.label_AdminSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_AdminSettings.Location = new System.Drawing.Point(461, 42);
            this.label_AdminSettings.Name = "label_AdminSettings";
            this.label_AdminSettings.Size = new System.Drawing.Size(113, 18);
            this.label_AdminSettings.TabIndex = 373;
            this.label_AdminSettings.Text = "ADMIN SETTINGS\r\n";
            this.label_AdminSettings.Click += new System.EventHandler(this.label_AdminSettings_Click);
            // 
            // panel_Indicator
            // 
            this.panel_Indicator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.panel_Indicator.Location = new System.Drawing.Point(260, 66);
            this.panel_Indicator.Name = "panel_Indicator";
            this.panel_Indicator.Size = new System.Drawing.Size(158, 4);
            this.panel_Indicator.TabIndex = 374;
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1342, 687);
            this.Controls.Add(this.panel_Indicator);
            this.Controls.Add(this.label_AdminSettings);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.metroPanel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_CLEAR);
            this.Controls.Add(this.button_FIND);
            this.Controls.Add(this.textBox_Search);
            this.Controls.Add(this.postedMunton);
            this.Controls.Add(this.Panel_Line);
            this.Controls.Add(this.label_RegisterUsers);
            this.Controls.Add(this.panel1);
            this.Name = "AdminForm";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.postedMunton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_General;
        private System.Windows.Forms.Label label_RegisterUsers;
        private MetroFramework.Controls.MetroPanel metroPanel4;
        private MetroFramework.Controls.MetroPanel Panel_Line;
        private MetroFramework.Controls.MetroGrid postedMunton;
        private MetroFramework.Controls.MetroTextBox textBox_Search;
        private System.Windows.Forms.Button button_FIND;
        private System.Windows.Forms.Button button_CLEAR;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button_License;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn USERNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASSWORD;
        private System.Windows.Forms.Label label_AdminSettings;
        private System.Windows.Forms.Panel panel_Indicator;
    }
}