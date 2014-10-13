using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;

namespace BISTel.eSPC.Common
{
    /// <summary>
    /// APCChecker에 대한 요약 설명입니다.
    /// </summary>
    public class Common
    {
        public Common()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
        }

        #region : check method

        /// <summary>
        /// Name     : Check_string
        /// Contents : 문자만 허용 (허용=true/불허=false)
        /// Author   : VMS Product Team, Jiny
        /// Create   : Jiny(2008.08.25)
        /// Modify   : 
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>허용=true/불허=false</returns>
        public bool Check_string(String text)
        {
            Char[] itemlist = text.ToCharArray();

            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"[A-Za-z]");

            foreach (Char item in itemlist)
            {
                if (!rg.IsMatch(item.ToString()) && item.ToString() != "_")
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Name     : Check_Numeric
        /// Contents : 숫자만 허용 (허용=true/불허=false)
        /// Author   : VMS Product Team, Jiny
        /// Create   : Jiny(2008.08.25)
        /// Modify   : Danny(2009.05.22)
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>허용=true/불허=false</returns>
        public bool Check_Numeric(string strIn)
        {
            try
            {
                double d = 0.0;

                return double.TryParse(strIn, out d);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Name     : Check_VMS_charactor
        /// Contents : VMS에서 허용하는 문자입력 값 Check (알파벳, 숫자,_ ,;,:만 허용)
        /// Author   : VMS Product Team, Jiny
        /// Create   : Jiny(2008.08.13)
        /// Modify   : 
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>허용=true/불허=false</returns>
        public bool Check_charactor(String text)
        {

            Char[] itemlist = text.ToCharArray();

            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"[a-zA-Z0-9_;:,=.-]");

            foreach (Char item in itemlist)
            {

                if (!rg.IsMatch(item.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Name     : Check_VMS_charactor
        /// Contents : VMS에서 허용하는 Naming문자입력 값 Check (알파벳, 숫자,_ 만 허용)
        /// Author   : VMS Product Team, Jiny
        /// Create   : Jiny(2008.10.08)
        /// Modify   : 
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>허용=true/불허=false</returns>
        public bool Check_Naming_charactor(String text)
        {
            Char[] itemlist = text.ToCharArray();

            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"[a-zA-Z0-9_]");

            foreach (Char item in itemlist)
            {
                if (!rg.IsMatch(item.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Name     : Check_VMS_charactor
        /// Contents : VMS에서 허용하는 Char (알파벳, 숫자,!@#$%^*()-_';:,.=+{}[]/?| 만 허용)
        /// Author   : VMS Product Team, Jiny
        /// Create   : Jiny(2008.10.08)
        /// Modify   : 
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>허용=true/불허=false</returns>
        public bool Check_Xml_charactor(String text)
        {
            Char[] itemlist = text.ToCharArray();

            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"[a-zA-Z0-9,!@#$%^*()_\-';:.=+{}[]/?|]");

            foreach (Char item in itemlist)
            {

                if (!rg.IsMatch(item.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// search후 check method
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="thispage"></param>
        /// <param name="isShowMsg"></param>
        /// <returns></returns>
        public bool CheckSearchMessage(DataSet ds, BasePageUCtrl page, bool isShowMsg)
        {
            if (DSUtil.GetResultSucceed(ds) == Definition.FAIL)
            {
                page.MsgClose();

                if (isShowMsg && DSUtil.GetResultMsg(ds) != "")
                    MSGHandler.DisplayMessage(MSGType.Error, DSUtil.GetResultMsg(ds));

                return false;
            }
            else
            {
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    page.MsgClose();

                    if (isShowMsg)
                        MSGHandler.DisplayMessage(MSGType.Information, "GENERAL_NO_DATA", null, null);
                }

                return true;
            }
        }

        public bool CheckSearchMessage(DataSet ds, BasePopupFrm page, bool isShowMsg)
        {
            if (DSUtil.GetResultSucceed(ds) == Definition.FAIL)
            {
                page.MsgClose();

                if (isShowMsg && DSUtil.GetResultMsg(ds) != "")
                    MSGHandler.DisplayMessage(MSGType.Error, DSUtil.GetResultMsg(ds));

                return false;
            }
            else
            {
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    page.MsgClose();

                    if (isShowMsg)
                        MSGHandler.DisplayMessage(MSGType.Information, "GENERAL_NO_DATA", null, null);
                }

                return true;
            }
        }

        public bool CheckSaveMessage(DataSet ds, BasePageUCtrl page, bool isShowMsg)
        {
            if (DSUtil.GetResultSucceed(ds) == Definition.FAIL)
            {
                page.MsgClose();

                if (isShowMsg && DSUtil.GetResultMsg(ds) != "")
                    MSGHandler.DisplayMessage(MSGType.Error, DSUtil.GetResultMsg(ds));
                else if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Warning, "GENERAL_SAVE_FAIL", null, null);

                return false;
            }
            else
            {
                page.MsgClose();

                if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Information, "GENERAL_SAVE_SUCCESS", null, null);

                return true;
            }
        }

        public bool CheckSaveMessage(DataSet ds, BasePopupFrm page, bool isShowMsg)
        {
            if (DSUtil.GetResultSucceed(ds) == Definition.FAIL)
            {
                page.MsgClose();

                if (isShowMsg && DSUtil.GetResultMsg(ds) != "")
                    MSGHandler.DisplayMessage(MSGType.Error, DSUtil.GetResultMsg(ds));
                else if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Warning, "GENERAL_SAVE_FAIL", null, null);

                return false;
            }
            else
            {
                page.MsgClose();

                if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Information, "GENERAL_SAVE_SUCCESS", null, null);

                return true;
            }
        }

        // TODO : Added by Danny [2009.05.13]
        public bool CheckSaveMessage(DataSet ds, bool isShowMsg)
        {
            if (DSUtil.GetResultSucceed(ds) == Definition.FAIL)
            {
                if (isShowMsg && DSUtil.GetResultMsg(ds) != "")
                    MSGHandler.DisplayMessage(MSGType.Error, DSUtil.GetResultMsg(ds));
                else if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Warning, "GENERAL_SAVE_FAIL", null, null);

                return false;
            }
            else
            {
                if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Information, "GENERAL_SAVE_SUCCESS", null, null);

                return true;
            }
        }

        public bool CheckRemoveMessage(DataSet ds, BasePageUCtrl page, string item, bool isShowMsg)
        {
            if (DSUtil.GetResultSucceed(ds) == Definition.FAIL)
            {
                page.MsgClose();

                if (isShowMsg && DSUtil.GetResultMsg(ds) != "")
                    MSGHandler.DisplayMessage(MSGType.Warning, "VMS_DELETE_FAIL_USEDATA", new string[] { item }, null);

                return false;
            }
            else
            {
                page.MsgClose();

                if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Information, "VMS_DELETE_SUCCESS", null, null);

                return true;
            }
        }

        public bool CheckRemoveMessage(DataSet ds, BasePopupFrm page, string item, bool isShowMsg)
        {
            if (DSUtil.GetResultSucceed(ds) == Definition.FAIL)
            {
                page.MsgClose();

                if (isShowMsg && DSUtil.GetResultMsg(ds) != "")
                    MSGHandler.DisplayMessage(MSGType.Warning, "VMS_DELETE_FAIL_USEDATA", new string[] { item }, null);

                return false;
            }
            else
            {
                page.MsgClose();

                if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Information, "VMS_DELETE_SUCCESS", null, null);

                return true;
            }
        }

        /// <summary>
        /// SAVE전 CHECK
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="thispage"></param>
        /// <param name="isShowMsg"></param>
        /// <returns></returns>
        public bool CheckSaveData(DataSet ds, BasePageUCtrl page, bool isShowMsg)
        {
            bool bCheck = false;

            if (ds.Tables.Count > 0 && (ds.Tables.Contains("INSERT") || ds.Tables.Contains("UPDATE")))
            {
                bCheck = true;
            }
            else if (ds.Tables.Contains("DELETE"))
            {
                bCheck = true;
            }

            if (bCheck)
            {
                return true;
            }
            else
            {
                page.MsgClose();

                if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Information, "GENERAL_NO_SAVE_DATA", null, null);

                return false;
            }
        }

        public bool CheckSaveData(DataSet ds, BasePopupFrm page, bool isShowMsg)
        {
            bool bCheck = false;

            if (ds.Tables.Count > 0 && (ds.Tables.Contains("INSERT") || ds.Tables.Contains("UPDATE")))
            {
                bCheck = true;
            }
            else if (ds.Tables.Contains("DELETE"))
            {
                bCheck = true;
            }

            if (bCheck)
            {
                return true;
            }
            else
            {
                page.MsgClose();

                if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Information, "GENERAL_NO_SAVE_DATA", null, null);

                return false;
            }
        }

