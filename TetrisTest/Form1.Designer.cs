namespace TetrisTest
{
    partial class TetrisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TetrisForm));
            GameArea = new PictureBox();
            label1 = new Label();
            labelScore = new Label();
            label2 = new Label();
            gameStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)GameArea).BeginInit();
            SuspendLayout();
            // 
            // GameArea
            // 
            GameArea.BorderStyle = BorderStyle.Fixed3D;
            GameArea.Location = new Point(12, 12);
            GameArea.Name = "GameArea";
            GameArea.Size = new Size(205, 405);
            GameArea.TabIndex = 0;
            GameArea.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(234, 12);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 1;
            label1.Text = "Score:";
            // 
            // labelScore
            // 
            labelScore.AutoSize = true;
            labelScore.Location = new Point(279, 12);
            labelScore.Name = "labelScore";
            labelScore.Size = new Size(0, 15);
            labelScore.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(234, 52);
            label2.Name = "label2";
            label2.Size = new Size(75, 15);
            label2.TabIndex = 3;
            label2.Text = "Game status:";
            // 
            // gameStatus
            // 
            gameStatus.AutoSize = true;
            gameStatus.Location = new Point(241, 79);
            gameStatus.Name = "gameStatus";
            gameStatus.Size = new Size(0, 15);
            gameStatus.TabIndex = 4;
            // 
            // TetrisForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 431);
            Controls.Add(gameStatus);
            Controls.Add(label2);
            Controls.Add(labelScore);
            Controls.Add(label1);
            Controls.Add(GameArea);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(400, 470);
            MinimumSize = new Size(400, 470);
            Name = "TetrisForm";
            Text = "Tetris";
            Load += TetrisForm_Load;
            KeyDown += TetrisForm_KeyDown;
            KeyUp += TetrisForm_KeyUp_1;
            ((System.ComponentModel.ISupportInitialize)GameArea).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox GameArea;
        private Label label1;
        private Label labelScore;
        private Label label2;
        private Label gameStatus;
    }
}