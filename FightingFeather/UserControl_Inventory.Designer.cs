﻿namespace FightingFeather
{
    partial class UserControl_Inventory
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.metroPanel11 = new MetroFramework.Controls.MetroPanel();
            this.metroScrollBar1 = new MetroFramework.Controls.MetroScrollBar();
            this.postedMunton = new MetroFramework.Controls.MetroGrid();
            this.MUNTON = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOTAL_ENTRY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOTAL_PLASADA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.metroPanel3 = new MetroFramework.Controls.MetroPanel();
            this.button_ViewMunton = new System.Windows.Forms.Button();
            this.textBox_Search = new MetroFramework.Controls.MetroTextBox();
            this.button_Search = new System.Windows.Forms.Button();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.button_ExportMunton = new System.Windows.Forms.Button();
            this.metroPanel29 = new MetroFramework.Controls.MetroPanel();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.raDateTimePicker1 = new FightingFeather.RaDateTimePicker();
            this.metroPanel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postedMunton)).BeginInit();
            this.metroPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroPanel11
            // 
            this.metroPanel11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.metroPanel11.Controls.Add(this.metroScrollBar1);
            this.metroPanel11.Controls.Add(this.postedMunton);
            this.metroPanel11.HorizontalScrollbarBarColor = true;
            this.metroPanel11.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel11.HorizontalScrollbarSize = 10;
            this.metroPanel11.Location = new System.Drawing.Point(331, 49);
            this.metroPanel11.Name = "metroPanel11";
            this.metroPanel11.Size = new System.Drawing.Size(751, 588);
            this.metroPanel11.TabIndex = 231;
            this.metroPanel11.UseCustomBackColor = true;
            this.metroPanel11.UseStyleColors = true;
            this.metroPanel11.VerticalScrollbarBarColor = true;
            this.metroPanel11.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel11.VerticalScrollbarSize = 10;
            // 
            // metroScrollBar1
            // 
            this.metroScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.metroScrollBar1.LargeChange = 10;
            this.metroScrollBar1.Location = new System.Drawing.Point(746, 0);
            this.metroScrollBar1.Maximum = 100;
            this.metroScrollBar1.Minimum = 0;
            this.metroScrollBar1.MouseWheelBarPartitions = 10;
            this.metroScrollBar1.Name = "metroScrollBar1";
            this.metroScrollBar1.Orientation = MetroFramework.Controls.MetroScrollOrientation.Vertical;
            this.metroScrollBar1.ScrollbarSize = 5;
            this.metroScrollBar1.Size = new System.Drawing.Size(5, 588);
            this.metroScrollBar1.Style = MetroFramework.MetroColorStyle.Lime;
            this.metroScrollBar1.TabIndex = 384;
            this.metroScrollBar1.UseBarColor = true;
            this.metroScrollBar1.UseCustomBackColor = true;
            this.metroScrollBar1.UseSelectable = true;
            // 
            // postedMunton
            // 
            this.postedMunton.AllowUserToAddRows = false;
            this.postedMunton.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Bahnschrift SemiLight SemiConde", 9.75F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(235)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.postedMunton.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.postedMunton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.postedMunton.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.postedMunton.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(253)))));
            this.postedMunton.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.postedMunton.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.postedMunton.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(251)))), ((int)(((byte)(253)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 7.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(85)))), ((int)(((byte)(109)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(251)))), ((int)(((byte)(253)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(85)))), ((int)(((byte)(109)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.postedMunton.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.postedMunton.ColumnHeadersHeight = 35;
            this.postedMunton.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MUNTON,
            this.DATE,
            this.TOTAL_ENTRY,
            this.TOTAL_PLASADA});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Bahnschrift SemiLight SemiConde", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(235)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.postedMunton.DefaultCellStyle = dataGridViewCellStyle5;
            this.postedMunton.EnableHeadersVisualStyles = false;
            this.postedMunton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.postedMunton.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.postedMunton.Location = new System.Drawing.Point(4, 4);
            this.postedMunton.MultiSelect = false;
            this.postedMunton.Name = "postedMunton";
            this.postedMunton.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(251)))), ((int)(((byte)(253)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.postedMunton.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.postedMunton.RowHeadersWidth = 5;
            this.postedMunton.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Bahnschrift SemiLight SemiConde", 9.75F);
            this.postedMunton.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.postedMunton.RowTemplate.Height = 23;
            this.postedMunton.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.postedMunton.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.postedMunton.Size = new System.Drawing.Size(741, 576);
            this.postedMunton.TabIndex = 10;
            this.postedMunton.UseCustomBackColor = true;
            this.postedMunton.UseCustomForeColor = true;
            this.postedMunton.UseStyleColors = true;
            // 
            // MUNTON
            // 
            this.MUNTON.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MUNTON.DividerWidth = 3;
            this.MUNTON.FillWeight = 91.15936F;
            this.MUNTON.HeaderText = "MUNTON";
            this.MUNTON.Name = "MUNTON";
            // 
            // DATE
            // 
            this.DATE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(10, 4, 0, 4);
            this.DATE.DefaultCellStyle = dataGridViewCellStyle3;
            this.DATE.HeaderText = "MTN DATE";
            this.DATE.Name = "DATE";
            // 
            // TOTAL_ENTRY
            // 
            this.TOTAL_ENTRY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TOTAL_ENTRY.HeaderText = "TOTAL ENTRY";
            this.TOTAL_ENTRY.Name = "TOTAL_ENTRY";
            // 
            // TOTAL_PLASADA
            // 
            this.TOTAL_PLASADA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.TOTAL_PLASADA.DefaultCellStyle = dataGridViewCellStyle4;
            this.TOTAL_PLASADA.HeaderText = "TOTAL PLASADA";
            this.TOTAL_PLASADA.Name = "TOTAL_PLASADA";
            // 
            // metroPanel3
            // 
            this.metroPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroPanel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.metroPanel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.metroPanel3.HorizontalScrollbarBarColor = true;
            this.metroPanel3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel3.HorizontalScrollbarSize = 10;
            this.metroPanel3.Location = new System.Drawing.Point(330, 47);
            this.metroPanel3.Name = "metroPanel3";
            this.metroPanel3.Size = new System.Drawing.Size(751, 1);
            this.metroPanel3.TabIndex = 332;
            this.metroPanel3.UseCustomBackColor = true;
            this.metroPanel3.UseStyleColors = true;
            this.metroPanel3.VerticalScrollbarBarColor = true;
            this.metroPanel3.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel3.VerticalScrollbarSize = 10;
            // 
            // button_ViewMunton
            // 
            this.button_ViewMunton.BackColor = System.Drawing.Color.Transparent;
            this.button_ViewMunton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_ViewMunton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button_ViewMunton.FlatAppearance.BorderSize = 0;
            this.button_ViewMunton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_ViewMunton.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ViewMunton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(42)))));
            this.button_ViewMunton.Image = global::FightingFeather.Properties.Resources.binoculars_24px;
            this.button_ViewMunton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_ViewMunton.Location = new System.Drawing.Point(360, 10);
            this.button_ViewMunton.Name = "button_ViewMunton";
            this.button_ViewMunton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button_ViewMunton.Size = new System.Drawing.Size(133, 31);
            this.button_ViewMunton.TabIndex = 335;
            this.button_ViewMunton.Text = "VIEW DATA ITEM";
            this.button_ViewMunton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_ViewMunton.UseVisualStyleBackColor = true;
            this.button_ViewMunton.Click += new System.EventHandler(this.button_ViewMunton_Click);
            // 
            // textBox_Search
            // 
            // 
            // 
            // 
            this.textBox_Search.CustomButton.Image = null;
            this.textBox_Search.CustomButton.Location = new System.Drawing.Point(246, 1);
            this.textBox_Search.CustomButton.Name = "";
            this.textBox_Search.CustomButton.Size = new System.Drawing.Size(25, 25);
            this.textBox_Search.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.textBox_Search.CustomButton.TabIndex = 1;
            this.textBox_Search.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textBox_Search.CustomButton.UseSelectable = true;
            this.textBox_Search.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.textBox_Search.Lines = new string[0];
            this.textBox_Search.Location = new System.Drawing.Point(28, 72);
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
            this.textBox_Search.Size = new System.Drawing.Size(272, 27);
            this.textBox_Search.TabIndex = 329;
            this.textBox_Search.UseSelectable = true;
            this.textBox_Search.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.textBox_Search.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // button_Search
            // 
            this.button_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.button_Search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_Search.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(195)))), ((int)(((byte)(237)))));
            this.button_Search.FlatAppearance.BorderSize = 2;
            this.button_Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Search.Font = new System.Drawing.Font("Bahnschrift SemiBold", 8F, System.Drawing.FontStyle.Bold);
            this.button_Search.ForeColor = System.Drawing.Color.MintCream;
            this.button_Search.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_Search.Location = new System.Drawing.Point(28, 116);
            this.button_Search.Name = "button_Search";
            this.button_Search.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button_Search.Size = new System.Drawing.Size(272, 29);
            this.button_Search.TabIndex = 330;
            this.button_Search.Text = "SEARCH";
            this.button_Search.UseVisualStyleBackColor = false;
            this.button_Search.Click += new System.EventHandler(this.button_Search_Click);
            // 
            // metroPanel2
            // 
            this.metroPanel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.metroPanel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(30, 49);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(270, 1);
            this.metroPanel2.TabIndex = 331;
            this.metroPanel2.UseCustomBackColor = true;
            this.metroPanel2.UseStyleColors = true;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // button_ExportMunton
            // 
            this.button_ExportMunton.BackColor = System.Drawing.Color.Transparent;
            this.button_ExportMunton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_ExportMunton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button_ExportMunton.FlatAppearance.BorderSize = 0;
            this.button_ExportMunton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_ExportMunton.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ExportMunton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(42)))));
            this.button_ExportMunton.Image = global::FightingFeather.Properties.Resources.send_file_24px;
            this.button_ExportMunton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_ExportMunton.Location = new System.Drawing.Point(22, 10);
            this.button_ExportMunton.Name = "button_ExportMunton";
            this.button_ExportMunton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button_ExportMunton.Size = new System.Drawing.Size(119, 31);
            this.button_ExportMunton.TabIndex = 332;
            this.button_ExportMunton.Text = "EXPORT";
            this.button_ExportMunton.UseVisualStyleBackColor = true;
            this.button_ExportMunton.Click += new System.EventHandler(this.button_ExportMunton_Click);
            // 
            // metroPanel29
            // 
            this.metroPanel29.BackColor = System.Drawing.Color.WhiteSmoke;
            this.metroPanel29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.metroPanel29.HorizontalScrollbarBarColor = true;
            this.metroPanel29.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel29.HorizontalScrollbarSize = 10;
            this.metroPanel29.Location = new System.Drawing.Point(30, 168);
            this.metroPanel29.Name = "metroPanel29";
            this.metroPanel29.Size = new System.Drawing.Size(270, 1);
            this.metroPanel29.TabIndex = 334;
            this.metroPanel29.UseCustomBackColor = true;
            this.metroPanel29.UseStyleColors = true;
            this.metroPanel29.VerticalScrollbarBarColor = true;
            this.metroPanel29.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel29.VerticalScrollbarSize = 10;
            // 
            // metroPanel1
            // 
            this.metroPanel1.BackColor = System.Drawing.Color.White;
            this.metroPanel1.Controls.Add(this.metroPanel29);
            this.metroPanel1.Controls.Add(this.raDateTimePicker1);
            this.metroPanel1.Controls.Add(this.button_ExportMunton);
            this.metroPanel1.Controls.Add(this.metroPanel2);
            this.metroPanel1.Controls.Add(this.button_Search);
            this.metroPanel1.Controls.Add(this.textBox_Search);
            this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(0, 0);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(324, 640);
            this.metroPanel1.TabIndex = 232;
            this.metroPanel1.UseCustomBackColor = true;
            this.metroPanel1.UseStyleColors = true;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // raDateTimePicker1
            // 
            this.raDateTimePicker1.BorderColor = System.Drawing.Color.Black;
            this.raDateTimePicker1.BorderSize = 0;
            this.raDateTimePicker1.CalendarForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.raDateTimePicker1.FillColor = System.Drawing.Color.White;
            this.raDateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.raDateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.raDateTimePicker1.Location = new System.Drawing.Point(175, 6);
            this.raDateTimePicker1.MinimumSize = new System.Drawing.Size(4, 35);
            this.raDateTimePicker1.Name = "raDateTimePicker1";
            this.raDateTimePicker1.Size = new System.Drawing.Size(123, 35);
            this.raDateTimePicker1.TabIndex = 333;
            this.raDateTimePicker1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(54)))), ((int)(((byte)(63)))));
            this.raDateTimePicker1.ValueChanged += new System.EventHandler(this.raDateTimePicker1_ValueChanged);
            // 
            // UserControl_Inventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button_ViewMunton);
            this.Controls.Add(this.metroPanel3);
            this.Controls.Add(this.metroPanel1);
            this.Controls.Add(this.metroPanel11);
            this.Name = "UserControl_Inventory";
            this.Size = new System.Drawing.Size(1084, 640);
            this.metroPanel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.postedMunton)).EndInit();
            this.metroPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel metroPanel11;
        private MetroFramework.Controls.MetroGrid postedMunton;
        private MetroFramework.Controls.MetroPanel metroPanel3;
        private System.Windows.Forms.Button button_ViewMunton;
        private System.Windows.Forms.DataGridViewTextBoxColumn MUNTON;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOTAL_ENTRY;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOTAL_PLASADA;
        private MetroFramework.Controls.MetroTextBox textBox_Search;
        private System.Windows.Forms.Button button_Search;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private System.Windows.Forms.Button button_ExportMunton;
        private RaDateTimePicker raDateTimePicker1;
        private MetroFramework.Controls.MetroPanel metroPanel29;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroScrollBar metroScrollBar1;
    }
}
