﻿#define OLDVERSION
//#define OPERATOR2
//#define OPERATOR3
//#define OPERATOR4


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Event;
using System.Collections;
using System.Xml.Linq;
using System.Threading;

namespace StalkerOnlineQuesterEditor
{
    using NPCQuestDict = Dictionary<int, CDialog>;
    using StalkerOnlineQuesterEditor.Forms;

    //! Главная форма программы, туча строк кода
    public partial class MainForm : Form
    {
        //! Текущий выбранный NPC (в комбобоксе вверху)
        public string currentNPC = "";
        private int selectedDialogID = 0;
        //! Ссылка на экземпляр класса CDialogs, хранит все данные и функции по работе с диалогами
        CDialogs dialogs;
        //! Ссылка на экземпляр класса CQuests
        CQuests quests;
        NodeDragHandler Listener;
        PLayer edgeLayer;
        PNodeList nodeLayer;

        Dictionary<PNode, GraphProperties> graphs = new Dictionary<PNode, GraphProperties>();
        Dictionary<Panel, int> panels = new Dictionary<Panel, int>();

        List<int> rootElements = new List<int>();
        public TreeView tree;
        List<PNode> subNodes = new List<PNode>();
        Dictionary<LinkLabel,int> titles;
        List<NPCNameDataSourceObject> npcNames = new List<NPCNameDataSourceObject>();

        public СQuestConstants questConst;
        public CItemConstants itemConst;
        public CNPCConstants npcConst;
        public CMobConstants mobConst;
        public CZoneConstants zoneConst;
        public CSpacesConstants spacesConst;
        public CTriggerConstants triggerConst;
        public CTPConstants tpConst;
        public CSettings settings;
        public COperNotes manageNotes;
        public CFracConstants fractions;
        public CGUIConst gui;
        public CEffectConstants effects;
        public int currentQuest;

        public MainForm()
        {
            InitializeComponent();
            Listener = new NodeDragHandler(this);
            settings = new CSettings(this);
            dialogs = new CDialogs(this);
            quests = new CQuests(this);
            tpConst = new CTPConstants();
            settings.checkMode();

            tree = treeDialogs;
            questConst = new СQuestConstants();
            itemConst = new CItemConstants();
            npcConst = new CNPCConstants();
            spacesConst = new CSpacesConstants();
            triggerConst = new CTriggerConstants();
            manageNotes = new COperNotes("ManNotes.xml");
            fractions = new CFracConstants();
            gui = new CGUIConst();
            effects = new CEffectConstants();

            treeQuest.AfterSelect += new TreeViewEventHandler(this.treeQuestSelected);
            //fillNPCBox();
            fillLocationsBox();
            DialogShower.AddInputEventListener(Listener);

            foreach (string name in dialogs.getListOfNPC())
                if (!npcConst.NPCs.Keys.Contains(name))
                 npcConst.NPCs.Add(name, new CNPCDescription(name));

            this.mobConst = new CMobConstants();
            this.zoneConst = new CZoneConstants();
        }

        //! Очищает данные о квестах - дерево квестов, комбобокс, подквесты
        void clearQuestTab()
        {
            treeQuest.Nodes.Clear();
            splitQuestsContainer.Panel2.Controls.Clear();
            QuestBox.Items.Clear();
        }

        //! Сменили NPC в комбобоксе выбора персонажа
        private void NPCBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearQuestTab();
            try
            {
                currentNPC = NPCBox.SelectedValue.ToString();
            }
            catch {
                return;
            }
            if (CentralDock.SelectedIndex == 0)
            {
                fillQuestChangeBox(true);
                QuestBox.Enabled = false;
                bAddQuest.Enabled = false;
                bRemoveQuest.Enabled = false;
                bAddDialog.Enabled = false;
                bEditDialog.Enabled = false;
                bRemoveDialog.Enabled = false;
                EmulatorsplitContainer.Panel2.Controls.Clear();
                DialogSelected(true);
            }
            else if (CentralDock.SelectedIndex == 1)
            {
                fillQuestChangeBox(false);
                QuestBox.Text = "Пожалуйста, выберите квест";
            }
            tabQuests.Text = "Квесты (" + quests.getCountOfQuests(currentNPC) + ")";
        }

        //! Поиск в комбобоксe NPC по имени общему, или локализованному русскому, английскому
        private void NPCBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string text = NPCBox.Text;
                if (dialogs.dialogs.Keys.Contains(text) )
                    return;
                string name = "";
                if (settings.getMode() == settings.MODE_EDITOR)
                {
                    if (dialogs.rusNamesToNPC.ContainsKey(text))
                    {
                        name = dialogs.rusNamesToNPC[text];
                        NPCBox.SelectedValue = name;
                    }
                }
                else if (settings.getMode() == settings.MODE_LOCALIZATION)
                {
                    if (dialogs.engNamesToNPC.ContainsKey(text))
                    {
                        name = dialogs.engNamesToNPC[text];
                        NPCBox.SelectedValue = name;
                    }
                }
            }
        }

        //! Заполняет комбобокс со списком квестов у данного персонажа
        void fillQuestChangeBox(bool onlyDialogs)
        {
            QuestBox.SelectedItem = null;
            QuestBox.Items.Clear();
            if (!onlyDialogs)
            {
                if (settings.getMode() == settings.MODE_EDITOR)
                {
                    foreach (CQuest quest in quests.getQuestAndTitleOnNPCName(currentNPC))
                        QuestBox.Items.Add(quest.QuestID + ": " + quest.QuestInformation.Title);
                }
                else if (settings.getMode() == settings.MODE_LOCALIZATION)
                {
                    string locale = settings.getCurrentLocale();
                    foreach (CQuest quest in quests.getQuestAndTitleOnNPCName(currentNPC))
                    {
                        int id = quest.QuestID;
                        QuestBox.Items.Add(quest.QuestID + ": " + quests.locales[locale][id].QuestInformation.Title);
                    }
                }
            }
        }

        //! Сменили квест в комбобоксе, выводим дерево всех подквестов
        private void QuestBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            splitQuestsContainer.Panel2.Controls.Clear();
            treeQuest.Nodes.Clear();

            if (CentralDock.SelectedIndex == 1)
            {
                int index = QuestBox.SelectedItem.ToString().IndexOf(':');
                string qID = QuestBox.SelectedItem.ToString().Substring(0, index);
                int questID = int.Parse(qID);
                addNodeOnTreeQuest(questID);
                bRemoveQuest.Enabled = true;
            }
            treeQuest.ExpandAll();
        } 

        //! Заполнение итемов в комбобоксе NPC
        void fillNPCBox()
        {
            npcNames.Clear();
            NPCBox.AutoCompleteCustomSource.Clear();
            foreach (string holder in this.dialogs.dialogs.Keys)
            {
                string npcName = holder;
                string localName = "";
                if (dialogs.NpcData.ContainsKey(holder))
                {
                    if (settings.getMode() == settings.MODE_EDITOR)
                        localName = dialogs.NpcData[holder].rusName;
                    else if (settings.getMode() == settings.MODE_LOCALIZATION)
                        localName = dialogs.NpcData[holder].engName;

                    NPCBox.AutoCompleteCustomSource.Add(localName);
                    npcName += " (" + localName + ")";
                }
                npcNames.Add ( new NPCNameDataSourceObject(holder, npcName));
                NPCBox.AutoCompleteCustomSource.Add(npcName);
            }
            npcNames.Sort();
            NPCBox.DataSource = null;       // костыль для обновления данных в кмобобоксе NPC при добавлении/удалении
            NPCBox.DisplayMember = "DisplayString";
            NPCBox.ValueMember = "Value";
            NPCBox.DataSource = npcNames;
            NPCBox.SelectedIndex = settings.getLastNpcIndex();

            NPCBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            NPCBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                       
            QuestBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            QuestBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            QuestBox.AutoCompleteCustomSource.AddRange(quests.getQuestsIDasString());
        }
      
