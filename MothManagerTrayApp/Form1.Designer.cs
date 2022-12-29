namespace MothManagerTrayApp;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanButton = new System.Windows.Forms.Button();
            this.discoveredDeviceListBox = new System.Windows.Forms.ListBox();
            this.knownDevicePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.saveSettingsButton = new System.Windows.Forms.Button();
            this.loadSettingsButton = new System.Windows.Forms.Button();
            this.addDiscoveredButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "MothManager";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(104, 48);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.showToolStripMenuItem.Text = "&Show";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // scanButton
            // 
            this.scanButton.AutoSize = true;
            this.scanButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.scanButton.Location = new System.Drawing.Point(3, 3);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(199, 41);
            this.scanButton.TabIndex = 3;
            this.scanButton.Text = "Scan for Devices";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // discoveredDeviceListBox
            // 
            this.discoveredDeviceListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.discoveredDeviceListBox.Font = new System.Drawing.Font("SimSun", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.discoveredDeviceListBox.FormattingEnabled = true;
            this.discoveredDeviceListBox.ItemHeight = 11;
            this.discoveredDeviceListBox.Location = new System.Drawing.Point(3, 50);
            this.discoveredDeviceListBox.Name = "discoveredDeviceListBox";
            this.discoveredDeviceListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.discoveredDeviceListBox.Size = new System.Drawing.Size(199, 256);
            this.discoveredDeviceListBox.TabIndex = 5;
            this.discoveredDeviceListBox.SelectedIndexChanged += new System.EventHandler(this.discoveredDeviceListBox_SelectedIndexChanged);
            // 
            // knownDevicePanel
            // 
            this.knownDevicePanel.AutoScroll = true;
            this.knownDevicePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.knownDevicePanel.Location = new System.Drawing.Point(0, 0);
            this.knownDevicePanel.Name = "knownDevicePanel";
            this.knownDevicePanel.Size = new System.Drawing.Size(591, 450);
            this.knownDevicePanel.TabIndex = 6;
            this.knownDevicePanel.WrapContents = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.knownDevicePanel);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 205;
            this.splitContainer1.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.saveSettingsButton, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.loadSettingsButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.addDiscoveredButton, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.discoveredDeviceListBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.scanButton, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(205, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // saveSettingsButton
            // 
            this.saveSettingsButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.saveSettingsButton.Location = new System.Drawing.Point(3, 406);
            this.saveSettingsButton.Name = "saveSettingsButton";
            this.saveSettingsButton.Size = new System.Drawing.Size(199, 41);
            this.saveSettingsButton.TabIndex = 7;
            this.saveSettingsButton.Text = "Save Settings";
            this.saveSettingsButton.UseVisualStyleBackColor = true;
            this.saveSettingsButton.Click += new System.EventHandler(this.saveSettingsButton_Click);
            // 
            // loadSettingsButton
            // 
            this.loadSettingsButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.loadSettingsButton.Location = new System.Drawing.Point(3, 359);
            this.loadSettingsButton.Name = "loadSettingsButton";
            this.loadSettingsButton.Size = new System.Drawing.Size(199, 41);
            this.loadSettingsButton.TabIndex = 8;
            this.loadSettingsButton.Text = "Load Settings";
            this.loadSettingsButton.UseVisualStyleBackColor = true;
            this.loadSettingsButton.Click += new System.EventHandler(this.loadSettingsButton_Click);
            // 
            // addDiscoveredButton
            // 
            this.addDiscoveredButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.addDiscoveredButton.Location = new System.Drawing.Point(3, 312);
            this.addDiscoveredButton.Name = "addDiscoveredButton";
            this.addDiscoveredButton.Size = new System.Drawing.Size(199, 41);
            this.addDiscoveredButton.TabIndex = 4;
            this.addDiscoveredButton.Text = "Add Discovered Devices";
            this.addDiscoveredButton.UseVisualStyleBackColor = true;
            this.addDiscoveredButton.Click += new System.EventHandler(this.addDiscoveredButton_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "MothManager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    private NotifyIcon notifyIcon1;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem showToolStripMenuItem;
    private ToolStripMenuItem exitToolStripMenuItem;
    private Button scanButton;
    private ListBox discoveredDeviceListBox;
    private FlowLayoutPanel knownDevicePanel;
    private SplitContainer splitContainer1;
    private Button addDiscoveredButton;
    private TableLayoutPanel tableLayoutPanel1;
    private Button saveSettingsButton;
    private Button loadSettingsButton;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
}