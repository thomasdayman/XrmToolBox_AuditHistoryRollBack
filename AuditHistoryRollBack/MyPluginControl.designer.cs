namespace AuditHistoryRollBack
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyPluginControl));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.LoadDataButton = new System.Windows.Forms.ToolStripButton();
            this.fetchXML = new System.Windows.Forms.ToolStripButton();
            this.tsbSample = new System.Windows.Forms.ToolStripButton();
            this.loadAuditButton = new System.Windows.Forms.Button();
            this.rollbackbutton = new System.Windows.Forms.Button();
            this.entitiesList = new System.Windows.Forms.ComboBox();
            this.recordGuid = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.guidLabel = new System.Windows.Forms.Label();
            this.showNewestValues = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.autoCopyGuidFromClipboard = new System.Windows.Forms.CheckBox();
            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadDataButton,
            this.fetchXML,
            this.tsbSample});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMenu.Size = new System.Drawing.Size(1182, 31);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // LoadDataButton
            // 
            this.LoadDataButton.Image = ((System.Drawing.Image)(resources.GetObject("LoadDataButton.Image")));
            this.LoadDataButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LoadDataButton.Name = "LoadDataButton";
            this.LoadDataButton.Size = new System.Drawing.Size(102, 28);
            this.LoadDataButton.Text = "Load Entities";
            this.LoadDataButton.Click += new System.EventHandler(this.LoadDataButton_Click);
            // 
            // fetchXML
            // 
            this.fetchXML.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fetchXML.Image = ((System.Drawing.Image)(resources.GetObject("fetchXML.Image")));
            this.fetchXML.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fetchXML.Name = "fetchXML";
            this.fetchXML.Size = new System.Drawing.Size(28, 28);
            this.fetchXML.Text = "FetchXML";
            this.fetchXML.Visible = false;
            // 
            // tsbSample
            // 
            this.tsbSample.Name = "tsbSample";
            this.tsbSample.Size = new System.Drawing.Size(23, 28);
            // 
            // loadAuditButton
            // 
            this.loadAuditButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadAuditButton.Location = new System.Drawing.Point(14, 238);
            this.loadAuditButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadAuditButton.Name = "loadAuditButton";
            this.loadAuditButton.Size = new System.Drawing.Size(266, 49);
            this.loadAuditButton.TabIndex = 5;
            this.loadAuditButton.Text = "Load Audit History";
            this.loadAuditButton.UseVisualStyleBackColor = true;
            this.loadAuditButton.Click += new System.EventHandler(this.LoadAuditHistoryButton);
            // 
            // rollbackbutton
            // 
            this.rollbackbutton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rollbackbutton.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.rollbackbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rollbackbutton.Location = new System.Drawing.Point(297, 33);
            this.rollbackbutton.Margin = new System.Windows.Forms.Padding(2);
            this.rollbackbutton.Name = "rollbackbutton";
            this.rollbackbutton.Size = new System.Drawing.Size(883, 53);
            this.rollbackbutton.TabIndex = 8;
            this.rollbackbutton.Text = "Rollback";
            this.rollbackbutton.UseVisualStyleBackColor = false;
            this.rollbackbutton.Click += new System.EventHandler(this.rollbackbutton_Click);
            // 
            // entitiesList
            // 
            this.entitiesList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.entitiesList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.entitiesList.FormattingEnabled = true;
            this.entitiesList.Location = new System.Drawing.Point(14, 93);
            this.entitiesList.Margin = new System.Windows.Forms.Padding(2);
            this.entitiesList.Name = "entitiesList";
            this.entitiesList.Size = new System.Drawing.Size(266, 21);
            this.entitiesList.TabIndex = 9;
            this.entitiesList.SelectedIndexChanged += new System.EventHandler(this.entitiesList_SelectedIndexChanged);
            // 
            // recordGuid
            // 
            this.recordGuid.AcceptsReturn = true;
            this.recordGuid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recordGuid.Location = new System.Drawing.Point(14, 135);
            this.recordGuid.Margin = new System.Windows.Forms.Padding(2);
            this.recordGuid.MaxLength = 38;
            this.recordGuid.Name = "recordGuid";
            this.recordGuid.Size = new System.Drawing.Size(266, 22);
            this.recordGuid.TabIndex = 10;
            this.recordGuid.Click += new System.EventHandler(this.recordGuid_Click);
            this.recordGuid.KeyUp += new System.Windows.Forms.KeyEventHandler(this.recordGuid_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 77);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Entity";
            // 
            // guidLabel
            // 
            this.guidLabel.AutoSize = true;
            this.guidLabel.Location = new System.Drawing.Point(11, 118);
            this.guidLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.guidLabel.Name = "guidLabel";
            this.guidLabel.Size = new System.Drawing.Size(67, 13);
            this.guidLabel.TabIndex = 12;
            this.guidLabel.Text = "Record Guid";
            // 
            // showNewestValues
            // 
            this.showNewestValues.AutoSize = true;
            this.showNewestValues.Enabled = false;
            this.showNewestValues.Location = new System.Drawing.Point(14, 173);
            this.showNewestValues.Margin = new System.Windows.Forms.Padding(2);
            this.showNewestValues.Name = "showNewestValues";
            this.showNewestValues.Size = new System.Drawing.Size(175, 17);
            this.showNewestValues.TabIndex = 13;
            this.showNewestValues.Text = "Show most recent audit records";
            this.showNewestValues.UseVisualStyleBackColor = true;
            this.showNewestValues.CheckedChanged += new System.EventHandler(this.showNewestValues_CheckedChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(297, 93);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(883, 456);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // autoCopyGuidFromClipboard
            // 
            this.autoCopyGuidFromClipboard.AutoSize = true;
            this.autoCopyGuidFromClipboard.Checked = true;
            this.autoCopyGuidFromClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoCopyGuidFromClipboard.Location = new System.Drawing.Point(14, 195);
            this.autoCopyGuidFromClipboard.Name = "autoCopyGuidFromClipboard";
            this.autoCopyGuidFromClipboard.Size = new System.Drawing.Size(170, 17);
            this.autoCopyGuidFromClipboard.TabIndex = 14;
            this.autoCopyGuidFromClipboard.Text = "Auto-Copy Guid from Clipboard";
            this.autoCopyGuidFromClipboard.UseVisualStyleBackColor = true;
            this.autoCopyGuidFromClipboard.CheckedChanged += new System.EventHandler(this.autoCopyGuidFromClipboard_CheckedChanged);
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoCopyGuidFromClipboard);
            this.Controls.Add(this.showNewestValues);
            this.Controls.Add(this.guidLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.recordGuid);
            this.Controls.Add(this.entitiesList);
            this.Controls.Add(this.rollbackbutton);
            this.Controls.Add(this.loadAuditButton);
            this.Controls.Add(this.toolStripMenu);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(1182, 551);
            this.OnCloseTool += new System.EventHandler(this.MyPluginControl_OnCloseTool);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbSample;
        private System.Windows.Forms.Button loadAuditButton;
        private System.Windows.Forms.Button rollbackbutton;
        private System.Windows.Forms.ComboBox entitiesList;
        private System.Windows.Forms.ToolStripButton LoadDataButton;
        private System.Windows.Forms.TextBox recordGuid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label guidLabel;
        private System.Windows.Forms.CheckBox showNewestValues;
        private System.Windows.Forms.ToolStripButton fetchXML;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox autoCopyGuidFromClipboard;
    }
}