// ************************ WORK WITH DIALOGS****************************************************

        void fillNPCBoxSubquests(CDialog sub)
        {
            foreach (int subdialog in sub.Nodes)
            {
                foreach (TreeNode treeNode in this.treeDialogs.Nodes.Find("Active", true))
                {
                    //System.Console.WriteLine("Write dialog:" + subdialog.ToString());
                    if (!treeNode.Nodes.ContainsKey(subdialog.ToString()))
                    {
                        treeNode.Nodes.Add(subdialog.ToString(), subdialog.ToString());
                        dialogs.dialogs[currentNPC][subdialog].coordinates.Active = true;
                    }
                }
                this.fillNPCBoxSubquests(this.dialogs.dialogs[currentNPC][subdialog]);
            }
        }

        internal void onSelectNode(int dialogID)
        {
            bAddDialog.Enabled = true;
            bEditDialog.Enabled = true;
            if (!isRoot(dialogID))
                bRemoveDialog.Enabled = true;
        }

        public void selectNodeOnDialogTree(int dialogID)
        {
            //treeDialogs.Focus();
            foreach (TreeNode node in treeDialogs.Nodes.Find(dialogID.ToString(), true))
                treeDialogs.SelectedNode = node;
        }

        internal void onDeselectNode()
        {
            bRemoveDialog.Enabled = false;
            bEditDialog.Enabled = false;
        }
        //! Удаление диалога в зависимости от статуса - в корзину или навсегда
        private void bRemoveDialog_Click(object sender, EventArgs e)
        {
            if (settings.getMode() == settings.MODE_EDITOR)
            {
                if (Listener.curNode != null)
                    removeNodeFromDialogGraphView(getDialogIDOnNode(Listener.curNode));
                else
                    removeDialog(int.Parse(treeDialogs.SelectedNode.Text));

                bRemoveDialog.Enabled = false;
                bEditDialog.Enabled = false;
            }
        }

        public bool isRoot(int dialogID)
        {
            return rootElements.Contains(dialogID);
        }

        public bool isRoot(PNode node)
        {
            return isRoot(getDialogIDOnNode(node));
        }

        private void treeDialogs_GotFocus(object sender, TreeViewEventArgs e)
        {
            if ((treeDialogs.SelectedNode.Text != "Active") && (treeDialogs.SelectedNode.Text != "Recycle"))
            {
                int treeID = int.Parse(treeDialogs.SelectedNode.Text);

                if ((treeID != 0) && (!Listener.getCurDialogID().Equals(treeID)))
                {
                    deselectSubNodesDialogGraphView();
                    if (treeDialogs.SelectedNode.Parent.Text == "Active")
                    {
                        foreach (PNode node in DialogShower.Layer.AllNodes)
                            if (getDialogIDOnNode(node).Equals(treeID))
                                Listener.setCurrentNode(treeID);
                    }
                    else if (treeDialogs.SelectedNode.Parent.Text == "Recycle")
                    {
                        onSelectNode(treeID);
                        Listener.setCurrentNode(0);
                    }
                }
            }
        }
        //! Добавляет поддиалог к текущему диалогу
        private void bAddDialog_Click(object sender, EventArgs e)
        {
            if (settings.getMode() == settings.MODE_EDITOR)
            {
                if (!treeDialogs.SelectedNode.ToString().Equals("Recycle") && !treeDialogs.SelectedNode.ToString().Equals("Active"))
                {
                    if (!(Listener.getCurDialogID() == 0))
                    {
                        EditDialogForm editDialogForm = new EditDialogForm(true, this, int.Parse(treeDialogs.SelectedNode.Text));
                        editDialogForm.Visible = true;
                    }
                    else
                    {
                        AddPassiveDialogForm AddPassiveDialog = new AddPassiveDialogForm(this, int.Parse(treeDialogs.SelectedNode.Text));
                        AddPassiveDialog.Visible = true;
                    }
                }
            }
        }
        //! Нажатие на кнопку "Редактирование диалогов" - открывает редактор или переводчик диалогов
        public void bEditDialog_Click(object sender, EventArgs e)
        {
            if (settings.getMode() == settings.MODE_EDITOR)
            {
                EditDialogForm editDialogForm = new EditDialogForm(false, this, int.Parse(treeDialogs.SelectedNode.Text));
                editDialogForm.Visible = true;
            }
            else
            {
                LocaleDialogForm editLocaleDialogForm = new LocaleDialogForm(this, int.Parse(treeDialogs.SelectedNode.Text));
                editLocaleDialogForm.Visible = true;
            }
        }
        //! Полностью удаляет диалог из эдитора (выбор в Recycled -> удаление)
        void removeDialog(int dialogID)
        {
            dialogs.dialogs[currentNPC].Remove(dialogID);
            foreach (CDialog dialog in dialogs.dialogs[currentNPC].Values)
                if (dialog.Actions.ToDialog == dialogID)
                    dialog.Actions.ToDialog = 0;
            // удаляем диалог из переводов
            dialogs.locales[settings.getListLocales()[0]][currentNPC].Remove(dialogID);
            foreach (CDialog dialog in dialogs.locales[settings.getListLocales()[0]][currentNPC].Values)
                if (dialog.Actions.ToDialog == dialogID)
                    dialog.Actions.ToDialog = 0;

            CDialog rootDialog = getRootDialog();
            if (rootDialog!= null)
                fillDialogTree(rootDialog, this.dialogs.dialogs[currentNPC]);
        }

        //! Старует эмулятор диалога (язык диалога зависит от режима, непереведенные фрагменты помечаются красным)
        public void startEmulator(int dialogID)//, bool isHandle)
        {
            // получаем фразу NPC
            CDialog rootDialog = getDialogOnIDConditional(dialogID);
            selectedDialogID = dialogID;
            EmulatorsplitContainer.Panel2.Controls.Clear();
            titles = new Dictionary<LinkLabel,int>();
            Label NPCText = new Label();
            NPCText.Text = rootDialog.Text;
            NPCText.ForeColor = (rootDialog.version != 0) ? (Color.Black) : (Color.Red);
            NPCText.AutoSize = false;
            NPCText.AutoEllipsis = true;
            
            //NPCText.Size = new Size(30, 30);
            NPCText.Dock = DockStyle.Top;
            
            foreach (int dialog in rootDialog.Nodes)
            {
                LinkLabel dialogLink = new LinkLabel();
                dialogLink.LinkClicked += new LinkLabelLinkClickedEventHandler(dialogLink_LinkClicked);
                string openedQuests = "";
                string closedQuests = "";

                foreach (int quid in dialogs.dialogs[currentNPC][dialog].Actions.CompleteQuests)
                    if (closedQuests == "")
                        closedQuests += quid.ToString();
                    else
                        closedQuests += "," + quid.ToString();

                foreach (int quid in dialogs.dialogs[currentNPC][dialog].Actions.GetQuests)
                    if (openedQuests == "")
                        openedQuests += quid.ToString();
                    else
                        openedQuests += "," + quid.ToString();

                string actionResult = "";
                if (openedQuests != "")
                    actionResult += " Взять:" + openedQuests;
                if (closedQuests != "")
                    actionResult += " Закрыть:" + closedQuests;
                if (actionResult != "")
                    actionResult = "(" + actionResult + ")";


                //dialogLink.Text = dialog + ". " + dialogs.dialogs[currentNPC][dialog].Title + actionResult;
                CDialog answer = getDialogOnIDConditional(dialog);
                dialogLink.Text = dialog + ". " + answer.Title + actionResult;
                dialogLink.BackColor = (answer.version != 0) ? (Color.FromKnownColor(KnownColor.Transparent)) : (Color.FromArgb(0x7FAA45E0));             
                dialogLink.AutoSize = true;
                dialogLink.Dock = DockStyle.Top;
                dialogLink.Links.Add(0, 0, dialog);
                titles.Add(dialogLink,dialog);
                EmulatorsplitContainer.Panel2.Controls.Add(dialogLink);
            }
            EmulatorsplitContainer.Panel2.Controls.Add(NPCText);
        }

        public bool isDialogActive(int dialogID)
        {
            foreach (TreeNode node in treeDialogs.Nodes.Find("Active",true)[0].Nodes)
                if (int.Parse(node.Name).Equals(dialogID))
                    return true;
            return false;
        }

        //! Отработка клика по ответу в эмуляторе диалогов, вывод результатов в статусбар и переход к след диалогу
        private void dialogLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            int choosenDialogID = (int)e.Link.LinkData;
            CDialog choosenDialog = dialogs.dialogs[currentNPC][choosenDialogID];
            toolStripStatusLabel.Text = "";
            if (choosenDialog.Actions.CompleteQuests.Any())
                toolStripStatusLabel.Text += "Завершенные: " + getListAsString(choosenDialog.Actions.CompleteQuests);
            if (choosenDialog.Actions.GetQuests.Any())
                toolStripStatusLabel.Text += " Взятые: " + getListAsString(choosenDialog.Actions.GetQuests);

            if (choosenDialog.Actions.Event == (int) DialogEvents.trade)
                addActionTextToEmulator(" Торговля. Выход.", choosenDialogID);
            else if (choosenDialog.Actions.Event == (int) DialogEvents.change)
                addActionTextToEmulator(" Обмен. Выход.", choosenDialogID);
            else if (choosenDialog.Actions.Event == (int)DialogEvents.create_clan)
                addActionTextToEmulator(" Создание клана. Выход.", choosenDialogID);
            else if (choosenDialog.Actions.Event == (int)DialogEvents.barter)
                addActionTextToEmulator(" Бартер. Выход.", choosenDialogID);
            else if (choosenDialog.Actions.Event == (int)DialogEvents.repair)
                addActionTextToEmulator(" Починка. Выход.", choosenDialogID);
            else if (choosenDialog.Actions.Event == (int)DialogEvents.complex_repair)
                addActionTextToEmulator(" Комплексная починка. Выход.", choosenDialogID);
            else if (choosenDialog.Actions.Event == (int)DialogEvents.teleport)
                addActionTextToEmulator(" Телепорт. Выход.", choosenDialogID);
            else if (choosenDialog.Actions.Event == (int)DialogEvents.clan_base)
                addActionTextToEmulator(" Телепорт на базу. Выход.", choosenDialogID);
            else if (choosenDialog.Actions.Event == (int)DialogEvents.go_pvp_area)
                addActionTextToEmulator(" Идти в зону PvP. Выход.", choosenDialogID);
            else if (choosenDialog.Actions.Event == (int)DialogEvents.colorize_item)
                addActionTextToEmulator(" Покрасить предмет. Выход.", choosenDialogID);
            else if (choosenDialog.Actions.Exit)
                addActionTextToEmulator(" Выход.", choosenDialogID);

            else if (choosenDialog.Actions.ToDialog != 0)
            {
                toolStripStatusLabel.Text += " Переход на диал.: " + choosenDialog.Actions.ToDialog.ToString();
                //startEmulator(choosenDialog.Actions.ToDialog, false);
                Listener.setCurrentNode(choosenDialog.Actions.ToDialog);
            }
            else
            {
                //startEmulator(choosenDialogID,false);
                Listener.setCurrentNode(choosenDialogID);
            }            
        }
        //! Антиговнокод - добавление примечания к фразе диалога с действием
        void addActionTextToEmulator(string text, int dialogID)
        {
            toolStripStatusLabel.Text += text;
            EmulatorsplitContainer.Panel2.Controls.Clear();
            Listener.setCurrentNode(dialogID);
        }
        //! Выводит координаты узла как прямоугольника. Для отладки.
        public void setXYCoordinates(float X, float Y, float w, float h)
        {
            this.labelXNode.Text = "X=" + X.ToString();
            this.labelYNode.Text = "Y=" + Y.ToString();
            this.labelXNode.Text += " w=" + w.ToString();
            this.labelYNode.Text += " h=" + h.ToString();
        }

