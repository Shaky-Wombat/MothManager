using Cyotek.Windows.Forms;

namespace MothManagerTrayApp.Controls
{
    partial class NeewerLedPanelSettings
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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.deviceNameLabel = new System.Windows.Forms.Label();
            this.idLabel = new System.Windows.Forms.Label();
            this.togglePowerButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.lNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.lColorBar = new Cyotek.Windows.Forms.LightnessColorSlider();
            this.bLabel = new System.Windows.Forms.Label();
            this.sNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.sColorBar = new Cyotek.Windows.Forms.SaturationColorSlider();
            this.sLabel = new System.Windows.Forms.Label();
            this.hColorBar = new Cyotek.Windows.Forms.HueColorSlider();
            this.hNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.hLabel = new System.Windows.Forms.Label();
            this.tColorBar = new MothManagerTrayApp.Controls.TemperatureColorSlider();
            this.tNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.tLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.powerLabel = new System.Windows.Forms.Label();
            this.modeLabel = new System.Windows.Forms.Label();
            this.powerOnButton = new System.Windows.Forms.Button();
            this.powerOffButton = new System.Windows.Forms.Button();
            this.whiteModeButton = new System.Windows.Forms.Button();
            this.colorModeButton = new System.Windows.Forms.Button();
            this.sceneModeButton = new System.Windows.Forms.Button();
            this.sceneComboBox = new System.Windows.Forms.ComboBox();
            this.sceneLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tNumericUpDown)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameTextBox
            // 
            this.nameTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.nameTextBox.Location = new System.Drawing.Point(3, 3);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(204, 23);
            this.nameTextBox.TabIndex = 0;
            this.nameTextBox.Text = "Name";
            this.nameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // deviceNameLabel
            // 
            this.deviceNameLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.deviceNameLabel.Location = new System.Drawing.Point(3, 25);
            this.deviceNameLabel.Name = "deviceNameLabel";
            this.deviceNameLabel.Size = new System.Drawing.Size(204, 20);
            this.deviceNameLabel.TabIndex = 1;
            this.deviceNameLabel.Text = "Device Name";
            this.deviceNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // idLabel
            // 
            this.idLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.idLabel.Location = new System.Drawing.Point(3, 45);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(102, 16);
            this.idLabel.TabIndex = 2;
            this.idLabel.Text = "DeviceID";
            this.idLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // togglePowerButton
            // 
            this.togglePowerButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.togglePowerButton.Location = new System.Drawing.Point(73, 73);
            this.togglePowerButton.Name = "togglePowerButton";
            this.togglePowerButton.Size = new System.Drawing.Size(64, 24);
            this.togglePowerButton.TabIndex = 3;
            this.togglePowerButton.Text = "Toggle";
            this.togglePowerButton.UseVisualStyleBackColor = true;
            this.togglePowerButton.Click += new System.EventHandler(this.togglePower_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.statusLabel.Location = new System.Drawing.Point(105, 45);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(102, 16);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "Status";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lNumericUpDown
            // 
            this.lNumericUpDown.Location = new System.Drawing.Point(149, 180);
            this.lNumericUpDown.Name = "lNumericUpDown";
            this.lNumericUpDown.Size = new System.Drawing.Size(58, 20);
            this.lNumericUpDown.TabIndex = 20;
            this.lNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lNumericUpDown.ValueChanged += new System.EventHandler(this.lNumericUpDown_ValueChanged);
            // 
            // lColorBar
            // 
            this.lColorBar.Location = new System.Drawing.Point(27, 180);
            this.lColorBar.Name = "lColorBar";
            this.lColorBar.Size = new System.Drawing.Size(116, 20);
            this.lColorBar.TabIndex = 21;
            this.lColorBar.ValueChanged += new System.EventHandler(this.lColorBar_ValueChanged);
            // 
            // bLabel
            // 
            this.bLabel.AutoSize = true;
            this.bLabel.Location = new System.Drawing.Point(3, 182);
            this.bLabel.Name = "bLabel";
            this.bLabel.Size = new System.Drawing.Size(17, 13);
            this.bLabel.TabIndex = 19;
            this.bLabel.Text = "B:";
            // 
            // sNumericUpDown
            // 
            this.sNumericUpDown.Location = new System.Drawing.Point(149, 154);
            this.sNumericUpDown.Name = "sNumericUpDown";
            this.sNumericUpDown.Size = new System.Drawing.Size(58, 20);
            this.sNumericUpDown.TabIndex = 17;
            this.sNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.sNumericUpDown.ValueChanged += new System.EventHandler(this.sNumericUpDown_ValueChanged);
            // 
            // sColorBar
            // 
            this.sColorBar.Location = new System.Drawing.Point(27, 154);
            this.sColorBar.Name = "sColorBar";
            this.sColorBar.Size = new System.Drawing.Size(116, 20);
            this.sColorBar.TabIndex = 18;
            this.sColorBar.ValueChanged += new System.EventHandler(this.sColorBar_ValueChanged);
            // 
            // sLabel
            // 
            this.sLabel.AutoSize = true;
            this.sLabel.Location = new System.Drawing.Point(4, 156);
            this.sLabel.Name = "sLabel";
            this.sLabel.Size = new System.Drawing.Size(17, 13);
            this.sLabel.TabIndex = 16;
            this.sLabel.Text = "S:";
            // 
            // hColorBar
            // 
            this.hColorBar.Location = new System.Drawing.Point(27, 128);
            this.hColorBar.Name = "hColorBar";
            this.hColorBar.Size = new System.Drawing.Size(116, 20);
            this.hColorBar.TabIndex = 15;
            this.hColorBar.ValueChanged += new System.EventHandler(this.hColorBar_ValueChanged);
            // 
            // hNumericUpDown
            // 
            this.hNumericUpDown.Location = new System.Drawing.Point(149, 128);
            this.hNumericUpDown.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.hNumericUpDown.Name = "hNumericUpDown";
            this.hNumericUpDown.Size = new System.Drawing.Size(58, 20);
            this.hNumericUpDown.TabIndex = 14;
            this.hNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.hNumericUpDown.ValueChanged += new System.EventHandler(this.hNumericUpDown_ValueChanged);
            // 
            // hLabel
            // 
            this.hLabel.AutoSize = true;
            this.hLabel.Location = new System.Drawing.Point(3, 130);
            this.hLabel.Name = "hLabel";
            this.hLabel.Size = new System.Drawing.Size(18, 13);
            this.hLabel.TabIndex = 13;
            this.hLabel.Text = "H:";
            // 
            // tColorBar
            // 
            this.tColorBar.Location = new System.Drawing.Point(27, 102);
            this.tColorBar.Name = "tColorBar";
            this.tColorBar.Size = new System.Drawing.Size(116, 20);
            this.tColorBar.TabIndex = 25;
            this.tColorBar.ValueChanged += new System.EventHandler(this.tColorBar_ValueChanged);
            // 
            // tNumericUpDown
            // 
            this.tNumericUpDown.Location = new System.Drawing.Point(149, 102);
            this.tNumericUpDown.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.tNumericUpDown.Name = "tNumericUpDown";
            this.tNumericUpDown.Size = new System.Drawing.Size(58, 20);
            this.tNumericUpDown.TabIndex = 24;
            this.tNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tNumericUpDown.ValueChanged += new System.EventHandler(this.tNumericUpDown_ValueChanged);
            // 
            // tLabel
            // 
            this.tLabel.AutoSize = true;
            this.tLabel.Location = new System.Drawing.Point(3, 104);
            this.tLabel.Name = "tLabel";
            this.tLabel.Size = new System.Drawing.Size(17, 13);
            this.tLabel.TabIndex = 23;
            this.tLabel.Text = "T:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.powerLabel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.modeLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.togglePowerButton, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.powerOnButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.powerOffButton, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.whiteModeButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.colorModeButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.sceneModeButton, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 252);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(210, 100);
            this.tableLayoutPanel1.TabIndex = 26;
            // 
            // powerLabel
            // 
            this.powerLabel.AutoSize = true;
            this.powerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.powerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.powerLabel.Location = new System.Drawing.Point(73, 50);
            this.powerLabel.Name = "powerLabel";
            this.powerLabel.Size = new System.Drawing.Size(64, 20);
            this.powerLabel.TabIndex = 4;
            this.powerLabel.Text = "Power";
            this.powerLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // modeLabel
            // 
            this.modeLabel.AutoSize = true;
            this.modeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.modeLabel.Location = new System.Drawing.Point(73, 0);
            this.modeLabel.Name = "modeLabel";
            this.modeLabel.Size = new System.Drawing.Size(64, 20);
            this.modeLabel.TabIndex = 5;
            this.modeLabel.Text = "Mode";
            this.modeLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // powerOnButton
            // 
            this.powerOnButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.powerOnButton.Location = new System.Drawing.Point(3, 73);
            this.powerOnButton.Name = "powerOnButton";
            this.powerOnButton.Size = new System.Drawing.Size(64, 24);
            this.powerOnButton.TabIndex = 6;
            this.powerOnButton.Text = "On";
            this.powerOnButton.UseVisualStyleBackColor = true;
            this.powerOnButton.Click += new System.EventHandler(this.powerOnButton_Click);
            // 
            // powerOffButton
            // 
            this.powerOffButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.powerOffButton.Location = new System.Drawing.Point(143, 73);
            this.powerOffButton.Name = "powerOffButton";
            this.powerOffButton.Size = new System.Drawing.Size(64, 24);
            this.powerOffButton.TabIndex = 7;
            this.powerOffButton.Text = "Off";
            this.powerOffButton.UseVisualStyleBackColor = true;
            this.powerOffButton.Click += new System.EventHandler(this.powerOffButton_Click);
            // 
            // whiteModeButton
            // 
            this.whiteModeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whiteModeButton.Location = new System.Drawing.Point(3, 23);
            this.whiteModeButton.Name = "whiteModeButton";
            this.whiteModeButton.Size = new System.Drawing.Size(64, 24);
            this.whiteModeButton.TabIndex = 8;
            this.whiteModeButton.Text = "White";
            this.whiteModeButton.UseVisualStyleBackColor = true;
            this.whiteModeButton.Click += new System.EventHandler(this.whiteModeButton_Click);
            // 
            // colorModeButton
            // 
            this.colorModeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorModeButton.Location = new System.Drawing.Point(73, 23);
            this.colorModeButton.Name = "colorModeButton";
            this.colorModeButton.Size = new System.Drawing.Size(64, 24);
            this.colorModeButton.TabIndex = 9;
            this.colorModeButton.Text = "Color";
            this.colorModeButton.UseVisualStyleBackColor = true;
            this.colorModeButton.Click += new System.EventHandler(this.colorModeButton_Click);
            // 
            // sceneModeButton
            // 
            this.sceneModeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneModeButton.Location = new System.Drawing.Point(143, 23);
            this.sceneModeButton.Name = "sceneModeButton";
            this.sceneModeButton.Size = new System.Drawing.Size(64, 24);
            this.sceneModeButton.TabIndex = 10;
            this.sceneModeButton.Text = "Scene";
            this.sceneModeButton.UseVisualStyleBackColor = true;
            this.sceneModeButton.Click += new System.EventHandler(this.sceneModeButton_Click);
            // 
            // sceneComboBox
            // 
            this.sceneComboBox.FormattingEnabled = true;
            this.sceneComboBox.Location = new System.Drawing.Point(59, 206);
            this.sceneComboBox.Name = "sceneComboBox";
            this.sceneComboBox.Size = new System.Drawing.Size(148, 21);
            this.sceneComboBox.TabIndex = 27;
            this.sceneComboBox.SelectedIndexChanged += new System.EventHandler(this.sceneComboBox_SelectedIndexChanged);
            // 
            // sceneLabel
            // 
            this.sceneLabel.AutoSize = true;
            this.sceneLabel.Location = new System.Drawing.Point(3, 209);
            this.sceneLabel.Name = "sceneLabel";
            this.sceneLabel.Size = new System.Drawing.Size(41, 13);
            this.sceneLabel.TabIndex = 28;
            this.sceneLabel.Text = "Scene:";
            // 
            // NeewerLedPanelSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sceneLabel);
            this.Controls.Add(this.sceneComboBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.deviceNameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.lNumericUpDown);
            this.Controls.Add(this.lColorBar);
            this.Controls.Add(this.bLabel);
            this.Controls.Add(this.sNumericUpDown);
            this.Controls.Add(this.sColorBar);
            this.Controls.Add(this.sLabel);
            this.Controls.Add(this.hColorBar);
            this.Controls.Add(this.hNumericUpDown);
            this.Controls.Add(this.hLabel);
            this.Controls.Add(this.tColorBar);
            this.Controls.Add(this.tNumericUpDown);
            this.Controls.Add(this.tLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "NeewerLedPanelSettings";
            this.Size = new System.Drawing.Size(210, 352);
            this.Load += new System.EventHandler(this.NeewerLEDPanelSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tNumericUpDown)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox nameTextBox;
        private Label deviceNameLabel;
        private Label idLabel;
        private Button togglePowerButton;
        private Label statusLabel;
        
        private System.Windows.Forms.NumericUpDown lNumericUpDown;
        private LightnessColorSlider lColorBar;
        private System.Windows.Forms.Label bLabel;
        private System.Windows.Forms.NumericUpDown sNumericUpDown;
        private SaturationColorSlider sColorBar;
        private System.Windows.Forms.Label sLabel;
        private HueColorSlider hColorBar;
        private System.Windows.Forms.NumericUpDown hNumericUpDown;
        private System.Windows.Forms.Label hLabel;
        private TemperatureColorSlider tColorBar;
        private System.Windows.Forms.NumericUpDown tNumericUpDown;
        private System.Windows.Forms.Label tLabel;
        private TableLayoutPanel tableLayoutPanel1;
        private Label powerLabel;
        private Label modeLabel;
        private Button powerOnButton;
        private Button powerOffButton;
        private Button whiteModeButton;
        private Button colorModeButton;
        private Button sceneModeButton;
        private ComboBox sceneComboBox;
        private Label sceneLabel;
    }
}
