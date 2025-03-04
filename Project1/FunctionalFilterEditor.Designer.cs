namespace Project1
{
    partial class FunctionalFilterEditor
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
            pictureBoxGraph = new PictureBox();
            btnAddPoint = new Button();
            btnDeletePoint = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxGraph).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxGraph
            // 
            pictureBoxGraph.Location = new Point(26, 26);
            pictureBoxGraph.Name = "pictureBoxGraph";
            pictureBoxGraph.Size = new Size(256, 256);
            pictureBoxGraph.TabIndex = 0;
            pictureBoxGraph.TabStop = false;
            pictureBoxGraph.Paint += pictureBoxGraph_Paint;
            pictureBoxGraph.MouseDown += pictureBoxGraph_MouseDown;
            pictureBoxGraph.MouseMove += pictureBoxGraph_MouseMove;
            pictureBoxGraph.MouseUp += pictureBoxGraph_MouseUp;
            // 
            // btnAddPoint
            // 
            btnAddPoint.Location = new Point(328, 68);
            btnAddPoint.Name = "btnAddPoint";
            btnAddPoint.Size = new Size(118, 29);
            btnAddPoint.TabIndex = 1;
            btnAddPoint.Text = "Add Point";
            btnAddPoint.UseVisualStyleBackColor = true;
            btnAddPoint.Click += btnAddPoint_Click;
            // 
            // btnDeletePoint
            // 
            btnDeletePoint.Location = new Point(328, 116);
            btnDeletePoint.Name = "btnDeletePoint";
            btnDeletePoint.Size = new Size(118, 29);
            btnDeletePoint.TabIndex = 2;
            btnDeletePoint.Text = "Delete Point";
            btnDeletePoint.UseVisualStyleBackColor = true;
            btnDeletePoint.Click += btnDeletePoint_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(328, 163);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(118, 29);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save + Apply";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(328, 212);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(118, 29);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // FunctionalFilterEditor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(483, 310);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(btnDeletePoint);
            Controls.Add(btnAddPoint);
            Controls.Add(pictureBoxGraph);
            Name = "FunctionalFilterEditor";
            Text = "FunctionalFilterEditor";
            ((System.ComponentModel.ISupportInitialize)pictureBoxGraph).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxGraph;
        private Button btnAddPoint;
        private Button btnDeletePoint;
        private Button btnSave;
        private Button btnCancel;
    }
}