// ************************END DIALOGS BLOCK****************************************************

        //! Возвращает список как строку со значениями через запятую
        string getListAsString(List<int> list)
        {
            string str = "";
            foreach (int element in list)
            {
                if (str.Equals(""))
                    str += element.ToString();
                else
                    str += "," + element.ToString();
            }
            return str;
        }

        public void clearToolstripLabel()
        {
            toolStripStatusLabel.Text = "";
        }

        //! Устанавливает доступность компонентов формы
        private void SetControlsAbility(bool enable)
        {
            NPCBox.Enabled = enable;
            bAddNPC.Enabled = enable;
            bDelNPC.Enabled = enable;
            QuestBox.Enabled = enable;
            bRemoveQuest.Enabled = enable;
            bAddQuest.Enabled = enable;
        }
        //! Блокировка компонентов и обновление данных при смене вкладки на форме
        private void onSelectTab(object sender, EventArgs e)
        {
            bCopyEvents.Enabled = false;
            int index = CentralDock.SelectedIndex;
            switch (index)
            {
                case 0:         // вкладка Диалоги
                    SetControlsAbility(true);
                    if (!currentNPC.Equals(""))
                    {
                        QuestBox.Items.Clear();
                        fillQuestChangeBox(true);
                        QuestBox.Enabled = false;
                        bRemoveQuest.Enabled = false;
                        bAddQuest.Enabled = false;
                        QuestBox.Text = "Число квестов: " + quests.getCountOfQuests(currentNPC);
                        DialogSelected(true);
                    }
                    break;
                case 1:         // вкладка Квесты
                    SetControlsAbility(true);
                    bRemoveQuest.Enabled = false;
                    QuestBox.Items.Clear();
                    fillQuestChangeBox(false);
                    QuestBox.Text = "Пожалуйста, выберите квест";
                    break;                    
                case 2:         // Вкладка Связи NPC 
                    SetControlsAbility(false);
                    NPCBox.Enabled = true;
                    break;                
                case 3: case 5: case 6:     // Вкладки Проверка, Перевод, Баланс
                    SetControlsAbility(false);
                    break;
                case 4:         // Вкладка Управление (квестами)
                    FillTabManage();
                    break;
            }
        }

