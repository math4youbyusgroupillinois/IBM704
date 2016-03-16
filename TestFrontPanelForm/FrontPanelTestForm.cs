using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IBM704.FrontPanel;

namespace TestFrontPanelForm
{
    public partial class FrontPanelTestForm : Form
    {
        #region Declarations

        private FrontPanel _frontPanel;
        private FrontPanelInterface _physicalPanel;
        private Boolean _locked = false;

        #endregion

        public FrontPanelTestForm()
        {
            InitializeComponent();

            btn_LoadTape.Enabled = false;
            btn_MultipleStep.Enabled = false;
            btn_SingleStep.Enabled = false;
            btn_Reset.Enabled = false;
            btn_SingleStep.Enabled = false;
            btn_Start.Enabled = false;
            btn_Clear.Enabled = false;
            btn_DisplayA.Enabled = false;
            btn_DisplayB.Enabled = false;
            btn_DisplayC.Enabled = false;
            btn_DisplayEffAddress.Enabled = false;
            btn_DisplayStorage.Enabled = false;
            btn_EnterInstr.Enabled = false;
            btn_EnterMQ.Enabled = false;
            btn_LoadDrum.Enabled = false;

            btn_EnablePhysical.Enabled = false;
            btn_DisablePhysical.Enabled = true;

            txtbx_InputOctal.Enabled = false;

            _frontPanel = new FrontPanel();
            _physicalPanel = new FrontPanelInterface();

            UpdateDisplay();

            timerCheckKeys.Start();
        }
        
        #region Display Control

        private void UpdateDisplay()
        {
            //Update Physical Display
            _physicalPanel.SetLights(_frontPanel);

            //Update Register Display
            lbl_AccDisplay.Text = _frontPanel.AccumDisplay;
            lbl_ICDisplay.Text = _frontPanel.InstructionCounterDisplay;
            lbl_IndexDisplay.Text = _frontPanel.IndexRegisterDisplay;
            lbl_InstructionDisplay.Text = _frontPanel.InstructionRegisterDisplay;
            lbl_MQDisplay.Text = _frontPanel.MQRegisterDisplay;
            lbl_StorageDisp.Text = _frontPanel.StorageRegisterDisplay;
            
            

            //Update Indicators
            UpdateIndicators(btn_ProgStopLt, _frontPanel.ProgramStopLight);
            UpdateIndicators(btn_PowerLt, _frontPanel.PowerLight);
            UpdateIndicators(btn_AutomaticLt, _frontPanel.AutomaticLight);
            UpdateIndicators(btn_AccOverflowLt, _frontPanel.AccumulatorOverflowLight);
            UpdateIndicators(btn_divideCheckLt, _frontPanel.DivideCheckLight);
            //UpdateIndicators(btn_RWCheckLt, _frontPanel.ReadWriteCheckLight);
            //UpdateIndicators(btn_RWSelectLt, _frontPanel.ReadWriteSelectLight);

            
            //rbtn_TapeCheckLt.Checked = _frontPanel.TapeCheckLight;
            rbtn_Trap.Checked = _frontPanel.TrapIndicator;
            txtbx_InputBinary.Text = _frontPanel.KeyedInWord;

            if (_frontPanel.IsAutomatic)
                chkbx_AutoMan.Checked = true;
            else
                chkbx_AutoMan.Checked = false;

        }

        private void UpdateIndicators(Button button, bool on)
        {
            if (on)
                button.BackColor = Color.Red;
            else
                button.BackColor = Color.Empty;
        }

        #endregion

        #region Button Actions

        private void chkbx_AutoMan_CheckedChanged(object sender, EventArgs e)
        {
            if( chkbx_AutoMan.Checked )
                _frontPanel.ChangeToAutomatic();
            else 
                _frontPanel.ChangeToManual();
            
            UpdateDisplay();
        }

        private void btn_DisplayA_Click(object sender, EventArgs e)
        {
            _frontPanel.SetDisplayIndex(IndexDisplay.IndexA);

            UpdateDisplay();
        }

        private void btn_DisplayB_Click(object sender, EventArgs e)
        {
            _frontPanel.SetDisplayIndex(IndexDisplay.IndexB);
            UpdateDisplay();
        }

