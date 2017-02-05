using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KadrWPF.Helpers
{
    class AppCommands
    {
        private static RoutedUICommand _btMainDb, _btArchives, _btTempDb, _btResume,
            _btEditEmp, _btViewTransfer, _ddTransfer, _ddTransferNow, _btAddTransfer, _btEditTransfer, _btDeleteTransfer,
            _btTransferComb, _ddTransferOld, _btAddOld, _btEditOld, _btDeleteOld, _btRecoveryTransfer, _btReverseTransfer, _btAdd_Agreement_For_Emp,
            _btDismiss, _btDismissToFR, _btDismissComb, _btProject_Order_Dismiss,
            _btTransfer_Term, _btTransferCond_Of_Work, _btTransfer_Emp_For_Group_Hire, _btBase_Exchange, _btProject_Statement,
            _ddReportGH, _btRepCountHire, _btListOrdHire, _ddBkOrdHire, _btBkOrdHireForPeriod, _btBkOrdHireYear, _btViolations_By_Period,
            _ddReportPers, _btRepCountDismiss, _btRepCountDismissR, _btRepReasonDismiss1, _btRepReasonDismiss2, _btListOrdDismiss,
            _btEndOfContr, _btOrdDismiss, _btOrdTransfer, _ddListDismiss, _btListDismissFull, _btListDismissShort, _btListDismissForDir,
            _btListDismissJobman, _btListAcadDegree, _btRep_Staff_Subdiv,
            _ddNotice_Expiry, _btNotice_Expiry_Contract, _btNotice_Expiry_Add_Agreement, _ddBlank_Reference, _btReference_On_Place_Requirements, _btReference_Child_Realing_Leave,
            _ddRepStaff_Pension, _btRepNonState_Pens_Prov, _btRepNPP_With_Date, _ddListInvalid, _btListInvalid, _btListInvalidSub,
            _ddRetirer, _ddRetirerStruct, _btListRetirerStruct, _btListRetirerStructSubdiv, _ddListRetirer, _btListRetirerPlant, _btListRetirerSubdiv,
            _btListRetirerDismiss, _btEmptyInsurNum,
            _btSprObj, _btPersonalCard, _btRepOtherType, _btRepMRP, _ddReportMSFO, _btRepMSFOSal, _btRepMSFODisEmp,
            _ddReportsPrivPos, _btListEmpPrivPos, _btTransferPrivPos, _btRepChild_Realing_Leave, _btEmp_By_Service,            
            _ddVeteran, _ddNumbersVeteran, _btNumbersVeteranPlant, _btNumbersVeteranSub,
            _ddListVeteran, _btListVeteranPlant, _btListVeteranSub, _btListDismissVeteran, _btRepTransfer_By_Period, _btRepYoung_Specialist, _btOrderOnEncouraging, 
            _btAdd_Agreement_On_Subdiv, _ddAdd_Agreement_ShortDay, _btAdd_Agreement_On_ShortDay1, _btAdd_Agreement_On_ShortDay2,
            _btRepChild_Realing_Leave2, _ddNotification_For_Emp, _btNotification_Short_Week, _btNotification_Early_Ending, _btCount_Short_Graph,
            _btDataFill, _bt_sbros_IBM, _btFilterRas, _btFindRas, _btUpdateRas, _btView_All_Transfer, _btOutside_Emp,
            _btPrintAD, _R_btRasCombList, _R_btRASInvalidEmp, _R_btRASProfEmp, _R_btRASNonProfEmp, _btProfUnion_Entered, _btProfUnion_CameOut,
            _btProtocolDump, _btRepTransSub, _btRepEmpContract, _btProtocolTr_Date_Order;
        
        private static RoutedUICommand _btPermit, _btEditFilterPermit, _btType_Permit, _btViewViolators, _btDismissed_Emp,
            _btTransfer_Emp, _btAccess_Template, _btList_Emp_With_Template,
            _btPrevFR_Emp, _btReleaseBuffer, _btFilterFR_Emp, _btFindFR_Emp,
            _btSubdivSort, _btFIOSort, _btSubdivFIOSort, _btPositionSort, _btDate_StartSort;
        private static RoutedUICommand _btType_Study, _btType_Edu, _btInstit, _btStudy_City, _btGroup_Spec, _btQual, _btSpeciality,
            _btLang, _btLevel_Know, _btComm, _btMil_Spec, _btMed_Classif, _btMil_Rank, _btMil_Cat, _btType_Troop, 
            _btType_Priv, _btType_Rise_Qual, _btPrivileged_Position, _btType_Postg_Study, _btSource_Employability, _btReason_dismiss,
            _btPosition, _btSubdiv, _btReward_Name, _btType_Reward, _btConditions_Of_Work, _btType_Condition;

        static AppCommands()
        {
            Sql_Trace_Enabled = new RoutedUICommand("Трассировка сессии", "Sql_Trace_Enabled", typeof(AppCommands));
            _btMainDb = new RoutedUICommand("Основная база", "BtMainDb", typeof(AppCommands));
            _btArchives = new RoutedUICommand("Архивная база", "BtArchives", typeof(AppCommands));
            _btTempDb = new RoutedUICommand("Приемная база", "BtTempDb", typeof(AppCommands));
            _btResume = new RoutedUICommand("Резюме", "BtResume", typeof(AppCommands));
            _btTransfer_Term = new RoutedUICommand("Срочные ТД и доп.соглашения", "BtTransfer_Term", typeof(AppCommands));
            _btTransferCond_Of_Work = new RoutedUICommand("Условия труда", "BtTransferCond_Of_Work", typeof(AppCommands));
            _btTransfer_Emp_For_Group_Hire = new RoutedUICommand("Список переводов для обработки", "BtTransfer_Emp_For_Group_Hire", typeof(AppCommands));
            _btBase_Exchange = new RoutedUICommand("Обменная база", "BtBase_Exchange", typeof(AppCommands));
            _btProject_Statement = new RoutedUICommand("Заявления о переводе", "BtProject_Statement", typeof(AppCommands));
            Project_Transfer_Combined = new RoutedUICommand("Проекты переводов", "Project_Transfer_Combined", typeof(AppCommands));
            View_Emp_Mobile_Comm = new RoutedUICommand("Список абонентов мобильной связи", "View_Emp_Mobile_Comm", typeof(AppCommands));
            AddEmp_Mobile_Comm = new RoutedUICommand("Добавить абонента", "AddEmp_Mobile_Comm", typeof(AppCommands));
            EditEmp_Mobile_Comm = new RoutedUICommand("Редактировать абонента", "EditEmp_Mobile_Comm", typeof(AppCommands));
            DeleteEmp_Mobile_Comm = new RoutedUICommand("Удалить абонента", "DeleteEmp_Mobile_Comm", typeof(AppCommands));

            #region Команды для работы с переводами
            _btEditEmp = new RoutedUICommand("Просмотр личных данных сотрудника", "BtEditEmp", typeof(AppCommands));
            _btViewTransfer = new RoutedUICommand("Просмотр перевода", "BtViewTransfer", typeof(AppCommands));
            _ddTransfer = new RoutedUICommand("Работа с переводами", "DdTransfer", typeof(AppCommands));
            _ddTransferNow = new RoutedUICommand("Основная деятельность", "DdTransferNow", typeof(AppCommands));
            _btAddTransfer = new RoutedUICommand("Добавить перевод", "BtAddTransfer", typeof(AppCommands));
            _btEditTransfer = new RoutedUICommand("Редактировать перевод", "BtEditTransfer", typeof(AppCommands));
            _btDeleteTransfer = new RoutedUICommand("Удалить перевод", "BtDeleteTransfer", typeof(AppCommands));
            _btTransferComb = new RoutedUICommand("Совмещение", "BtTransferComb", typeof(AppCommands));
            _ddTransferOld = new RoutedUICommand("Старые переводы", "DdTransferOld", typeof(AppCommands));
            _btAddOld = new RoutedUICommand("Добавить перевод", "BtAddOld", typeof(AppCommands));
            _btEditOld = new RoutedUICommand("Редактировать перевод", "BtEditOld", typeof(AppCommands));
            _btDeleteOld = new RoutedUICommand("Удалить перевод", "BtDeleteOld", typeof(AppCommands));
            _btRecoveryTransfer = new RoutedUICommand("Восстановить работника", "BtRecoveryTransfer", typeof(AppCommands));
            _btReverseTransfer = new RoutedUICommand("Обратный перевод", "BtReverseTransfer", typeof(AppCommands));
            _btAdd_Agreement_For_Emp = new RoutedUICommand("Бланк дополнительного соглашения", "BtAdd_Agreement_For_Emp", typeof(AppCommands));
            _btDismiss = new RoutedUICommand("Увольнение с завода", "BtDismiss", typeof(AppCommands));
            _btDismissToFR = new RoutedUICommand("Увольнение в стороннюю организацию на заводе", "BtDismissToFR", typeof(AppCommands));
            _btDismissComb = new RoutedUICommand("Увольнения с совмещения", "BtDismissComb", typeof(AppCommands));
            _btProject_Order_Dismiss = new RoutedUICommand("Проект приказа об увольнении", "BtProject_Order_Dismiss", typeof(AppCommands));
            #endregion

            #region Отчеты АСУ Кадры
            _ddReportGH = new RoutedUICommand("Отчеты ГРУППЫ ПРИЕМА", "DdReportGH", typeof(AppCommands));
            _btRepCountHire = new RoutedUICommand("Отчёт о количестве принятых", "BtRepCountHire", typeof(AppCommands));
            _btListOrdHire = new RoutedUICommand("Опись приказов о приёме", "BtListOrdHire", typeof(AppCommands));
            _ddBkOrdHire = new RoutedUICommand("Книга приказов о приеме", "DdBkOrdHire", typeof(AppCommands));
            _btBkOrdHireForPeriod = new RoutedUICommand("За период", "BtBkOrdHireForPeriod", typeof(AppCommands));
            _btBkOrdHireYear = new RoutedUICommand("Год и номер посл. напечат. приказа", "BtBkOrdHireYear", typeof(AppCommands));
            _btViolations_By_Period = new RoutedUICommand("Нарушения за период", "BtViolations_By_Period", typeof(AppCommands));
            _ddReportPers = new RoutedUICommand("Отчеты ГРУППЫ ЛИЧНОГО СТОЛА", "DdReportPers", typeof(AppCommands));
            _btRepCountDismiss = new RoutedUICommand("Отчет о количестве уволенных", "BtRepCountDismiss", typeof(AppCommands));
            _btRepCountDismissR = new RoutedUICommand("Отчёт о количестве уволенных (обратная сторона)", "BtRepCountDismissR", typeof(AppCommands));
            _btRepReasonDismiss1 = new RoutedUICommand("Отчёт по причинам увольнений (1 форма)", "BtRepReasonDismiss1", typeof(AppCommands));
            _btRepReasonDismiss2 = new RoutedUICommand("Отчёт по причинам увольнений (2 форма)", "BtRepReasonDismiss2", typeof(AppCommands));
            _btListOrdDismiss = new RoutedUICommand("Опись приказов об увольнении", "BtListOrdDismiss", typeof(AppCommands));
            _btEndOfContr = new RoutedUICommand("Окончание срока договора", "BtEndOfContr", typeof(AppCommands));
            _btOrdDismiss = new RoutedUICommand("Книга приказов об увольнении", "BtOrdDismiss", typeof(AppCommands));
            _btOrdTransfer = new RoutedUICommand("Книга приказов о переводах", "BtOrdTransfer", typeof(AppCommands));
            _ddListDismiss = new RoutedUICommand("Список уволенных", "DdListDismiss", typeof(AppCommands));
            _btListDismissFull = new RoutedUICommand("Полный", "BtListDismissFull", typeof(AppCommands));
            _btListDismissShort = new RoutedUICommand("Без адресов", "BtListDismissShort", typeof(AppCommands));
            _btListDismissForDir = new RoutedUICommand("Без адресов (новая форма)", "BtListDismissForDir", typeof(AppCommands));
            _btListDismissJobman = new RoutedUICommand("Список уволенных (сдельщики)", "BtListDismissJobman", typeof(AppCommands));
            _btListAcadDegree = new RoutedUICommand("Список работников, имеющих учёную степень", "BtListAcadDegree", typeof(AppCommands));
            _btRep_Staff_Subdiv = new RoutedUICommand("Отчёт по численности сотрудников", "BtRep_Staff_Subdiv", typeof(AppCommands));
            _ddNotice_Expiry = new RoutedUICommand("Формирование уведомлений", "DdNotice_Expiry", typeof(AppCommands));
            _btNotice_Expiry_Contract = new RoutedUICommand("Истечение срока трудового договора", "BtNotice_Expiry_Contract", typeof(AppCommands));
            _btNotice_Expiry_Add_Agreement = new RoutedUICommand("Истечение срока дополнительного соглашения", "BtNotice_Expiry_Add_Agreement", typeof(AppCommands));
            _ddBlank_Reference = new RoutedUICommand("Бланки справок", "DdBlank_Reference", typeof(AppCommands));
            _btReference_On_Place_Requirements = new RoutedUICommand("Справка по месту требования (Ф.005-039)", "BtReference_On_Place_Requirements", typeof(AppCommands));
            _btReference_Child_Realing_Leave = new RoutedUICommand("Справка об отпуске по уходу за ребенком (Ф.005-049)", "BtReference_Child_Realing_Leave", typeof(AppCommands));
            _ddRepStaff_Pension = new RoutedUICommand("Отчеты ПЕНСИОННОГО БЮРО", "DdRepStaff_Pension", typeof(AppCommands));
            _btRepNonState_Pens_Prov = new RoutedUICommand("Участие в программе НПО", "BtRepNonState_Pens_Prov", typeof(AppCommands));
            _btRepNPP_With_Date = new RoutedUICommand("Участие в НПО с датой назначения пенсии", "BtRepNPP_With_Date", typeof(AppCommands));
            _ddListInvalid = new RoutedUICommand("Список инвалидов", "DdListInvalid", typeof(AppCommands));
            _btListInvalid = new RoutedUICommand("По заводу", "BtListInvalid", typeof(AppCommands));
            _btListInvalidSub = new RoutedUICommand("По подразделению", "BtListInvalidSub", typeof(AppCommands));
            _ddRetirer = new RoutedUICommand("Данные по пенсионерам", "DdRetirer", typeof(AppCommands));
            _ddRetirerStruct = new RoutedUICommand("Численность, состав и движение", "DdRetirerStruct", typeof(AppCommands));
            _btListRetirerStruct = new RoutedUICommand("По заводу", "BtListRetirerStruct", typeof(AppCommands));
            _btListRetirerStructSubdiv = new RoutedUICommand("По подразделению", "BtListRetirerStructSubdiv", typeof(AppCommands));
            _ddListRetirer = new RoutedUICommand("Список пенсионеров", "DdListRetirer", typeof(AppCommands));
            _btListRetirerPlant = new RoutedUICommand("По заводу", "BtListRetirerPlant", typeof(AppCommands));
            _btListRetirerSubdiv = new RoutedUICommand("По подразделению", "BtListRetirerSubdiv", typeof(AppCommands));
            _btListRetirerDismiss = new RoutedUICommand("Список уволенных", "BtListRetirerDismiss", typeof(AppCommands));
            _btEmptyInsurNum = new RoutedUICommand("Список для заполнения страх.св-в", "BtEmptyInsurNum", typeof(AppCommands));
            _btSprObj = new RoutedUICommand("Справка-объективка", "BtSprObj", typeof(AppCommands));
            _btPersonalCard = new RoutedUICommand("Личная карточка", "BtPersonalCard", typeof(AppCommands));
            _btRepOtherType = new RoutedUICommand("Отчёт произвольной формы", "BtRepOtherType", typeof(AppCommands));
            _btRepMRP = new RoutedUICommand("Отчет по МОЛ", "BtRepMRP", typeof(AppCommands));
            _ddReportMSFO = new RoutedUICommand("Отчеты МСФО", "DdReportMSFO", typeof(AppCommands));
            _btRepMSFOSal = new RoutedUICommand("Демографическая и финансовая статистика", "BtRepMSFOSal", typeof(AppCommands));
            _btRepMSFODisEmp = new RoutedUICommand("Статистика текучести персонала", "BtRepMSFODisEmp", typeof(AppCommands));
            _ddReportsPrivPos = new RoutedUICommand("Отчеты по льготным профессиям", "DdReportsPrivPos", typeof(AppCommands));
            _btListEmpPrivPos = new RoutedUICommand("Список льготников", "BtListEmpPrivPos", typeof(AppCommands));
            _btTransferPrivPos = new RoutedUICommand("Список переводов", "BtTransferPrivPos", typeof(AppCommands));
            _btRepChild_Realing_Leave = new RoutedUICommand("Список декретниц за период (ограничение периодов)", "BtRepChild_Realing_Leave", typeof(AppCommands));
            _btEmp_By_Service = new RoutedUICommand("Сотрудники по службам", "BtEmp_By_Service", typeof(AppCommands));
            _ddVeteran = new RoutedUICommand("Данные по ветеранам труда", "DdVeteran", typeof(AppCommands));
            _ddNumbersVeteran = new RoutedUICommand("Численность, состав и движение", "DdNumbersVeteran", typeof(AppCommands));
            _btNumbersVeteranPlant = new RoutedUICommand("По заводу", "BtNumbersVeteranPlant", typeof(AppCommands));
            _btNumbersVeteranSub = new RoutedUICommand("По подразделению", "BtNumbersVeteranSub", typeof(AppCommands));
            _ddListVeteran = new RoutedUICommand("Список ветеранов", "DdListVeteran", typeof(AppCommands));
            _btListVeteranPlant = new RoutedUICommand("По заводу", "BtListVeteranPlant", typeof(AppCommands));
            _btListVeteranSub = new RoutedUICommand("По подразделению", "BtListVeteranSub", typeof(AppCommands));
            _btListDismissVeteran = new RoutedUICommand("Список уволенных", "BtListDismissVeteran", typeof(AppCommands));
            _btRepTransfer_By_Period = new RoutedUICommand("Переводы за период", "BtRepTransfer_By_Period", typeof(AppCommands));
            _btRepYoung_Specialist = new RoutedUICommand("Отчет по молодым специалистам", "BtRepYoung_Specialist", typeof(AppCommands));
            _btOrderOnEncouraging = new RoutedUICommand("Приказ о поощрении", "BtOrderOnEncouraging", typeof(AppCommands));
            _btAdd_Agreement_On_Subdiv = new RoutedUICommand("Доп. соглашения по подразделению", "BtAdd_Agreement_On_Subdiv", typeof(AppCommands));
            _ddAdd_Agreement_ShortDay = new RoutedUICommand("Доп. соглашения сокр. день", "DdAdd_Agreement_ShortDay", typeof(AppCommands));
            _btAdd_Agreement_On_ShortDay1 = new RoutedUICommand("Сокращенный день", "BtAdd_Agreement_On_ShortDay1", typeof(AppCommands));
            _btAdd_Agreement_On_ShortDay2 = new RoutedUICommand("Сокращенная неделя", "BtAdd_Agreement_On_ShortDay2", typeof(AppCommands));
            _btRepChild_Realing_Leave2 = new RoutedUICommand("Список декретниц за период", "BtRepChild_Realing_Leave2", typeof(AppCommands));
            _ddNotification_For_Emp = new RoutedUICommand("Уведомления работникам", "DdNotification_For_Emp", typeof(AppCommands));
            _btNotification_Short_Week = new RoutedUICommand("Уведомление для неполной рабочей недели", "BtNotification_Short_Week", typeof(AppCommands));
            _btNotification_Early_Ending = new RoutedUICommand("Уведомление для раннего окончания работы", "BtNotification_Early_Ending", typeof(AppCommands));
            _btCount_Short_Graph = new RoutedUICommand("Количество сотрудников на НС и НН", "BtCount_Short_Graph", typeof(AppCommands));
            BtCorporate_Support_Contract = new RoutedUICommand("Договор на оказание корпоративной поддержки", "BtCorporate_Support_Contract", typeof(AppCommands));
            BtCorporate_Support_Agreement = new RoutedUICommand("Дополнительное соглашение о корпоративной поддержке", "BtCorporate_Support_Agreement", typeof(AppCommands));
            #endregion

            #region Вкладка Бюро пропусков, Сторонние сотрудники
            _btPermit = new RoutedUICommand("Учет разрешений", "BtPermit", typeof(AppCommands));
            _btEditFilterPermit = new RoutedUICommand("Изменить фильтрацию", "BtEditFilterPermit", typeof(AppCommands));
            _btType_Permit = new RoutedUICommand("Типы разрешений", "BtType_Permit", typeof(AppCommands));
            _btViewViolators = new RoutedUICommand("Нарушители режима", "BtViewViolators", typeof(AppCommands));
            _btDismissed_Emp = new RoutedUICommand("Уволенные сотрудники", "BtDismissed_Emp", typeof(AppCommands));
            _btTransfer_Emp = new RoutedUICommand("Переведенные сотрудники", "BtTransfer_Emp", typeof(AppCommands));
            _btAccess_Template = new RoutedUICommand("Шаблоны доступа", "BtAccess_Template", typeof(AppCommands));
            _btList_Emp_With_Template = new RoutedUICommand("Сотрудники и шаблоны", "BtList_Emp_With_Template", typeof(AppCommands));

            _btPrevFR_Emp = new RoutedUICommand("Просмотр списка сторонних сотрудников", "BtPrevFR_Emp", typeof(AppCommands));
            _btReleaseBuffer = new RoutedUICommand("Очистить буфер", "BtReleaseBuffer", typeof(AppCommands));
            _btFilterFR_Emp = new RoutedUICommand("Фильтр", "BtFilterFR_Emp", typeof(AppCommands));
            _btFindFR_Emp = new RoutedUICommand("Поиск", "BtFindFR_Emp", typeof(AppCommands));
            _btSubdivSort = new RoutedUICommand("По подразделению", "BtSubdivSort", typeof(AppCommands));
            _btFIOSort = new RoutedUICommand("По фамилии, имени, отчеству", "BtFIOSort", typeof(AppCommands));
            _btSubdivFIOSort = new RoutedUICommand("По подразделению и фамилии, имени, отчеству", "BtSubdivFIOSort", typeof(AppCommands));
            _btPositionSort = new RoutedUICommand("По наименованию должности", "BtPositionSort", typeof(AppCommands));
            _btDate_StartSort = new RoutedUICommand("По дате начала работы", "BtDate_StartSort", typeof(AppCommands));
            #endregion

            #region Справочники
            _btType_Study = new RoutedUICommand("Вид обучения", "BtType_Study", typeof(AppCommands));
            _btType_Edu = new RoutedUICommand("Вид образования", "BtType_Edu", typeof(AppCommands));
            _btInstit = new RoutedUICommand("Учебные заведения", "BtInstit", typeof(AppCommands));
            _btStudy_City = new RoutedUICommand("Города учебных заведений", "BtStudy_City", typeof(AppCommands));
            _btGroup_Spec = new RoutedUICommand("Группы специальностей", "BtGroup_Spec", typeof(AppCommands));
            _btQual = new RoutedUICommand("Квалификации", "BtQual", typeof(AppCommands));
            _btSpeciality = new RoutedUICommand("Специальности", "BtSpeciality", typeof(AppCommands));
            _btLang = new RoutedUICommand("Иностранные языки", "BtLang", typeof(AppCommands));
            _btLevel_Know = new RoutedUICommand("Степень владения языком", "BtLevel_Know", typeof(AppCommands));
            _btComm = new RoutedUICommand("Военный комиссариат", "BtComm", typeof(AppCommands));
            _btMil_Spec = new RoutedUICommand("Коды ВУС", "BtMil_Spec", typeof(AppCommands));
            _btMed_Classif = new RoutedUICommand("Категории годности", "BtMed_Classif", typeof(AppCommands));
            _btMil_Rank = new RoutedUICommand("Воинские звания", "BtMil_Rank", typeof(AppCommands));
            _btMil_Cat = new RoutedUICommand("Воинский состав", "BtMil_Cat", typeof(AppCommands));
            _btType_Troop = new RoutedUICommand("Виды войск", "BtType_Troop", typeof(AppCommands));
            _btType_Priv = new RoutedUICommand("Типы льгот", "BtType_Priv", typeof(AppCommands));
            _btType_Rise_Qual = new RoutedUICommand("Вид повышения квалификации", "BtType_Rise_Qual", typeof(AppCommands));
            _btPrivileged_Position = new RoutedUICommand("Льготные профессии", "BtPrivileged_Position", typeof(AppCommands));
            _btType_Postg_Study = new RoutedUICommand("Вид послевузовского образования", "BtType_Postg_Study", typeof(AppCommands));
            _btSource_Employability = new RoutedUICommand("Источники трудоустройства", "BtSource_Employability", typeof(AppCommands));
            _btReason_dismiss = new RoutedUICommand("Причина увольнения", "BtReason_dismiss", typeof(AppCommands));
            _btPosition = new RoutedUICommand("Справочник должностей", "BtPosition", typeof(AppCommands));
            _btSubdiv = new RoutedUICommand("Справочник подразделений", "BtSubdiv", typeof(AppCommands));
            _btReward_Name = new RoutedUICommand("Справочник наименований наград", "BtReward_Name", typeof(AppCommands));
            _btType_Reward = new RoutedUICommand("Справочник типов наград", "BtType_Reward", typeof(AppCommands));
            _btConditions_Of_Work = new RoutedUICommand("Подклассы условий труда", "BtConditions_Of_Work", typeof(AppCommands));
            _btType_Condition = new RoutedUICommand("Типы условий труда", "BtType_Condition", typeof(AppCommands));
            #endregion

            #region АРМ Бухгалтера
            _btDataFill = new RoutedUICommand("Список работников", "BtDataFill", typeof(AppCommands));
            _bt_sbros_IBM = new RoutedUICommand("Сброс файла на IBM", "Bt_sbros_IBM", typeof(AppCommands));
            _btFilterRas = new RoutedUICommand("Фильтр", "BtFilterRas", typeof(AppCommands));
            _btFindRas = new RoutedUICommand("Поиск", "BtFindRas", typeof(AppCommands));
            _btUpdateRas = new RoutedUICommand("Редактировать", "BtUpdateRas", typeof(AppCommands));
            _btView_All_Transfer = new RoutedUICommand("Просмотр переводов", "BtView_All_Transfer", typeof(AppCommands));
            _btOutside_Emp = new RoutedUICommand("Сторонние сотрудники", "BtOutside_Emp", typeof(AppCommands));
            _btPrintAD = new RoutedUICommand("Справочник работающих", "BtPrintAD", typeof(AppCommands));
            _R_btRasCombList = new RoutedUICommand("Список совместителей", "R_btRasCombList", typeof(AppCommands));
            _R_btRASInvalidEmp = new RoutedUICommand("Список работающих инвалидов", "R_btRASInvalidEmp", typeof(AppCommands));
            _R_btRASProfEmp = new RoutedUICommand("Список работников состоящих в профсоюзе", "R_btRASProfEmp", typeof(AppCommands));
            _R_btRASNonProfEmp = new RoutedUICommand("Список работников не состоящих в профсоюзе", "R_btRASNonProfEmp", typeof(AppCommands));
            _btProfUnion_Entered = new RoutedUICommand("Список вступивших в профсоюз", "BtProfUnion_Entered", typeof(AppCommands));
            _btProfUnion_CameOut = new RoutedUICommand("Список вышедших из профсоюза", "BtProfUnion_CameOut", typeof(AppCommands));
            _btProtocolDump = new RoutedUICommand("Протокол пустых дат на выслугу лет", "BtProtocolDump", typeof(AppCommands));
            _btRepTransSub = new RoutedUICommand("Протокол переводов в подразделении", "BtRepTransSub", typeof(AppCommands));
            _btRepEmpContract = new RoutedUICommand("Список работников со сроком окончания доп.соглашения", "BtRepEmpContract", typeof(AppCommands));
            _btProtocolTr_Date_Order = new RoutedUICommand("Протокол несоответствия месяца даты движения", "BtProtocolTr_Date_Order", typeof(AppCommands));
            #endregion

            #region Табельный учет
            BtCalendar = new RoutedUICommand("Производственный календарь", "BtCalendar", typeof(AppCommands));
            BtTable = new RoutedUICommand("Табель", "BtTable", typeof(AppCommands));
            BtFindEmp = new RoutedUICommand("Поиск сотрудников", "BtFindEmp", typeof(AppCommands));
            BtRefresh = new RoutedUICommand("Обновление табеля", "BtRefresh", typeof(AppCommands));
            BtRefTableList = new RoutedUICommand("Обновление списка", "BtRefTableList", typeof(AppCommands));
            BtGr_Work = new RoutedUICommand("Графики работы", "BtGr_Work", typeof(AppCommands));
            BtTableForAdvance = new RoutedUICommand("Табель на аванс", "BtTableForAdvance", typeof(AppCommands));
            BtReportTable = new RoutedUICommand("Табель на зарплату", "BtReportTable", typeof(AppCommands));
            BtReportPopul = new RoutedUICommand("Отчет по численности", "BtReportPopul", typeof(AppCommands));
            BtRepHoursByOrders = new RoutedUICommand("Отработанные часы по заказам", "BtRepHoursByOrders", typeof(AppCommands));
            BtTableTotal = new RoutedUICommand("Общий табель", "BtTableTotal", typeof(AppCommands));
            BtTable_By_Period = new RoutedUICommand("Табель за период", "BtTable_By_Period", typeof(AppCommands));
            BtCloseTableAdvance = new RoutedUICommand("Закрытие табеля на аванс", "BtCloseTableAdvance", typeof(AppCommands));
            BtCloseTableSalary = new RoutedUICommand("Закрытие табеля на зарплату", "BtCloseTableSalary", typeof(AppCommands));
            BtStart_Approval_Table = new RoutedUICommand("Табель подразделения готов к закрытию", "BtStart_Approval_Table", typeof(AppCommands));
            BtDocsOnPay_Type = new RoutedUICommand("Отчет по оправдательным документам", "BtDocsOnPay_Type", typeof(AppCommands));
            BtLateness = new RoutedUICommand("Отчет по опозданиям", "BtLateness", typeof(AppCommands));
            BtWorkOut = new RoutedUICommand("Отчет о работе за территорией", "BtWorkOut", typeof(AppCommands));
            BtHoursOnDegree = new RoutedUICommand("Часы 101,102,106,124,125,110 ш. по кат.", "BtHoursOnDegree", typeof(AppCommands));
            BtHoursOnSubdiv = new RoutedUICommand("Часы 101,102,106,124,125,110 ш. по подр.", "BtHoursOnSubdiv", typeof(AppCommands));
            BtRepHoursOnSubdiv = new RoutedUICommand("Часы по видам оплат в разрезе ГМ", "BtRepHoursOnSubdiv", typeof(AppCommands));
            BtRepWorkHol = new RoutedUICommand("Отчет о работе в выходные дни", "BtRepWorkHol", typeof(AppCommands));
            BtRepWorkPT = new RoutedUICommand("Отчет по 124 шифру", "BtRepWorkPT", typeof(AppCommands));
            BtRepMission = new RoutedUICommand("Отчет по командировкам", "BtRepMission", typeof(AppCommands));
            BtWorkOrder = new RoutedUICommand("Рабочий наряд на выходные дни", "BtWorkOrder", typeof(AppCommands));
            BtRepAbsenceOnSubdiv = new RoutedUICommand("Отчет о доступных часах отгулов", "BtRepAbsenceOnSubdiv", typeof(AppCommands));
            BtProtocolTable = new RoutedUICommand("Протокол ошибок в данных", "BtProtocolTable", typeof(AppCommands));
            BtErrorGraph = new RoutedUICommand("Протокол неверных графиков", "BtErrorGraph", typeof(AppCommands));
            BtProtoсolForAccount = new RoutedUICommand("Протокол ошибок для бухгалтерии", "BtProtoсolForAccount", typeof(AppCommands));
            BtRepFailedOrders = new RoutedUICommand("Протокол закрытых и несуществующих заказов", "BtRepFailedOrders", typeof(AppCommands));
            BtOrderHoliday = new RoutedUICommand("Приказы на выходные и сверхурочные", "BtOrderHoliday", typeof(AppCommands));
            BtViewReplEmpTable = new RoutedUICommand("Приказы на замещения", "BtViewReplEmpTable", typeof(AppCommands));
            BtRepCalc_Salary = new RoutedUICommand("Ведомость расчета табеля", "BtRepCalc_Salary", typeof(AppCommands));
            R_btProtokolReplSalT = new RoutedUICommand("Протокол начисления замещений", "R_btProtokolReplSalT", typeof(AppCommands));
            R_btCombReplReport = new RoutedUICommand("Ведомость начисления совмещений (153)", "R_btCombReplReport", typeof(AppCommands));
            BtList_Closing_Table = new RoutedUICommand("Закрытие табеля", "BtList_Closing_Table", typeof(AppCommands));
            BtOrderTruancy = new RoutedUICommand("Отчет по прогульщикам", "BtOrderTruancy", typeof(AppCommands));
            BtRepOnAdministrVac = new RoutedUICommand("Отчет по административным отпускам", "BtRepOnAdministrVac", typeof(AppCommands));
            BtRepOnHospital = new RoutedUICommand("Отчет по временной нетрудоспособности", "BtRepOnHospital", typeof(AppCommands));
            BtRepOnPregnancy = new RoutedUICommand("Отчет по отпуску по беременности и родам", "BtRepOnPregnancy", typeof(AppCommands));
            BtRepOnChildCare = new RoutedUICommand("Отчет по отпуску по уходу за ребенком до 3-х лет", "BtRepOnChildCare", typeof(AppCommands));
            BtRepAverage_Number_On_Plant = new RoutedUICommand("Отчет по численности (завод и по подразделениям)", "BtRepAverage_Number_On_Plant", typeof(AppCommands));
            BtRepAverage_Number_Personnel = new RoutedUICommand("Справка по среднесписочной численности", "BtRepAverage_Number_Personnel", typeof(AppCommands));
            BtRepFailureToAppear = new RoutedUICommand("Отчет по отсутствию", "BtRepFailureToAppear", typeof(AppCommands));
            BtRepLate_Pass = new RoutedUICommand("Отчет Поздний выход", "BtRepLate_Pass", typeof(AppCommands));
            BtRepLateness = new RoutedUICommand("Отчет по опозданиям", "BtRepLateness", typeof(AppCommands));
            BtRepWorkOut = new RoutedUICommand("Отчет по работе за территорией", "BtRepWorkOut", typeof(AppCommands));
            BtRepAverage_Number = new RoutedUICommand("Отчет по среднесписочной численности", "BtRepAverage_Number", typeof(AppCommands));
            BtRepAverage_Number_Consolidated = new RoutedUICommand("Сводный отчет по среднесписочной численности", "BtRepAverage_Number_Consolidated", typeof(AppCommands));
            BtRepListEmp_Number = new RoutedUICommand("Отчет по списочной численности", "BtRepListEmp_Number", typeof(AppCommands));
            BtRepUseOfWorkTime = new RoutedUICommand("Отчет использования рабочего времени по месяцам", "BtRepUseOfWorkTime", typeof(AppCommands));
            BtRepUseOfWorkTimeOnSub = new RoutedUICommand("Отчет использования рабочего времени по подразделениям", "BtRepUseOfWorkTimeOnSub", typeof(AppCommands));
            BtRepTimeNotWorker = new RoutedUICommand("Сводный отчет по неотработанному времени и ССЧ", "BtRepTimeNotWorker", typeof(AppCommands));
            BtRepTimeNotWorkerOnSub = new RoutedUICommand("Отчет по неявкам (подразделений)", "BtRepTimeNotWorkerOnSub", typeof(AppCommands));
            BtRepHoursByOrdersPlant = new RoutedUICommand("Часы 102 ш.о. по заказам по предприятию", "BtRepHoursByOrdersPlant", typeof(AppCommands));
            BtRep_Pay_Type_545 = new RoutedUICommand("Отчет по часам режима неполного рабочего времени", "BtRep_Pay_Type_545", typeof(AppCommands));
            BtRepAll_Limits_By_Subdiv = new RoutedUICommand("Отчет об использовании сверхурочного рабочего времени", "BtRepAll_Limits_By_Subdiv", typeof(AppCommands));
            BtRepDocsOnPay_Type = new RoutedUICommand("Отчет по оправдательным документам", "BtRepDocsOnPay_Type", typeof(AppCommands));
            BtLimit = new RoutedUICommand("Лимиты", "BtLimit", typeof(AppCommands));
            BtLimit303 = new RoutedUICommand("Лимиты по работе в счет отгула", "BtLimit303", typeof(AppCommands));
            BtViewOrders = new RoutedUICommand("Приказы на выходные и сверхурочные", "BtViewOrders", typeof(AppCommands));
            AddReg_Doc = new RoutedUICommand("Добавить документ", "AddReg_Doc", typeof(AppCommands));
            EditReg_Doc = new RoutedUICommand("Редактировать документ", "EditReg_Doc", typeof(AppCommands));
            DeleteReg_Doc = new RoutedUICommand("Удалить документ", "DeleteReg_Doc", typeof(AppCommands));
            SelectOrderChange = new RoutedUICommand("Выбрать заказ для изменения", "SelectOrderChange", typeof(AppCommands));
            RestoreOrderTable = new RoutedUICommand("Восстановить заказы по умолчанию", "RestoreOrderTable", typeof(AppCommands));
            ChangeOrderTable = new RoutedUICommand("Установить заказ", "ChangeOrderTable", typeof(AppCommands));
            ViewEmpTable = new RoutedUICommand("Просмотр сведений по сотруднику", "ViewEmpTable", typeof(AppCommands));
            AddWork_Pay_Type = new RoutedUICommand("Добавить запись по видам оплат", "AddWork_Pay_Type", typeof(AppCommands));
            EditWork_Pay_Type = new RoutedUICommand("Редактировать запись по виду оплат", "EditWork_Pay_Type", typeof(AppCommands));
            DeleteWork_Pay_Type = new RoutedUICommand("Удалить запись по виду оплат", "DeleteWork_Pay_Type", typeof(AppCommands));
            TimePercoToTimeGraph = new RoutedUICommand("Принять время по проходам (время по проходам будет записано во время по графику)", "TimePercoToTimeGraph", typeof(AppCommands));
            TimePayTypeToTimePerco = new RoutedUICommand("Принять время по текущему виду оплат (время по виду оплат будет записано во время по проходам)", "TimePayTypeToTimePerco", typeof(AppCommands));
            TimeSumPayTypeToTimePerco = new RoutedUICommand("Принять общее время по видам оплат (время по всем видам оплат будет записано во время по проходам)", "TimeSumPayTypeToTimePerco", typeof(AppCommands));
            CalcWorked_Day = new RoutedUICommand("Пересчитать время", "CalcWorked_Day", typeof(AppCommands));
            EditOrderPayType = new RoutedUICommand("Редактировать заказ по текущему виду оплат", "EditOrderPayType", typeof(AppCommands));
            SaveDocWorked_Day = new RoutedUICommand("Сохранить изменения в документах", "SaveDocWorked_Day", typeof(AppCommands));
            BtAddAbsence = new RoutedUICommand("Добавить запись в отгулы", "BtAddAbsence", typeof(AppCommands));
            BtDeleteAbsence = new RoutedUICommand("Удалить запись из отгулов", "BtDeleteAbsence", typeof(AppCommands));

            CompareTransferWithWorkedDay = new RoutedUICommand("Сопоставить переводы с днями работы", "CompareTransferAndWorkDay", typeof(AppCommands));
            // = new RoutedUICommand("", "", typeof(AppCommands));
            #endregion

            /*********************************************************************************************************************************/
            /****     ///    ///   ///     ///////////////             ///       ///////////////                                      ********/
            /****     ///    ///   ///          ////                 /////            ////                                            ********/
            /****     ///    ///   ///          ////                //////            ////                                            ********/
            /****     ///    ///   ///          ////               /// ///            ////                                            ********/
            /****     ///    ///   ///          ////              ///  ///            ////                                            ********/
            /****     ///    ///   ///          ////             /////////            ////                                            ********/
            /****     ///    ///   ///          ////            ///    ///            ////                                            ********/
            /****     ////////////////          ////           ///     ///            ////                                            ********/
            /*********************************************************************************************************************************/
            #region  Команды штатного расписания

            OpenViewManningTable = new RoutedUICommand("Просмотр штатного расписания", "ViewManningTable", typeof(AppCommands));

            OpenViewManningProject = new RoutedUICommand("Просмотр проектов штатного расписания", "ViewManningProject", typeof(AppCommands));
            OpenViewManningTableEmp = new RoutedUICommand("Просмотр штатной расстановки", "ViewManningTableEmp", typeof(AppCommands));

            AddEmpStaff = new RoutedUICommand("Добавить в штатную расстановку работника", "EditEmpStaff", typeof(AppCommands));
            EditEmpStaff = new RoutedUICommand("Редактировать штатную расстановку", "EditEmpStaff", typeof(AppCommands));
            DeleteEmpStaff = new RoutedUICommand("Удалить из штатной расстановки работника", "EditEmpStaff", typeof(AppCommands));
            SaveEmpStaff = new RoutedUICommand("Сохранить штатную расстановку", "EditEmpStaff", typeof(AppCommands));

            AddStaff = new RoutedUICommand("Добавить штатные единицы", "EditStaff", typeof(AppCommands));
            EditStaff = new RoutedUICommand("Редактировать штатные единицы", "EditStaff", typeof(AppCommands));
            DeleteStaff = new RoutedUICommand("Удалить штатные единицы", "EditStaff", typeof(AppCommands));
            SaveStaff = new RoutedUICommand("Сохранить штатное расписание", "EditStaff", typeof(AppCommands));
            ChooseStaffWorkPlace = new RoutedUICommand("Выбрать рабочее место", "EditStaff", typeof(AppCommands));
            DeleteStaffWorkPlace = new RoutedUICommand("Убрать карточку из штатной единицы", "EditStaff", typeof(AppCommands));

            SaveStaffSection = new RoutedUICommand("Сохранить раздел", "EditManningCatalog", typeof(AppCommands));
            AddStaffSection = new RoutedUICommand("Добавить раздел", "EditManningCatalog", typeof(AppCommands));
            EditStaffSection = new RoutedUICommand("Редактировать раздел", "EditManningCatalog", typeof(AppCommands));
            DeleteStaffSection = new RoutedUICommand("Удалить раздел", "EditManningCatalog", typeof(AppCommands));

            SaveSubdivTypePart = new RoutedUICommand("Сохранить типы внутриструктурных подразделений", "EditManningCatalog", typeof(AppCommands));
            AddSubdivTypePart = new RoutedUICommand("Добавить тип подструктуры", "EditManningCatalog", typeof(AppCommands));
            DeleteSubdivTypePart = new RoutedUICommand("Удалить тип подструктуры", "EditManningCatalog", typeof(AppCommands));

            ViewSubdivPartition = new RoutedUICommand("Просмотр структурного деления предприятия", "ViewManningCatalog", typeof(AppCommands));
            AddSubdivPartition = new RoutedUICommand("Добавить внутриструктурное подразделение", "EditManningCatalog", typeof(AppCommands));
            EditSubdivPartition = new RoutedUICommand("Редактировать внутриструктурное подразделение", "EditManningCatalog", typeof(AppCommands));
            DeleteSubdivPartition = new RoutedUICommand("Удалить внутриструктурное подразделение", "EditManningCatalog", typeof(AppCommands));
            SaveSubdivPartition = new RoutedUICommand("Сохранить внутриструктурное подразделение", "EditManningCatalog", typeof(AppCommands));

            ViewIndividProtectionCatalog = new RoutedUICommand("Средства индивидуальной защиты", "ViewIndividProtection", typeof(AppCommands));
            SaveIndividProtection = new RoutedUICommand("Сохранить справочник СИЗ", "EditIndividProtectionType", typeof(AppCommands));

            ViewWorkPlace = new RoutedUICommand("Просмотр рабочих мест предприятия", "ViewWorkPlace", typeof(AppCommands));
            AddWorkPlace = new RoutedUICommand("Добавить карту рабочего места", "EditWorkPlace", typeof(AppCommands));
            EditWorkPlace = new RoutedUICommand("Редактировать карту рабочего места", "EditWorkPlace", typeof(AppCommands));
            DeleteWorkPlace = new RoutedUICommand("Удалить карту рабочего места", "EditWorkPlace", typeof(AppCommands));
            SaveWorkPlace = new RoutedUICommand("Сохранить карту рабочего места", "EditWorkPlace", typeof(AppCommands));
            AddWPProtection = new RoutedUICommand("Добавить СИЗ к рабочему месту", "EditWorkPlace", typeof(AppCommands));
            DeleteWPProtection = new RoutedUICommand("Удалить СИЗ из рабочего места", "EditWorkPlace", typeof(AppCommands));

            AddGrWork = new RoutedUICommand("Добавить новый график работы", "EditGrWorkInManningTable", typeof(AppCommands));
            EditGrWork = new RoutedUICommand("Редактировать график работы", "EditGrWorkInManningTable", typeof(AppCommands));
            DeleteGrWork = new RoutedUICommand("Удалить график работы", "EditGrWorkInManningTable", typeof(AppCommands));

            #endregion


            #region Графики отпусков

            AddNewVacCommand = new RoutedUICommand("Добавить новый отпуск", "btAddVS", typeof(AppCommands));
            EditVacCommand = new RoutedUICommand("Редактировать отпуск", "btEditVS", typeof(AppCommands));
            BtPrivateKardVS = new RoutedUICommand("Просмотр карточки сотрудника", "btPrivateKardVS", typeof(AppCommands));
            ConfirmVSCommand = new RoutedUICommand("Согласовать отпуска", "btConfirmVS", typeof(AppCommands));
            BtCancelConfirmVS = new RoutedUICommand("Отменить согласование", "btCancelConfirmVS", typeof(AppCommands));
            AllSubdivVacConfirmStatistic = new RoutedUICommand("Статистика согласования отпусков", "tsbtAllSubdivVacConfirmStatistic", typeof(AppCommands));

            RecheckVacs = new RoutedUICommand("Проверить отпуска сотрудников", "btConfirmVS", typeof(AppCommands));

            BtViewArchivVS = new RoutedUICommand("Архив отпусков для уволенных сотрудников", "btArchivVS", typeof(AppCommands));
            BtViewMakeVS = new RoutedUICommand("Общий список сотрудников для отпусков", "btMakeVS", typeof(AppCommands));
            BtConfirmVS = new RoutedUICommand("Формирование плана на год", "btConfirmVS", typeof(AppCommands));
            BtDiagramsVS = new RoutedUICommand("Диаграммы по графикам отпусков", "btDiagramsVS", typeof(AppCommands));
            BtViewEmpLocationVac = new RoutedUICommand("Разделение сотрудников по участкам/бюро", "btViewEmpLocationVac", typeof(AppCommands));

            BtPlanOnYearVS = new RoutedUICommand("Отчет план-график на год", "btPlanOnYearVS", typeof(AppCommands));
            Bt_alloc_on_Months_VS = new RoutedUICommand("Распределение отпусков по месяцам", "bt_alloc_on_Months_VS", typeof(AppCommands));
            BtSvodVS = new RoutedUICommand("Сводный график отпусков", "btSvodVS", typeof(AppCommands));
            BtVSByGroupMaster = new RoutedUICommand("Отпуска по группам мастера", "btVSByGroupMaster", typeof(AppCommands));
            R_btActualVacByRegion = new RoutedUICommand("Отчет фактических отпусков по группе/бюро/участку", "R_btActualVacByRegion", typeof(AppCommands));
            R_btConsolidVacByRegion = new RoutedUICommand("Сводный отчет по группе/бюро/участку", "R_btConsolidVacByRegion", typeof(AppCommands));
            BtVS_RemainVacs = new RoutedUICommand("Неиспользованные отпуска", "btVS_RemainVacs", typeof(AppCommands));
            BtListVsBlock = new RoutedUICommand("Список сотрудников для блокировки во время отпуска", "btListVsBlock", typeof(AppCommands));

            R_btPrikazZavodVS = new RoutedUICommand("Приказ по заводу", "R_btPrikazZavodVS", typeof(AppCommands));
            R_btSubPrikazVS = new RoutedUICommand("Приказ по подразделению", "R_btSubPrikazVS", typeof(AppCommands));
            TsbtRNoteVacVS = new RoutedUICommand("Уведомление об отпуске", "tsbtRNoteVacVS", typeof(AppCommands));
            BtNoteAccountVS = new RoutedUICommand("Записка-расчет", "btNoteAccountVS", typeof(AppCommands));

            R_btActualDatesByPeriod = new RoutedUICommand("Записка-расчет", "R_btActualDatesByPeriod", typeof(AppCommands));
            R_CountPlanSumDaysVS = new RoutedUICommand("Записка-расчет", "R_CountPlanSumDaysVS", typeof(AppCommands));
            BtAggReservDataPeriodVS = new RoutedUICommand("Записка-расчет", "btAggReservDataPeriodVS", typeof(AppCommands));



            #endregion
        }

        public static RoutedUICommand BtMainDb
        {
            get { return AppCommands._btMainDb; }
            set { AppCommands._btMainDb = value; }
        }

        public static RoutedUICommand BtArchives
        {
            get { return AppCommands._btArchives; }
            set { AppCommands._btArchives = value; }
        }

        public static RoutedUICommand BtTempDb
        {
            get { return AppCommands._btTempDb; }
            set { AppCommands._btTempDb = value; }
        }

        public static RoutedUICommand BtResume
        {
            get { return AppCommands._btResume; }
            set { AppCommands._btResume = value; }
        }

        public static RoutedUICommand BtEditEmp
        {
            get { return AppCommands._btEditEmp; }
            set { AppCommands._btEditEmp = value; }
        }

        public static RoutedUICommand BtViewTransfer
        {
            get { return AppCommands._btViewTransfer; }
            set { AppCommands._btViewTransfer = value; }
        }

        public static RoutedUICommand DdTransfer
        {
            get { return AppCommands._ddTransfer; }
            set { AppCommands._ddTransfer = value; }
        }

        public static RoutedUICommand DdTransferNow
        {
            get { return AppCommands._ddTransferNow; }
            set { AppCommands._ddTransferNow = value; }
        }

        public static RoutedUICommand BtAddTransfer
        {
            get { return AppCommands._btAddTransfer; }
            set { AppCommands._btAddTransfer = value; }
        }

        public static RoutedUICommand BtEditTransfer
        {
            get { return AppCommands._btEditTransfer; }
            set { AppCommands._btEditTransfer = value; }
        }

        public static RoutedUICommand BtDeleteTransfer
        {
            get { return AppCommands._btDeleteTransfer; }
            set { AppCommands._btDeleteTransfer = value; }
        }

        public static RoutedUICommand BtTransferComb
        {
            get { return AppCommands._btTransferComb; }
            set { AppCommands._btTransferComb = value; }
        }

        public static RoutedUICommand DdTransferOld
        {
            get { return AppCommands._ddTransferOld; }
            set { AppCommands._ddTransferOld = value; }
        }

        public static RoutedUICommand BtAddOld
        {
            get { return AppCommands._btAddOld; }
            set { AppCommands._btAddOld = value; }
        }

        public static RoutedUICommand BtEditOld
        {
            get { return AppCommands._btEditOld; }
            set { AppCommands._btEditOld = value; }
        }

        public static RoutedUICommand BtDeleteOld
        {
            get { return AppCommands._btDeleteOld; }
            set { AppCommands._btDeleteOld = value; }
        }

        public static RoutedUICommand BtRecoveryTransfer
        {
            get { return AppCommands._btRecoveryTransfer; }
            set { AppCommands._btRecoveryTransfer = value; }
        }

        public static RoutedUICommand BtReverseTransfer
        {
            get { return AppCommands._btReverseTransfer; }
            set { AppCommands._btReverseTransfer = value; }
        }

        public static RoutedUICommand BtAdd_Agreement_For_Emp
        {
            get { return AppCommands._btAdd_Agreement_For_Emp; }
            set { AppCommands._btAdd_Agreement_For_Emp = value; }
        }

        public static RoutedUICommand BtDismiss
        {
            get { return AppCommands._btDismiss; }
            set { AppCommands._btDismiss = value; }
        }

        public static RoutedUICommand BtDismissToFR
        {
            get { return AppCommands._btDismissToFR; }
            set { AppCommands._btDismissToFR = value; }
        }

        public static RoutedUICommand BtDismissComb
        {
            get { return AppCommands._btDismissComb; }
            set { AppCommands._btDismissComb = value; }
        }

        public static RoutedUICommand BtProject_Order_Dismiss
        {
            get { return AppCommands._btProject_Order_Dismiss; }
            set { AppCommands._btProject_Order_Dismiss = value; }
        }

        public static RoutedUICommand BtTransfer_Term
        {
            get { return AppCommands._btTransfer_Term; }
            set { AppCommands._btTransfer_Term = value; }
        }

        public static RoutedUICommand BtTransferCond_Of_Work
        {
            get { return AppCommands._btTransferCond_Of_Work; }
            set { AppCommands._btTransferCond_Of_Work = value; }
        }

        public static RoutedUICommand BtTransfer_Emp_For_Group_Hire
        {
            get { return AppCommands._btTransfer_Emp_For_Group_Hire; }
            set { AppCommands._btTransfer_Emp_For_Group_Hire = value; }
        }

        public static RoutedUICommand BtBase_Exchange
        {
            get { return AppCommands._btBase_Exchange; }
            set { AppCommands._btBase_Exchange = value; }
        }

        public static RoutedUICommand BtProject_Statement
        {
            get { return AppCommands._btProject_Statement; }
            set { AppCommands._btProject_Statement = value; }
        }

        public static RoutedUICommand DdReportGH
        {
            get { return AppCommands._ddReportGH; }
            set { AppCommands._ddReportGH = value; }
        }

        public static RoutedUICommand BtRepCountHire
        {
            get { return AppCommands._btRepCountHire; }
            set { AppCommands._btRepCountHire = value; }
        }

        public static RoutedUICommand BtListOrdHire
        {
            get { return AppCommands._btListOrdHire; }
            set { AppCommands._btListOrdHire = value; }
        }

        public static RoutedUICommand DdBkOrdHire
        {
            get { return AppCommands._ddBkOrdHire; }
            set { AppCommands._ddBkOrdHire = value; }
        }

        public static RoutedUICommand BtBkOrdHireForPeriod
        {
            get { return AppCommands._btBkOrdHireForPeriod; }
            set { AppCommands._btBkOrdHireForPeriod = value; }
        }

        public static RoutedUICommand BtBkOrdHireYear
        {
            get { return AppCommands._btBkOrdHireYear; }
            set { AppCommands._btBkOrdHireYear = value; }
        }

        public static RoutedUICommand BtViolations_By_Period
        {
            get { return AppCommands._btViolations_By_Period; }
            set { AppCommands._btViolations_By_Period = value; }
        }

        public static RoutedUICommand DdReportPers
        {
            get { return AppCommands._ddReportPers; }
            set { AppCommands._ddReportPers = value; }
        }

        public static RoutedUICommand BtRepCountDismiss
        {
            get { return AppCommands._btRepCountDismiss; }
            set { AppCommands._btRepCountDismiss = value; }
        }

        public static RoutedUICommand BtRepCountDismissR
        {
            get { return AppCommands._btRepCountDismissR; }
            set { AppCommands._btRepCountDismissR = value; }
        }

        public static RoutedUICommand BtRepReasonDismiss1
        {
            get { return AppCommands._btRepReasonDismiss1; }
            set { AppCommands._btRepReasonDismiss1 = value; }
        }

        public static RoutedUICommand BtRepReasonDismiss2
        {
            get { return AppCommands._btRepReasonDismiss2; }
            set { AppCommands._btRepReasonDismiss2 = value; }
        }

        public static RoutedUICommand BtListOrdDismiss
        {
            get { return AppCommands._btListOrdDismiss; }
            set { AppCommands._btListOrdDismiss = value; }
        }

        public static RoutedUICommand BtEndOfContr
        {
            get { return AppCommands._btEndOfContr; }
            set { AppCommands._btEndOfContr = value; }
        }

        public static RoutedUICommand BtOrdDismiss
        {
            get { return AppCommands._btOrdDismiss; }
            set { AppCommands._btOrdDismiss = value; }
        }

        public static RoutedUICommand BtOrdTransfer
        {
            get { return AppCommands._btOrdTransfer; }
            set { AppCommands._btOrdTransfer = value; }
        }

        public static RoutedUICommand DdListDismiss
        {
            get { return AppCommands._ddListDismiss; }
            set { AppCommands._ddListDismiss = value; }
        }

        public static RoutedUICommand BtListDismissFull
        {
            get { return AppCommands._btListDismissFull; }
            set { AppCommands._btListDismissFull = value; }
        }

        public static RoutedUICommand BtListDismissShort
        {
            get { return AppCommands._btListDismissShort; }
            set { AppCommands._btListDismissShort = value; }
        }

        public static RoutedUICommand BtListDismissForDir
        {
            get { return AppCommands._btListDismissForDir; }
            set { AppCommands._btListDismissForDir = value; }
        }

        public static RoutedUICommand BtListDismissJobman
        {
            get { return AppCommands._btListDismissJobman; }
            set { AppCommands._btListDismissJobman = value; }
        }

        public static RoutedUICommand BtListAcadDegree
        {
            get { return AppCommands._btListAcadDegree; }
            set { AppCommands._btListAcadDegree = value; }
        }

        public static RoutedUICommand BtRep_Staff_Subdiv
        {
            get { return AppCommands._btRep_Staff_Subdiv; }
            set { AppCommands._btRep_Staff_Subdiv = value; }
        }

        public static RoutedUICommand DdNotice_Expiry
        {
            get { return AppCommands._ddNotice_Expiry; }
            set { AppCommands._ddNotice_Expiry = value; }
        }

        public static RoutedUICommand BtNotice_Expiry_Contract
        {
            get { return AppCommands._btNotice_Expiry_Contract; }
            set { AppCommands._btNotice_Expiry_Contract = value; }
        }

        public static RoutedUICommand BtNotice_Expiry_Add_Agreement
        {
            get { return AppCommands._btNotice_Expiry_Add_Agreement; }
            set { AppCommands._btNotice_Expiry_Add_Agreement = value; }
        }

        public static RoutedUICommand DdBlank_Reference
        {
            get { return AppCommands._ddBlank_Reference; }
            set { AppCommands._ddBlank_Reference = value; }
        }

        public static RoutedUICommand BtReference_On_Place_Requirements
        {
            get { return AppCommands._btReference_On_Place_Requirements; }
            set { AppCommands._btReference_On_Place_Requirements = value; }
        }

        public static RoutedUICommand BtReference_Child_Realing_Leave
        {
            get { return AppCommands._btReference_Child_Realing_Leave; }
            set { AppCommands._btReference_Child_Realing_Leave = value; }
        }

        public static RoutedUICommand DdRepStaff_Pension
        {
            get { return AppCommands._ddRepStaff_Pension; }
            set { AppCommands._ddRepStaff_Pension = value; }
        }

        public static RoutedUICommand BtRepNonState_Pens_Prov
        {
            get { return AppCommands._btRepNonState_Pens_Prov; }
            set { AppCommands._btRepNonState_Pens_Prov = value; }
        }

        public static RoutedUICommand BtRepNPP_With_Date
        {
            get { return AppCommands._btRepNPP_With_Date; }
            set { AppCommands._btRepNPP_With_Date = value; }
        }

        public static RoutedUICommand DdListInvalid
        {
            get { return AppCommands._ddListInvalid; }
            set { AppCommands._ddListInvalid = value; }
        }

        public static RoutedUICommand BtListInvalid
        {
            get { return AppCommands._btListInvalid; }
            set { AppCommands._btListInvalid = value; }
        }

        public static RoutedUICommand BtListInvalidSub
        {
            get { return AppCommands._btListInvalidSub; }
            set { AppCommands._btListInvalidSub = value; }
        }

        public static RoutedUICommand DdRetirer
        {
            get { return AppCommands._ddRetirer; }
            set { AppCommands._ddRetirer = value; }
        }

        public static RoutedUICommand DdRetirerStruct
        {
            get { return AppCommands._ddRetirerStruct; }
            set { AppCommands._ddRetirerStruct = value; }
        }

        public static RoutedUICommand BtListRetirerStruct
        {
            get { return AppCommands._btListRetirerStruct; }
            set { AppCommands._btListRetirerStruct = value; }
        }

        public static RoutedUICommand BtListRetirerStructSubdiv
        {
            get { return AppCommands._btListRetirerStructSubdiv; }
            set { AppCommands._btListRetirerStructSubdiv = value; }
        }

        public static RoutedUICommand DdListRetirer
        {
            get { return AppCommands._ddListRetirer; }
            set { AppCommands._ddListRetirer = value; }
        }

        public static RoutedUICommand BtListRetirerPlant
        {
            get { return AppCommands._btListRetirerPlant; }
            set { AppCommands._btListRetirerPlant = value; }
        }

        public static RoutedUICommand BtListRetirerSubdiv
        {
            get { return AppCommands._btListRetirerSubdiv; }
            set { AppCommands._btListRetirerSubdiv = value; }
        }

        public static RoutedUICommand BtListRetirerDismiss
        {
            get { return AppCommands._btListRetirerDismiss; }
            set { AppCommands._btListRetirerDismiss = value; }
        }

        public static RoutedUICommand BtEmptyInsurNum
        {
            get { return AppCommands._btEmptyInsurNum; }
            set { AppCommands._btEmptyInsurNum = value; }
        }

        public static RoutedUICommand BtSprObj
        {
            get { return AppCommands._btSprObj; }
            set { AppCommands._btSprObj = value; }
        }

        public static RoutedUICommand BtPersonalCard
        {
            get { return AppCommands._btPersonalCard; }
            set { AppCommands._btPersonalCard = value; }
        }

        public static RoutedUICommand BtRepOtherType
        {
            get { return AppCommands._btRepOtherType; }
            set { AppCommands._btRepOtherType = value; }
        }

        public static RoutedUICommand BtRepMRP
        {
            get { return AppCommands._btRepMRP; }
            set { AppCommands._btRepMRP = value; }
        }

        public static RoutedUICommand DdReportMSFO
        {
            get { return AppCommands._ddReportMSFO; }
            set { AppCommands._ddReportMSFO = value; }
        }

        public static RoutedUICommand BtRepMSFOSal
        {
            get { return AppCommands._btRepMSFOSal; }
            set { AppCommands._btRepMSFOSal = value; }
        }

        public static RoutedUICommand BtRepMSFODisEmp
        {
            get { return AppCommands._btRepMSFODisEmp; }
            set { AppCommands._btRepMSFODisEmp = value; }
        }

        public static RoutedUICommand DdReportsPrivPos
        {
            get { return AppCommands._ddReportsPrivPos; }
            set { AppCommands._ddReportsPrivPos = value; }
        }

        public static RoutedUICommand BtListEmpPrivPos
        {
            get { return AppCommands._btListEmpPrivPos; }
            set { AppCommands._btListEmpPrivPos = value; }
        }

        public static RoutedUICommand BtTransferPrivPos
        {
            get { return AppCommands._btTransferPrivPos; }
            set { AppCommands._btTransferPrivPos = value; }
        }

        public static RoutedUICommand BtRepChild_Realing_Leave
        {
            get { return AppCommands._btRepChild_Realing_Leave; }
            set { AppCommands._btRepChild_Realing_Leave = value; }
        }

        public static RoutedUICommand BtEmp_By_Service
        {
            get { return AppCommands._btEmp_By_Service; }
            set { AppCommands._btEmp_By_Service = value; }
        }

        public static RoutedUICommand DdVeteran
        {
            get { return AppCommands._ddVeteran; }
            set { AppCommands._ddVeteran = value; }
        }

        public static RoutedUICommand DdNumbersVeteran
        {
            get { return AppCommands._ddNumbersVeteran; }
            set { AppCommands._ddNumbersVeteran = value; }
        }

        public static RoutedUICommand BtNumbersVeteranPlant
        {
            get { return AppCommands._btNumbersVeteranPlant; }
            set { AppCommands._btNumbersVeteranPlant = value; }
        }

        public static RoutedUICommand BtNumbersVeteranSub
        {
            get { return AppCommands._btNumbersVeteranSub; }
            set { AppCommands._btNumbersVeteranSub = value; }
        }

        public static RoutedUICommand DdListVeteran
        {
            get { return AppCommands._ddListVeteran; }
            set { AppCommands._ddListVeteran = value; }
        }

        public static RoutedUICommand BtListVeteranPlant
        {
            get { return AppCommands._btListVeteranPlant; }
            set { AppCommands._btListVeteranPlant = value; }
        }

        public static RoutedUICommand BtListVeteranSub
        {
            get { return AppCommands._btListVeteranSub; }
            set { AppCommands._btListVeteranSub = value; }
        }

        public static RoutedUICommand BtListDismissVeteran
        {
            get { return AppCommands._btListDismissVeteran; }
            set { AppCommands._btListDismissVeteran = value; }
        }

        public static RoutedUICommand BtRepTransfer_By_Period
        {
            get { return AppCommands._btRepTransfer_By_Period; }
            set { AppCommands._btRepTransfer_By_Period = value; }
        }

        public static RoutedUICommand BtRepYoung_Specialist
        {
            get { return AppCommands._btRepYoung_Specialist; }
            set { AppCommands._btRepYoung_Specialist = value; }
        }

        public static RoutedUICommand BtOrderOnEncouraging
        {
            get { return AppCommands._btOrderOnEncouraging; }
            set { AppCommands._btOrderOnEncouraging = value; }
        }

        public static RoutedUICommand BtAdd_Agreement_On_Subdiv
        {
            get { return AppCommands._btAdd_Agreement_On_Subdiv; }
            set { AppCommands._btAdd_Agreement_On_Subdiv = value; }
        }

        public static RoutedUICommand DdAdd_Agreement_ShortDay
        {
            get { return AppCommands._ddAdd_Agreement_ShortDay; }
            set { AppCommands._ddAdd_Agreement_ShortDay = value; }
        }

        public static RoutedUICommand BtAdd_Agreement_On_ShortDay1
        {
            get { return AppCommands._btAdd_Agreement_On_ShortDay1; }
            set { AppCommands._btAdd_Agreement_On_ShortDay1 = value; }
        }

        public static RoutedUICommand BtAdd_Agreement_On_ShortDay2
        {
            get { return AppCommands._btAdd_Agreement_On_ShortDay2; }
            set { AppCommands._btAdd_Agreement_On_ShortDay2 = value; }
        }

        public static RoutedUICommand BtRepChild_Realing_Leave2
        {
            get { return AppCommands._btRepChild_Realing_Leave2; }
            set { AppCommands._btRepChild_Realing_Leave2 = value; }
        }

        public static RoutedUICommand DdNotification_For_Emp
        {
            get { return AppCommands._ddNotification_For_Emp; }
            set { AppCommands._ddNotification_For_Emp = value; }
        }

        public static RoutedUICommand BtNotification_Short_Week
        {
            get { return AppCommands._btNotification_Short_Week; }
            set { AppCommands._btNotification_Short_Week = value; }
        }

        public static RoutedUICommand BtNotification_Early_Ending
        {
            get { return AppCommands._btNotification_Early_Ending; }
            set { AppCommands._btNotification_Early_Ending = value; }
        }

        public static RoutedUICommand BtCount_Short_Graph
        {
            get { return AppCommands._btCount_Short_Graph; }
            set { AppCommands._btCount_Short_Graph = value; }
        }

        public static RoutedUICommand BtPermit
        {
            get { return AppCommands._btPermit; }
            set { AppCommands._btPermit = value; }
        }

        public static RoutedUICommand BtEditFilterPermit
        {
            get { return AppCommands._btEditFilterPermit; }
            set { AppCommands._btEditFilterPermit = value; }
        }

        public static RoutedUICommand BtType_Permit
        {
            get { return AppCommands._btType_Permit; }
            set { AppCommands._btType_Permit = value; }
        }

        public static RoutedUICommand BtViewViolators
        {
            get { return AppCommands._btViewViolators; }
            set { AppCommands._btViewViolators = value; }
        }

        public static RoutedUICommand BtDismissed_Emp
        {
            get { return AppCommands._btDismissed_Emp; }
            set { AppCommands._btDismissed_Emp = value; }
        }

        public static RoutedUICommand BtTransfer_Emp
        {
            get { return AppCommands._btTransfer_Emp; }
            set { AppCommands._btTransfer_Emp = value; }
        }

        public static RoutedUICommand BtAccess_Template
        {
            get { return AppCommands._btAccess_Template; }
            set { AppCommands._btAccess_Template = value; }
        }

        public static RoutedUICommand BtList_Emp_With_Template
        {
            get { return AppCommands._btList_Emp_With_Template; }
            set { AppCommands._btList_Emp_With_Template = value; }
        }

        public static RoutedUICommand BtType_Study
        {
            get { return AppCommands._btType_Study; }
            set { AppCommands._btType_Study = value; }
        }

        public static RoutedUICommand BtType_Edu
        {
            get { return AppCommands._btType_Edu; }
            set { AppCommands._btType_Edu = value; }
        }

        public static RoutedUICommand BtInstit
        {
            get { return AppCommands._btInstit; }
            set { AppCommands._btInstit = value; }
        }

        public static RoutedUICommand BtStudy_City
        {
            get { return AppCommands._btStudy_City; }
            set { AppCommands._btStudy_City = value; }
        }

        public static RoutedUICommand BtGroup_Spec
        {
            get { return AppCommands._btGroup_Spec; }
            set { AppCommands._btGroup_Spec = value; }
        }

        public static RoutedUICommand BtQual
        {
            get { return AppCommands._btQual; }
            set { AppCommands._btQual = value; }
        }

        public static RoutedUICommand BtSpeciality
        {
            get { return AppCommands._btSpeciality; }
            set { AppCommands._btSpeciality = value; }
        }

        public static RoutedUICommand BtLang
        {
            get { return AppCommands._btLang; }
            set { AppCommands._btLang = value; }
        }

        public static RoutedUICommand BtLevel_Know
        {
            get { return AppCommands._btLevel_Know; }
            set { AppCommands._btLevel_Know = value; }
        }

        public static RoutedUICommand BtComm
        {
            get { return AppCommands._btComm; }
            set { AppCommands._btComm = value; }
        }

        public static RoutedUICommand BtMil_Spec
        {
            get { return AppCommands._btMil_Spec; }
            set { AppCommands._btMil_Spec = value; }
        }

        public static RoutedUICommand BtMed_Classif
        {
            get { return AppCommands._btMed_Classif; }
            set { AppCommands._btMed_Classif = value; }
        }

        public static RoutedUICommand BtMil_Rank
        {
            get { return AppCommands._btMil_Rank; }
            set { AppCommands._btMil_Rank = value; }
        }

        public static RoutedUICommand BtMil_Cat
        {
            get { return AppCommands._btMil_Cat; }
            set { AppCommands._btMil_Cat = value; }
        }

        public static RoutedUICommand BtType_Troop
        {
            get { return AppCommands._btType_Troop; }
            set { AppCommands._btType_Troop = value; }
        }

        public static RoutedUICommand BtType_Priv
        {
            get { return AppCommands._btType_Priv; }
            set { AppCommands._btType_Priv = value; }
        }

        public static RoutedUICommand BtType_Rise_Qual
        {
            get { return AppCommands._btType_Rise_Qual; }
            set { AppCommands._btType_Rise_Qual = value; }
        }

        public static RoutedUICommand BtPrivileged_Position
        {
            get { return AppCommands._btPrivileged_Position; }
            set { AppCommands._btPrivileged_Position = value; }
        }

        public static RoutedUICommand BtType_Postg_Study
        {
            get { return AppCommands._btType_Postg_Study; }
            set { AppCommands._btType_Postg_Study = value; }
        }

        public static RoutedUICommand BtSource_Employability
        {
            get { return AppCommands._btSource_Employability; }
            set { AppCommands._btSource_Employability = value; }
        }

        public static RoutedUICommand BtReason_dismiss
        {
            get { return AppCommands._btReason_dismiss; }
            set { AppCommands._btReason_dismiss = value; }
        }

        public static RoutedUICommand BtPosition
        {
            get { return AppCommands._btPosition; }
            set { AppCommands._btPosition = value; }
        }

        public static RoutedUICommand BtSubdiv
        {
            get { return AppCommands._btSubdiv; }
            set { AppCommands._btSubdiv = value; }
        }

        public static RoutedUICommand BtReward_Name
        {
            get { return AppCommands._btReward_Name; }
            set { AppCommands._btReward_Name = value; }
        }

        public static RoutedUICommand BtType_Reward
        {
            get { return AppCommands._btType_Reward; }
            set { AppCommands._btType_Reward = value; }
        }

        public static RoutedUICommand BtConditions_Of_Work
        {
            get { return AppCommands._btConditions_Of_Work; }
            set { AppCommands._btConditions_Of_Work = value; }
        }

        public static RoutedUICommand BtType_Condition
        {
            get { return AppCommands._btType_Condition; }
            set { AppCommands._btType_Condition = value; }
        }

        public static RoutedUICommand BtPrevFR_Emp
        {
            get { return AppCommands._btPrevFR_Emp; }
            set { AppCommands._btPrevFR_Emp = value; }
        }

        public static RoutedUICommand BtReleaseBuffer
        {
            get { return AppCommands._btReleaseBuffer; }
            set { AppCommands._btReleaseBuffer = value; }
        }

        public static RoutedUICommand BtFilterFR_Emp
        {
            get { return AppCommands._btFilterFR_Emp; }
            set { AppCommands._btFilterFR_Emp = value; }
        }

        public static RoutedUICommand BtFindFR_Emp
        {
            get { return AppCommands._btFindFR_Emp; }
            set { AppCommands._btFindFR_Emp = value; }
        }

        public static RoutedUICommand BtSubdivSort
        {
            get { return AppCommands._btSubdivSort; }
            set { AppCommands._btSubdivSort = value; }
        }

        public static RoutedUICommand BtFIOSort
        {
            get { return AppCommands._btFIOSort; }
            set { AppCommands._btFIOSort = value; }
        }

        public static RoutedUICommand BtSubdivFIOSort
        {
            get { return AppCommands._btSubdivFIOSort; }
            set { AppCommands._btSubdivFIOSort = value; }
        }

        public static RoutedUICommand BtPositionSort
        {
            get { return AppCommands._btPositionSort; }
            set { AppCommands._btPositionSort = value; }
        }

        public static RoutedUICommand BtDate_StartSort
        {
            get { return AppCommands._btDate_StartSort; }
            set { AppCommands._btDate_StartSort = value; }
        }

        public static RoutedUICommand BtDataFill
        {
            get { return AppCommands._btDataFill; }
            set { AppCommands._btDataFill = value; }
        }

        public static RoutedUICommand Bt_sbros_IBM
        {
            get { return AppCommands._bt_sbros_IBM; }
            set { AppCommands._bt_sbros_IBM = value; }
        }

        public static RoutedUICommand BtFilterRas
        {
            get { return AppCommands._btFilterRas; }
            set { AppCommands._btFilterRas = value; }
        }

        public static RoutedUICommand BtFindRas
        {
            get { return AppCommands._btFindRas; }
            set { AppCommands._btFindRas = value; }
        }

        public static RoutedUICommand BtUpdateRas
        {
            get { return AppCommands._btUpdateRas; }
            set { AppCommands._btUpdateRas = value; }
        }

        public static RoutedUICommand BtView_All_Transfer
        {
            get { return AppCommands._btView_All_Transfer; }
            set { AppCommands._btView_All_Transfer = value; }
        }

        public static RoutedUICommand BtOutside_Emp
        {
            get { return AppCommands._btOutside_Emp; }
            set { AppCommands._btOutside_Emp = value; }
        }

        public static RoutedUICommand BtPrintAD
        {
            get { return AppCommands._btPrintAD; }
            set { AppCommands._btPrintAD = value; }
        }

        public static RoutedUICommand R_btRasCombList
        {
            get { return AppCommands._R_btRasCombList; }
            set { AppCommands._R_btRasCombList = value; }
        }

        public static RoutedUICommand R_btRASInvalidEmp
        {
            get { return AppCommands._R_btRASInvalidEmp; }
            set { AppCommands._R_btRASInvalidEmp = value; }
        }

        public static RoutedUICommand R_btRASProfEmp
        {
            get { return AppCommands._R_btRASProfEmp; }
            set { AppCommands._R_btRASProfEmp = value; }
        }

        public static RoutedUICommand R_btRASNonProfEmp
        {
            get { return AppCommands._R_btRASNonProfEmp; }
            set { AppCommands._R_btRASNonProfEmp = value; }
        }

        public static RoutedUICommand BtProfUnion_Entered
        {
            get { return AppCommands._btProfUnion_Entered; }
            set { AppCommands._btProfUnion_Entered = value; }
        }

        public static RoutedUICommand BtProfUnion_CameOut
        {
            get { return AppCommands._btProfUnion_CameOut; }
            set { AppCommands._btProfUnion_CameOut = value; }
        }

        public static RoutedUICommand BtProtocolDump
        {
            get { return AppCommands._btProtocolDump; }
            set { AppCommands._btProtocolDump = value; }
        }

        public static RoutedUICommand BtRepTransSub
        {
            get { return AppCommands._btRepTransSub; }
            set { AppCommands._btRepTransSub = value; }
        }

        public static RoutedUICommand BtRepEmpContract
        {
            get { return AppCommands._btRepEmpContract; }
            set { AppCommands._btRepEmpContract = value; }
        }

        public static RoutedUICommand BtProtocolTr_Date_Order
        {
            get { return AppCommands._btProtocolTr_Date_Order; }
            set { AppCommands._btProtocolTr_Date_Order = value; }
        }

        public static RoutedUICommand BtCalendar { get; private set; }
        public static RoutedUICommand BtTable { get; private set; }
        public static RoutedUICommand BtFindEmp { get; private set; }
        public static RoutedUICommand BtRefresh { get; private set; }
        public static RoutedUICommand BtRefTableList { get; private set; }
        public static RoutedUICommand BtGr_Work { get; private set; }
        public static RoutedUICommand BtTableForAdvance { get; private set; }
        public static RoutedUICommand BtReportTable { get; private set; }
        public static RoutedUICommand BtReportPopul { get; private set; }
        public static RoutedUICommand BtRepHoursByOrders { get; private set; }
        public static RoutedUICommand BtTableTotal { get; private set; }
        public static RoutedUICommand BtTable_By_Period { get; private set; }
        public static RoutedUICommand BtCloseTableAdvance { get; private set; }
        public static RoutedUICommand BtCloseTableSalary { get; private set; }
        public static RoutedUICommand BtStart_Approval_Table { get; private set; }
        public static RoutedUICommand BtDocsOnPay_Type { get; private set; }
        public static RoutedUICommand BtLateness { get; private set; }
        public static RoutedUICommand BtHoursOnDegree { get; private set; }
        public static RoutedUICommand BtWorkOut { get; private set; }
        public static RoutedUICommand BtRepHoursOnSubdiv { get; private set; }
        public static RoutedUICommand BtHoursOnSubdiv { get; private set; }
        public static RoutedUICommand BtRepWorkHol { get; private set; }
        public static RoutedUICommand BtRepWorkPT { get; private set; }
        public static RoutedUICommand BtRepMission { get; private set; }
        public static RoutedUICommand BtWorkOrder { get; private set; }
        public static RoutedUICommand BtRepAbsenceOnSubdiv { get; private set; }
        public static RoutedUICommand BtProtocolTable { get; private set; }
        public static RoutedUICommand BtErrorGraph { get; private set; }
        public static RoutedUICommand BtProtoсolForAccount { get; private set; }
        public static RoutedUICommand BtRepFailedOrders { get; private set; }
        public static RoutedUICommand BtOrderHoliday { get; private set; }
        public static RoutedUICommand BtViewReplEmpTable { get; private set; }
        public static RoutedUICommand R_btProtokolReplSalT { get; private set; }
        public static RoutedUICommand R_btCombReplReport { get; private set; }
        public static RoutedUICommand R_btMiddleWatterSubdivTbl { get; private set; }
        public static RoutedUICommand BtOrderTruancy { get; private set; }
        public static RoutedUICommand BtList_Closing_Table { get; private set; }
        public static RoutedUICommand BtRepOnAdministrVac { get; private set; }
        public static RoutedUICommand BtRepOnHospital { get; private set; }
        public static RoutedUICommand BtRepOnPregnancy { get; private set; }
        public static RoutedUICommand BtRepOnChildCare { get; private set; }
        public static RoutedUICommand BtRepAverage_Number_On_Plant { get; private set; }
        public static RoutedUICommand BtRepAverage_Number_Personnel { get; private set; }
        public static RoutedUICommand BtRepFailureToAppear { get; private set; }
        public static RoutedUICommand BtRepLate_Pass { get; private set; }
        public static RoutedUICommand BtRepLateness { get; private set; }
        public static RoutedUICommand BtRepWorkOut { get; private set; }
        public static RoutedUICommand BtRepAverage_Number { get; private set; }
        public static RoutedUICommand BtRepAverage_Number_Consolidated { get; private set; }
        public static RoutedUICommand BtRepListEmp_Number { get; private set; }
        public static RoutedUICommand BtRepUseOfWorkTime { get; private set; }
        public static RoutedUICommand BtRepUseOfWorkTimeOnSub { get; private set; }
        public static RoutedUICommand BtRepTimeNotWorker { get; private set; }
        public static RoutedUICommand BtRepTimeNotWorkerOnSub { get; private set; }
        public static RoutedUICommand BtRepHoursByOrdersPlant { get; private set; }
        public static RoutedUICommand BtRep_Pay_Type_545 { get; private set; }
        public static RoutedUICommand BtRepAll_Limits_By_Subdiv { get; private set; }
        public static RoutedUICommand BtRepDocsOnPay_Type { get; private set; }
        public static RoutedUICommand BtLimit { get; private set; }
        public static RoutedUICommand BtLimit303 { get; private set; }
        public static RoutedUICommand BtViewOrders { get; private set; }
        public static RoutedUICommand BtRepCalc_Salary { get; private set; }
        public static RoutedUICommand AddReg_Doc { get; private set; }
        public static RoutedUICommand EditReg_Doc { get; private set; }
        public static RoutedUICommand DeleteReg_Doc { get; private set; }
        public static RoutedUICommand SelectOrderChange { get; private set; }
        public static RoutedUICommand RestoreOrderTable { get; private set; }
        public static RoutedUICommand ChangeOrderTable { get; private set; }
        public static RoutedUICommand ViewEmpTable { get; private set; }
        public static RoutedUICommand AddEmpStaff { get; private set; }
        public static RoutedUICommand AddStaff { get; private set; }
        public static RoutedUICommand AddGroupStaff { get; private set; }
        public static RoutedUICommand DeleteGroupStaff { get; private set; }
        public static RoutedUICommand SaveStaff { get; private set; }
        public static RoutedUICommand DeleteStaff { get; private set; }
        public static RoutedUICommand AddStaffSection { get; private set; }
        public static RoutedUICommand DeleteStaffSection { get; private set; }
        public static RoutedUICommand SaveStaffSection { get; private set; }
        public static RoutedUICommand SaveSubdivTypePart { get; private set; }
        public static RoutedUICommand AddSubdivTypePart { get; private set; }
        public static RoutedUICommand EditStaffSection { get; private set; }
        public static RoutedUICommand DeleteSubdivTypePart { get; private set; }
        public static RoutedUICommand EditSubdivPartition { get; private set; }
        public static RoutedUICommand DeleteSubdivPartition { get; private set; }
        public static RoutedUICommand SaveIndividProtection { get; private set; }
        public static RoutedUICommand AddSubdivPartition { get; private set; }
        public static RoutedUICommand SaveSubdivPartition { get; private set; }
        public static RoutedUICommand AddWorkPlace { get; private set; }
        public static RoutedUICommand EditWorkPlace { get; private set; }
        public static RoutedUICommand DeleteWorkPlace { get; private set; }
        public static RoutedUICommand SaveWorkPlace { get; private set; }
        public static RoutedUICommand BtViewArchivVS { get; private set; }
        public static RoutedUICommand BtViewMakeVS { get; private set; }
        public static RoutedUICommand BtConfirmVS { get; private set; }
        public static RoutedUICommand BtDiagramsVS { get; private set; }
        public static RoutedUICommand BtViewEmpLocationVac { get; private set; }
        public static RoutedUICommand BtPlanOnYearVS { get; private set; }
        public static RoutedUICommand Bt_alloc_on_Months_VS { get; private set; }
        public static RoutedUICommand BtSvodVS { get; private set; }
        public static RoutedUICommand BtVSByGroupMaster { get; private set; }
        public static RoutedUICommand R_btActualVacByRegion { get; private set; }
        public static RoutedUICommand R_btConsolidVacByRegion { get; private set; }
        public static RoutedUICommand BtVS_RemainVacs { get; private set; }
        public static RoutedUICommand BtListVsBlock { get; private set; }
        public static RoutedUICommand R_btPrikazZavodVS { get; private set; }
        public static RoutedUICommand R_btSubPrikazVS { get; private set; }
        public static RoutedUICommand TsbtRNoteVacVS { get; private set; }
        public static RoutedUICommand BtNoteAccountVS { get; private set; }
        public static RoutedUICommand R_btActualDatesByPeriod { get; private set; }
        public static RoutedUICommand R_CountPlanSumDaysVS { get; private set; }
        public static RoutedUICommand BtAggReservDataPeriodVS { get; private set; }
        public static RoutedUICommand AddNewVacCommand { get; private set; }
        public static RoutedUICommand EditVacCommand { get; private set; }
        public static RoutedUICommand ConfirmVSCommand { get; private set; }
        public static RoutedUICommand BtPrivateKardVS { get; private set; }
        public static RoutedUICommand BtCancelConfirmVS { get; private set; }
        public static RoutedUICommand AllSubdivVacConfirmStatistic { get; private set; }
        public static RoutedUICommand RecheckVacs { get; private set; }
        public static RoutedUICommand ViewSubdivPartition { get; private set; }
        public static RoutedUICommand AddWPProtection { get; private set; }
        public static RoutedUICommand DeleteWPProtection { get; private set; }
        public static RoutedUICommand ViewWorkPlace { get; private set; }
        public static RoutedUICommand ViewIndividProtectionCatalog { get; private set; }
        public static RoutedUICommand OpenViewManningTable { get; private set; }
        public static RoutedUICommand OpenViewManningProject { get; private set; }
        public static RoutedUICommand OpenViewManningTableEmp { get; private set; }
        public static RoutedUICommand EditStaff { get; private set; }
        public static RoutedUICommand EditEmpStaff { get; private set; }
        public static RoutedUICommand DeleteEmpStaff { get; private set; }
        public static RoutedUICommand BtCorporate_Support_Agreement { get; private set; }
        public static RoutedUICommand BtCorporate_Support_Contract { get; private set; }
        public static RoutedUICommand Project_Transfer_Combined { get; private set; }
        public static RoutedUICommand AddWork_Pay_Type { get; private set; }
        public static RoutedUICommand EditWork_Pay_Type { get; private set; }
        public static RoutedUICommand DeleteWork_Pay_Type { get; private set; }
        public static RoutedUICommand TimePercoToTimeGraph { get; private set; }
        public static RoutedUICommand TimePayTypeToTimePerco { get; private set; }
        public static RoutedUICommand TimeSumPayTypeToTimePerco { get; private set; }
        public static RoutedUICommand CalcWorked_Day { get; private set; }
        public static RoutedUICommand EditOrderPayType { get; private set; }
        public static RoutedUICommand SaveDocWorked_Day { get; private set; }
        public static RoutedUICommand BtAddAbsence { get; private set; }
        public static RoutedUICommand BtDeleteAbsence { get; private set; }
        public static RoutedUICommand Sql_Trace_Enabled { get; private set; }
        public static RoutedUICommand View_Emp_Mobile_Comm { get; private set; }
        public static RoutedUICommand AddEmp_Mobile_Comm { get; private set; }
        public static RoutedUICommand EditEmp_Mobile_Comm { get; private set; }
        public static RoutedUICommand DeleteEmp_Mobile_Comm { get; private set; }
        public static RoutedUICommand ChooseStaffWorkPlace { get; private set; }
        public static RoutedUICommand DeleteStaffWorkPlace { get; private set; }
        public static RoutedUICommand AddGrWork { get; private set; }
        public static RoutedUICommand EditGrWork { get; private set; }
        public static RoutedUICommand DeleteGrWork { get; private set; }
        public static RoutedUICommand CompareTransferWithWorkedDay { get; private set; }
        public static RoutedUICommand SaveEmpStaff { get; private set; }
    }
}