// ******************************* WORK WITH QUESTS ***************************************
        //! Возвращает экземпляр CQuest по его ID
        public CQuest getQuestOnQuestID(int questID)
        {
            return quests.getQuest(questID);
        }

        void addNodeOnTreeQuest(int currentQuest)
        {
            CQuest curQuest = getQuestOnQuestID(currentQuest);
            treeQuest.BeginUpdate();
            if (!curQuest.Additional.IsSubQuest.Equals(0))
            {
                TreeNode[] nodes = treeQuest.Nodes.Find(getQuestOnQuestID(curQuest.Additional.IsSubQuest).QuestID.ToString(), true);
                if (nodes.Any())
                {
                    TreeNode parent = nodes[0];
                    parent.Nodes.Add(curQuest.QuestID.ToString(), curQuest.QuestID.ToString());
                    int lastIndex = parent.Nodes.Count - 1;
                    if (curQuest.Target.onFin == 1)
                        parent.Nodes[lastIndex].BackColor = Color.Green;
                    else
                        parent.Nodes[lastIndex].BackColor = Color.Red;
                }
            }
            else 
            {
                treeQuest.Nodes.Add(curQuest.QuestID.ToString(), curQuest.QuestID.ToString());
                int lastIndex = treeQuest.Nodes.Count - 1;
                if (curQuest.Target.onFin == 1)
                    treeQuest.Nodes[lastIndex].BackColor = Color.Green;
                else
                    treeQuest.Nodes[lastIndex].BackColor = Color.Red;
            }
                //if (!questConst.isSimple(curQuest.Target.QuestType) && (curQuest.Additional.ListOfSubQuest.Any()))
            if ( (!questConst.isSimple(curQuest.Target.QuestType) || settings.getMode() == settings.MODE_LOCALIZATION)
                    && ( curQuest.Additional.ListOfSubQuest.Any() ) )
                    foreach (int subquest in curQuest.Additional.ListOfSubQuest)
                        addNodeOnTreeQuest(subquest);
            treeQuest.EndUpdate();
            treeQuest.Refresh();
            treeQuest.Update();
        }

        //! Создает плашку с информацией о субквесте
        void createQuestPanels(int questID)
        {
            //CQuest quest = getQuestOnQuestID(questID);
            CQuest quest = quests.getQuestLocalized(questID);

            Panel questPanel = new Panel();
            questPanel.AutoSize = true;
            questPanel.Dock = DockStyle.Top;
            questPanel.Size = new Size(splitQuestsContainer.Panel2.Width - 5, 100);
            questPanel.BorderStyle = BorderStyle.FixedSingle;
            questPanel.Click += questPanel_Click;

            GroupBox questBox = new GroupBox();
            questBox.AutoSize = true;
            questBox.Text = "Квест ID: " + questID;
            questBox.Dock = DockStyle.Top;

            Label eventLabel = new Label();
            eventLabel.Text = "Событие: " + questConst.getDescription(quest.Target.QuestType);
            eventLabel.Dock = DockStyle.Top;
            questBox.Controls.Add(eventLabel);
            questBox.Controls.SetChildIndex(eventLabel, 1);

            if (quests.getTargetString(quest) != "")
            {
                Label targetLabel = new Label();
                targetLabel.Text = "Цель: " + quests.getTargetString(quest);
                targetLabel.Dock = DockStyle.Top;
                questBox.Controls.Add(targetLabel);
                questBox.Controls.SetChildIndex(targetLabel, 1);
            }

            GroupBox infoBox = new GroupBox();
            infoBox.AutoSize = true;
            infoBox.Text = "Информация";
            infoBox.Dock = DockStyle.Top;
            infoBox.Click += questPanel_Click;
            infoBox.Tag = questID;

            Label titleLabel = new Label();
            titleLabel.Text = "Заголовок:" + quest.QuestInformation.Title;
            titleLabel.BackColor = (quest.Version != 0) ? (Color.FromKnownColor(KnownColor.Transparent)) : (Color.FromArgb(0x7FAA45E0));
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Click += questPanel_Click;
            titleLabel.Tag = questID;
            infoBox.Controls.Add(titleLabel);

            Label descriptionLabel = new Label();
            descriptionLabel.Text = "Описание:" + quest.QuestInformation.Description;
            descriptionLabel.BackColor = (quest.Version != 0) ? (Color.FromKnownColor(KnownColor.Transparent)) : (Color.FromArgb(0x7FAA45E0));
            descriptionLabel.Dock = DockStyle.Top;
            descriptionLabel.Click += questPanel_Click;
            descriptionLabel.Tag = questID;
            infoBox.Controls.Add(descriptionLabel);

            questBox.Controls.Add(infoBox);
            questBox.Controls.SetChildIndex(infoBox,0);

            questPanel.Controls.Add(questBox);

            questBox.Controls.SetChildIndex(eventLabel, 2);
            panels.Add(questPanel, questID);

            if (quest.Additional.ListOfSubQuest.Any())
                foreach (int subquest in quest.Additional.ListOfSubQuest)
                    createQuestPanels(subquest);
        }

        void questPanel_Click(object sender, EventArgs e)
        {
            currentQuest = (int)((Control)sender).Tag;
            treeQuestClicked(sender, e);
            //MessageBox.Show("It works! " + currentQuest.ToString());            
        }
        //! Выделение квеста в дереве квестов
        private void treeQuestSelected(object sender, EventArgs e)
        {
            this.currentQuest = int.Parse(treeQuest.SelectedNode.Text);
//            int questID = int.Parse(treeQuest.SelectedNode.Text);
            checkQuestButton(getQuestOnQuestID(currentQuest).Target.QuestType, currentQuest);
            fillQuestPanel();
        }
        //! Заполнение подквестов в виде списка в splitQuestsContainer
        void fillQuestPanel()
        {
            panels.Clear();
            splitQuestsContainer.Panel2.Controls.Clear();
            createQuestPanels(currentQuest);
            foreach (int subquest in getQuestOnQuestID(currentQuest).Additional.ListOfSubQuest)
            {
                Panel panel = getQuestPanelOnQuestID(subquest);
                splitQuestsContainer.Panel2.Controls.Add(panel);
                splitQuestsContainer.Panel2.Controls.SetChildIndex(panel, 0);
            }
            Panel Mpanel = getQuestPanelOnQuestID(currentQuest);

            splitQuestsContainer.Panel2.Controls.Add(Mpanel);
            splitQuestsContainer.Panel2.Controls.SetChildIndex(Mpanel, getQuestOnQuestID(currentQuest).Additional.ListOfSubQuest.Count);
        }

        Panel getQuestPanelOnQuestID(int questID)
        {
            foreach (Panel panel in panels.Keys)
                if (panels[panel] == questID)
                    return panel;
            return null;
        }
        //! Двойной клик по квесту в дереве квестов. Открытие окна редактирования или перевода квеста
        private void treeQuestClicked(object sender, EventArgs e)
        {
            bCopyEvents.Enabled = true;
            if (settings.getMode() == settings.MODE_EDITOR)
            {
                if (getQuestOnQuestID(currentQuest).Additional.IsSubQuest == 0)
                {
                    //openQuestEditForm(false);
                    EditQuestForm questEditor = new EditQuestForm(this, currentQuest, 2);
                    questEditor.Visible = true;
                    this.Enabled = false;
                }
                else
                {
                    EditQuestForm questEditor = new EditQuestForm(this, currentQuest, 3);
                    questEditor.Visible = true;
                    this.Enabled = false;
                }
            }
            else
            {
                LocaleQuestForm questForm = new LocaleQuestForm(this, currentQuest);
                questForm.Show();
            }
        }

        //! Нажатие кнопки Добавить NPC - открывает форму с именем
        private void bAddNPC_Click(object sender, EventArgs e)
        {
            NewNPC newNPC = new NewNPC(this);
            newNPC.Visible = true;
        }
        //! Добавляет нового NPC в систему
        public void addNewNPC(string Name)
        {
            Dictionary<int,CDialog> firstDialog = new Dictionary<int,CDialog>();
            int dialogID = getDialogsNewID();
            NodeCoordinates nc = new NodeCoordinates(179, 125, true, true);
            firstDialog.Add(dialogID, new CDialog(Name, "", "", new CDialogPrecondition(), new Actions() ,new List<int>(),
                    dialogID, 0, nc));

            dialogs.dialogs.Add(Name, firstDialog);
            // добавляем ТЗс в английскую локаль, делаем копию словаря
            Dictionary<int, CDialog> engDialog = new Dictionary<int, CDialog>();
            engDialog.Add(dialogID, new CDialog(Name, "", "", new CDialogPrecondition(), new Actions(), new List<int>(),
                    dialogID, 0, nc));
            dialogs.locales[settings.getListLocales()[0]].Add(Name, engDialog);
            
            fillNPCBox();
            NPCBox.SelectedValue = Name;
            npcConst.NPCs.Add(Name, new CNPCDescription(Name));
        }
        //! Нажатие на кнопку Удаление Персонажа NPC
        private void bDelNPC_Click(object sender, EventArgs e)
        {
            graphs.Clear();
            treeDialogs.Nodes.Clear();
            DialogShower.Layer.RemoveAllChildren();

            List<int> removedQuests = new List<int>();
            foreach (CQuest quest in quests.quest.Values)
                if (quest.Additional.Holder.Equals(currentNPC))
                    removedQuests.Add(quest.QuestID);

            foreach (int item in removedQuests)
            {
                quests.quest.Remove(item);
                quests.locales[settings.getListLocales()[0]].Remove(item);
            }
            dialogs.dialogs.Remove(currentNPC);
            dialogs.locales[settings.getListLocales()[0]].Remove(currentNPC);
            currentNPC = "";
            fillNPCBox();
        }
        //! Устанавливает доступность средств работы с квестами
        public void checkQuestButton(int questType, int questID)
        {
            bEditEvent.Enabled = true;
            if (questConst.isSimple(questType))// && getQuestOnQuestID(questID).Additional.ListOfSubQuest.Any())
                bAddEvent.Enabled = false;
            else
                bAddEvent.Enabled = true;

            if (getQuestOnQuestID(questID).Additional.IsSubQuest.Equals(0))
                bRemoveEvent.Enabled = false;
            else
                bRemoveEvent.Enabled = true;

            if (getQuestOnQuestID(questID).Additional.IsSubQuest != 0)
            {
                CQuest parent = getQuestOnQuestID(getQuestOnQuestID(questID).Additional.IsSubQuest);

                if (parent.Additional.ListOfSubQuest.Any())
                {
                    if (parent.Additional.ListOfSubQuest.Last().Equals(questID))
                        bQuestDown.Enabled = false;
                    else
                        bQuestDown.Enabled = true;

                    if (parent.Additional.ListOfSubQuest.First().Equals(questID))
                        bQuestUp.Enabled = false;
                    else
                        bQuestUp.Enabled = true;
                }
            }
        }

        public int getQuestNewID()
        {
            int iFirstQuestID = 1 + this.settings.getOperatorNumber() * 400;
            for (int questi = iFirstQuestID; ; questi++)
                if (!quests.quest.Keys.Contains(questi) && !quests.m_Buffer.Keys.Contains(questi))
                    return questi;
        }

        //! Создает новый корневой квест у персонажа (не подквест, а именно новый)
        public void createNewQuest(CQuest newQuest)
        {
            quests.quest.Add(newQuest.QuestID, newQuest);
            CQuest engQuest = new CQuest();
            engQuest = (CQuest)newQuest.Clone();
            quests.locales[settings.getListLocales()[0]].Add(engQuest.QuestID, engQuest);

            QuestBox.Items.Add(newQuest.QuestID.ToString() + ": " + newQuest.QuestInformation.Title);
            QuestBox.SelectedIndex = QuestBox.Items.Count - 1;
        }

        //! Добавление квеста в дерево квестов, вызывается из окна редактирования EditQuestForm
        public void addQuest(CQuest quest, int parent)
        {
            quests.quest[parent].Additional.ListOfSubQuest.Add(quest.QuestID);
            quests.quest.Add(quest.QuestID, quest);

            CQuest engQuest = new CQuest();
            engQuest = (CQuest)quest.Clone();
            quests.locales[settings.getListLocales()[0]][parent].Additional.ListOfSubQuest.Add(engQuest.QuestID);
            quests.locales[settings.getListLocales()[0]].Add(engQuest.QuestID, engQuest);

            checkQuestButton(quest.Target.QuestType, quest.QuestID);            
            addNodeOnTreeQuest(quest.QuestID);
            treeQuest.ExpandAll();
            fillQuestPanel();
        }
        //! Заменяет данные квеста при редактировании
        public void replaceQuest(CQuest quest)
        {
            //CQuest replacedQuest = quests.quest[quest.QuestID];
            //quests.quest[quest.QuestID] = quest;
            quests.replaceQuest(quest);
            if (quests.quest.Keys.Contains(quest.QuestID))
                quests.locales[settings.getListLocales()[0]][quest.QuestID].InsertNonTextData(quest);
            checkQuestButton(quest.Target.QuestType, quest.QuestID);
        }
        //! Нажатие на кнопку "Добавление квеста", вызов окна EditQuestForm
        private void bAddEvent_Click(object sender, EventArgs e)
        {
            EditQuestForm questEditor = new EditQuestForm(this, currentQuest, 4);
            questEditor.Visible = true;
            this.Enabled = false;
        }
        //! Правка квеста, в зависимости от режима вызывается окно правки или окно перевода
        private void bEditEvent_Click(object sender, EventArgs e)
        {
            if (settings.getMode() == settings.MODE_EDITOR)
            {
                if (getQuestOnQuestID(currentQuest).Additional.IsSubQuest == 0)
                {
                    //openQuestEditForm(false);
                    EditQuestForm questEditor = new EditQuestForm(this, currentQuest, 2);
                    questEditor.Visible = true;
                    this.Enabled = false;
                }
                else
                {
                    EditQuestForm questEditor = new EditQuestForm(this, currentQuest, 3);
                    questEditor.Visible = true;
                    this.Enabled = false;
                }
            }
            else
            {
                LocaleQuestForm quest_form = new LocaleQuestForm(this, currentQuest);
                quest_form.Visible = true;
                this.Enabled = false;
            }
        }
        //! Сохранение данных по диалогам и квестам (если режим редактора - то сохранение и локализаций)
        void saveData()
        {
            this.Enabled = false;
            if (settings.getMode() == settings.MODE_EDITOR)
            {
                dialogs.saveDialogs(settings.getDialogsPath());  // settings.dialogXML
                quests.saveQuests(settings.getQuestsPath());     // settings.questXML
            }
            //else
            //{
            // сохраняем изменения в локализациях
            dialogs.saveLocales(settings.getDialogLocalePath());  // settings.dialogXML
            quests.saveLocales(settings.getQuestLocalePath());    // settings.questXML
            //}
            Thread.Sleep(1000);
            toolStripStatusLabel.Text = "Данные успешно сохранены.";
            this.Enabled = true;
        }
        //! Нажатие на кнопку "Удаление квеста"
        private void bRemoveEvent_Click(object sender, EventArgs e)
        {
            this.removeQuest(currentQuest, false);
        }

        //! Удаляет квест и  все его подквесты
        void removeQuest(int questID, bool recursiveCall)
        {
            List<int> temp = getQuestOnQuestID(questID).Additional.ListOfSubQuest;
            foreach (int subquest in temp ) //getQuestOnQuestID(questID).Additional.ListOfSubQuest)
                removeQuest(subquest, true);

            if ( !recursiveCall )
                if (getQuestOnQuestID(questID).Additional.IsSubQuest != 0)
                    quests.quest[getQuestOnQuestID(questID).Additional.IsSubQuest].Additional.ListOfSubQuest.Remove(questID);

            // удаляем квест из локализаций
            CQuest english = quests.locales[settings.getListLocales()[0]][questID];
            foreach (int subquest in english.Additional.ListOfSubQuest)
                removeQuest(subquest, true);
            if (english.Additional.IsSubQuest != 0)
                quests.locales[settings.getListLocales()[0]][english.Additional.IsSubQuest].Additional.ListOfSubQuest.Remove(questID);
            quests.locales[settings.getListLocales()[0]].Remove(questID);

            treeQuest.Nodes.Find(questID.ToString(), true)[0].Remove();
            quests.quest.Remove(questID);
        }
        //! Перемещение квеста вверх по дереву квестов
        private void bQuestUp_Click(object sender, EventArgs e)
        {
            int temp;
            CQuest parent = getQuestOnQuestID(getQuestOnQuestID(currentQuest).Additional.IsSubQuest);
            for (int i = 0; i < parent.Additional.ListOfSubQuest.Count; i++)
                if (parent.Additional.ListOfSubQuest[i] == currentQuest)
                {
                    temp = parent.Additional.ListOfSubQuest[i - 1];
                    parent.Additional.ListOfSubQuest[i - 1] = currentQuest;
                    parent.Additional.ListOfSubQuest[i] = temp;

                    treeQuest.Nodes.Find(parent.QuestID.ToString(), true)[0].Nodes.Clear();
                    foreach (int subquests in parent.Additional.ListOfSubQuest)
                        addNodeOnTreeQuest(subquests);
                    treeQuest.ExpandAll();
                    findAndSelectNodeOnTreeQuest(currentQuest);

                    break;
                }

            int id = parent.QuestID;
            quests.locales[settings.getListLocales()[0]][id].Additional.ListOfSubQuest = new List<int> (parent.Additional.ListOfSubQuest);
        }
        //! Перемещение квеста вниз по дереву квестов
        private void bQuestDown_Click(object sender, EventArgs e)
        {
            int temp;
            CQuest parent = getQuestOnQuestID(getQuestOnQuestID(currentQuest).Additional.IsSubQuest);
            for (int i = 0; i < parent.Additional.ListOfSubQuest.Count; i++)
                if (parent.Additional.ListOfSubQuest[i] == currentQuest)
                {
                    temp = parent.Additional.ListOfSubQuest[i + 1];
                    parent.Additional.ListOfSubQuest[i + 1] = currentQuest;
                    parent.Additional.ListOfSubQuest[i] = temp;

                    treeQuest.Nodes.Find(parent.QuestID.ToString(), true)[0].Nodes.Clear();
                    foreach (int subquests in parent.Additional.ListOfSubQuest)
                        addNodeOnTreeQuest(subquests);
                    treeQuest.ExpandAll();
                    findAndSelectNodeOnTreeQuest(currentQuest);
                    break;
                }

            int id = parent.QuestID;
            quests.locales[settings.getListLocales()[0]][id].Additional.ListOfSubQuest = new List<int> (parent.Additional.ListOfSubQuest);
        }

        void findAndSelectNodeOnTreeQuest(int questID)
        {
            treeQuest.SelectedNode = treeQuest.Nodes.Find(questID.ToString(), true)[0];
        }
        //! Нажатие на кнопку "Удаление корневого квеста", т.е. удаляется корневой (не субквест) квест у персонажа
        private void bRemoveQuest_Click(object sender, EventArgs e)
        {
            int removedQuest = int.Parse(QuestBox.SelectedItem.ToString().Split(':')[0].Trim());
            QuestBox.Items.Remove(QuestBox.SelectedItem);
            removeQuest(removedQuest, false);
        }
        //! Создание нового корневого квеста у персонажа
        private void bAddQuest_Click(object sender, EventArgs e)
        {
            EditQuestForm newQuest = new EditQuestForm(this, getQuestNewID(), 1); //, true, true);
            newQuest.Visible = true;
            this.Enabled = false;
        }
 
        public void setQuestISClan(int questID, bool ClanState)
        {
            quests.setClan(questID, ClanState);
        }

        List<CQuest> getTreeItemIDs(int questID)
        {
            List<CQuest> ret = new List<CQuest>();
            CQuest quest = getQuestOnQuestID(questID);
            if (quest.Reward.AttrOfItems.Contains(1) || quest.QuestRules.AttrOfItems.Contains(1))
                ret.Add(quest);
            foreach (int subquestID in quest.Additional.ListOfSubQuest)
                ret.AddRange(getTreeItemIDs(subquestID));
            return ret;
        }

        public List<CQuest> getTreesItemIDs(int questID)
        {
            CQuest quest = getQuestOnQuestID(questID);
            if (quest != null)
            {
                if (!quest.Additional.IsSubQuest.Equals(0))
                    return getTreesItemIDs(quest.Additional.IsSubQuest);
                else
                    return getTreeItemIDs(questID);
            }
            else return new List<CQuest>();
        }
        //! Возвращает ID всех субквестов любой глубины вложенности (т.е субквесты субквеста и т.д.)
        public List<int> getSubIDs(int questID)
        {
            List<int> ret = new List<int>();
            CQuest quest = getQuestOnQuestID(questID);
            if (quest != null)
            {
                ret.Add(questID);
                foreach (int subquestID in quest.Additional.ListOfSubQuest)
                    ret.AddRange(getSubIDs(subquestID));
            }
            return ret;
        }
        
        //! Заполняет вкладку Управление
        void FillTabManage()
        {
            dgvManage.Rows.Clear();
            
            ((DataGridViewComboBoxColumn)dgvManage.Columns[18]).Items.Clear();
            ((DataGridViewComboBoxColumn)dgvManage.Columns[18]).Items.Add("Да.");
            ((DataGridViewComboBoxColumn)dgvManage.Columns[18]).Items.Add("Нет.");
            ((DataGridViewComboBoxColumn)dgvManage.Columns[18]).Items.Add("Правка.");

            foreach (string npcName in npcConst.NPCs.Keys)
                foreach (CQuest quest in quests.quest.Values)
                    if (quest.Additional.Holder == npcName && (quest.Additional.IsSubQuest==0) )
                    {

                        string id = quest.QuestID.ToString();
                        List<int> iSubIDS = getSubIDs(quest.QuestID);
                        List<string> sNPCLink = new List<string>();
                        int rewardExpBattle = 0;
                        int rewardExpSurvival = 0;
                        int rewardExpSupport = 0;
                        float rewardCredits = 0;
                        Dictionary<int, int> rewardItems = new Dictionary<int, int>();
                        int pkStatus = 0;

                        foreach (int quid in iSubIDS)
                        {
                            CQuest q = getQuestOnQuestID(quid);

                            //if (q.Reward.Expirience.Any())
                            //{
                            //    rewardExpBattle += q.Reward.Expirience[0];
                            //    rewardExpSurvival += q.Reward.Expirience[1];
                            //    rewardExpSupport += q.Reward.Expirience[2];
                            //}

                            rewardCredits += q.Reward.Credits;
                            pkStatus += q.Reward.KarmaPK;

                            for (int index = 0; index < q.Reward.TypeOfItems.Count; ++index)
                            {
                                int type = q.Reward.TypeOfItems[index];
                                int count = q.Reward.NumOfItems[index];
                                int attr = 0;
                                if (q.Reward.AttrOfItems.Any())
                                     attr = q.Reward.AttrOfItems[index];
                                if (attr == 0)
                                {
                                    if (rewardItems.Keys.Contains(type))
                                        rewardItems[type] += count;
                                    else
                                        rewardItems[type] = count;
                                }
                            }
                        }

                        string npcLinks = "";
                        string getDialogs = "";
                        string subIDs = getListAsString(iSubIDS);
                        string title = quest.QuestInformation.Title;
                        string description = quest.QuestInformation.Description;
                        string npcNe = quest.Additional.Holder;
                        string srewardExpBattle = rewardExpBattle.ToString();
                        string srewardSurvival = rewardExpSurvival.ToString();
                        string srewardExpSupport = rewardExpSupport.ToString();
                        string srewardCredits = rewardCredits.ToString();
                        string sPkStatus = pkStatus.ToString();
                        string sRewardItem = "";
                        string sRepeat = quest.Precondition.Repeat.ToString();
                        string sPeriod = quest.Precondition.TakenPeriod.ToString();

                        string sLevel = "";
                        string sAuthor = "";
                        string sLegend = "";
                        string sWorked = "Нет.";

                        COperNote note = manageNotes.getNote(quest.QuestID);
                        if (note != null)
                        {
                            sAuthor = note.iOperator;
                            sLegend = note.sHistory;
                            sLevel = note.iLevel;
                            if (note.iWorked == 1)
                                sWorked = "Да.";
                            else if (note.iWorked == 2)
                                sWorked = "Правка.";
                        }   

                        //NPC dialog statistic
                        foreach (KeyValuePair<string, Dictionary<int, CDialog>> dialogNPC in dialogs.dialogs)
                        {
                            foreach (KeyValuePair<int, CDialog> dialog in dialogNPC.Value)
                            {
                                if (dialog.Value.Holder == npcName)
                                    if (dialog.Value.Actions.GetQuests.Contains(quest.QuestID))
                                    {
                                        if (getDialogs == "")
                                            getDialogs += dialog.Value.DialogID.ToString();
                                        else
                                            getDialogs += ("," + dialog.Value.DialogID.ToString());
                                    }
                                if (!sNPCLink.Contains(dialog.Value.Holder) && dialog.Value.Holder != npcNe)
                                {
                                    foreach (int q in dialog.Value.Actions.CompleteQuests)
                                        if ((iSubIDS.Contains(q)) && (!sNPCLink.Contains(dialog.Value.Holder)))
                                                sNPCLink.Add(dialog.Value.Holder);
                                }
                            }

                        }
                        //NPC dialog statistic

                        foreach (string npc in sNPCLink)
                        {
                            if (npcLinks == "")
                                npcLinks += npc;
                            else
                                npcLinks += "," + npc;
                        }

                        //rewardItem
                        
                        foreach (int type in rewardItems.Keys)
                        {
                            string itemName = itemConst.getDescriptionOnID(type);
                            string count = rewardItems[type].ToString();

                            if (sRewardItem == "")
                                sRewardItem += itemName + ":" + count.ToString();
                            else
                                sRewardItem += "\n" + itemName + ":" + count.ToString();
                        }
                        object[] row = { id, subIDs, title, description, npcNe, npcLinks, getDialogs, srewardExpBattle, srewardSurvival, srewardExpSupport, srewardCredits, sRewardItem, sPkStatus, sRepeat, sPeriod, sLevel, sAuthor, sLegend, sWorked };
                        dgvManage.Rows.Add(row);
                    }
        }

        private void bSaveManage_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvManage.Rows)
            {
                try
                {
                    int questID = int.Parse(row.Cells[0].FormattedValue.ToString());
                    string level = row.Cells[15].FormattedValue.ToString();
                    string author = row.Cells[16].FormattedValue.ToString();
                    string history = row.Cells[17].FormattedValue.ToString();
                    string worked = row.Cells[18].FormattedValue.ToString();

                    int iWorked = 0;
                    if (worked == "Да.")
                        iWorked = 1;
                    else if (worked == "Правка.")
                        iWorked = 2;
                    if (level != "" || author != "" || history != "" || iWorked != 0)
                        manageNotes.addNote(questID, level, iWorked, author, history);
                }
                catch
                {
                    System.Console.WriteLine("Can't parse row");
                }
            }
            manageNotes.save();
        }

