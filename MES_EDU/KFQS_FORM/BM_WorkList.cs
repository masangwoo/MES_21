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
    public partial class BM_WorkList : DC00_WinForm.BaseMDIChildForm
    {
        //그리드를 세팅할 수 있도록 도와주는 함수 클래스
        UltraGridUtil _GridUtil = new UltraGridUtil();
        //공장변수 입력
        //private sPlantCode = LoginInfo.

        public BM_WorkList()
        {
            InitializeComponent();
        }

        private void BM_WorkList_Load(object sender, EventArgs e)
        {
            //그리드를 셋팅한다
            try
            {
                _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
                _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE",  "공장",      true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "WORKERID",   "작업자ID",  true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "WORKERNAME", "작업자 명", true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "BANCODE",    "작업반",    true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "GRPID",      "그룹",      true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "DEPTCODE",   "부서",      true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "PHONE",      "연락처",    true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "INDATE",     "입사일",    true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "OUTDATE",    "퇴사일",    true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "USEFLAG",    "사용여부",  true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE",   "등록일시",  true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKER",      "등록자",    true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "EDITDATE",   "수정일시",  true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "EDITOR",     "수정자" ,   true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.SetInitUltraGridBind(grid1);//셋팅내역 그리드와 바인딩

                Common _Common = new Common();
                DataTable dtTemp = new DataTable();
                //PLANTCODE 기준정보 가져와서 데이터 테이블에 추가
                dtTemp = _Common.Standard_CODE("PLANTCODE");
                //데이터 테이블에 있는 데이터를 해당 콤보박스에 추가
                Common.FillComboboxMaster(this.cboPlantCode_H, dtTemp, dtTemp.Columns["CODE_ID"].ColumnName, dtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", dtTemp, "CODE_ID", "CODE_NAME");

                dtTemp = _Common.Standard_CODE("BANCODE");
                //데이터 테이블에 있는 데이터를 해당 콤보박스에 추가
                Common.FillComboboxMaster(this.cboBanCode_H, dtTemp, dtTemp.Columns["CODE_ID"].ColumnName, dtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "BANCODE", dtTemp, "CODE_ID", "CODE_NAME");


                dtTemp = _Common.Standard_CODE("USEFLAG");
                //데이터 테이블에 있는 데이터를 해당 콤보박스에 추가
                Common.FillComboboxMaster(this.cboUseFlag_H, dtTemp, dtTemp.Columns["CODE_ID"].ColumnName, dtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "USEFLAG", dtTemp, "CODE_ID", "CODE_NAME");

                //부서
                dtTemp = _Common.Standard_CODE("DEPTCODE");
                //데이터 테이블에 있는 데이터를 해당 콤보박스에 추가
                UltraGridUtil.SetComboUltraGrid(this.grid1, "DEPTCODE", dtTemp, "CODE_ID", "CODE_NAME");

                //그룹
                dtTemp = _Common.Standard_CODE("GRPID");
                //데이터 테이블에 있는 데이터를 해당 콤보박스에 추가
                UltraGridUtil.SetComboUltraGrid(this.grid1, "GRPID", dtTemp, "CODE_ID", "CODE_NAME");


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
                string sPlantcode =  cboPlantCode_H.Value.ToString();
                string sWorkerid =   txtWorkerID_H.Text.ToString();
                string sWorkerName = txtWorkerName_H.Text.ToString();
                string sBanCode =    cboBanCode_H.Value.ToString();
                string sUseflag =    cboUseFlag_H.Value.ToString();

                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("10BM_WorkList_S1", CommandType.StoredProcedure
                                                        , helper.CreateParameter("PLANTCODE",  sPlantcode,  DbType.String, ParameterDirection.Input)
                                                        , helper.CreateParameter("WORKERID",   sWorkerid,   DbType.String, ParameterDirection.Input)
                                                        , helper.CreateParameter("WORKERNAME", sWorkerName, DbType.String, ParameterDirection.Input)
                                                        , helper.CreateParameter("BANCODE",    sBanCode,    DbType.String, ParameterDirection.Input)
                                                        , helper.CreateParameter("USEFLAG",    sUseflag,    DbType.String, ParameterDirection.Input)
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

            this.grid1.ActiveRow.Cells["PLANTCODE"].Value = "1000";
            this.grid1.ActiveRow.Cells["GRPID"].Value = "SW";
            this.grid1.ActiveRow.Cells["USEFLAG"].Value = "Y";
            this.grid1.ActiveRow.Cells["INDATE"].Value = DateTime.Now.ToString("{ 0:yyyy - MM - 01}");

            grid1.ActiveRow.Cells["MAKER"].Activation = Activation.NoEdit;
            grid1.ActiveRow.Cells["MAKEDATE"].Activation = Activation.NoEdit;
            grid1.ActiveRow.Cells["EDITDATE"].Activation = Activation.NoEdit;
            grid1.ActiveRow.Cells["EDITOR"].Activation = Activation.NoEdit;
        }

        public override void DoDelete()
        {
            base.DoDelete();
            this.grid1.DeleteRow();
        }

        public override void DoSave()
            {
            base.DoSave();
            DataTable dtTemp = new DataTable();
            dtTemp = grid1.chkChange();
            if (dtTemp.Rows.Count == 0) return;

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

                            helper.ExecuteNoneQuery("10BM_WorkList_D1", CommandType.StoredProcedure
                                                       , helper.CreateParameter("PLANTCODE", Convert.ToString(drrow["PLANTCODE"]), DbType.String, ParameterDirection.Input)
                                                       , helper.CreateParameter("WORKERID",  Convert.ToString(drrow["WORKERID"]), DbType.String, ParameterDirection.Input)
                                                       );
                            break;
                        case DataRowState.Added:

                            if (Convert.ToString(drrow["WORKERID"]) == string.Empty)
                            {
                                this.ClosePrgForm();
                                this.ShowDialog("작업자 ID를 입력하세요.", DC00_WinForm.DialogForm.DialogType.OK);
                                return;
                            }

                            helper.ExecuteNoneQuery("10BM_WorkList_I1"
                                                    , CommandType.StoredProcedure
                                                    , helper.CreateParameter("PLANTCODE",  Convert.ToString(drrow["PLANTCODE"]),  DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("WORKERID",   Convert.ToString(drrow["WORKERID"]),   DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("WORKERNAME", Convert.ToString(drrow["WORKERNAME"]), DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("GRPID",      Convert.ToString(drrow["GRPID"]),      DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("DEPTCODE",   Convert.ToString(drrow["DEPTCODE"]),   DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("BANCODE",    Convert.ToString(drrow["BANCODE"]),    DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("USEFLAG",    Convert.ToString(drrow["USEFLAG"]),    DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("PHONENO",    Convert.ToString(drrow["PHONENO"]),    DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("INDATE",     Convert.ToString(drrow["INDATE"]),     DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("OUTDATE",    Convert.ToString(drrow["OUTDATE"]),    DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("MAKER",      LoginInfo.UserID,                      DbType.String, ParameterDirection.Input)
                                                    //, helper.CreateParameter("SPCFLAG",      Convert.ToString(drRow["SPCFLAG"]),      DbType.String, ParameterDirection.Input)
                                                    //, helper.CreateParameter("PATROLFLAG",      Convert.ToString(drRow["PATROLFLAG"]),      DbType.String, ParameterDirection.Input)
                                                    );
                            break;
                        case DataRowState.Modified:
                            helper.ExecuteNoneQuery("10BM_WorkList_U1"
                                                   , CommandType.StoredProcedure
                                                   , helper.CreateParameter("PLANTCODE",  Convert.ToString(drrow["PLANTCODE"]),  DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("WORKERID",   Convert.ToString(drrow["WORKERID"]),   DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("WORKERNAME", Convert.ToString(drrow["WORKERNAME"]), DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("GRPID",      Convert.ToString(drrow["GRPID"]),      DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("DEPTCODE",   Convert.ToString(drrow["DEPTCODE"]),   DbType.String, ParameterDirection.Input)
                                                   //, helper.CreateParameter("PDALOGINFLAG", Convert.ToString(drRow["PDALOGINFLAG"]), DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("BANCODE",    Convert.ToString(drrow["BANCODE"]),    DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("USEFLAG",    Convert.ToString(drrow["USEFLAG"]),    DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("PHONENO",    Convert.ToString(drrow["PHONENO"]),    DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("INDATE",     Convert.ToString(drrow["INDATE"]),     DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("OUTDATE",    Convert.ToString(drrow["OUTDATE"]),    DbType.String, ParameterDirection.Input)
                                                   , helper.CreateParameter("MAKER",      LoginInfo.UserID,                      DbType.String, ParameterDirection.Input)
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
