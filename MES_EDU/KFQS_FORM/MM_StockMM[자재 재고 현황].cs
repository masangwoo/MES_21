using DC00_assm;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace KFQS_Form
{
    public partial class MM_StockMM : DC00_WinForm.BaseMDIChildForm
    {
        //그리드를 세팅할 수 있도록 도와주는 함수 클래스
        UltraGridUtil _GridUtil = new UltraGridUtil();
        //공장변수 입력
        //private sPlantCode = LoginInfo.

        public MM_StockMM()
        {
            InitializeComponent();
        }

        private void MM_StockMM_Load(object sender, EventArgs e)
        {
            //그리드를 셋팅한다
            try
            {
                _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
                _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "공장",      true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE",  "품목",      true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME",  "품목명",    true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "LOTNO",     "LOT번호",   true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WHCODE",    "창고",      true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "STOCKQTY",  "재고수량",  true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE",  "단위",      true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKER",     "생성자",    true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE",  "생성일시",  true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.SetInitUltraGridBind(grid1);//셋팅내역 그리드와 바인딩

                Common _Common = new Common();
                DataTable dtTemp = new DataTable();
                //PLANTCODE 기준정보 가져와서 데이터 테이블에 추가
                dtTemp = _Common.Standard_CODE("PLANTCODE");
                //데이터 테이블에 있는 데이터를 해당 콤보박스에 추가
                Common.FillComboboxMaster(this.cboPlantCode_H, dtTemp, dtTemp.Columns["CODE_ID"].ColumnName, dtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", dtTemp, "CODE_ID", "CODE_NAME");

                dtTemp = _Common.Standard_CODE("ITEMCODE");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "ITEMCODE", dtTemp, "CODE_ID", "CODE_NAME");

                dtTemp = _Common.Standard_CODE("ITEMNAME");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "ITEMNAME", dtTemp, "CODE_ID", "CODE_NAME");

                dtTemp = _Common.GET_ItemCodeFERT_Code("ROH");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "ITEMCODE", dtTemp, "CODE_ID", "CODE_NAME");


            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message, DC00_WinForm.DialogForm.DialogType.OK);
            }
        }

        public override void DoInquire()
        {
            base.DoInquire();
            DBHelper helper = new DBHelper(false);
            try
            {
                string sPlantcode = cboPlantCode_H.Value.ToString();
                string sItemcode  = txtItem_H.Text.ToString();
                string sItemname  = txtItemName_H.Text.ToString();
        

                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("10MM_StockMM_S1", CommandType.StoredProcedure
                                                        , helper.CreateParameter("PLANTCODE", sPlantcode,  DbType.String, ParameterDirection.Input)
                                                        , helper.CreateParameter("ITEMCODE",  sItemcode,   DbType.String, ParameterDirection.Input)
                                                        , helper.CreateParameter("ITEMNAME",  sItemname,   DbType.String, ParameterDirection.Input)
                                                        );

                this.ClosePrgForm();//
                if (dtTemp.Rows.Count > 0)
                {
                    grid1.DataSource = dtTemp;
                    grid1.DataBinds(dtTemp);
                }
                //데이터가 없으면 그리드 치워야한다
                else
                {
                    _GridUtil.Grid_Clear(grid1);
                    ShowDialog("조회할 데이터가 없습니다.", DC00_WinForm.DialogForm.DialogType.OK);
                }

            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message, DC00_WinForm.DialogForm.DialogType.OK);
            }
            finally
            {
                helper.Close();
            }
        }

        public override void DoNew()
        {
            base.DoNew();
            this.grid1.InsertRow();

            this.grid1.ActiveRow.Cells["PLANTCODE"].Value = LoginInfo.PlantCode;

            grid1.ActiveRow.Cells["ITEMCODE"].Activation      = Activation.NoEdit;
            grid1.ActiveRow.Cells["ITEMNAME"].Activation    = Activation.NoEdit;  
            grid1.ActiveRow.Cells["LOTNO"].Activation   = Activation.NoEdit;      
            grid1.ActiveRow.Cells["WHCODE"].Activation = Activation.NoEdit;       
            grid1.ActiveRow.Cells["STOCKQTY"].Activation = Activation.NoEdit;     
            grid1.ActiveRow.Cells["UNITCODE"].Activation = Activation.NoEdit;                                                                       
            grid1.ActiveRow.Cells["MAKER"].Activation    = Activation.NoEdit;     
            grid1.ActiveRow.Cells["MAKEDATE"].Activation = Activation.NoEdit;     

        }

        public override void DoDelete()
        {
            base.DoDelete();
            //입고된 내역이 있으면 삭제되면 안된다
            if (Convert.ToString(this.grid1.ActiveRow.Cells["CHK"].Value) == "1")
            {
                ShowDialog("입고된 발주 내역은 삭제할 수 없습니다.", DC00_WinForm.DialogForm.DialogType.OK);
                return;
            }
            this.grid1.DeleteRow();
        }

        public override void DoSave()
            {
            base.DoSave();
            DataTable dtTemp = new DataTable();
            dtTemp = grid1.chkChange();
            if (dtTemp == null) return;

            DBHelper helper = new DBHelper("", true);

            try
            {
                //해당 내역을 저장하시겠습니까?
                if (ShowDialog("해당 사항을 저장하시겠습니까?", DC00_WinForm.DialogForm.DialogType.YESNO) == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                foreach (DataRow drrow in dtTemp.Rows)
                {
                  
                    switch (drrow.RowState)
                    {
                        case DataRowState.Deleted:
                            drrow.RejectChanges();

                            helper.ExecuteNoneQuery("10MM_StockMM_D1", CommandType.StoredProcedure
                                                       , helper.CreateParameter("PLANTCODE", Convert.ToString(drrow["PLANTCODE"]), DbType.String, ParameterDirection.Input)
                                                       , helper.CreateParameter("PONO",      Convert.ToString(drrow["PONO"]),      DbType.String, ParameterDirection.Input)
                                                       );
                            ShowDialog( "정상적으로 삭제되었습니다.", DC00_WinForm.DialogForm.DialogType.OK);
                            break;

                        case DataRowState.Added:
                            string sErrorMsg = string.Empty;
                            if (Convert.ToString(drrow["ITEMCODE"]) == "")
                            {
                                sErrorMsg += "품목";
                            }

                            if (Convert.ToString(drrow["POQTY"]) == "")
                            {
                                sErrorMsg += "발주 수량";
                            }

                            if (Convert.ToString(drrow["CUSTCODE"]) == "")
                            {
                                sErrorMsg += "거래처";
                            }

                            if (sErrorMsg != "")
                            {
                                this.ClosePrgForm();
                                ShowDialog(sErrorMsg + "을(를) 입력하지 않았습니다.", DC00_WinForm.DialogForm.DialogType.OK);
                                helper.Rollback();
                                return;
                            }

                          

                            helper.ExecuteNoneQuery("10MM_StockMM_I1"
                                                    , CommandType.StoredProcedure
                                                    , helper.CreateParameter("PLANTCODE",  Convert.ToString(drrow["PLANTCODE"]),  DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("ITEMCODE",   Convert.ToString(drrow["ITEMCODE"]),   DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("POQTY",      Convert.ToString(drrow["POQTY"]),      DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("UNITCODE",   Convert.ToString(drrow["UNITCODE"]),   DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("CUSTCODE",   Convert.ToString(drrow["CUSTCODE"]),   DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("MAKER",      LoginInfo.UserID,                      DbType.String, ParameterDirection.Input)
                                                    //, helper.CreateParameter("SPCFLAG",      Convert.ToString(drRow["SPCFLAG"]),      DbType.String, ParameterDirection.Input)
                                                    //, helper.CreateParameter("PATROLFLAG",      Convert.ToString(drRow["PATROLFLAG"]),      DbType.String, ParameterDirection.Input)
                                                    );
                            break;
                        case DataRowState.Modified:
                            helper.ExecuteNoneQuery("10MM_StockMM_U1"
                                                   , CommandType.StoredProcedure
                                                   , helper.CreateParameter("PLANTCODE",  Convert.ToString(drrow["PLANTCODE"]),  DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("PONO",       Convert.ToString(drrow["PONO"]),       DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("INQTY",      Convert.ToString(drrow["INQTY"]),      DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("EDITOR",     Convert.ToString(drrow["EDITOR"]),     DbType.String, ParameterDirection.Input)
                                                   //, helper.CreateParameter("DEPTCODE",   Convert.ToString(drrow["DEPTCODE"]),   DbType.String, ParameterDirection.Input)
                                                   //, helper.CreateParameter("PDALOGINFLAG", Convert.ToString(drRow["PDALOGINFLAG"]), DbType.String, ParameterDirection.Input)
                                                 
                                                   //, helper.CreateParameter("SPCFLAG",      Convert.ToString(drRow["SPCFLAG"]),      DbType.String, ParameterDirection.Input)
                                                   //, helper.CreateParameter("PATROLFLAG",      Convert.ToString(drRow["PATROLFLAG"]),      DbType.String, ParameterDirection.Input)
                                                   );
                            break;
                    }
                }
                if(helper.RSCODE=="S")
                { 
                    helper.Commit();
                    this.ShowDialog("정상적으로 등록되었습니다.", DC00_WinForm.DialogForm.DialogType.OK);
                    DoInquire();
                }
            }
            catch (Exception ex)
            {
                helper.Rollback();
            }
            finally
            {
                helper.Close();
            }
        }
    }
}