//*************************************************BUFFER'S WORK*******************

        //! Нажатие на кнопку "Копировать" квесты в буфер обмена
        private void bCopyEvents_Click(object sender, EventArgs e)
        {
            quests.PutQuestsToBuffer(currentQuest, false);
            treeQuestBuffer.Nodes.Clear();
            addNodeOnTreeBuffer(quests.m_Buffer.First().Value.QuestID);
        }

        //! Нажатие на кнопку "Вырезать" квесты и поместить их в буфер обмена квестов
        private void bCutEvents_Click(object sender, EventArgs e)
        {
            quests.PutQuestsToBuffer(currentQuest, true);
            treeQuestBuffer.Nodes.Clear();
            addNodeOnTreeBuffer(quests.m_Buffer.First().Value.QuestID);
        }

        private void treeQuest_Leave(object sender, EventArgs e)
        {
            if (currentQuest == 0)
                bCopyEvents.Enabled = false;
            //bPasteEvents.Enabled = false;
        }

        private void treeQuest_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.currentQuest = int.Parse(treeQuest.SelectedNode.Text);

            bCopyEvents.Enabled = true;
            if (this.quests.m_Buffer.Any())
                bPasteEvents.Enabled = true;
        }

        //! Добавляет узел квеста в дерево буфера квестов
        void addNodeOnTreeBuffer(int currentQuest)
        {
            CQuest curQuest = getQuestOnQuestID(currentQuest);

            if (!curQuest.Additional.IsSubQuest.Equals(0))
            {
                TreeNode[] nodes = treeQuestBuffer.Nodes.Find(getQuestOnQuestID(curQuest.Additional.IsSubQuest).QuestID.ToString(), true);
                if (nodes.Any())
                {
                    TreeNode parent = nodes[0];
                    parent.Nodes.Add(curQuest.QuestID.ToString(), curQuest.QuestID.ToString());
                    int lastIndex = parent.Nodes.Count - 1;
                    if (curQuest.Target.onFin == 1)
                        parent.Nodes[lastIndex].BackColor = Color.Green;
                    else
                        parent.Nodes[lastIndex].BackColor = Color.Red;
                }
            }
            else
            {
                treeQuestBuffer.Nodes.Add(curQuest.QuestID.ToString(), curQuest.QuestID.ToString());
                int lastIndex = treeQuest.Nodes.Count - 1;
                if (curQuest.Target.onFin == 1)
                    treeQuestBuffer.Nodes[lastIndex].BackColor = Color.Green;
                else
                    treeQuestBuffer.Nodes[lastIndex].BackColor = Color.Red;
            }
            if (!questConst.isSimple(curQuest.Target.QuestType) && (curQuest.Additional.ListOfSubQuest.Any()))
                foreach (int subquest in curQuest.Additional.ListOfSubQuest)
                    addNodeOnTreeBuffer(subquest);
        }
        //! Дабл клик по квесту в буфере обмена, открывает редактор квеста
        private void treeQuestBuffer_DoubleClick(object sender, EventArgs e)
        {
            bCopyEvents.Enabled = false;

            //this.currentQuest = int.Parse(treeQuestBuffer.SelectedNode.Text);
            if (getQuestOnQuestID(currentQuest).Additional.IsSubQuest == 0)
            {
                EditQuestForm questEditor = new EditQuestForm(this, currentQuest, 2);
                questEditor.Visible = true;
                this.Enabled = false;
            }

            else
            {
                EditQuestForm questEditor = new EditQuestForm(this, currentQuest, 3);
                questEditor.Visible = true;
                this.Enabled = false;
            }
        }

        private void treeQuestBuffer_Click(object sender, EventArgs e)
        {
            try
            {
                this.currentQuest = int.Parse(treeQuestBuffer.SelectedNode.Text);
            }
            catch
            {
            }
            bCopyEvents.Enabled = false;
            bPasteEvents.Enabled = false;
        }

        private void treeQuest_Click(object sender, EventArgs e)
        {
            try
            {
                this.currentQuest = int.Parse(treeQuest.SelectedNode.Text);
            }
            catch
            {
            }
            if (quests.m_Buffer.Any())
                bPasteEvents.Enabled = true;
        }

        //! Нажатие на кнопку "Вставить" квесты из буфера в дерево квестов
        private void bPasteEvents_Click(object sender, EventArgs e)
        {
            //System.Console.WriteLine("MainForm::bPasteEvents_Click");
            var quest = getQuestOnQuestID(currentQuest);
            if (quest != null)
            {
                if (!questConst.isSimple(quest.Target.QuestType))
                {
                    switch (MessageBox.Show("Вставить как подквест?", "Выберите тип вставки", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes: pasteBuffer(); break;
                        case DialogResult.No: replaceBuffer(); break;
                    }
                }
                else
                    replaceBuffer();
            }
        }

        //! Вставляет квесты из буфера обмена как подквест 
        private void pasteBuffer()
        {
            quests.PasteBuffer(currentQuest);
            var rootID = quests.getRoot(currentQuest);
            treeQuest.Nodes.Clear();
            addNodeOnTreeQuest(rootID);
            treeQuest.ExpandAll();

            if (quests.CutQuests)
                clearQuestsBuffer();
        }

        //! Заменяет выделенный квест на квесты из буфера обмена
        private void replaceBuffer()
        {
            var quest = getQuestOnQuestID(currentQuest);
            var parentID = quest.Additional.IsSubQuest;
            quests.ReplaceBuffer(currentQuest);
            var root = quests.getRoot(parentID);
            treeQuest.Nodes.Clear();
            addNodeOnTreeQuest(root);

            treeQuest.ExpandAll();
            if (quests.CutQuests)
                clearQuestsBuffer();
        }

        private void treeQuestBuffer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //System.Console.WriteLine("treeQuestBuffer_AfterSelect");
            this.currentQuest = int.Parse(treeQuestBuffer.SelectedNode.Text);
            //System.Console.WriteLine("currentQuest:" + currentQuest.ToString());
        }

        public void setTutorial(int questID, bool isTutor)
        {
            quests.setTutorial(questID, isTutor);
        }

        //! Очищает буфер обмена квестов
        public void clearQuestsBuffer()
        {
            quests.m_Buffer.Clear();
            quests.m_EngBuffer.Clear();
            treeQuestBuffer.Nodes.Clear();
            quests.bufferTop = 0;
            bPasteEvents.Enabled = false;
        }

        //! Нажатие на кнопку Очистка буфера обмена квестов
        private void bClearBuffer_Click(object sender, EventArgs e)
        {
            clearQuestsBuffer();
        }

