﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroSet_UI.Forms;
using Anda.Fluid.Domain.FluProgram;
using Anda.Fluid.Infrastructure.Common;
using Anda.Fluid.Domain.FluProgram.Grammar;
using Anda.Fluid.Domain.FluProgram.Common;
using Anda.Fluid.App.EditCmdLineForms;
using Anda.Fluid.Drive;
using Anda.Fluid.Infrastructure.Msg;
using Anda.Fluid.App.Common;
using Anda.Fluid.Infrastructure.Reflection;
using Anda.Fluid.Drive.HotKeys;
using Anda.Fluid.Drive.HotKeys.HotKeySort;

namespace Anda.Fluid.App.Metro.EditControls
{
    public partial class EditFinishShotMetro : MetroSetUserControl, IMsgSender,ICanSelectButton
    {
        private Pattern pattern;
        private PointD origin;
        private bool isCreating;
        private FinishShotCmdLine finishShotCmdLine;
        private FinishShotCmdLine finishShotCmdLineBackUp;
        /// <summary>
        /// 仅用于生成语言文本
        /// </summary>
        private EditFinishShotMetro()
        {
            InitializeComponent();
        }
        public EditFinishShotMetro(Pattern pattern) : this(pattern, null)
        {
        }

        public EditFinishShotMetro(Pattern pattern, FinishShotCmdLine finishShotCmdLine)
        {
            InitializeComponent();
            this.pattern = pattern;
            this.origin = pattern.GetOriginPos();
            if (finishShotCmdLine == null)
            {
                isCreating = true;
                this.finishShotCmdLine = new FinishShotCmdLine();
                this.finishShotCmdLine.Position.X = Properties.Settings.Default.DotX;
                this.finishShotCmdLine.Position.Y = Properties.Settings.Default.DotY;
                this.finishShotCmdLine.DotStyle = (DotStyle)Properties.Settings.Default.DotStyle;
                this.finishShotCmdLine.IsWeightControl = false;
                this.finishShotCmdLine.Weight = 0;
            }
            else
            {
                isCreating = false;
                this.finishShotCmdLine = finishShotCmdLine;
            }
            //系统坐标->机械坐标
            PointD dotMachine = this.pattern.MachineRel(this.finishShotCmdLine.Position);
            tbLocationX.Text = dotMachine.X.ToString("0.000");
            tbLocationY.Text = dotMachine.Y.ToString("0.000");
            for (int i = 0; i < 10; i++)
            {
                comboBoxDotType.Items.Add("Type " + (i + 1));
            }
            comboBoxDotType.SelectedIndex = (int)this.finishShotCmdLine.DotStyle;
            if (this.finishShotCmdLine != null)
            {
                this.finishShotCmdLineBackUp = (FinishShotCmdLine)this.finishShotCmdLine.Clone();
            }
            //this.ReadLanguageResources();
        }

        private void btnEditDotParams_Click(object sender, EventArgs e)
        {
            new EditDotParamsForm(FluidProgram.Current.ProgramSettings.DotParamList).ShowDialog();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            tbLocationX.Text = (Machine.Instance.Robot.PosX - origin.X).ToString("0.000");
            tbLocationY.Text = (Machine.Instance.Robot.PosY - origin.Y).ToString("0.000");
        }

        private void btnGoTo_Click(object sender, System.EventArgs e)
        {
            if (!tbLocationX.IsValid || !tbLocationY.IsValid)
            {
                return;
            }
            Machine.Instance.Robot.MoveSafeZ();
            //Machine.Instance.Robot.MovePosXY(origin.X + tbLocationX.Value, origin.Y + tbLocationY.Value);
            Machine.Instance.Robot.ManualMovePosXY(origin.X + tbLocationX.Value, origin.Y + tbLocationY.Value);
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (!tbLocationX.IsValid || !tbLocationY.IsValid)
            {
                //MessageBox.Show("Please input valid values.");
                MetroSetMessageBox.Show(this, "请输入正确的值");
                return;
            }
            //机械坐标->系统坐标
            PointD dotMap = this.pattern.SystemRel(tbLocationX.Value, tbLocationY.Value);
            finishShotCmdLine.Position.X = dotMap.X;
            finishShotCmdLine.Position.Y = dotMap.Y;
            finishShotCmdLine.DotStyle = (DotStyle)comboBoxDotType.SelectedIndex;
            finishShotCmdLine.IsWeightControl = false;
            finishShotCmdLine.Weight = 0;
            if (isCreating)
            {
                MsgCenter.Broadcast(Constants.MSG_FINISH_ADDING_CMD_LINE, this, finishShotCmdLine);
            }
            else
            {
                MsgCenter.Broadcast(Constants.MSG_FINISH_EDITING_CMD_LINE, this, finishShotCmdLine);
            }
            Properties.Settings.Default.DotX = finishShotCmdLine.Position.X;
            Properties.Settings.Default.DotY = finishShotCmdLine.Position.Y;
            Properties.Settings.Default.DotStyle = (int)finishShotCmdLine.DotStyle;
            if (this.isCreating)
            {
                return;
            }
            if (this.finishShotCmdLine != null && this.finishShotCmdLineBackUp != null)
            {
                CompareObj.CompareProperty(this.finishShotCmdLine, this.finishShotCmdLineBackUp, null, this.GetType().Name, true);
                CompareObj.CompareField(this.finishShotCmdLine, this.finishShotCmdLineBackUp, null, this.GetType().Name, true);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MsgCenter.Broadcast(MsgDef.MSG_PARAMPAGE_CLEAR, null);
        }

        public void SetSelectButtons()
        {
            List<Button> buttons = new List<Button>();
            buttons.Add(this.btnSelect);
            buttons.Add(this.btnOk);
            HookHotKeyMgr.Instance.GetSelectKey().SetButtons(buttons);
        }
    }
}
