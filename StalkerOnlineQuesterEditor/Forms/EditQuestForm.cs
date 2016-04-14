﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace StalkerOnlineQuesterEditor
{
    //! Форма для редактирования условий квеста
    public partial class EditQuestForm : Form
    {
        int SHOW_MESSAGE_TAKE = 1;
        int SHOW_MESSAGE_CLOSE = 2;
        int SHOW_MESSAGE_PROGRESS = 4;
        int SHOW_JOURNAL = 8;
        int SHOW_ONWIN = 16;
        int SHOW_ONFAILED = 32;
        int SHOW_TUTORIAL = 64;

        const int ADD_NEW = 1;
        const int EDIT = 2;
        const int EDIT_SUB = 3;
        const int ADD_SUB = 4;

        int ITEM_REWARD = 0;
        int ITEM_QUESTRULES = 1;
        int ITEM_LOCALIZATION = 2;

        int QuestType;

        int iInformationHeight = 0;
        int iTargetHeight = 0;
        int iRulesHeight = 0;
        int iRewardHeight = 0;
        int QuestID;

        Dictionary<int, string> dReputation = new Dictionary<int, string>();

        public CQuestTarget editTarget = new CQuestTarget();
        public CQuestRules editQuestRules = new CQuestRules();
        public CQuestReward editQuestReward = new CQuestReward();
        public CQuestInformation editInformation = new CQuestInformation();
        public MainForm parent;
        public CQuest quest;
        int iState;

        //! Конструктор, заполянет форму данными
        public EditQuestForm(MainForm parent, int currentQuest, int iState)
        {
            InitializeComponent();
            this.parent = parent;
            this.iState = iState;
            this.QuestID = currentQuest;

            if (iState != ADD_NEW)
                this.quest = parent.getQuestOnQuestID(currentQuest);
            else
                this.quest = new CQuest();
            if (iState == ADD_SUB || iState == EDIT_SUB)
            {
                titleTextBox.Enabled = false;
                showCloseCheckBox.Enabled = false;
                showCloseCheckBox.Checked = false;
                showTakeCheckBox.Enabled = false;
                showTakeCheckBox.Checked = false;
                tutorialCheckBox.Enabled = false;              
            }

            if (iState == ADD_SUB)
            {
                CQuest parent_quest = parent.getQuestOnQuestID(quest.Additional.IsSubQuest);
                if (parent_quest != null)
                    quest.Reward.Difficulty = parent_quest.Reward.Difficulty;
            }

            if (iState == EDIT || iState == EDIT_SUB)
            {
                editQuestRules = quest.QuestRules;
                editQuestReward = quest.Reward;
                editTarget = quest.Target;
                editInformation = quest.QuestInformation;
            }
            groupQuestBox.Text += " " + currentQuest.ToString();
            this.Text += (": " + this.quest.QuestID.ToString() + "   Версия: " + this.quest.Version.ToString());
            fillForm();
        }

        void setDefaultTargetState()
        {
           foreach (Control control in targetBox.Controls)
               control.Enabled = false;
        }

        void clearTargetContent()
        {
            targetComboBox.Items.Clear();
            //targetComboBox.SelectedText = "";
//          repeatComboBox.Items.Clear();
        }
        //! Заполняет форму данными о квесте
        void fillForm()
        {
            //bItemQuestRules.ImageKey = "";
            //bItemQuestRules.ImageKey = "but_indicate";
            foreach (СQuestType eventDescription in parent.questConst.getListQuests())
                eventComboBox.Items.Add(eventDescription.getDescription());

            showProgressCheckBox.Checked = true;
            showCloseCheckBox.Checked = true;
            showTakeCheckBox.Checked = true;
            showJournalCheckBox.Checked = true;

            difficultyComboBox.SelectedIndex = 0;

            CQuest parentQuest =  new CQuest();
            //if (iState == EDIT_SUB)
            //{
            //    parentQuest = parent.getQuestOnQuestID(quest.Additional.IsSubQuest);
            //}
            if (iState == ADD_SUB)
            {
                if ((quest.Additional.ShowProgress & this.SHOW_TUTORIAL)> 0)
                {
                    tutorialCheckBox.Checked = true;
                    showJournalCheckBox.Checked = false;
                    showJournalCheckBox.Enabled = false;
                }
            }
            else if (iState != ADD_NEW)
            {
                if ((quest.Additional.ShowProgress & this.SHOW_TUTORIAL) > 0)
                {
                    //System.Console.WriteLine("iState != ADD_NEW set tutorial true");
                    tutorialCheckBox.Checked = true;
                }
                else
                    tutorialCheckBox.Checked = false;
            }

            fillPreconditiomForm();
            fillQuestRulesForm();
            if (iState != ADD_NEW)
                fillTargetForm(quest.Target.QuestType);

            if (iState == EDIT || iState == EDIT_SUB)
            {
                if (quest.Target.onFin == 1)
                    winRButton.Checked = true;
                else
                    loseRButton.Checked = true;

                eventComboBox.SelectedItem = parent.questConst.getDescription(quest.Target.QuestType);
                titleTextBox.Text = quest.QuestInformation.Title;
                descriptionTextBox.Text = quest.QuestInformation.Description;
                onWonTextBox.Text = quest.QuestInformation.onWin;
                onFailedTextBox.Text = quest.QuestInformation.onFailed;
                cantCancelCheckBox.Checked = quest.Additional.CantCancel;

                if ((quest.Additional.ShowProgress & this.SHOW_MESSAGE_CLOSE) > 0)
                    showCloseCheckBox.Checked = true;
                else
                    showCloseCheckBox.Checked = false;
                if ((quest.Additional.ShowProgress & this.SHOW_MESSAGE_TAKE) > 0)
                    showTakeCheckBox.Checked = true;
                else
                    showTakeCheckBox.Checked = false;
                if ((quest.Additional.ShowProgress & this.SHOW_MESSAGE_PROGRESS) > 0)
                    showProgressCheckBox.Checked = true;
                else
                    showProgressCheckBox.Checked = false;
                if ((quest.Additional.ShowProgress & this.SHOW_JOURNAL) > 0)
                    showJournalCheckBox.Checked = true;
                else
                    showJournalCheckBox.Checked = false;

                if ((quest.Additional.ShowProgress & this.SHOW_ONWIN) > 0)
                    showWinCheckBox.Checked = true;
                else
                    showWinCheckBox.Checked = false;

                if ((quest.Additional.ShowProgress & this.SHOW_ONFAILED) > 0)
                    showFailedCheckBox.Checked = true;
                else
                    showFailedCheckBox.Checked = false;

                fillTarget();
                fillPrecondition();
                fillQuestRules();
                fillReward();
            }
            else
            {
                winRButton.Checked = true;
            }
        }
        //! Заполняет форму условиями квеста (повторное взятие)
        void fillPreconditiomForm()
        {
            repeatComboBox.Items.Add("Повторное взятие квеста после его закрытия.");
            repeatComboBox.Items.Add("Нет повтора.");
            repeatComboBox.SelectedIndex = 1;
            if (iState == ADD_SUB || iState == EDIT_SUB)
                takenPeriodTextBox.Enabled = false;
        }
        //! Заполняет данные об условиях повторного взятия квеста
        void fillPrecondition()
        {
            if (quest.Precondition.Repeat == 1)
                repeatComboBox.SelectedIndex = 0;
            else
                repeatComboBox.SelectedIndex = 1;

            takenPeriodTextBox.Text = quest.Precondition.TakenPeriod.ToString();
        }       
        //! Настраивает форму на определенный тип квеста
        void fillTargetForm(int QuestType)
        {
            this.QuestType = QuestType;
            setDefaultTargetState();            
            dynamicCheckBox.Enabled = false;
            lTargetAttr1.Enabled = false;
            targetAttributeComboBox2.Enabled = false;
            lTargetAttr1.Text = "Атрибут2";
            ltargetResult.Text = "Результат";

            if (parent.questConst.isSimple(QuestType))
            {
                targetComboBox.Enabled = true;
                lNameObject.Enabled = true;
                lQuantity.Enabled = true;
                quantityUpDown.Enabled = true;

                targetComboBox.SelectedItem = null;
                targetComboBox.SelectedText = "";
                targetComboBox.Items.Clear();

                if (QuestType == 9)
                {
                    targetComboBox.Enabled = false;
                    quantityUpDown.Enabled = false;
                    resultextBox.Enabled = true;
                    ltargetResult.Text = "Время (сек):";
                }

                if ((QuestType == 0) || (QuestType == 16))
                {
                    lNameObject.Text = "Тип предмета:";
                    foreach (CItem description in parent.itemConst.getAllItems().Values)
                        targetComboBox.Items.Add(description.getDescription());
                    targetAttributeComboBox.Items.Clear();
                    targetAttributeComboBox.Items.Add("Обычный.");
                    targetAttributeComboBox.Items.Add("Квестовый.");
                    targetAttributeComboBox.Enabled = true;
                    labelTargetAttr.Enabled = true;
                    ltargetResult.Enabled = true;
                    dynamicCheckBox.Enabled = true;
                }

                else if (QuestType == 7)
                {
                    lNameObject.Text = "Тип предмета:";
                    foreach (CItem description in parent.itemConst.getAllItems().Values)
                        targetComboBox.Items.Add(description.getDescription());
                    targetAttributeComboBox.Items.Clear();
                    targetAttributeComboBox.Items.Add("Обычный.");
                    targetAttributeComboBox.Items.Add("Квестовый.");
                    targetAttributeComboBox.SelectedItem = "Квестовый.";
                    quantityUpDown.Value = 1;
                    quantityUpDown.Enabled = false;
                    bTargetAddDynamic.Enabled = false;
                    bTargetClearDynamic.Enabled = false;
                }
                else if (QuestType == 1)
                {
                    lNameObject.Text = "Имя NPC:";
                    foreach (CNPCDescription description in parent.npcConst.getAllNPCsDescription().Values)
                        targetComboBox.Items.Add(description.getName());
                    lQuantity.Enabled = false;
                    quantityUpDown.Enabled = false;
                }
                else if ((QuestType == 2) || (QuestType == 3))
                {
                    lNameObject.Text = "Тип моба:";
                    ltargetResult.Text = "Уровень моба";
                    ltargetResult.Enabled = true;
                    lTargetAttr1.Enabled = true;
                    //targetAttributeComboBox2.Enabled = true;
                    resultextBox.Enabled = true;

                    foreach (CMobDescription description in parent.mobConst.getAllDescriptions().Values)
                        targetComboBox.Items.Add(description.getName());
                    
                    lQuantity.Text = "Количество:";
                    labelTargetAttr.Text = "Зона:";
                    targetAttributeComboBox.Enabled = true;
                    targetAttributeComboBox.Items.Add("");
                    foreach (CZoneDescription description in parent.zoneConst.getAllZones().Values)
                        targetAttributeComboBox.Items.Add(description.getName());
                    targetAttributeComboBox.SelectedIndex = 0;
                    //targetAttributeComboBox2.SelectedIndex = 0;
                    dynamicCheckBox.Enabled = true;
                }
                else if ((QuestType == 4) || (QuestType == 8))
                {
                    lNameObject.Text = "Имя зоны:";
                    foreach (CZoneDescription description in parent.zoneConst.getAllZones().Values)
                        targetComboBox.Items.Add(description.getName());
                    lQuantity.Enabled = false;
                    quantityUpDown.Enabled = false;
                    dynamicCheckBox.Enabled = true;
                }
                else if (QuestType == 5)
                {
                    lNameObject.Text = "Деньги:";
                    targetComboBox.SelectedText = "";
                    targetComboBox.Enabled = false;
                    lQuantity.Enabled = true;
                    quantityUpDown.Enabled = true;
                    targetAttributeComboBox.Enabled = false;
                    labelTargetAttr.Enabled = false;
                }
                else if (QuestType == 6)
                {
                    lNameObject.Text = "Триггер:";
                    foreach (string item in parent.triggerConst.getTriggersDescription())
                        targetComboBox.Items.Add(item);
                    lQuantity.Enabled = false;
                    quantityUpDown.Enabled = false;
                    targetAttributeComboBox.Enabled = false;
                    labelTargetAttr.Enabled = false;
                    dynamicCheckBox.Enabled = true;
                }
                else if (QuestType == 201 ||
                    QuestType == 202 ||
                    QuestType == 203 ||
                    QuestType == 204 )
                {
                }
                else if (QuestType == 19 || QuestType == 20){
                    lNameObject.Text = "Тип предмета:";
                    foreach (CItem description in parent.itemConst.getAllItems().Values)
                        targetComboBox.Items.Add(description.getDescription());
                    quantityUpDown.Enabled = false;
                    lQuantity.Enabled = false;
                }
                else if (QuestType == 206)
                {
                    foreach (string descr in parent.gui.guiIDs.Keys)
                        targetComboBox.Items.Add(descr);
                    targetAttributeComboBox.Enabled = true;
                    targetAttributeComboBox.Items.Add("Показать.");
                    targetAttributeComboBox.Items.Add("Скрыть.");
                    targetAttributeComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                targetComboBox.Enabled = false;
                lNameObject.Enabled = false;
                lQuantity.Enabled = false;
                quantityUpDown.Enabled = false;
            }

            if (iState != ADD_NEW)
            {
                isClanCheckBox.Checked = quest.Target.IsClan;
                //IsGroupCheckBox.Checked = quest.Target.IsGroup;
            }

            if (iState == ADD_SUB || iState == EDIT_SUB)
            {
                isClanCheckBox.Enabled = false;
                takenPeriodTextBox.Enabled = false;
                lDaily.Enabled = false; 
                lH.Enabled = false;
                //IsGroupCheckBox.Enabled = false;
            }
        }
        //! Заполняет данные о целях квеста (о боги какой говнокод)
        void fillTarget()
        {
            if ((quest.Target.QuestType == 0) || (quest.Target.QuestType == 16) || (quest.Target.QuestType == 7))
            {
                targetComboBox.SelectedItem = parent.itemConst.getDescriptionOnID(quest.Target.ObjectType);
                quantityUpDown.Value = quest.Target.NumOfObjects;
                targetAttributeComboBox.SelectedIndex = quest.Target.ObjectAttr;
            }
            else if (quest.Target.QuestType == 1)
            {
                targetComboBox.SelectedItem = parent.npcConst.getDescriptionOnKey(quest.Target.ObjectName).getName();
            }
            else if ((quest.Target.QuestType == 2) || (quest.Target.QuestType == 3))
            {
                targetComboBox.SelectedItem = parent.mobConst.getDescriptionOnType(quest.Target.ObjectType).getName();
                if (!quest.Target.AreaName.Equals(""))
                    targetAttributeComboBox.SelectedItem = parent.zoneConst.getDescriptionOnKey(quest.Target.AreaName).getName();

                //if (quest.Target.ObjectAttr < 0)
                //    targetAttributeComboBox2.SelectedIndex = 0;
                //else
                //    targetAttributeComboBox2.SelectedIndex = parent.mobConst.getDescriptionOnType(quest.Target.ObjectType).getIndexOnLevel(quest.Target.ObjectAttr);
                resultextBox.Text = quest.Target.ObjectAttr.ToString();


                if (quest.Target.ObjectName.Contains(','))
                {
                    dynamicCheckBox.Checked = true;
                    resultextBox.Text = quest.Target.ObjectName;
                }
                else
                {
                    quantityUpDown.Value = quest.Target.NumOfObjects;
                }

            }
            else if ((quest.Target.QuestType == 4) || (quest.Target.QuestType == 8))
            {

                if (quest.Target.ObjectName.Contains(','))
                {
                    dynamicCheckBox.Checked = true;
                    resultextBox.Text = quest.Target.ObjectName;
                }
                else
                {
                    targetComboBox.SelectedItem = parent.zoneConst.getDescriptionOnKey(quest.Target.ObjectName).getName();
                }
            }
            else if (quest.Target.QuestType == 5)
            {
                quantityUpDown.Value = quest.Target.NumOfObjects;
            }
            else if (quest.Target.QuestType == 6)
            {

                if (quest.Target.ObjectName.Contains(','))
                {
                    dynamicCheckBox.Checked = true;
                    resultextBox.Text = quest.Target.ObjectName;
                }
                else
                {
                    targetComboBox.SelectedItem = parent.triggerConst.getDescriptionOnId(quest.Target.ObjectType);
                }
            }
            else if (quest.Target.QuestType == 9)
            {
                resultextBox.Text = resultextBox.Text.Replace('.', ',');
                resultextBox.Text = quest.Target.Time.ToString();
            }
            else if (quest.Target.QuestType == 19 || quest.Target.QuestType == 20)
            {
                //System.Console.WriteLine("getQuest: type" + quest.Target.QuestType.ToString());
                targetComboBox.SelectedItem = parent.itemConst.getDescriptionOnID(quest.Target.ObjectType);
            }
            else if (quest.Target.QuestType == 206)
            {
                targetComboBox.SelectedItem = parent.gui.getDescriptionOnID(quest.Target.ObjectType);
                targetAttributeComboBox.SelectedIndex = quest.Target.ObjectAttr;
            }
        }   

        void fillQuestRulesForm()
        {
            foreach (string space in parent.spacesConst.getSpacesDescription())
                instanceComboBox.Items.Add(space);
        }
        //! Заполняет раздел Правила на форме - уровень игрока, сценарий... (???)
        void fillQuestRules()
        {
            foreach (int item in quest.QuestRules.Scenarios)
            {
                if (scenariosTextBox.Text == "")
                    scenariosTextBox.Text += item.ToString();
                else
                    scenariosTextBox.Text += ("," + item.ToString());
            }            
            checkQuestRulesIndicates();
        }
        //! Создает индикацию зеленым светом в графе Награда, если выдаются предметы, эффекты, или статус
        public void checkRewardIndicates()
        {
            if (editQuestReward.TypeOfItems.Any())
                bRewardItem.Image = Properties.Resources.but_indicate;
            else
                bRewardItem.Image = null;
            //if (editQuestReward.Fractions.Any())
            //if (editQuestReward.Reputation.Any())
            if (editQuestReward.ReputationNotEmpty())
                bRewardReputation.Image = Properties.Resources.but_indicate;
            else
                bRewardReputation.Image = null;
            if (editQuestReward.Effects.Any())
                bRewardEffects.Image = Properties.Resources.but_indicate;
            else
                bRewardEffects.Image = null;
        }
        //! Создает индиикацию кнопки Предметы в графе Правила квеста
        public void checkQuestRulesIndicates()
        {
            if (editQuestRules.TypeOfItems.Any())
                bItemQuestRules.Image = Properties.Resources.but_indicate;
        }
        //! Заполняет данные о награде - опыт, деньги, карму
        void fillReward()
        {
            checkRewardIndicates();
            if (quest.Reward.Difficulty != 0)
                difficultyComboBox.SelectedItem = quest.Reward.Difficulty.ToString();
            else
                difficultyComboBox.SelectedItem = "1";

            if (quest.Reward.Experience.Count == 3)
            {
                tExperience.Text = quest.Reward.Experience[0].ToString();
                tSurvival.Text = quest.Reward.Experience[1].ToString();
                tSupport.Text = quest.Reward.Experience[2].ToString();
            }
            creditsTextBox.Text = quest.Reward.Credits.ToString();
            textBoxKarmaPK.Text = quest.Reward.KarmaPK.ToString();
        }
        //! Собирает данные с формы, и возвращает экземпляр CQuest с этими данными
        public CQuest getQuest()
        {
            //System.Console.WriteLine("EditQuestForm::getQuest");
            CQuestAdditional additional = new CQuestAdditional();
            CQuestInformation information = new CQuestInformation();
            CQuestPrecondition precondition = new CQuestPrecondition();
            CQuestReward reward = new CQuestReward();
            CQuestRules rules = new CQuestRules();
            CQuestTarget target = new CQuestTarget();
            CQuestPenalty penalty = new CQuestPenalty();

            information.Description = descriptionTextBox.Text;
            information.Title = titleTextBox.Text;
            additional.Holder = parent.GetCurrentNPC();
            information.onWin = onWonTextBox.Text;
            information.onFailed = onFailedTextBox.Text;

            target.QuestType = parent.questConst.getQuestTypeOnDescription(eventComboBox.SelectedItem.ToString());
            if (loseRButton.Checked)
                target.onFin = 0;
            else
                target.onFin = 1;

            if ((target.QuestType == 0) || (target.QuestType == 16) || (target.QuestType == 7))
            {
                target.ObjectType = parent.itemConst.getIDOnDescription(targetComboBox.SelectedItem.ToString());
                target.NumOfObjects = int.Parse(quantityUpDown.Value.ToString());
                target.ObjectAttr = targetAttributeComboBox.SelectedIndex;
            }
            else if (target.QuestType == 1)
            {
                string key = parent.npcConst.getKeyOnDescription(targetComboBox.SelectedItem.ToString());
                if (key.Equals(""))
                    target.ObjectName = targetComboBox.SelectedItem.ToString();
                else
                    target.ObjectName = key;
            }
            else if ((target.QuestType == 2) || (target.QuestType == 3))
            {
                target.ObjectType = parent.mobConst.getTypeOnDescription(targetComboBox.SelectedItem.ToString());
                //if (targetAttributeComboBox2.SelectedIndex < 0)
                //    target.ObjectAttr = 0;
                //else
                //{
                //    target.ObjectAttr = parent.mobConst.getDescriptionOnType(target.ObjectType).getLevelOnIndex(targetAttributeComboBox2.SelectedIndex);
                //}

                int level = 0 ;
                try
                {
                    level = int.Parse(resultextBox.Text);
                }
                catch
                {

                }

                target.ObjectAttr = level;

                if (dynamicCheckBox.Checked)
                    target.ObjectName = resultextBox.Text;
                else
                    target.NumOfObjects = int.Parse(quantityUpDown.Value.ToString());

                if (targetAttributeComboBox.SelectedItem.ToString().Equals(""))
                    target.AreaName = "";
                else
                    target.AreaName = parent.zoneConst.getKeyOnDescription(targetAttributeComboBox.SelectedItem.ToString());

            }
            else if ((target.QuestType == 4) || (target.QuestType == 8))
            {

                if (dynamicCheckBox.Checked)
                    target.ObjectName = resultextBox.Text;
                else
                {
                    string zone = parent.zoneConst.getKeyOnDescription(targetComboBox.SelectedItem.ToString());
                    if (zone.Equals(""))
                        target.ObjectName = targetComboBox.SelectedItem.ToString();
                    else
                        target.ObjectName = zone;
                }
            }
            else if (target.QuestType == 5)
            {
                target.NumOfObjects = (int)quantityUpDown.Value;
            }
            else if (target.QuestType == 6)
            {
                if (dynamicCheckBox.Checked)
                    target.ObjectName = resultextBox.Text;
                else
                    target.ObjectType = parent.triggerConst.getIdOnKey(targetComboBox.SelectedItem.ToString());
            }

            else if (target.QuestType == 9)
            {
                try
                {
                    target.Time = float.Parse(resultextBox.Text.ToString());
                }
                catch
                {
                    return null;
                }
            }
            else if ((target.QuestType == 19) || (target.QuestType == 20))
            {
                System.Console.WriteLine("QuestType == 19 || 20");
                target.ObjectType = parent.itemConst.getIDOnDescription(targetComboBox.SelectedItem.ToString());
            }
            else if (target.QuestType == 201 ||
                     target.QuestType == 202 ||
                    target.QuestType == 203 ||
                    target.QuestType == 204)
            {
            }
            else if (target.QuestType == 206)
            {
                target.ObjectType = parent.gui.getIDOnDescription(targetComboBox.SelectedItem.ToString());
                target.ObjectAttr = targetAttributeComboBox.SelectedIndex;
            }


            if (iState == EDIT_SUB || iState == EDIT)
                additional = quest.Additional;

            //target.IsGroup = IsGroupCheckBox.Checked;
            target.IsClan = isClanCheckBox.Checked;

            if (iState == EDIT)
            {
                parent.setQuestISClan(quest.QuestID, target.IsClan);
                //parent.setQuestISGroup(quest.QuestID, target.IsGroup);
            }

            if (repeatComboBox.SelectedItem.ToString().Equals("Повторное взятие квеста после его закрытия."))
                precondition.Repeat = 1;

            if (takenPeriodTextBox.Enabled && !takenPeriodTextBox.Text.Equals(""))
                precondition.TakenPeriod =  double.Parse(takenPeriodTextBox.Text);

            foreach (string item in scenariosTextBox.Text.Split(','))
                if (!item.Equals(""))
                    rules.Scenarios.Add(int.Parse(item));
            
            if (tExperience.Text.Equals(""))
                reward.Experience.Add(0);
            else
                reward.Experience.Add(int.Parse(tExperience.Text));
            
            if (tSurvival.Text.Equals(""))
                reward.Experience.Add(0);
            else 
                reward.Experience.Add(int.Parse(tSurvival.Text));

            if (tSupport.Text.Equals(""))
                reward.Experience.Add(0);
            else 
                reward.Experience.Add(int.Parse(tSupport.Text));

            if (!creditsTextBox.Text.Equals(""))
                reward.Credits = int.Parse(creditsTextBox.Text);

            if (difficultyComboBox.SelectedItem.ToString() != "")
                reward.Difficulty = int.Parse(difficultyComboBox.SelectedItem.ToString());

            if (!textBoxKarmaPK.Text.Equals(""))
                reward.KarmaPK = int.Parse(textBoxKarmaPK.Text);

            int iProgressResult = 0;
            if (showCloseCheckBox.Checked)
                iProgressResult |= this.SHOW_MESSAGE_CLOSE;
            if (showTakeCheckBox.Checked)
                iProgressResult |= this.SHOW_MESSAGE_TAKE;
            if (showJournalCheckBox.Checked)
                iProgressResult |= this.SHOW_JOURNAL;
            if (showProgressCheckBox.Checked)
                iProgressResult |= this.SHOW_MESSAGE_PROGRESS;
            if (showWinCheckBox.Checked)
                iProgressResult |= this.SHOW_ONWIN;
            if (showFailedCheckBox.Checked)
                iProgressResult |= this.SHOW_ONFAILED;
            if (tutorialCheckBox.Checked)
            {
                System.Console.WriteLine("tutorialCheckBox.Checked != SHOW_TUTORIAL");
                iProgressResult |= this.SHOW_TUTORIAL;
            }
            additional.ShowProgress = iProgressResult;
            additional.CantCancel = cantCancelCheckBox.Checked;
            CQuest retQuest;

            reward.TypeOfItems = editQuestReward.TypeOfItems;
            reward.NumOfItems = editQuestReward.NumOfItems;
            reward.AttrOfItems = editQuestReward.AttrOfItems;
            reward.Probability = editQuestReward.Probability;
            reward.Fractions = editQuestReward.Fractions;
            reward.Reputation = editQuestReward.Reputation;
            reward.Effects = editQuestReward.Effects;

            rules.TypeOfItems = editQuestRules.TypeOfItems;
            rules.NumOfItems = editQuestRules.NumOfItems;
            rules.AttrOfItems = editQuestRules.AttrOfItems;            
            information.Items = editInformation.Items;

            target.AObjectAttrs = editTarget.AObjectAttrs;

            if (iState == ADD_NEW || iState == ADD_SUB)
            {
                if (iState == ADD_SUB)
                    additional.IsSubQuest = quest.QuestID;
                retQuest = new CQuest(parent.getQuestNewID(), 1 , information, precondition, rules, reward, additional, target, penalty);
            }
            else
            {
                int version = quest.Version;
                if (quest.QuestInformation.Title != information.Title || quest.QuestInformation.Description != information.Description 
                        || quest.QuestInformation.onWin != information.onWin || quest.QuestInformation.onFailed != information.onFailed)
                    version++;
                 
                retQuest = new CQuest(quest.QuestID, version, information, precondition, rules, reward, additional, target, penalty);
            }
            return retQuest;
        }
        //! Действия при смене типа кевеста
        private void eventComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedQuestType = parent.questConst.getQuestTypeOnDescription(eventComboBox.SelectedItem.ToString());
            //if (selectedQuestType != 0)
            //{
                clearTargetContent();
                fillTargetForm(selectedQuestType);
            //}
        }
        //! Закрытие окна без сохранения
        private void cancelButton_Click(object sender, EventArgs e)
        {
            parent.Enabled = true;
            this.Close();
        }
        //! Нажатие OK - сохранение квеста или создание нового
        private void okButton_Click(object sender, EventArgs e)
        {
            CQuest result = getQuest();
            if (result != null)
            {
                if (iState == ADD_NEW)
                    parent.createNewQuest(result);
                else if (iState == ADD_SUB)
                    parent.addQuest(result, quest.QuestID);
                else
                    parent.replaceQuest(result);

                if (result.Additional.IsSubQuest == 0)
                    parent.setTutorial(result.QuestID, (((result.Additional.ShowProgress & this.SHOW_TUTORIAL) > 0)));

                parent.Enabled = true;
                this.Close();
            }
        }

        private void isClanCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dynamicCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (dynamicCheckBox.Checked)
            {
                bTargetAddDynamic.Enabled = true;
                bTargetClearDynamic.Enabled = true;
                resultextBox.Enabled = true;

                if (this.QuestType == 2 || this.QuestType == 3)
                    ltargetResult.Text = "Мин,Макс:";
                else if (this.QuestType == 4)
                    ltargetResult.Text = "Зоны:";
                else if (this.QuestType == 6)
                    ltargetResult.Text = "Триггеры:";
            }
            else
            {
                ltargetResult.Text = "Результат:";
                bTargetAddDynamic.Enabled = false;
                bTargetClearDynamic.Enabled = false;
                resultextBox.Enabled = false;
            }
        }

        private void bTargetClearDynamic_Click(object sender, EventArgs e)
        {
            resultextBox.Text = "";
        }

        private void bTargetAddDynamic_Click(object sender, EventArgs e)
        {
            string str = "";

            if (this.QuestType == 2 || this.QuestType == 3)
            {
                str += quantityUpDown.Value.ToString();
            }
            else if (this.QuestType == 4)
            {
                str += parent.zoneConst.getKeyOnDescription(targetComboBox.SelectedItem.ToString());
            }
            else if (this.QuestType == 6)
            {
                str += parent.triggerConst.getIdOnKey(targetComboBox.SelectedItem.ToString());
            }

            if (resultextBox.Text.Equals(""))
                resultextBox.Text += str;
            else
                resultextBox.Text += ("," + str);
        }

        private void targetAttributeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (targetAttributeComboBox.SelectedItem.ToString().Equals("Квестовый."))
                bItemQID.Enabled = true;
            else
                bItemQID.Enabled = false;
        }

        private void targetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //! @todo впилить сюда что и было - выбор в комбобоксе уровня мобов
            if (this.QuestType == 2 || this.QuestType == 3)
            {
                //targetAttributeComboBox2.Items.Clear();
                //List<int> lLevels = parent.mobConst.getLevelsOnDescription(targetComboBox.SelectedItem.ToString());
                //if (lLevels != null)
                //{
                //    targetAttributeComboBox2.Items.Add("");
                //    foreach (int iLevel in lLevels)
                //        targetAttributeComboBox2.Items.Add(iLevel.ToString());
                //    targetAttributeComboBox2.SelectedIndex = 0;
                //}
            }
        }
        //! Кнопка Скрыть информацию о квесте
        private void bHideInformation_Click(object sender, EventArgs e)
        {
            lTitle.Visible = !lTitle.Visible;
            titleTextBox.Visible = !titleTextBox.Visible;
            lDescription.Visible = !lDescription.Visible;
            descriptionTextBox.Visible = !descriptionTextBox.Visible;
            showCloseCheckBox.Visible = !showCloseCheckBox.Visible;
            showJournalCheckBox.Visible = !showJournalCheckBox.Visible;
            showProgressCheckBox.Visible = !showProgressCheckBox.Visible;
            showTakeCheckBox.Visible = !showTakeCheckBox.Visible;

            if (lTitle.Visible)
            {
                bHideInformation.Text = "Показать";
                iInformationHeight = questInformationBox.Height;
                questInformationBox.Height = 35;
            }
            else
            {
                bHideInformation.Text = "Скрыть";
                questInformationBox.Height = iInformationHeight;
            }
        }
        //! Кнопка Скрыть цели квеста
        private void bHideTarget_Click(object sender, EventArgs e)
        {
            ltargetResult.Visible = !ltargetResult.Visible;
            resultextBox.Visible = !resultextBox.Visible;
            dynamicCheckBox.Visible = !dynamicCheckBox.Visible;
            lQuantity.Visible = !lQuantity.Visible;
            quantityUpDown.Visible = !quantityUpDown.Visible;
            lNameObject.Visible = !lNameObject.Visible;
            targetComboBox.Visible = !targetComboBox.Visible;
            bTargetAddDynamic.Visible = !bTargetAddDynamic.Visible;
            labelTargetAttr.Visible = !labelTargetAttr.Visible;
            targetAttributeComboBox.Visible = !targetAttributeComboBox.Visible;
            bTargetClearDynamic.Visible = !bTargetClearDynamic.Visible;
            lTargetAttr1.Visible = !lTargetAttr1.Visible;
            targetAttributeComboBox2.Visible = !targetAttributeComboBox2.Visible;
            IsGroupCheckBox.Visible = !IsGroupCheckBox.Visible;
            isClanCheckBox.Visible = !isClanCheckBox.Visible;

            if (bHideTarget.Text == "Скрыть")
            {
                bHideTarget.Text = "Показать";
                iTargetHeight = targetBox.Height;
                targetBox.Height = 35;
            }
            else
            {
                bHideTarget.Text = "Скрыть";
                targetBox.Height = iTargetHeight;
            }
        }
        //! Кнопка Скрыть условия квеста
        private void bHideRules_Click(object sender, EventArgs e)
        {
            bItemQuestRules.Visible = !bItemQuestRules.Visible;
            labelScenarios.Visible = !labelScenarios.Visible;
            scenariosTextBox.Visible = !scenariosTextBox.Visible;
            groupQuestRulesBox.Visible = !groupQuestRulesBox.Visible;

            if (bHideRules.Text == "Скрыть")
            {
                bHideRules.Text = "Показать";
                iRulesHeight = lQuestRules.Height;
                lQuestRules.Height = 35;
            }
            else
            {
                lQuestRules.Height = iRulesHeight;
                bHideRules.Text = "Скрыть";
            }
        }
        //! Кнопка Скрыть Награду квеста
        private void bHideReward_Click(object sender, EventArgs e)
        {
            difficultyComboBox.Visible = !difficultyComboBox.Visible;
            lDifficulty.Visible = !lDifficulty.Visible;
            lCredits.Visible = !lCredits.Visible;
            creditsTextBox.Visible = !creditsTextBox.Visible;
            lCombatSkills.Visible = !lCombatSkills.Visible;
            tExperience.Visible = !tExperience.Visible;
            lSurvivalSkills.Visible = !lSurvivalSkills.Visible;
            tSurvival.Visible = !tSurvival.Visible;
            lSupportSkills.Visible = !lSupportSkills.Visible;
            tSupport.Visible = !tSupport.Visible;
            lKarmaPK.Visible = !lKarmaPK.Visible;
            textBoxKarmaPK.Visible = !textBoxKarmaPK.Visible;

            if (lCredits.Visible)
            {
                bHideReward.Text = "Показать";
                iRewardHeight = rewardGroupBox.Height;
                rewardGroupBox.Height = 35;
            }
            else
            {
                bHideReward.Text = "Скрыть";
                rewardGroupBox.Height = iRewardHeight;
            }
        }
            
        private void QuestEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.parent.Enabled = true;
        }

        private void bItemQID_Click(object sender, EventArgs e)
        {
            SelectQuestItem selectQItem = new SelectQuestItem(this, this.QuestID);
            this.Enabled = false;
        }
        //! Нажатие Репутация в наградах квеста - открывает окно редактирования репутаций
        private void bRewardReputation_Click(object sender, EventArgs e)
        {
            RewardFractions formFractions = new RewardFractions(this);
            formFractions.Visible = true;
            this.Enabled = false;
        }
        //! Изменение состояния квеста - Туториал или нет
        private void tutorialCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            showJournalCheckBox.Checked = !tutorialCheckBox.Checked;
            showJournalCheckBox.Enabled = !tutorialCheckBox.Checked;
        }
        //! Открывает окно редактирования эффектов (бафов)
        private void bRewardEffects_Click(object sender, EventArgs e)
        {
            EditDialogEffect edit_effect = new EditDialogEffect(this.parent, this, this.QuestID);
            edit_effect.Visible = true;
            this.Enabled = false;
        }

        private void descriptionTextBox_DoubleClick(object sender, EventArgs e)
        {

        }

        //! Нажатие Предметы в правилах квеста - открывает форму с редактором предметов
        private void bItemQuestRules_Click(object sender, EventArgs e)
        {
            ItemDialog itemDialog = new ItemDialog(this.parent, this, null, quest.QuestID, this.ITEM_QUESTRULES);
            itemDialog.Enabled = true;
            itemDialog.Visible = true;
            this.Enabled = false;
        }
        //! Нажатие Предметы в наградах квеста - открывает форму с редактором предметов
        private void bRewardItem_Click(object sender, EventArgs e)
        {
            ItemDialog itemDialog = new ItemDialog(this.parent, this, null, quest.QuestID, this.ITEM_REWARD);
            itemDialog.Enabled = true;
            itemDialog.Visible = true;
            this.Enabled = false;
        }
        
    }
}