//*************************************************LOCALES*******************

        public CDialog getLocaleDialog(int dialogID, string npcName)
        {
            var locale = settings.getCurrentLocale();
            return dialogs.getLocaleDialog(dialogID, locale, npcName);
        }

        public void addLocaleDialog(CDialog dialog)
        {
            this.dialogs.addLocaleDialog(dialog, settings.getCurrentLocale());
        }

        public CQuest getLocaleQuest(int questID)
        {
            var locale = settings.getCurrentLocale();
            return quests.getLocaleQuest(questID, locale);
        }

        public void addLocaleQuest(CQuest quest)
        {
            var locale = settings.getCurrentLocale();
            this.quests.addLocaleQuest(quest, locale);
        }

        //! Изменения на форме при смене режима (Editor <-> Localization)
        public void onChangeMode()
        {
            int currentNpcIndex = NPCBox.SelectedIndex;
            if (settings.getMode() == settings.MODE_LOCALIZATION)
            {
                for (int i = 4; i < CentralDock.TabPages.Count; i++)
                {
                    if (CentralDock.TabPages[i].Name != "tabTranslate" && CentralDock.TabPages[i].Name != "tabSearch")
                        CentralDock.TabPages[i].Enabled = false;
                    else
                        CentralDock.TabPages[i].Enabled = true;
                }
            }
            else
            {
                for (int i = 2; i < CentralDock.TabPages.Count; i++)
                {
                    if (CentralDock.TabPages[i].Name == "tabTranslate")
                        CentralDock.TabPages[i].Enabled = false;
                    else
                        CentralDock.TabPages[i].Enabled = true;
                }
            }
            fillNPCBox();
            if (currentNpcIndex > -1)
                NPCBox.SelectedIndex = currentNpcIndex;
            if (selectedDialogID != 0)
                Listener.setCurrentNode(selectedDialogID);
        }

        //! Выводит диалоги для локализации. В зависимости от помеченных чекбоксов - актуальные или устаревшие
        private void bFindDialogDifference_Click(object sender, EventArgs e)
        {
            int actual = (ActualCheckBox.Checked) ? (1) : (0);
            int outdated = (OutdatedCheckBox.Checked) ? (1) : (0);
            FindType findType = (FindType)(actual + (outdated << 1) );
            this.translate_checker = 1;
            dgvLocaleDiff.Rows.Clear();
            string loc = settings.getCurrentLocale();
            var diff = dialogs.getDialogDifference(settings.getCurrentLocale(), findType);
            var type = "Диалог";
            int count = 0;

            foreach (var name in diff.Keys)
            {
                foreach (var id in diff[name].Keys)
                {
                    string location = (dialogs.NpcData.ContainsKey(name)) ? (dialogs.NpcData[name].location) : ("НЕТ ИМЕНИ");
                    string rustext1 = dialogs.dialogs[name][id].Title;
                    string engtext1 = dialogs.locales[loc][name][id].Title;
                    object[] row = { type, name, id, diff[name][id].old_version, diff[name][id].cur_version, location, rustext1, engtext1 };
                    dgvLocaleDiff.Rows.Add(row);
                    count++;
                }
            }
            labelLocalizeOuput.Text = "Выведено: "+ count.ToString();
            labelLocalizeOuput.Update();
        }

        int translate_checker = 0;
        private void dgvLocaleDiff_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var index = e.RowIndex;
            var id = int.Parse(dgvLocaleDiff.Rows[index].Cells[2].Value.ToString());
            currentNPC = dgvLocaleDiff.Rows[index].Cells[1].Value.ToString();
            if (this.translate_checker == 1)
            {
                LocaleDialogForm editLocaleDialogForm = new LocaleDialogForm(this, id);
                editLocaleDialogForm.Visible = true;
            }
            else if (this.translate_checker == 2)
            {
                LocaleQuestForm editLocaleQuestform = new LocaleQuestForm(this, id);
                editLocaleQuestform.Visible = true;
            }
        }
        //! Выводит квесты для локализации
        private void bFindQuestDifference_Click(object sender, EventArgs e)
        {
            int actual = (ActualCheckBox.Checked) ? (1) : (0);
            int outdated = (OutdatedCheckBox.Checked) ? (1) : (0);
            FindType findType = (FindType)(actual + (outdated << 1));
            this.translate_checker = 2;
            dgvLocaleDiff.Rows.Clear();
            string loc = settings.getCurrentLocale();
            var diff = quests.getQuestDifference(settings.getCurrentLocale(), findType);
            var type = "Квест";
            int count = 0;

            foreach (var name in diff.Keys)
            {
                foreach (var id in diff[name].Keys)
                {
                    string location = (dialogs.NpcData.ContainsKey(name)) ? (dialogs.NpcData[name].location) : ("НЕТ ИМЕНИ");
                    string rustext1 = quests.quest[id].QuestInformation.Title;
                    string engtext1 = quests.locales[loc][id].QuestInformation.Title;
                    object[] row = { type, name, id, diff[name][id].old_version, diff[name][id].cur_version, location, rustext1, engtext1 };
                    dgvLocaleDiff.Rows.Add(row);
                    count++;
                }
            }
            labelLocalizeOuput.Text = "Выведено: " + count.ToString();
            labelLocalizeOuput.Update();
        }

        //! Сохраняет Баланс фракций
        private void bSaveBalance_Click(object sender, EventArgs e)
        {

        }
        //! Нажатие на кнопку Отцентрировать - приводит DialogShower к исходному виду
        private void bCenterizeDialogShower_Click(object sender, EventArgs e)
        {
            // важное место - ставим зум на 1
            DialogShower.Camera.ViewScale = 1;
            // сдвиг ставим на 0 -камера возвращается в исходное положение
            DialogShower.Camera.SetViewOffset(0, 0);
        }

