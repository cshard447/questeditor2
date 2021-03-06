﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StalkerOnlineQuesterEditor.Forms
{
    //! Форма локализации квеста, заголовок, описание и т.д.
    public partial class LocaleQuestForm : Form
    {
        int ITEM_REWARD = 0;
        int ITEM_QUESTRULES = 1;
        int ITEM_LOCALIZATION_RULES = 2;
        int ITEM_LOCALIZATION_REWARD = 3;

        MainForm parent;
        public CQuest cur_locale_quest;
        public CQuest pub_quest;
        int pub_version;

        //! Конструктор, заполняем поля формы русскими текстами и локализированными
        public LocaleQuestForm(MainForm parent, int questID)
        {
            InitializeComponent();
            this.parent = parent;

            CQuest locale_quest = parent.getLocaleQuest(questID);
            CQuest quest = parent.getQuestOnQuestID(questID);
            pub_version = quest.Version;
            lViewNpcName.Text = quest.Additional.Holder;
            lViewQuestID.Text = quest.QuestID.ToString();

            if (locale_quest == null)
            {
                // если нет локализации совсем - берем за основу русский квест и обнуляем данные с полями текста
                locale_quest = (CQuest)quest.Clone();
                locale_quest.QuestInformation.Description = "";
                locale_quest.QuestInformation.onFailed = "";
                locale_quest.QuestInformation.onWin = "";
                locale_quest.QuestInformation.Title = "";
                foreach (var key in locale_quest.QuestInformation.Items.Keys)
                {
                    locale_quest.QuestInformation.Items[key].description = "";
                    locale_quest.QuestInformation.Items[key].title = "";
                    locale_quest.QuestInformation.Items[key].activation = "";
                }
            }
            
            cur_locale_quest = (CQuest)locale_quest.Clone();
            pub_quest = (CQuest)quest.Clone();

            titleTextBox.Text = quest.QuestInformation.Title;
            descriptionTextBox.Text = quest.QuestInformation.Description;
            onWonTextBox.Text = quest.QuestInformation.onWin;
            onFailedTextBox.Text = quest.QuestInformation.onFailed;

            localeLitleTextBox.Text = locale_quest.QuestInformation.Title;
            localeDescriptionTextBox.Text = locale_quest.QuestInformation.Description;
            localeOnWonTextBox.Text = locale_quest.QuestInformation.onWin;
            localeOnFailedTextBox.Text = locale_quest.QuestInformation.onFailed;

        }
        //! Закрытие формы
        private void LocaleQuestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Enabled = true;
        }

        private void bItemQuestRules_Click(object sender, EventArgs e)
        {
            ItemDialog dialog = new ItemDialog(this.parent, null, this, cur_locale_quest.QuestID, ITEM_LOCALIZATION_RULES);
            dialog.Enabled = true;
            dialog.Visible = true;
            this.Enabled = false;
        }

        private void bItemReward_Click(object sender, EventArgs e)
        {
            ItemDialog dialog = new ItemDialog(this.parent, null, this, cur_locale_quest.QuestID, ITEM_LOCALIZATION_REWARD);
            dialog.Enabled = true;
            dialog.Visible = true;
            this.Enabled = false;

        }
        //! Нажатие ОК - сохраняем данные о переводе
        private void bOK_Click(object sender, EventArgs e)
        {
            cur_locale_quest.QuestInformation.Title = localeLitleTextBox.Text;
            cur_locale_quest.QuestInformation.Description = localeDescriptionTextBox.Text;
            cur_locale_quest.QuestInformation.onWin = localeOnWonTextBox.Text;
            cur_locale_quest.QuestInformation.onFailed = localeOnFailedTextBox.Text;
            cur_locale_quest.Version = pub_version;
            // возможно здесь придется копировать данные из quest в cur_locale_quest
            parent.addLocaleQuest(cur_locale_quest);
            this.Close();
        }
        //! Нажатие Отмена - выходим без сохранения данных перевода
        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
