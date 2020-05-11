namespace AgCubioGUI
{
    /// <summary>
    /// Contains parameters for setting up the initial form
    /// </summary>
    partial class AgCubio_Form
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
            this.player_label = new System.Windows.Forms.Label();
            this.Server_label = new System.Windows.Forms.Label();
            this.player_textBox = new System.Windows.Forms.TextBox();
            this.Server_textBox = new System.Windows.Forms.TextBox();
            this.inform_textBox = new System.Windows.Forms.TextBox();
            this.FPS_label = new System.Windows.Forms.Label();
            this.food_label = new System.Windows.Forms.Label();
            this.mass_label = new System.Windows.Forms.Label();
            this.width_label = new System.Windows.Forms.Label();
            this.FPSNum_label = new System.Windows.Forms.Label();
            this.foodNum_label = new System.Windows.Forms.Label();
            this.massNum_label = new System.Windows.Forms.Label();
            this.widthNum_label = new System.Windows.Forms.Label();
            this.panel_Network = new System.Windows.Forms.Panel();
            this.button_Start = new System.Windows.Forms.Button();
            this.panel_Game = new AgCubioGUI.Game_Panel();
            this.panel_Network.SuspendLayout();
            this.SuspendLayout();
            // 
            // player_label
            // 
            this.player_label.AutoSize = true;
            this.player_label.Font = new System.Drawing.Font("SimSun", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.player_label.Location = new System.Drawing.Point(57, 24);
            this.player_label.Name = "player_label";
            this.player_label.Size = new System.Drawing.Size(166, 27);
            this.player_label.TabIndex = 0;
            this.player_label.Text = "Player Name";
            // 
            // Server_label
            // 
            this.Server_label.AutoSize = true;
            this.Server_label.Font = new System.Drawing.Font("SimSun", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Server_label.Location = new System.Drawing.Point(57, 93);
            this.Server_label.Name = "Server_label";
            this.Server_label.Size = new System.Drawing.Size(96, 27);
            this.Server_label.TabIndex = 1;
            this.Server_label.Text = "Server";
            // 
            // player_textBox
            // 
            this.player_textBox.Font = new System.Drawing.Font("SimSun", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.player_textBox.Location = new System.Drawing.Point(229, 13);
            this.player_textBox.Name = "player_textBox";
            this.player_textBox.Size = new System.Drawing.Size(200, 38);
            this.player_textBox.TabIndex = 2;
            this.player_textBox.Text = "player";
            this.player_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.player_textBox_KeyDown);
            // 
            // Server_textBox
            // 
            this.Server_textBox.Font = new System.Drawing.Font("SimSun", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Server_textBox.Location = new System.Drawing.Point(229, 82);
            this.Server_textBox.Name = "Server_textBox";
            this.Server_textBox.Size = new System.Drawing.Size(200, 38);
            this.Server_textBox.TabIndex = 3;
            this.Server_textBox.Text = "localhost";
            this.Server_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.player_textBox_KeyDown);
            // 
            // inform_textBox
            // 
            this.inform_textBox.BackColor = System.Drawing.SystemColors.Menu;
            this.inform_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.inform_textBox.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.inform_textBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.inform_textBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.inform_textBox.Location = new System.Drawing.Point(40, 183);
            this.inform_textBox.Multiline = true;
            this.inform_textBox.Name = "inform_textBox";
            this.inform_textBox.ReadOnly = true;
            this.inform_textBox.Size = new System.Drawing.Size(538, 272);
            this.inform_textBox.TabIndex = 4;
            this.inform_textBox.Text = "Welcome to AgCubio!\r\n\r\nEnter your name and server to start the game.\r\nNote: the p" +
    "layer name should not be empty!";
            this.inform_textBox.TextChanged += new System.EventHandler(this.inform_textBox_TextChanged);
            // 
            // FPS_label
            // 
            this.FPS_label.AutoSize = true;
            this.FPS_label.Location = new System.Drawing.Point(12, 17);
            this.FPS_label.Name = "FPS_label";
            this.FPS_label.Size = new System.Drawing.Size(23, 12);
            this.FPS_label.TabIndex = 5;
            this.FPS_label.Text = "FPS";
            // 
            // food_label
            // 
            this.food_label.AutoSize = true;
            this.food_label.Location = new System.Drawing.Point(12, 45);
            this.food_label.Name = "food_label";
            this.food_label.Size = new System.Drawing.Size(29, 12);
            this.food_label.TabIndex = 6;
            this.food_label.Text = "Food";
            // 
            // mass_label
            // 
            this.mass_label.AutoSize = true;
            this.mass_label.Location = new System.Drawing.Point(12, 75);
            this.mass_label.Name = "mass_label";
            this.mass_label.Size = new System.Drawing.Size(29, 12);
            this.mass_label.TabIndex = 7;
            this.mass_label.Text = "Mass";
            // 
            // width_label
            // 
            this.width_label.AutoSize = true;
            this.width_label.Location = new System.Drawing.Point(12, 102);
            this.width_label.Name = "width_label";
            this.width_label.Size = new System.Drawing.Size(35, 12);
            this.width_label.TabIndex = 8;
            this.width_label.Text = "Width";
            // 
            // FPSNum_label
            // 
            this.FPSNum_label.AutoSize = true;
            this.FPSNum_label.Location = new System.Drawing.Point(79, 17);
            this.FPSNum_label.Name = "FPSNum_label";
            this.FPSNum_label.Size = new System.Drawing.Size(23, 12);
            this.FPSNum_label.TabIndex = 9;
            this.FPSNum_label.Text = "FPS";
            // 
            // foodNum_label
            // 
            this.foodNum_label.AutoSize = true;
            this.foodNum_label.Location = new System.Drawing.Point(79, 45);
            this.foodNum_label.Name = "foodNum_label";
            this.foodNum_label.Size = new System.Drawing.Size(29, 12);
            this.foodNum_label.TabIndex = 10;
            this.foodNum_label.Text = "none";
            // 
            // massNum_label
            // 
            this.massNum_label.AutoEllipsis = true;
            this.massNum_label.AutoSize = true;
            this.massNum_label.Location = new System.Drawing.Point(79, 75);
            this.massNum_label.Name = "massNum_label";
            this.massNum_label.Size = new System.Drawing.Size(29, 12);
            this.massNum_label.TabIndex = 11;
            this.massNum_label.Text = "Mass";
            // 
            // widthNum_label
            // 
            this.widthNum_label.AutoSize = true;
            this.widthNum_label.Location = new System.Drawing.Point(79, 103);
            this.widthNum_label.Name = "widthNum_label";
            this.widthNum_label.Size = new System.Drawing.Size(35, 12);
            this.widthNum_label.TabIndex = 12;
            this.widthNum_label.Text = "Width";
            // 
            // panel_Network
            // 
            this.panel_Network.Controls.Add(this.button_Start);
            this.panel_Network.Controls.Add(this.player_label);
            this.panel_Network.Controls.Add(this.Server_label);
            this.panel_Network.Controls.Add(this.player_textBox);
            this.panel_Network.Controls.Add(this.Server_textBox);
            this.panel_Network.Controls.Add(this.inform_textBox);
            this.panel_Network.Location = new System.Drawing.Point(135, 155);
            this.panel_Network.Name = "panel_Network";
            this.panel_Network.Size = new System.Drawing.Size(670, 480);
            this.panel_Network.TabIndex = 13;
            this.panel_Network.VisibleChanged += new System.EventHandler(this.panel_Network_VisibleChanged);
            // 
            // button_Start
            // 
            this.button_Start.AutoSize = true;
            this.button_Start.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Start.Location = new System.Drawing.Point(503, 88);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 32);
            this.button_Start.TabIndex = 5;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // panel_Game
            // 
            this.panel_Game.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Game.Location = new System.Drawing.Point(0, 0);
            this.panel_Game.Name = "panel_Game";
            this.panel_Game.Size = new System.Drawing.Size(844, 655);
            this.panel_Game.TabIndex = 14;
            this.panel_Game.Visible = false;
            this.panel_Game.Paint += new System.Windows.Forms.PaintEventHandler(this.AgCubio_Form_Paint);
            // 
            // AgCubio_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(844, 655);
            this.Controls.Add(this.panel_Network);
            this.Controls.Add(this.widthNum_label);
            this.Controls.Add(this.massNum_label);
            this.Controls.Add(this.foodNum_label);
            this.Controls.Add(this.FPSNum_label);
            this.Controls.Add(this.width_label);
            this.Controls.Add(this.mass_label);
            this.Controls.Add(this.food_label);
            this.Controls.Add(this.FPS_label);
            this.Controls.Add(this.panel_Game);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Name = "AgCubio_Form";
            this.Text = "AgCubio";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Split);
            this.panel_Network.ResumeLayout(false);
            this.panel_Network.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label player_label;
        private System.Windows.Forms.Label Server_label;
        private System.Windows.Forms.TextBox player_textBox;
        private System.Windows.Forms.TextBox Server_textBox;
        private System.Windows.Forms.TextBox inform_textBox;
        private System.Windows.Forms.Label FPS_label;
        private System.Windows.Forms.Label food_label;
        private System.Windows.Forms.Label mass_label;
        private System.Windows.Forms.Label width_label;
        private System.Windows.Forms.Label FPSNum_label;
        private System.Windows.Forms.Label foodNum_label;
        private System.Windows.Forms.Label massNum_label;
        private System.Windows.Forms.Label widthNum_label;
        private System.Windows.Forms.Panel panel_Network;
        private Game_Panel panel_Game;
        private System.Windows.Forms.Button button_Start;
    }
}