        public bool CheckDeleteData(DataSet ds, BasePageUCtrl page, bool isShowMsg)
        {
            bool bCheck = false;

            if (ds.Tables.Contains("DELETE"))
            {
                bCheck = true;
            }

            if (bCheck)
            {
                return true;
            }
            else
            {
                page.MsgClose();

                if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Information, "VMS_NO_DELETE_DATA", null, null);

                return false;
            }
        }

        public bool CheckDeleteData(DataSet ds, BasePopupFrm page, bool isShowMsg)
        {
            bool bCheck = false;

            if (ds.Tables.Contains("DELETE"))
            {
                bCheck = true;
            }

            if (bCheck)
            {
                return true;
            }
            else
            {
                page.MsgClose();

                if (isShowMsg)
                    MSGHandler.DisplayMessage(MSGType.Information, "VMS_NO_DELETE_DATA", null, null);

                return false;
            }
        }

        #endregion

        #region :Member Fields

        private char[] TokenKeys = { '↑', '↕', '↔', '↖', '↓', '‡', '§', '√', '￠', '¤', '￥', '￡', '¿', '(', ')', '[', ']', '*', '/', '-', '+', '%', '^', '`', '!' };

        #endregion

        #region :Calculation

        public bool Verify(string formulaDefinition, string strErrorMSG)
        {
            string buffNumChar = "";		//숫자를 버퍼링할 버퍼
            string buffDataChar = "";		//parameter를 버퍼링할 버퍼
            string buffOperChar = "";		//operation 을 버퍼링할 버퍼

            bool bSuccess = false;	//전체 Success Flag. bError와 반대
            bool bError = false;	//Validation Error Flag
            bool isParameter = false;	//현재 읽는 데이터가 [] 안에 오는 parameter인지 나타내는 flag
            char currChar;		//현재 문자			

            int currIdx = 0;		//현재 문자 위치
            int cntLBrackets = 0;		//'(' 갯수
            int cntRBrackets = 0;		//')' 갯수
            int totalLength = 0;		//문자열 총 길이. 나중에 모든 문자를 다 검증했는지에 대해 formulaDefinition와 길이비교.

            try
            {
                try
                {
                    //FillDataList(das, this.m_ProductId, this.m_Route, this.m_ProcessArea);		//validation list 채우기
                    for (currIdx = 0; currIdx < formulaDefinition.Length; currIdx++)	//문장 전체를 한번 돌아감
                    {
                        if (bError == true)
                            break;

                        currChar = formulaDefinition[currIdx];

                        if (isParameter == false)	//parameter data가 아닐경우 숫자, (), operation을 구분해냄
                        {
                            switch (currChar)
                            {
                                //숫자
                                //앞에 올 수 있는건 '(', 2항 연산자. 단, 맨 처음일 경우 가능.
                                //'~'는 올 수 없음.
                                case '1':
                                case '2':
                                case '3':
                                case '4':
                                case '5':
                                case '6':
                                case '7':
                                case '8':
                                case '9':
                                case '0':
                                case '.':
                                    if (totalLength != 0 && buffNumChar.Length == 0)
                                        bError = !checkSingleOper(ref buffNumChar, ref buffOperChar, ref buffDataChar, currChar, ref strErrorMSG);
                                    buffNumChar += currChar;
                                    totalLength++;
                                    if (currIdx + 1 == formulaDefinition.Length)	//마지막이 숫자로 끝날경우 숫자 validation
                                    {
                                        if (IsValidNum(buffNumChar))
                                            buffNumChar = "";
                                        else
                                        {
                                            bError = true;
                                            strErrorMSG = "Wrong Number (" + buffNumChar + ")";
                                        }
                                    }
                                    break;
                                //2006.05.04 chandler 영문처리 추가
                                case 'A':
                                case 'B':
                                case 'C':
                                case 'D':
                                case 'E':
                                case 'F':
                                case 'G':
                                case 'H':
                                case 'I':
                                case 'J':
                                case 'K':
                                case 'L':
                                case 'M':
                                case 'N':
                                case 'O':
                                case 'P':
                                case 'Q':
                                case 'R':
                                case 'S':
                                case 'T':
                                case 'U':
                                case 'V':
                                case 'W':
                                case 'X':
                                case 'Y':
                                case 'Z':
                                case 'a':
                                case 'b':
                                case 'c':
                                case 'd':
                                case 'e':
                                case 'f':
                                case 'g':
                                case 'h':
                                case 'i':
                                case 'j':
                                case 'k':
                                case 'l':
                                case 'm':
                                case 'n':
                                case 'o':
                                case 'p':
                                case 'q':
                                case 'r':
                                case 's':
                                case 't':
                                case 'u':
                                case 'v':
                                case 'w':
                                case 'x':
                                case 'y':
                                case 'z':
                                case '`':
                                case '!':
                                    if (totalLength != 0 && buffNumChar.Length == 0)
                                        bError = !checkSingleOper(ref buffNumChar, ref buffOperChar, ref buffDataChar, currChar, ref strErrorMSG);
                                    buffNumChar += currChar;
                                    totalLength++;
                                    if (currIdx + 1 == formulaDefinition.Length) buffNumChar = "";
                                    break;

                                //'(' 괄호
                                //앞에 올 수 있는건 '(', 2항 연산자, 단항 연산자.  단, 맨 처음일 경우 가능.
                                case '(':
                                    if (totalLength != 0)
                                        bError = !checkOper(ref buffNumChar, ref buffOperChar, ref buffDataChar, currChar, ref strErrorMSG);
                                    buffOperChar = currChar.ToString();
                                    cntLBrackets++;
                                    totalLength++;
                                    break;

                                //')' 괄호
                                //앞에 올 수 있는건 ')',']', 숫자.
                                case ')':
                                    bError = !checkNum(ref buffNumChar, ref buffOperChar, ref buffDataChar, currChar, ref strErrorMSG);
                                    if (currIdx + 1 != formulaDefinition.Length)
                                        buffOperChar = currChar.ToString();
                                    cntRBrackets++;
                                    totalLength++;
                                    if ((cntLBrackets - cntRBrackets) < 0)	//'(' 보다 ')' 가 많을 경우 Error
                                    {
                                        bError = true;
                                        strErrorMSG = "Wrong brace (" + currChar.ToString() + ")";
                                    }
                                    break;

                                //Parameter Data 시작기호
                                //앞에 올 수 있는건 '(', 2항 연산자.  단, 맨 처음일 경우 가능.
                                case '[':
                                    if (totalLength != 0)
                                        bError = !checkSingleOper(ref buffNumChar, ref buffOperChar, ref buffDataChar, currChar, ref strErrorMSG);
                                    isParameter = true;	//data flag on : 다음번 루프에서는 switch로 들어오지 않음. ']'를 만날때까지 buffDataChar에 buffering
                                    buffDataChar += currChar;
                                    totalLength++;
                                    break;

                                //2항 연산자 
                                //앞에 올 수 있는 건 ')',']',숫자. 단, '(' 는 음수 표시할때 사용가능.
                                case '+':
                                case '-':
                                case '*':
                                case '/':
                                case '%':
                                case '^':
                                    bError = !checkDoubleOper(ref buffNumChar, ref buffOperChar, ref buffDataChar, currChar, ref strErrorMSG);
                                    buffOperChar = currChar.ToString();
                                    totalLength++;
                                    break;

                                //단항 연산자 
                                //앞에 올 수 있는건 '(', 2항 연산자. 단, 맨 처음 시작일때 는 가능
                                case '↑':
                                case '↓':
                                case '↕':
                                case '√':
                                case '§':
                                case '‡':
                                case '↔':
                                case '↖':
                                case '￠':
                                case '¤':
                                case '￥':
                                case '￡':
                                case '¿':
                                    if (totalLength != 0)
                                        bError = !checkSingleOper(ref buffNumChar, ref buffOperChar, ref buffDataChar, currChar, ref strErrorMSG);
                                    buffOperChar = currChar.ToString();
                                    totalLength++;
                                    break;

                                //By Joel Lee[2008.04.23]
                                //'~'연산자는 
                                //1. 앞에는 반드시 파라미터값이 오며 뒤에는 숫자가 와야한다.
                                //case '~':
                                //    if (totalLength != 0)
                                //    {
                                //        bError = !checkTildeOper(ref buffNumChar, ref buffOperChar, ref buffDataChar, currChar, ref strErrorMSG);
                                //        buffOperChar = currChar.ToString();
                                //        totalLength++;
                                //    }
                                //    else
                                //        bError = true;
                                //    break;

                                //잘못된 입력.
                                default:
                                    bError = true;
                                    strErrorMSG = "Wrong Character.";
                                    break;
                            }
                        }
                        else  //퍼라메타 데이타 인경우 
                        {
                            if (currChar == ']')		//parameter data end 기호가 나올때까지 buffering. 이후 validation
                            {
                                buffDataChar += currChar;
                                totalLength++;

                                //bError = !checkData(ref buffNumChar, ref buffOperChar, ref buffDataChar, currChar);//2006.05.04 chandler 주석처리
                                if (currIdx + 1 == formulaDefinition.Length)
                                    buffDataChar = "";
                                isParameter = false;
                            }
                            else
                            {
                                buffDataChar += currChar;
                                totalLength++;
                            }
                        }
                    }

                    if (bError == false)
                    {
                        if (cntLBrackets != cntRBrackets)		//괄호 갯수 비교
                        {
                            bError = true;
                            strErrorMSG = "Wrong Brackets Count.";
                        }

                        //버퍼에 찌꺼기 남았나 확인 and 문자열 총 길이와 지금까지 검증한 길이가 같은지 비교
                        if (buffDataChar.Length > 0 || buffOperChar.Length > 0 || buffNumChar.Length > 0 || totalLength != formulaDefinition.Length)
                        {
                            bError = true;
                            strErrorMSG = "Remain non-verified things";
                        }
                    }
                    else
                    {
                        strErrorMSG += " : " + totalLength.ToString();
                    }

                    bSuccess = !bError;

                }
                catch (Exception ex)
                {
                    strErrorMSG = ex.ToString();
                }
            }
            catch (Exception ex)
            {
                strErrorMSG = ex.ToString();
            }

            return bSuccess;
        }

        private bool checkNum(ref string buffNum, ref string buffOper, ref string buffData, char currChar, ref string strErrorMSG)	//제대로된 숫자인지 검증
        {
            //')',']', 숫자. 
            //double tempNum;
            if (buffNum.Length > 0 && buffOper.Length == 0 && buffData.Length == 0)	//숫자일때
            {
                //tempNum = double.Parse(buffNum);	//string -> double 변환되면 숫자임 //2006.05.04 chandler 주석처리
            }
            else if (buffOper.Length > 0 && buffNum.Length == 0 && buffData.Length == 0)			//')' 일때
            {
                if (buffOper != ")")
                {
                    strErrorMSG = "Wrong Operation (" + currChar.ToString() + ")";
                    return false;
                }
            }
            else if (buffData.Length > 0 && buffNum.Length == 0 && buffOper.Length == 0)		//']' 일때
            {
                if (buffData != "]")
                {
                    strErrorMSG = "Wrong Operation (" + currChar.ToString() + ")";
                    return false;
                }
                else
                    buffData = "";
            }
            else
            {
                strErrorMSG = "Wrong Operation (" + currChar.ToString() + ")";
                return false;
            }

            buffNum = "";
            buffOper = "";
            buffData = "";

            return true;
        }

        private bool IsValidNum(string num) //제대로된 숫자인지 판별
        {
            double tempnum;

            try
            {
                tempnum = double.Parse(num);
                return true;
            }
            catch
            {
                return false;
            }
        }


        private bool checkOper(ref string buffNum, ref string buffOper, ref string buffData, char currChar, ref string strErrorMSG)
        {
            //'('앞에 올 수 있는건 '(', 2항 연산자, 단항 연산자.
            if (buffOper.Length > 0 && buffData.Length == 0 && buffNum.Length == 0)
            {
                if (buffOper != "(" && !IsDoubleOperation(buffOper) && !IsSingleOperation(buffOper))
                {
                    strErrorMSG = "Wrong Operation (" + currChar.ToString() + ")";
                    return false;
                }
            }
            else
            {
                strErrorMSG = "Wrong Operation (" + currChar.ToString() + ")";
                return false;
            }

            buffNum = "";
            buffOper = "";
            buffData = "";

            return true;
        }

        private bool IsSingleOperation(string oper)	//단항연산자인지 판별
        {
            if (oper == "§" || oper == "‡" || oper == "↑" ||
                oper == "↓" || oper == "↕" || oper == "√" ||
                oper == "↔" || oper == "↖" || oper == "￠" ||
                oper == "¤" || oper == "￥" || oper == "¿" ||
                oper == "￡")
                return true;
            else
                return false;
        }

        private bool IsDoubleOperation(string oper) //2항연산자인지 판별
        {
            //if(oper == "+" || oper == "-" || oper == "*" ||	oper == "/" || oper == "^" || oper == "%" || oper == "~")
            if (oper == "+" || oper == "-" || oper == "*" || oper == "/" || oper == "^" || oper == "%")
                return true;
            else
                return false;
        }

        private bool checkSingleOper(ref string buffNum, ref string buffOper, ref string buffData, char currChar, ref string strErrorMSG)
        {
            //2항 연산자, '(' 뿐이다.
            if (buffOper.Length > 0 && buffData.Length == 0 && buffNum.Length == 0)
            {
                if (!IsDoubleOperation(buffOper) && buffOper != "(")
                {
                    strErrorMSG = "Wrong Operation (" + currChar.ToString() + ")";
                    return false;
                }
            }
            else
            {
                strErrorMSG = "Wrong Operation (" + currChar.ToString() + ")";
                return false;
            }

            buffNum = "";
            buffOper = "";
            buffData = "";

            return true;
        }

        private bool checkDoubleOper(ref string buffNum, ref string buffOper, ref string buffData, char currChar, ref string strErrorMSG)
        {
            //')',']',숫자 뿐이다. 단 음수 표시할때 '(' 가능 ex) (-29), (-[FB:1100A:ENERGY])
            if (buffOper.Length > 0 && buffData.Length == 0 && buffNum.Length == 0)
            {
                if (buffOper != ")" && (buffOper == "(" && currChar != '-'))
                {
                    strErrorMSG = "Wrong Operation (" + currChar.ToString() + ")";
                    return false;
                }
            }
            else if (buffData.Length > 0 && buffOper.Length == 0 && buffNum.Length == 0)
            {
                if (buffData != "]")
                {
                    strErrorMSG = "Wrong Operation (" + currChar.ToString() + ")";
                    return false;
                }
            }
            else if (buffNum.Length > 0 && buffOper.Length == 0 && buffData.Length == 0)
            {
                //				if(!IsValidNum(buffNum))//2006.05.04 chandler 주석처리
                //				{
                //					strErrorMSG = "Wrong Operation (" + currChar.ToString() + ")";
                //					return false;
                //				}
            }
            else
            {
                strErrorMSG = "Wrong Operation (" + currChar.ToString() + ")";
                return false;
            }

            buffNum = "";
            buffOper = "";
            buffData = "";

            return true;
        }

        public string ConvertString(string strFormula)
        {
            strFormula = strFormula.Replace("CEILING", "↑");
            strFormula = strFormula.Replace("ROUNDDOWN", "↔");
            strFormula = strFormula.Replace("ROUNDUP", "↖");
            strFormula = strFormula.Replace("ROUND", "↕");
            strFormula = strFormula.Replace("FLOOR", "↓");
            strFormula = strFormula.Replace("LOG", "‡");
            strFormula = strFormula.Replace("ABS", "§");
            strFormula = strFormula.Replace("SQRT", "√");
            strFormula = strFormula.Replace("COS", "￠");
            strFormula = strFormula.Replace("SIN", "¤");
            strFormula = strFormula.Replace("TAN", "￥");
            strFormula = strFormula.Replace("A￥", "￡");  //TAN은 ￥이므로 
            strFormula = strFormula.Replace("EXP", "¿");

            return strFormula;
        }

        public SortedList ParseOperand(string strFormula)
        {
            SortedList stlstOperand = new SortedList();
            //Common Checker = new Common();

            string strTemp = "";
            int i;

            for (i = 0; i <= strFormula.Length - 1; i++)
            {
                if (isToken(strFormula[i]))
                {
                    if (strTemp.Length != 0)
                    {
                        if (stlstOperand.IndexOfKey(strTemp) < 0 && Check_Numeric(strTemp) == false)
                        {//중복된 Operand는 저장 안함 //숫자는 Operand 아님
                            stlstOperand.Add(strTemp, i);
                        }
                        strTemp = "";
                    }
                }
                else
                {
                    strTemp = strTemp + strFormula[i].ToString();
                }
            }

            //제일 마지막 처리
            if (strTemp.Length != 0)
            {
                if (stlstOperand.IndexOfKey(strTemp) < 0 && Check_Numeric(strTemp) == false)
                {//중복된 Operand는 저장 안함 //숫자는 Operand 아님.
                    stlstOperand.Add(strTemp, i);
                }
                strTemp = "";
            }

            return stlstOperand;
        }

        /// <summary>
        /// 사용되는 기호(Token)인지 체크
        /// </summary>
        /// <param name="chrKeys">(Char) Check Value</param>
        /// <returns>True: Token False:Not Token</returns>
        public bool isToken(char chrKeys)
        {
            for (int i = 0; i <= TokenKeys.Length - 1; i++)
            {
                if (chrKeys == TokenKeys[i]) return true;
            }

            return false;
        }

        #endregion

        #region : ModelSet Process Model

        public string GetSpiltGainFormula(string exp)
        {
            string source = exp.Trim();
            source = source.Replace(" ", "");
            ArrayList alOperGain = new ArrayList();

            int nMinus = 0;
            int nPlus = 0;
            int nSplitNum = 0;

            while (true)
            {
                nMinus = source.IndexOf("-");
                nPlus = source.IndexOf("+");

                if (nMinus != -1 && nPlus != -1)
                {
                    if (nMinus == 0)
                    {
                        string strTemp = source.Remove(0, 1);
                        nMinus = strTemp.IndexOf("-") + 1;

                        if (nMinus != 0)
                            nSplitNum = Math.Min(nMinus, nPlus);
                        else
                            nSplitNum = nPlus;
                    }
                    else if (nPlus == 0)
                    {
                        string strTemp = source.Remove(0, 1);
                        nPlus = strTemp.IndexOf("+") + 1;

                        if (nPlus != 0)
                            nSplitNum = Math.Min(nMinus, nPlus);
                        else
                            nSplitNum = nMinus;
                    }

                    else
                        nSplitNum = Math.Min(nMinus, nPlus);
                }
                else if (nMinus != -1)
                {
                    if (nMinus == 0)
                    {
                        string strTemp = source.Remove(0, 1);
                        nMinus = strTemp.IndexOf("-") + 1;

                        if (nMinus == 0)
                        {
                            alOperGain.Add(source);
                            break;
                        }
                        else
                            nSplitNum = nMinus;
                    }
                    else
                        nSplitNum = nMinus;
                }
                else if (nPlus != -1)
                {
                    if (nPlus == 0)
                    {
                        string strTemp = source.Remove(0, 1);
                        nPlus = strTemp.IndexOf("+") + 1;

                        if (nPlus == 0)
                        {
                            alOperGain.Add(source);
                            break;
                        }
                        else
                            nSplitNum = nPlus;
                    }
                    else
                        nSplitNum = nPlus;
                }
                else
                {
                    alOperGain.Add(source);
                    break;
                }

                if (source == "")
                    break;

                alOperGain.Add(source.Substring(0, nSplitNum));
                source = source.Remove(0, nSplitNum);
            }

            ArrayList alRemoveIndex = new ArrayList();

            for (int i = 0; i < alOperGain.Count; i++)
            {
                string strTemp = (string)alOperGain[i];

                if (strTemp.IndexOf("[") == -1)
                    alRemoveIndex.Add(strTemp);
            }

            for (int i = 0; i < alRemoveIndex.Count; i++)
            {
                alOperGain.Remove((string)alRemoveIndex[i]);
            }

            string strGain = "";

            for (int i = 0; i < alOperGain.Count; i++)
            {
                strGain += (string)alOperGain[i];
            }

            if (strGain == "")
                return "0";
            else
                return strGain;
        }

        public ArrayList GetOperand(string exp)
        {
            ArrayList operand = new ArrayList();

            string data = exp;

            do
            {
                string name = GetString(data, "[", "]");
                if (name != string.Empty)
                {
                    operand.Add(name);
                    data = GetString(data, "[" + name + "]", string.Empty);
                }
                else
                {
                    break;
                }
            }
            while (true);

            return operand;
        }

        /// <summary>
        /// 해당 String Data에서 원하는 부분의 String을 추출
        /// </summary>
        /// <param name="source">전체 문자열</param>
        /// <param name="first">시작하는 문자열</param>
        /// <param name="dest">끝나는 문자열</param>
        /// <returns>추출된 문자열</returns>
        public string GetString(string source, string first, string dest)
        {
            if ((source == null) || (first == null) || (dest == null))
            {
                return string.Empty;
            }

            if (source == string.Empty)
            {
                return source;
            }

            // 전체 String에서 시작문자열이 끝나는 Index를 찾는다.
            int iFirstEnd = 0;

            if (first != string.Empty)
            {
                iFirstEnd = source.IndexOf(first);

                if (iFirstEnd == -1)
                {
                    return string.Empty;
                }
                else
                {
                    iFirstEnd += first.Length;
                }
            }

            // 시작문자열이 끝나는 Index부터 Data를 저장한다.
            string source1 = source.Substring(iFirstEnd);

            if (dest == string.Empty)
            {
                return source1;
            }

            // 저장된 문자열에서 dest가 시작하는 Index를 찾는다.
            int iDestStart = source1.IndexOf(dest);

            if (iDestStart == -1)
            {
                return string.Empty;
            }

            // 저장된 문자열에서 dest index까지 문자열을 추출한다.
            string source2 = source1.Substring(0, iDestStart);

            return source2;
        }

        #endregion

        /// <summary>
        /// Splits up data by ','. This distinguisher is used for search condition.
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public string[] SplitConditionData(string strData)
        {
            string[] saData = strData.Split(',');

            ArrayList alData = new ArrayList();

            for (int i = 0; i < saData.Length; i++)
            {
                alData.Add(saData[i].Trim().ToString());
            }

            string[] strData_2 = (string[])alData.ToArray(typeof(string));

            return strData_2;
        }
    }
}