        private void btn_DisplayC_Click(object sender, EventArgs e)
        {
            _frontPanel.SetDisplayIndex(IndexDisplay.IndexC);
            UpdateDisplay();
        }

        private void btn_MultipleStep_Click(object sender, EventArgs e)
        {
            _frontPanel.MultiStep();
            UpdateDisplay();
        }

        private void btn_SingleStep_Click(object sender, EventArgs e)
        {
            _frontPanel.SingleStep();
            UpdateDisplay();
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            _frontPanel.Reset();
            UpdateDisplay();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            _frontPanel.Start();
            UpdateDisplay();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            _frontPanel.Clear();
            UpdateDisplay();
        }

        private void btn_EnterMQ_Click(object sender, EventArgs e)
        {
            _frontPanel.KeyedInWordOctal = txtbx_InputOctal.Text;
            txtbx_InputBinary.Text = _frontPanel.KeyedInWord;

            UpdateDisplay();
        }

        private void btn_EnterInstr_Click(object sender, EventArgs e)
        {
            _frontPanel.KeyedInWordOctal = txtbx_InputOctal.Text;
            txtbx_InputBinary.Text = _frontPanel.KeyedInWord;

            _frontPanel.EnterInstruction();

            UpdateDisplay();
        }

        private void btn_DisplayStorage_Click(object sender, EventArgs e)
        {
            _frontPanel.KeyedInWordOctal = txtbx_InputOctal.Text;
            txtbx_InputBinary.Text = _frontPanel.KeyedInWord;

            _frontPanel.DisplayStorage();
            UpdateDisplay();
        }

        private void btn_DisplayEffAddress_Click(object sender, EventArgs e)
        {
            _frontPanel.DisplayEffectiveAddress();
            UpdateDisplay();
        }

        private void btn_LoadTape_Click(object sender, EventArgs e)
        {
            _frontPanel.LoadTape();
            UpdateDisplay();
        }

        private void btn_LoadDrum_Click(object sender, EventArgs e)
        {
            _frontPanel.LoadDrum();
            UpdateDisplay();
        }

        private void btn_DisablePhysical_Click(object sender, EventArgs e)
        {
            timerCheckKeys.Stop();

            //Enable Keys
            btn_LoadTape.Enabled = true;
            btn_MultipleStep.Enabled = true;
            btn_SingleStep.Enabled = true;
            btn_Reset.Enabled = true;
            btn_SingleStep.Enabled = true;
            btn_Start.Enabled = true;
            btn_Clear.Enabled = true;
            btn_DisplayA.Enabled = true;
            btn_DisplayB.Enabled = true;
            btn_DisplayC.Enabled = true;
            btn_DisplayEffAddress.Enabled = true;
            btn_DisplayStorage.Enabled = true;
            btn_EnterInstr.Enabled = true;
            btn_EnterMQ.Enabled = true;
            btn_LoadDrum.Enabled = true;

            txtbx_InputOctal.Enabled = true;

            btn_EnablePhysical.Enabled = true;
            btn_DisablePhysical.Enabled = false;
        }

        private void btn_EnablePhysical_Click(object sender, EventArgs e)
        {
            btn_LoadTape.Enabled = false;
            btn_MultipleStep.Enabled = false;
            btn_SingleStep.Enabled = false;
            btn_Reset.Enabled = false;
            btn_SingleStep.Enabled = false;
            btn_Start.Enabled = false;
            btn_Clear.Enabled = false;
            btn_DisplayA.Enabled = false;
            btn_DisplayB.Enabled = false;
            btn_DisplayC.Enabled = false;
            btn_DisplayEffAddress.Enabled = false;
            btn_DisplayStorage.Enabled = false;
            btn_EnterInstr.Enabled = false;
            btn_EnterMQ.Enabled = false;
            btn_LoadDrum.Enabled = false;

            btn_EnablePhysical.Enabled = false;
            btn_DisablePhysical.Enabled = true;

            txtbx_InputOctal.Enabled = false;

            timerCheckKeys.Start();
        }

        #endregion

        private void timerCheckKeys_Tick(object sender, EventArgs e)
        {
            // This lock will be set if the program is currently trying to update the display and grab controls.
            if(_locked)
                return;

            _locked = true;

            _physicalPanel.ReadKeys(ref _frontPanel);
            UpdateDisplay();

            _locked = false;
        }
    }
}