//*************************** DATA CHECK - missed dialogs, quests, NPC and so on**************************
        //! Заполнение итемов в комбобоксе выбора локации (во вкладке Проверка)
        void fillLocationsBox()
        {
            cbLocation.Sorted = true;
            cbLocation.DataSource = null;
            cbLocation.DataSource = dialogs.locationNames;
        }
        //! Устанавливает надписи в таблице вкладки Проверка для поиска нужны NPC
        void setNPCCheckEnvironment()
        {
            dgvReview.Columns[0].HeaderText = "Имя NPC";
            dgvReview.Columns[1].HeaderText = "Диалоги";
            dgvReview.Columns[2].HeaderText = "Квесты";
            dgvReview.Columns[3].HeaderText = "Карта";
            dgvReview.Columns[4].HeaderText = "Координаты";
            dgvReview.Columns[5].HeaderText = "Русское имя";
            dgvReview.Columns[4].Visible = true;
            dgvReview.Columns[5].Visible = true;
        }
        //! Устанавливает надписи в таблице вкладки Проверка для поиска квестов
        void setQuestCheckEnvironment()
        {
            dgvReview.Columns[0].HeaderText = "Имя NPC";
            dgvReview.Columns[1].HeaderText = "Квест";
            dgvReview.Columns[2].HeaderText = "Открыт квест";
            dgvReview.Columns[3].HeaderText = "Закрыт квест";
            dgvReview.Columns[4].Visible = false;
            dgvReview.Columns[5].Visible = false;
        }
 
        //! Нажатие "Найти NPC" на вкладке Проверка - поиск NPC с условием
        private void bFindNPC_Click(object sender, EventArgs e)
        {
            setNPCCheckEnvironment();
            dgvReview.Rows.Clear();
            bool checkDialog = cbNumDialogs.Checked;
            bool checkQuest = cbNumQuests.Checked;
            bool checkLocation = cbOnlyOnLocation.Checked;
            foreach (string npc in dialogs.dialogs.Keys)
            {
                int d_num = dialogs.dialogs[npc].Count;
                int q_num = quests.getCountOfQuests(npc);
                string neededLocation = cbLocation.Text;
                string location = (dialogs.NpcData.ContainsKey(npc)) ? (dialogs.NpcData[npc].location) : ("НЕТ ИМЕНИ");
                string rusname = (dialogs.NpcData.ContainsKey(npc)) ? (dialogs.NpcData[npc].rusName) : ("НЕ ПЕРЕВЕДЕН");
                string coord = (dialogs.NpcData.ContainsKey(npc)) ? (dialogs.NpcData[npc].coordinates) : ("НЕТ КООРДИНАТ");
                if( ( !checkDialog || (checkDialog && d_num < numDialogs.Value) )
                    && ( !checkQuest || (checkQuest && q_num < numQuests.Value) )
                    && ( !checkLocation || (checkLocation && location == neededLocation) ) )
                {
                    object[] row = { npc, d_num, q_num, location, coord, rusname };
                    dgvReview.Rows.Add( row );
                }
            }
            labelReviewOutputed.Text = "Выведено: " + dgvReview.RowCount.ToString();
        }
        //! Нажатие "Найти квесты", выводит несоответствия в квестах (не открыт, не закрыт, не существует)
        private void bFindQuest_Click(object sender, EventArgs e)
        {
            setQuestCheckEnvironment();
            dgvReview.Rows.Clear();

            foreach (CQuest quest in quests.quest.Values)
            {
                if (quest.Additional.IsSubQuest != 0)
                    continue;

                int questID = quest.QuestID;
                int open = 0, close = 0;
                foreach (string npc in dialogs.dialogs.Keys)
                    foreach (CDialog dialog in dialogs.dialogs[npc].Values)
                    { 
                        if (dialog.Actions.GetQuests.Contains(questID))
                            open = dialog.DialogID;
                        if (dialog.Actions.CompleteQuests.Contains(questID))
                            close = dialog.DialogID;
                    }
                if (open == 0 || close == 0)
                {
                    object[] row = { quest.Additional.Holder, questID, open, close };
                    dgvReview.Rows.Add(row);
                }
            }
            labelReviewOutputed.Text = "Выведено: " + dgvReview.RowCount.ToString();
        }
//***************************END OF DATA CHECK - missed dialogs, quests, NPC and so on**************************

        //! Поиск по номеру квеста в комбобоксе квеста и вывод информации
        private void QuestBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string text = QuestBox.Text;
                int id;
                if (int.TryParse(text, out id))     // вводят цифровой ID квеста
                {
                    CQuest quest = quests.getQuest(id);
                    if (quest != null)
                    {
                        CQuest temp = quest;
                        while (temp.Additional.IsSubQuest != 0)
                            temp = quests.getQuest(temp.Additional.IsSubQuest);

                        string qtext = temp.QuestID.ToString() + ": ";
                        qtext += temp.QuestInformation.Title;
                        NPCBox.SelectedValue = quest.Additional.Holder;
                        QuestBox.SelectedItem = qtext;
                    }
                }
                else    // вводят название квеста
                { 
                    int questNum = 0;
                    foreach (int questID in quests.quest.Keys)
                        if (quests.quest[questID].QuestInformation.Title == text)
                        {
                            questNum = questID;
                            break;
                        }
                    if (questNum != 0)
                    {
                        string qtext = questNum.ToString() + ": ";
                        qtext += quests.quest[questNum].QuestInformation.Title;
                        NPCBox.SelectedValue = quests.quest[questNum].Additional.Holder;
                        QuestBox.SelectedItem = qtext;
                    }
                }
            }
        }

        //! Тeстовая фукция "пробежать", пробегает всех NPC (для заполнения полей в тестовом режиме)
        private void bRunThroughNPC_Click(object sender, EventArgs e)
        {
            // копирование всех непереведенных частей из русского в английский
            // копирование диалогов
            for (int i = 0; i < NPCBox.Items.Count; i++)
            {
                NPCBox.SelectedIndex = i;

                // костыль для локализации                        
                string loc = settings.getCurrentLocale();
                bool exist = true;
                Dictionary<int, CDialog> Dialogs = this.dialogs.dialogs[currentNPC];
                try
                {
                    Dictionary<int, CDialog> local = this.dialogs.locales[loc][currentNPC];
                }
                catch
                {
                    exist = false;
                }
                foreach (CDialog dialog in Dialogs.Values)
                {
                    if (!exist || !this.dialogs.locales[loc][currentNPC].ContainsKey(dialog.DialogID))
                    {
                        if (!dialogs.locales[loc].ContainsKey(currentNPC))
                        {
                            CDialog toadd = new CDialog();
                            toadd = (CDialog)dialog.Clone();
                            toadd.version = 0;
                            Dictionary<int, CDialog> newdict = new Dictionary<int, CDialog>();
                            newdict.Add(toadd.DialogID, toadd);
                            dialogs.locales[loc].Add(currentNPC, newdict);
                        }

                        else if (!dialogs.locales[loc][currentNPC].ContainsKey(dialog.DialogID))
                        {
                            CDialog toadd = (CDialog)dialog.Clone();
                            toadd.version = 0;
                            dialogs.locales[loc][currentNPC].Add(toadd.DialogID, toadd);
                        }
                        //else
                        //    dialogs.locales[settings.getCurrentLocale()][currentNPC][dialog.DialogID].coordinates.RootDialog = true;
                        //result = dialog;                    
                    }
                }
            }
            // копирование квестов 
            foreach (CQuest quest in quests.quest.Values)
            {
                string loc = settings.getCurrentLocale();
                bool exist = true;                
                try
                {
                    CQuest local = this.quests.locales[loc][quest.QuestID];
                }
                catch
                {
                    exist = false;
                }
                if (!exist)
                {
                    CQuest toadd = new CQuest();
                    toadd = (CQuest) quest.Clone();
                    toadd.Version = 0;
                    quests.locales[loc].Add(toadd.QuestID, toadd);
                }
            }

        }
        private void bStartSearch_Click(object sender, EventArgs e)
        {
            int count = 0;
            string locale = settings.getCurrentLocale();
            string textToFind = tbPhraseToSearch.Text;
            string rusText = "";
            string engText = "";
            string npcName;
            bool found = false;
            dgvSearch.Rows.Clear();
            StringComparison compare = (cbIgnoreCase.Checked) ? (StringComparison.OrdinalIgnoreCase) : (StringComparison.Ordinal);
            foreach (CQuest quest in quests.quest.Values)
            {
                if (quest.QuestInformation.Title.IndexOf(textToFind, compare) != -1)
                {
                    rusText = quest.QuestInformation.Title;
                    engText = quests.locales[locale][quest.QuestID].QuestInformation.Title;
                    found = true;
                }
                else if (quest.QuestInformation.Description.IndexOf(textToFind, compare) != -1)
                {
                    rusText = quest.QuestInformation.Description;
                    engText = quests.locales[locale][quest.QuestID].QuestInformation.Description;
                    found = true;
                }
                if (found)
                {
                    npcName = quest.Additional.Holder;
                    count++;
                    object[] row = { "Квест", npcName, quest.QuestID, rusText, engText };
                    dgvSearch.Rows.Add(row);
                    found = false;
                }
            }

            found = false;
            foreach (string Npc in dialogs.dialogs.Keys)
                foreach (CDialog dialog in dialogs.dialogs[Npc].Values)
                {
                    if (dialog.Text.IndexOf(textToFind, compare) != -1)
                    {
                        rusText = dialog.Text;
                        engText = dialogs.locales[locale][Npc][dialog.DialogID].Text;
                        found = true;
                    }
                    else if (dialog.Title.IndexOf(textToFind, compare) != -1)
                    {
                        rusText = dialog.Title;
                        engText = dialogs.locales[locale][Npc][dialog.DialogID].Title;
                        found = true;
                    }
                    if (found)
                    {
                        count++;
                        object[] row = { "Диалог", Npc, dialog.DialogID, rusText, engText };
                        dgvSearch.Rows.Add(row);
                        found = false;
                    }
                }
            lSearchResult.Text = "Найдено: " + count.ToString();
        }

        private void dgvSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            int id = int.Parse(dgvSearch.Rows[index].Cells[2].Value.ToString());
            currentNPC = dgvSearch.Rows[index].Cells[1].Value.ToString();
            string type = dgvSearch.Rows[index].Cells[0].Value.ToString();
            if (type == "Квест")
            {
                if (settings.getMode() == settings.MODE_EDITOR)
                {
                    int mode = (quests.quest[id].Additional.IsSubQuest == 0) ? (2) : (3);
                    EditQuestForm editQuestForm = new EditQuestForm(this, id, mode);
                    editQuestForm.Visible = true;
                }
                else
                {
                    LocaleQuestForm editLocaleQuestform = new LocaleQuestForm(this, id);
                    editLocaleQuestform.Visible = true;
                }
            }
            else if (type == "Диалог")
            {
                if (settings.getMode() == settings.MODE_EDITOR)
                {
                    EditDialogForm editDialogForm = new EditDialogForm(false, this, id);
                    editDialogForm.Visible = true;
                }
                else
                {
                    LocaleDialogForm editLocaleDialogForm = new LocaleDialogForm(this, id);
                    editLocaleDialogForm.Visible = true;
                }

            }
        }
