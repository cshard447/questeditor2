﻿namespace StalkerOnlineQuesterEditor
{
    partial class EditDialogForm
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
            this.tPlayerText = new System.Windows.Forms.TextBox();
            this.lAnswerText = new System.Windows.Forms.Label();
            this.NPCSaidIs = new System.Windows.Forms.Label();
            this.NPCSaid = new System.Windows.Forms.Label();
            this.lAttention = new System.Windows.Forms.Label();
            this.actionsBox = new System.Windows.Forms.GroupBox();
            this.barterCheckBox = new System.Windows.Forms.CheckBox();
            this.toComplexRapairCheckBox = new System.Windows.Forms.CheckBox();
            this.teleportComboBox = new System.Windows.Forms.ComboBox();
            this.tleportCheckBox = new System.Windows.Forms.CheckBox();
            this.toRepairCheckBox = new System.Windows.Forms.CheckBox();
            this.changeCheckBox = new System.Windows.Forms.CheckBox();
            this.CompleteQuetsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.GetQuestsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.ToDialogComboBox = new System.Windows.Forms.ComboBox();
            this.ExitcheckBox = new System.Windows.Forms.CheckBox();
            this.toTradeCheckBox = new System.Windows.Forms.CheckBox();
            this.CompleteQuestsCheckBox = new System.Windows.Forms.CheckBox();
            this.GetQuestsCheckBox = new System.Windows.Forms.CheckBox();
            this.ToDialogCheckBox1 = new System.Windows.Forms.CheckBox();
            this.actionsCheckBox = new System.Windows.Forms.CheckBox();
            this.tNPCReactiontextBox = new System.Windows.Forms.TextBox();
            this.tSubDialogsTextBox = new System.Windows.Forms.MaskedTextBox();
            this.subDialogsLabel = new System.Windows.Forms.Label();
            this.PreconditionBox = new System.Windows.Forms.GroupBox();
            this.CheckLonerCheckBox = new System.Windows.Forms.CheckBox();
            this.bKarma = new System.Windows.Forms.Button();
            this.bReputation = new System.Windows.Forms.Button();
            this.CheckClanCheckBox = new System.Windows.Forms.CheckBox();
            this.CheckClanIDcheckBox = new System.Windows.Forms.CheckBox();
            this.QuestConditiongroupBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tMustHaveQuestsOnTest = new System.Windows.Forms.MaskedTextBox();
            this.tShouldntHaveCompletedQuests = new System.Windows.Forms.MaskedTextBox();
            this.tMustHaveOpenQuests = new System.Windows.Forms.MaskedTextBox();
            this.tShouldntHaveQuestsOnTest = new System.Windows.Forms.MaskedTextBox();
            this.tShouldntHaveOpenQuests = new System.Windows.Forms.MaskedTextBox();
            this.tMustHaveCompletedQuests = new System.Windows.Forms.MaskedTextBox();
            this.bEditDialogOk = new System.Windows.Forms.Button();
            this.bEditDialogCancel = new System.Windows.Forms.Button();
            this.NPCReactionText = new System.Windows.Forms.Label();
            this.MustPanel = new System.Windows.Forms.Panel();
            this.textGroupBox = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tShouldntHaveFailedQuests = new System.Windows.Forms.MaskedTextBox();
            this.tMustHaveFailedQuests = new System.Windows.Forms.MaskedTextBox();
            this.actionsBox.SuspendLayout();
            this.PreconditionBox.SuspendLayout();
            this.QuestConditiongroupBox.SuspendLayout();
            this.MustPanel.SuspendLayout();
            this.textGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tPlayerText
            // 
            this.tPlayerText.Location = new System.Drawing.Point(103, 171);
            this.tPlayerText.Name = "tPlayerText";
            this.tPlayerText.Size = new System.Drawing.Size(479, 20);
            this.tPlayerText.TabIndex = 0;
            this.tPlayerText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tPlayerText_KeyPress);
            // 
            // lAnswerText
            // 
            this.lAnswerText.AutoSize = true;
            this.lAnswerText.Location = new System.Drawing.Point(3, 171);
            this.lAnswerText.Name = "lAnswerText";
            this.lAnswerText.Size = new System.Drawing.Size(86, 13);
            this.lAnswerText.TabIndex = 1;
            this.lAnswerText.Text = "Вариант ответа";
            // 
            // NPCSaidIs
            // 
            this.NPCSaidIs.AutoSize = true;
            this.NPCSaidIs.Location = new System.Drawing.Point(107, 9);
            this.NPCSaidIs.Name = "NPCSaidIs";
            this.NPCSaidIs.Size = new System.Drawing.Size(13, 13);
            this.NPCSaidIs.TabIndex = 11;
            this.NPCSaidIs.Text = "  ";
            // 
            // NPCSaid
            // 
            this.NPCSaid.AutoSize = true;
            this.NPCSaid.Location = new System.Drawing.Point(3, 9);
            this.NPCSaid.Name = "NPCSaid";
            this.NPCSaid.Size = new System.Drawing.Size(98, 13);
            this.NPCSaid.TabIndex = 12;
            this.NPCSaid.Text = "Приветствие NPC";
            // 
            // lAttention
            // 
            this.lAttention.AutoSize = true;
            this.lAttention.ForeColor = System.Drawing.Color.DarkRed;
            this.lAttention.Location = new System.Drawing.Point(100, 155);
            this.lAttention.Name = "lAttention";
            this.lAttention.Size = new System.Drawing.Size(35, 13);
            this.lAttention.TabIndex = 13;
            this.lAttention.Text = "label6";
            // 
            // actionsBox
            // 
            this.actionsBox.Controls.Add(this.barterCheckBox);
            this.actionsBox.Controls.Add(this.toComplexRapairCheckBox);
            this.actionsBox.Controls.Add(this.teleportComboBox);
            this.actionsBox.Controls.Add(this.tleportCheckBox);
            this.actionsBox.Controls.Add(this.toRepairCheckBox);
            this.actionsBox.Controls.Add(this.changeCheckBox);
            this.actionsBox.Controls.Add(this.CompleteQuetsTextBox);
            this.actionsBox.Controls.Add(this.GetQuestsTextBox);
            this.actionsBox.Controls.Add(this.ToDialogComboBox);
            this.actionsBox.Controls.Add(this.ExitcheckBox);
            this.actionsBox.Controls.Add(this.toTradeCheckBox);
            this.actionsBox.Controls.Add(this.CompleteQuestsCheckBox);
            this.actionsBox.Controls.Add(this.GetQuestsCheckBox);
            this.actionsBox.Controls.Add(this.ToDialogCheckBox1);
            this.actionsBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.actionsBox.Enabled = false;
            this.actionsBox.Location = new System.Drawing.Point(0, 500);
            this.actionsBox.Name = "actionsBox";
            this.actionsBox.Size = new System.Drawing.Size(588, 115);
            this.actionsBox.TabIndex = 8;
            this.actionsBox.TabStop = false;
            this.actionsBox.Enter += new System.EventHandler(this.actionsBox_Enter);
            // 
            // barterCheckBox
            // 
            this.barterCheckBox.AutoSize = true;
            this.barterCheckBox.Location = new System.Drawing.Point(6, 83);
            this.barterCheckBox.Name = "barterCheckBox";
            this.barterCheckBox.Size = new System.Drawing.Size(62, 17);
            this.barterCheckBox.TabIndex = 14;
            this.barterCheckBox.Text = "Бартер";
            this.barterCheckBox.UseVisualStyleBackColor = true;
            this.barterCheckBox.CheckedChanged += new System.EventHandler(this.barterCheckBox_CheckedChanged);
            // 
            // toComplexRapairCheckBox
            // 
            this.toComplexRapairCheckBox.AutoSize = true;
            this.toComplexRapairCheckBox.Location = new System.Drawing.Point(117, 62);
            this.toComplexRapairCheckBox.Name = "toComplexRapairCheckBox";
            this.toComplexRapairCheckBox.Size = new System.Drawing.Size(139, 17);
            this.toComplexRapairCheckBox.TabIndex = 13;
            this.toComplexRapairCheckBox.Text = "Комплексная починка";
            this.toComplexRapairCheckBox.UseVisualStyleBackColor = true;
            this.toComplexRapairCheckBox.CheckedChanged += new System.EventHandler(this.toComplexRapairCheckBox_CheckedChanged);
            // 
            // teleportComboBox
            // 
            this.teleportComboBox.Enabled = false;
            this.teleportComboBox.FormattingEnabled = true;
            this.teleportComboBox.Location = new System.Drawing.Point(379, 83);
            this.teleportComboBox.Name = "teleportComboBox";
            this.teleportComboBox.Size = new System.Drawing.Size(100, 21);
            this.teleportComboBox.TabIndex = 12;
            // 
            // tleportCheckBox
            // 
            this.tleportCheckBox.AutoSize = true;
            this.tleportCheckBox.Location = new System.Drawing.Point(266, 85);
            this.tleportCheckBox.Name = "tleportCheckBox";
            this.tleportCheckBox.Size = new System.Drawing.Size(74, 17);
            this.tleportCheckBox.TabIndex = 11;
            this.tleportCheckBox.Text = "Телепорт";
            this.tleportCheckBox.UseVisualStyleBackColor = true;
            this.tleportCheckBox.CheckedChanged += new System.EventHandler(this.tleportCheckBox_CheckedChanged);
            // 
            // toRepairCheckBox
            // 
            this.toRepairCheckBox.AutoSize = true;
            this.toRepairCheckBox.Location = new System.Drawing.Point(117, 41);
            this.toRepairCheckBox.Name = "toRepairCheckBox";
            this.toRepairCheckBox.Size = new System.Drawing.Size(69, 17);
            this.toRepairCheckBox.TabIndex = 10;
            this.toRepairCheckBox.Text = "Починка";
            this.toRepairCheckBox.UseVisualStyleBackColor = true;
            this.toRepairCheckBox.CheckedChanged += new System.EventHandler(this.toRepairCheckBox_CheckedChanged);
            // 
            // changeCheckBox
            // 
            this.changeCheckBox.AutoSize = true;
            this.changeCheckBox.Location = new System.Drawing.Point(6, 62);
            this.changeCheckBox.Name = "changeCheckBox";
            this.changeCheckBox.Size = new System.Drawing.Size(60, 17);
            this.changeCheckBox.TabIndex = 9;
            this.changeCheckBox.Text = "Обмен";
            this.changeCheckBox.UseVisualStyleBackColor = true;
            this.changeCheckBox.CheckedChanged += new System.EventHandler(this.changeCheckBox_CheckedChanged);
            // 
            // CompleteQuetsTextBox
            // 
            this.CompleteQuetsTextBox.Enabled = false;
            this.CompleteQuetsTextBox.Location = new System.Drawing.Point(379, 39);
            this.CompleteQuetsTextBox.Name = "CompleteQuetsTextBox";
            this.CompleteQuetsTextBox.Size = new System.Drawing.Size(100, 20);
            this.CompleteQuetsTextBox.TabIndex = 8;
            // 
            // GetQuestsTextBox
            // 
            this.GetQuestsTextBox.Enabled = false;
            this.GetQuestsTextBox.Location = new System.Drawing.Point(379, 16);
            this.GetQuestsTextBox.Name = "GetQuestsTextBox";
            this.GetQuestsTextBox.Size = new System.Drawing.Size(100, 20);
            this.GetQuestsTextBox.TabIndex = 7;
            // 
            // ToDialogComboBox
            // 
            this.ToDialogComboBox.Enabled = false;
            this.ToDialogComboBox.FormattingEnabled = true;
            this.ToDialogComboBox.Location = new System.Drawing.Point(117, 19);
            this.ToDialogComboBox.Name = "ToDialogComboBox";
            this.ToDialogComboBox.Size = new System.Drawing.Size(100, 21);
            this.ToDialogComboBox.TabIndex = 6;
            // 
            // ExitcheckBox
            // 
            this.ExitcheckBox.AutoSize = true;
            this.ExitcheckBox.Location = new System.Drawing.Point(117, 83);
            this.ExitcheckBox.Name = "ExitcheckBox";
            this.ExitcheckBox.Size = new System.Drawing.Size(141, 17);
            this.ExitcheckBox.TabIndex = 5;
            this.ExitcheckBox.Text = "Закрыть окно диалога";
            this.ExitcheckBox.UseVisualStyleBackColor = true;
            this.ExitcheckBox.CheckedChanged += new System.EventHandler(this.ExitcheckBox_CheckedChanged);
            // 
            // toTradeCheckBox
            // 
            this.toTradeCheckBox.AutoSize = true;
            this.toTradeCheckBox.Location = new System.Drawing.Point(6, 42);
            this.toTradeCheckBox.Name = "toTradeCheckBox";
            this.toTradeCheckBox.Size = new System.Drawing.Size(79, 17);
            this.toTradeCheckBox.TabIndex = 4;
            this.toTradeCheckBox.Text = "Торговать";
            this.toTradeCheckBox.UseVisualStyleBackColor = true;
            this.toTradeCheckBox.CheckedChanged += new System.EventHandler(this.toTradeCheckBox_CheckedChanged);
            // 
            // CompleteQuestsCheckBox
            // 
            this.CompleteQuestsCheckBox.AutoSize = true;
            this.CompleteQuestsCheckBox.Location = new System.Drawing.Point(266, 42);
            this.CompleteQuestsCheckBox.Name = "CompleteQuestsCheckBox";
            this.CompleteQuestsCheckBox.Size = new System.Drawing.Size(119, 17);
            this.CompleteQuestsCheckBox.TabIndex = 3;
            this.CompleteQuestsCheckBox.Text = "Закончить квесты";
            this.CompleteQuestsCheckBox.UseVisualStyleBackColor = true;
            this.CompleteQuestsCheckBox.CheckedChanged += new System.EventHandler(this.CompleteQuestsCheckBox_CheckedChanged);
            // 
            // GetQuestsCheckBox
            // 
            this.GetQuestsCheckBox.AutoSize = true;
            this.GetQuestsCheckBox.Location = new System.Drawing.Point(266, 17);
            this.GetQuestsCheckBox.Name = "GetQuestsCheckBox";
            this.GetQuestsCheckBox.Size = new System.Drawing.Size(96, 17);
            this.GetQuestsCheckBox.TabIndex = 2;
            this.GetQuestsCheckBox.Text = "Взять квесты";
            this.GetQuestsCheckBox.UseVisualStyleBackColor = true;
            this.GetQuestsCheckBox.CheckedChanged += new System.EventHandler(this.GetQuestsCheckBox_CheckedChanged);
            // 
            // ToDialogCheckBox1
            // 
            this.ToDialogCheckBox1.AutoSize = true;
            this.ToDialogCheckBox1.Location = new System.Drawing.Point(6, 19);
            this.ToDialogCheckBox1.Name = "ToDialogCheckBox1";
            this.ToDialogCheckBox1.Size = new System.Drawing.Size(114, 17);
            this.ToDialogCheckBox1.TabIndex = 1;
            this.ToDialogCheckBox1.Text = "Переход на диал.";
            this.ToDialogCheckBox1.UseVisualStyleBackColor = true;
            this.ToDialogCheckBox1.CheckedChanged += new System.EventHandler(this.ToDialogCheckBox1_CheckedChanged);
            // 
            // actionsCheckBox
            // 
            this.actionsCheckBox.AutoSize = true;
            this.actionsCheckBox.Location = new System.Drawing.Point(6, 498);
            this.actionsCheckBox.Name = "actionsCheckBox";
            this.actionsCheckBox.Size = new System.Drawing.Size(76, 17);
            this.actionsCheckBox.TabIndex = 0;
            this.actionsCheckBox.Text = "Действия";
            this.actionsCheckBox.UseVisualStyleBackColor = true;
            this.actionsCheckBox.CheckedChanged += new System.EventHandler(this.actionsCheckBox_CheckedChanged);
            // 
            // tNPCReactiontextBox
            // 
            this.tNPCReactiontextBox.Location = new System.Drawing.Point(103, 77);
            this.tNPCReactiontextBox.Multiline = true;
            this.tNPCReactiontextBox.Name = "tNPCReactiontextBox";
            this.tNPCReactiontextBox.Size = new System.Drawing.Size(479, 75);
            this.tNPCReactiontextBox.TabIndex = 7;
            // 
            // tSubDialogsTextBox
            // 
            this.tSubDialogsTextBox.Location = new System.Drawing.Point(103, 197);
            this.tSubDialogsTextBox.Name = "tSubDialogsTextBox";
            this.tSubDialogsTextBox.Size = new System.Drawing.Size(479, 20);
            this.tSubDialogsTextBox.TabIndex = 10;
            // 
            // subDialogsLabel
            // 
            this.subDialogsLabel.AutoSize = true;
            this.subDialogsLabel.Location = new System.Drawing.Point(3, 204);
            this.subDialogsLabel.Name = "subDialogsLabel";
            this.subDialogsLabel.Size = new System.Drawing.Size(68, 13);
            this.subDialogsLabel.TabIndex = 9;
            this.subDialogsLabel.Text = "Поддиалоги";
            // 
            // PreconditionBox
            // 
            this.PreconditionBox.Controls.Add(this.CheckLonerCheckBox);
            this.PreconditionBox.Controls.Add(this.bKarma);
            this.PreconditionBox.Controls.Add(this.bReputation);
            this.PreconditionBox.Controls.Add(this.CheckClanCheckBox);
            this.PreconditionBox.Controls.Add(this.CheckClanIDcheckBox);
            this.PreconditionBox.Controls.Add(this.QuestConditiongroupBox);
            this.PreconditionBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.PreconditionBox.Location = new System.Drawing.Point(0, 227);
            this.PreconditionBox.Name = "PreconditionBox";
            this.PreconditionBox.Size = new System.Drawing.Size(588, 273);
            this.PreconditionBox.TabIndex = 4;
            this.PreconditionBox.TabStop = false;
            this.PreconditionBox.Text = "Условия активности узла";
            // 
            // CheckLonerCheckBox
            // 
            this.CheckLonerCheckBox.AutoSize = true;
            this.CheckLonerCheckBox.Location = new System.Drawing.Point(392, 78);
            this.CheckLonerCheckBox.Name = "CheckLonerCheckBox";
            this.CheckLonerCheckBox.Size = new System.Drawing.Size(134, 17);
            this.CheckLonerCheckBox.TabIndex = 48;
            this.CheckLonerCheckBox.Text = "Только для одиночек";
            this.CheckLonerCheckBox.UseVisualStyleBackColor = true;
            // 
            // bKarma
            // 
            this.bKarma.Location = new System.Drawing.Point(95, 175);
            this.bKarma.Name = "bKarma";
            this.bKarma.Size = new System.Drawing.Size(81, 23);
            this.bKarma.TabIndex = 47;
            this.bKarma.Text = "Карма";
            this.bKarma.UseVisualStyleBackColor = true;
            this.bKarma.Click += new System.EventHandler(this.bKarma_Click);
            // 
            // bReputation
            // 
            this.bReputation.Location = new System.Drawing.Point(8, 175);
            this.bReputation.Name = "bReputation";
            this.bReputation.Size = new System.Drawing.Size(81, 23);
            this.bReputation.TabIndex = 46;
            this.bReputation.Text = "Репутация";
            this.bReputation.UseVisualStyleBackColor = true;
            this.bReputation.Click += new System.EventHandler(this.bReputation_Click);
            // 
            // CheckClanCheckBox
            // 
            this.CheckClanCheckBox.AutoSize = true;
            this.CheckClanCheckBox.Location = new System.Drawing.Point(392, 54);
            this.CheckClanCheckBox.Name = "CheckClanCheckBox";
            this.CheckClanCheckBox.Size = new System.Drawing.Size(162, 17);
            this.CheckClanCheckBox.TabIndex = 8;
            this.CheckClanCheckBox.Text = "Только для имеющих клан";
            this.CheckClanCheckBox.UseVisualStyleBackColor = true;
            // 
            // CheckClanIDcheckBox
            // 
            this.CheckClanIDcheckBox.AutoSize = true;
            this.CheckClanIDcheckBox.Location = new System.Drawing.Point(392, 30);
            this.CheckClanIDcheckBox.Name = "CheckClanIDcheckBox";
            this.CheckClanIDcheckBox.Size = new System.Drawing.Size(147, 17);
            this.CheckClanIDcheckBox.TabIndex = 7;
            this.CheckClanIDcheckBox.Text = "Только для соклановца";
            this.CheckClanIDcheckBox.UseVisualStyleBackColor = true;
            // 
            // QuestConditiongroupBox
            // 
            this.QuestConditiongroupBox.Controls.Add(this.label6);
            this.QuestConditiongroupBox.Controls.Add(this.tShouldntHaveFailedQuests);
            this.QuestConditiongroupBox.Controls.Add(this.tMustHaveFailedQuests);
            this.QuestConditiongroupBox.Controls.Add(this.label5);
            this.QuestConditiongroupBox.Controls.Add(this.label4);
            this.QuestConditiongroupBox.Controls.Add(this.label3);
            this.QuestConditiongroupBox.Controls.Add(this.label2);
            this.QuestConditiongroupBox.Controls.Add(this.label1);
            this.QuestConditiongroupBox.Controls.Add(this.tMustHaveQuestsOnTest);
            this.QuestConditiongroupBox.Controls.Add(this.tShouldntHaveCompletedQuests);
            this.QuestConditiongroupBox.Controls.Add(this.tMustHaveOpenQuests);
            this.QuestConditiongroupBox.Controls.Add(this.tShouldntHaveQuestsOnTest);
            this.QuestConditiongroupBox.Controls.Add(this.tShouldntHaveOpenQuests);
            this.QuestConditiongroupBox.Controls.Add(this.tMustHaveCompletedQuests);
            this.QuestConditiongroupBox.Location = new System.Drawing.Point(8, 17);
            this.QuestConditiongroupBox.Name = "QuestConditiongroupBox";
            this.QuestConditiongroupBox.Size = new System.Drawing.Size(350, 152);
            this.QuestConditiongroupBox.TabIndex = 6;
            this.QuestConditiongroupBox.TabStop = false;
            this.QuestConditiongroupBox.Text = "Состояния квестов";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(227, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Не должно быть";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(140, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Должно быть";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Закрытые квесты";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Квесты \"к проверке\"";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Открытые квесты";
            // 
            // tMustHaveQuestsOnTest
            // 
            this.tMustHaveQuestsOnTest.Location = new System.Drawing.Point(128, 66);
            this.tMustHaveQuestsOnTest.Name = "tMustHaveQuestsOnTest";
            this.tMustHaveQuestsOnTest.Size = new System.Drawing.Size(100, 20);
            this.tMustHaveQuestsOnTest.TabIndex = 1;
            // 
            // tShouldntHaveCompletedQuests
            // 
            this.tShouldntHaveCompletedQuests.Location = new System.Drawing.Point(234, 92);
            this.tShouldntHaveCompletedQuests.Name = "tShouldntHaveCompletedQuests";
            this.tShouldntHaveCompletedQuests.Size = new System.Drawing.Size(100, 20);
            this.tShouldntHaveCompletedQuests.TabIndex = 5;
            // 
            // tMustHaveOpenQuests
            // 
            this.tMustHaveOpenQuests.Location = new System.Drawing.Point(128, 40);
            this.tMustHaveOpenQuests.Name = "tMustHaveOpenQuests";
            this.tMustHaveOpenQuests.Size = new System.Drawing.Size(100, 20);
            this.tMustHaveOpenQuests.TabIndex = 0;
            // 
            // tShouldntHaveQuestsOnTest
            // 
            this.tShouldntHaveQuestsOnTest.Location = new System.Drawing.Point(234, 66);
            this.tShouldntHaveQuestsOnTest.Name = "tShouldntHaveQuestsOnTest";
            this.tShouldntHaveQuestsOnTest.Size = new System.Drawing.Size(100, 20);
            this.tShouldntHaveQuestsOnTest.TabIndex = 4;
            // 
            // tShouldntHaveOpenQuests
            // 
            this.tShouldntHaveOpenQuests.Location = new System.Drawing.Point(234, 40);
            this.tShouldntHaveOpenQuests.Name = "tShouldntHaveOpenQuests";
            this.tShouldntHaveOpenQuests.Size = new System.Drawing.Size(100, 20);
            this.tShouldntHaveOpenQuests.TabIndex = 3;
            // 
            // tMustHaveCompletedQuests
            // 
            this.tMustHaveCompletedQuests.Location = new System.Drawing.Point(128, 92);
            this.tMustHaveCompletedQuests.Name = "tMustHaveCompletedQuests";
            this.tMustHaveCompletedQuests.Size = new System.Drawing.Size(100, 20);
            this.tMustHaveCompletedQuests.TabIndex = 2;
            // 
            // bEditDialogOk
            // 
            this.bEditDialogOk.Location = new System.Drawing.Point(426, 3);
            this.bEditDialogOk.Name = "bEditDialogOk";
            this.bEditDialogOk.Size = new System.Drawing.Size(75, 23);
            this.bEditDialogOk.TabIndex = 3;
            this.bEditDialogOk.Text = "Ok";
            this.bEditDialogOk.UseVisualStyleBackColor = true;
            this.bEditDialogOk.Click += new System.EventHandler(this.button2_Click);
            // 
            // bEditDialogCancel
            // 
            this.bEditDialogCancel.Location = new System.Drawing.Point(507, 3);
            this.bEditDialogCancel.Name = "bEditDialogCancel";
            this.bEditDialogCancel.Size = new System.Drawing.Size(75, 23);
            this.bEditDialogCancel.TabIndex = 2;
            this.bEditDialogCancel.Text = "Отмена";
            this.bEditDialogCancel.UseVisualStyleBackColor = true;
            this.bEditDialogCancel.Click += new System.EventHandler(this.bEditDialogCancel_Click);
            // 
            // NPCReactionText
            // 
            this.NPCReactionText.AutoSize = true;
            this.NPCReactionText.Location = new System.Drawing.Point(3, 79);
            this.NPCReactionText.Name = "NPCReactionText";
            this.NPCReactionText.Size = new System.Drawing.Size(75, 13);
            this.NPCReactionText.TabIndex = 6;
            this.NPCReactionText.Text = "Реакция NPC";
            // 
            // MustPanel
            // 
            this.MustPanel.Controls.Add(this.bEditDialogCancel);
            this.MustPanel.Controls.Add(this.bEditDialogOk);
            this.MustPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MustPanel.Location = new System.Drawing.Point(0, 615);
            this.MustPanel.Name = "MustPanel";
            this.MustPanel.Size = new System.Drawing.Size(588, 33);
            this.MustPanel.TabIndex = 13;
            // 
            // textGroupBox
            // 
            this.textGroupBox.Controls.Add(this.tSubDialogsTextBox);
            this.textGroupBox.Controls.Add(this.subDialogsLabel);
            this.textGroupBox.Controls.Add(this.tNPCReactiontextBox);
            this.textGroupBox.Controls.Add(this.tPlayerText);
            this.textGroupBox.Controls.Add(this.lAnswerText);
            this.textGroupBox.Controls.Add(this.NPCSaidIs);
            this.textGroupBox.Controls.Add(this.NPCSaid);
            this.textGroupBox.Controls.Add(this.lAttention);
            this.textGroupBox.Controls.Add(this.NPCReactionText);
            this.textGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.textGroupBox.Location = new System.Drawing.Point(0, 0);
            this.textGroupBox.Name = "textGroupBox";
            this.textGroupBox.Size = new System.Drawing.Size(588, 227);
            this.textGroupBox.TabIndex = 15;
            this.textGroupBox.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 119);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Проваленные квесты";
            // 
            // tShouldntHaveFailedQuests
            // 
            this.tShouldntHaveFailedQuests.Location = new System.Drawing.Point(234, 119);
            this.tShouldntHaveFailedQuests.Name = "tShouldntHaveFailedQuests";
            this.tShouldntHaveFailedQuests.Size = new System.Drawing.Size(100, 20);
            this.tShouldntHaveFailedQuests.TabIndex = 12;
            // 
            // tMustHaveFailedQuests
            // 
            this.tMustHaveFailedQuests.Location = new System.Drawing.Point(128, 119);
            this.tMustHaveFailedQuests.Name = "tMustHaveFailedQuests";
            this.tMustHaveFailedQuests.Size = new System.Drawing.Size(100, 20);
            this.tMustHaveFailedQuests.TabIndex = 11;
            // 
            // EditDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 654);
            this.Controls.Add(this.actionsCheckBox);
            this.Controls.Add(this.MustPanel);
            this.Controls.Add(this.actionsBox);
            this.Controls.Add(this.PreconditionBox);
            this.Controls.Add(this.textGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EditDialogForm";
            this.Text = "Редактирование диалога";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditDialogForm_FormClosed);
            this.actionsBox.ResumeLayout(false);
            this.actionsBox.PerformLayout();
            this.PreconditionBox.ResumeLayout(false);
            this.PreconditionBox.PerformLayout();
            this.QuestConditiongroupBox.ResumeLayout(false);
            this.QuestConditiongroupBox.PerformLayout();
            this.MustPanel.ResumeLayout(false);
            this.textGroupBox.ResumeLayout(false);
            this.textGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tPlayerText;
        private System.Windows.Forms.Label lAnswerText;
        private System.Windows.Forms.Label NPCSaidIs;
        private System.Windows.Forms.Label NPCSaid;
        private System.Windows.Forms.Label lAttention;
        private System.Windows.Forms.GroupBox actionsBox;
        private System.Windows.Forms.CheckBox toComplexRapairCheckBox;
        private System.Windows.Forms.ComboBox teleportComboBox;
        private System.Windows.Forms.CheckBox tleportCheckBox;
        private System.Windows.Forms.CheckBox toRepairCheckBox;
        private System.Windows.Forms.CheckBox changeCheckBox;
        private System.Windows.Forms.MaskedTextBox CompleteQuetsTextBox;
        private System.Windows.Forms.MaskedTextBox GetQuestsTextBox;
        private System.Windows.Forms.ComboBox ToDialogComboBox;
        private System.Windows.Forms.CheckBox ExitcheckBox;
        private System.Windows.Forms.CheckBox toTradeCheckBox;
        private System.Windows.Forms.CheckBox CompleteQuestsCheckBox;
        private System.Windows.Forms.CheckBox GetQuestsCheckBox;
        private System.Windows.Forms.CheckBox ToDialogCheckBox1;
        private System.Windows.Forms.TextBox tNPCReactiontextBox;
        private System.Windows.Forms.MaskedTextBox tSubDialogsTextBox;
        private System.Windows.Forms.Label subDialogsLabel;
        private System.Windows.Forms.CheckBox actionsCheckBox;
        private System.Windows.Forms.GroupBox PreconditionBox;
        private System.Windows.Forms.CheckBox CheckClanCheckBox;
        private System.Windows.Forms.CheckBox CheckClanIDcheckBox;
        private System.Windows.Forms.GroupBox QuestConditiongroupBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox tMustHaveQuestsOnTest;
        private System.Windows.Forms.MaskedTextBox tShouldntHaveCompletedQuests;
        private System.Windows.Forms.MaskedTextBox tMustHaveOpenQuests;
        private System.Windows.Forms.MaskedTextBox tShouldntHaveQuestsOnTest;
        private System.Windows.Forms.MaskedTextBox tShouldntHaveOpenQuests;
        private System.Windows.Forms.MaskedTextBox tMustHaveCompletedQuests;
        private System.Windows.Forms.Button bEditDialogOk;
        private System.Windows.Forms.Button bEditDialogCancel;
        private System.Windows.Forms.Label NPCReactionText;
        private System.Windows.Forms.Panel MustPanel;
        private System.Windows.Forms.GroupBox textGroupBox;
        private System.Windows.Forms.Button bReputation;
        private System.Windows.Forms.Button bKarma;
        private System.Windows.Forms.CheckBox CheckLonerCheckBox;
        private System.Windows.Forms.CheckBox barterCheckBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MaskedTextBox tShouldntHaveFailedQuests;
        private System.Windows.Forms.MaskedTextBox tMustHaveFailedQuests;
    }
}