WITH GRANTED_ROLES as
    (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS )
select TYPE_APPROVAL_ID, TYPE_APPROVAL_NAME, SIGN_APPROVAL_NOTE, SIGN_CONTINUE_APPROVAL, SIGN_REWORK, SIGN_CANCELLATION
from {0}.TYPE_APPROVAL TA
WHERE TA.TYPE_APPROVAL_ID in ( 
    select TAR.TYPE_APPROVAL_ID from {0}.TYPE_APPROVAL_BY_ROLE TAR
    JOIN {0}.PROJECT_PLAN_APPROVAL PPA on TAR.PROJECT_PLAN_APPROVAL_ID = PPA.PROJECT_PLAN_APPROVAL_ID 
    WHERE TAR.ROLE_NAME in (SELECT GRANTED_ROLE FROM GRANTED_ROLES ) and
        PPA.PROJECT_PLAN_APPROVAL_ID = :p_PROJECT_PLAN_APPROVAL_ID and
        /* �� ������ �� ���� ������ �� ������������ �� = 9 ������ ���-�� �������������, ���������� ��*/
        (PPA.SIGN_APPROVAL_ECON_DEPT = 0 
        or 
        (PPA.SIGN_APPROVAL_ECON_DEPT = 1 and 
            ((select PT.FORM_OPERATION_ID
            from {0}.PROJECT_TRANSFER PT where PT.PROJECT_TRANSFER_ID = :p_PROJECT_TRANSFER_ID) != 9)
            or 
            ((select PT.FORM_OPERATION_ID
            from {0}.PROJECT_TRANSFER PT where PT.PROJECT_TRANSFER_ID = :p_PROJECT_TRANSFER_ID) = 9
                and exists(select null from GRANTED_ROLES WHERE GRANTED_ROLE = 'STAFF_VIEW_ECON_SERVICE')))) 
	)