//***************************MAIN MENU ITEMS **************************
        //! Пункт главного меню - открытие папки с данными 
        private void ExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String path = System.IO.Path.GetDirectoryName(settings.getDialogsPath());
            System.Diagnostics.Process.Start(path);
        }
        //! Пункт главного меню - сохранение всех данных
        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveData();
        }
        //! Пункт главного меню - Настройки
        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperatorSettings fOperator = new OperatorSettings(this);
            this.Enabled = false;
            fOperator.Show();
        }
        //! Пункт главного меню - Синхронизация 
        private void SynchroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //синхронизация диалогов
            string loc = settings.getCurrentLocale();
            int added = 0;
            int copied = 0;
            foreach (string npc in dialogs.dialogs.Keys)
            {
                if (!dialogs.locales[loc].ContainsKey(npc))
                {
                    dialogs.locales[loc].Add(npc, new Dictionary<int, CDialog>());
                }
                foreach (int dialogID in dialogs.dialogs[npc].Keys)
                {
                    bool exist = true;
                    CDialog rus = dialogs.dialogs[npc][dialogID];
                    CDialog eng = new CDialog();
                    try
                    {
                        eng = dialogs.locales[loc][npc][dialogID];
                    }
                    catch
                    {
                        exist = false;
                    }

                    eng.Precondition = rus.Precondition;
                    eng.Nodes = rus.Nodes;
                    eng.Actions = rus.Actions;
                    eng.coordinates.Active = rus.coordinates.Active;

                    if (!exist)
                    {
                        eng.Holder = rus.Holder;
                        eng.coordinates = rus.coordinates;
                        eng.DialogID = rus.DialogID;
                        eng.Text = rus.Text;
                        eng.Title = rus.Title;
                        eng.version = 0;
                        dialogs.locales[loc][npc].Add(dialogID, eng);
                        added++;
                    }
                    else
                    {
                        dialogs.locales[loc][npc][dialogID] = eng;

                        // копирование русских строчек вместо пустых
                        if (rus.Title.Length > 0 && eng.Title.Length == 0)
                        {
                            eng.Title = rus.Title;
                            eng.version = rus.version - 1;
                            copied++;
                        }
                        if (rus.Text.Length > 0 && eng.Text.Length == 0)
                        {
                            eng.Text = rus.Text;
                            eng.version = rus.version - 1;
                            copied++;
                        }
                    }

                }
            }
            // удаление лишних диалогов
            int garb = 0;
            Dictionary<string, int> del = new Dictionary<string, int>();
            foreach (string npc in dialogs.locales[loc].Keys)
                foreach (int dialID in dialogs.locales[loc][npc].Keys)
                {
                    if (!dialogs.dialogs.ContainsKey(npc) && !del.ContainsKey(npc))
                    {
                        del.Add(npc, 0);
                        garb++;
                    }
                    else if (!dialogs.dialogs.ContainsKey(npc) && del.ContainsKey(npc))
                    {
                        garb++;
                        del[npc]++;
                    }
                    //else if (dialogs.dialogs.ContainsKey(npc) && !dialogs.dialogs[npc].ContainsKey(dialID))
                    //    del.Add(npc, dialID);
                }
            foreach (string npc in del.Keys)
                dialogs.locales[loc].Remove(npc);

            // синхронизация квестов            
            int empty = 0;
            int title = 0, desc = 0;
            foreach (CQuest quest in quests.quest.Values)
            {
                bool exist = true;
                CQuest local = new CQuest();
                try
                {
                    local = this.quests.locales[loc][quest.QuestID];
                }
                catch
                {
                    exist = false;
                }
                if (!exist)
                {
                    CQuest toadd = new CQuest();
                    toadd = (CQuest)quest.Clone();
                    toadd.Version = 0;
                    quests.locales[loc].Add(toadd.QuestID, toadd);
                    empty++;
                }
                else
                {
                    local.QuestInformation.Items = quest.QuestInformation.Items;
                    local.Additional.ShowProgress = quest.Additional.ShowProgress;
                    local.Additional.ListOfSubQuest = quest.Additional.ListOfSubQuest;
                    local.Additional.IsSubQuest = quest.Additional.IsSubQuest;
                    local.Precondition = quest.Precondition;
                    local.QuestPenalty = quest.QuestPenalty;
                    local.QuestRules = quest.QuestRules;
                    local.Reward = quest.Reward;
                    local.Target = quest.Target;
                    // копирование русских строчек вместо пустых
                    if (quest.QuestInformation.Title.Length > 0 && local.QuestInformation.Title.Length == 0)
                    {
                        local.QuestInformation.Title = quest.QuestInformation.Title;
                        local.Version = quest.Version - 1;
                        title++;
                    }
                    if (quest.QuestInformation.Description.Length > 0 && local.QuestInformation.Description.Length == 0)
                    {
                        local.QuestInformation.Description = quest.QuestInformation.Description;
                        local.Version = quest.Version - 1;
                        desc++;
                    }
                    if (quest.QuestInformation.onWin.Length > 0 && local.QuestInformation.onWin.Length == 0)
                    {
                        local.QuestInformation.onWin = quest.QuestInformation.onWin;
                        local.Version = quest.Version - 1;
                        title++;
                    }
                    if (quest.QuestInformation.onFailed.Length > 0 && local.QuestInformation.onFailed.Length == 0)
                    {
                        local.QuestInformation.onFailed = quest.QuestInformation.onFailed;
                        local.Version = quest.Version - 1;
                        desc++;
                    }
                }
            }


            // удаляем лишние квесты 
            List<int> trash = new List<int>();
            foreach (CQuest local in quests.locales[loc].Values)
            {
                if (!quests.quest.ContainsKey(local.QuestID))
                {
                    trash.Add(local.QuestID);
                    //if (local.QuestID <= 3500 && local.QuestID >= 2500)
                }
            }
            foreach (int shit in trash)
                quests.locales[loc].Remove(shit);

            string info = "Синхронизация исходных данных и переводов: \n";
            info += "Диалогов добавлено: " + added.ToString() + " Удалено: " + garb.ToString() + " Скопировано: " + copied.ToString() + "\n";
            info += "Квестов добавлено: " + empty.ToString() + " Удалено: " + trash.Count.ToString() + " Заголовков: " + title.ToString() + " Описаний: " + desc.ToString();
            MessageBox.Show(info, "Результаты синхронизации", MessageBoxButtons.OK, MessageBoxIcon.Information);

            /*
             * // дублированные ID диалогов
            Dictionary<int, int> shitty = new Dictionary<int, int>();
            Dictionary<int, int> dupl = new Dictionary<int, int>();
            foreach (string npc in dialogs.dialogs.Keys)
            {
                foreach (int dialogID in dialogs.dialogs[npc].Keys)
                {
                    if (!shitty.ContainsKey(dialogID))
                        shitty.Add(dialogID, 1);
                    else
                        shitty[dialogID]++;
                }
            }
            int stup = 0;
            foreach (int key in shitty.Keys)
                if (shitty[key] > 1)
                {
                    dupl.Add(key, shitty[key]);
                    stup += shitty[key];
                }
            labelXNode.Text = "dupl = " + dupl.Count.ToString() + ", total=" + stup.ToString();
             */
        }

        //! Пункт главного меню - Cтатистика
        private void StatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatisticsForm sf = new StatisticsForm(this, NPCBox.Items.Count, quests, dialogs);
            sf.Show();
        }
        //! Пункт главного меню - Выход
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.setLastNpcIndex(NPCBox.SelectedIndex);
            settings.saveSettings();
        }

    }
}
 