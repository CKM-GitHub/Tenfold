﻿const _url = {};
$(function () {
    _url.getCustomerInformationWaitingCount = common.appPath + '/t_dashboard/GetCustomerInformationWaitingCount';
    _url.getChatConfirmationWaitingCount = common.appPath + '/t_dashboard/GetChatConfirmationWaitingCount';
    _url.getNewRequestCasesCount = common.appPath + '/t_dashboard/GetNewRequestCasesCount';
    _url.getDuringnegotiationsCasesCount = common.appPath + '/t_dashboard/GetDuringnegotiationsCasesCount';
    _url.getContractCasesCount = common.appPath + '/t_dashboard/GetContractCasesCount';
    _url.getDeclineCasesCount = common.appPath + '/t_dashboard/GetDeclineCasesCount';
});

$(document).ready(function () {
    common.bindValidationEvent('#form1', "");
    let model = {
        Customer_waiting_count: null
    };
    common.callAjax(_url.getCustomerInformationWaitingCount, model,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#Customer_Information_Waiting_Count').text(data.Customer_Information_Waiting_Count + "件待ち");
            }
        }
    )
    common.callAjax(_url.getChatConfirmationWaitingCount, model,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#Chat_Confirmation_Waiting_Count').text(data.Chat_Confirmation_Waiting_Count + "件待ち");
            }
        }
    )
    common.callAjax(_url.getNewRequestCasesCount, model,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#New_Request_Cases_Count').text(data.New_Request_Cases_Count + "件");
            }
        }
    )
    common.callAjax(_url.getDuringnegotiationsCasesCount, model,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#During_negotiations_Cases_Count').text(data.During_negotiations_Cases_Count + "件");
            }
        }
    )
    common.callAjax(_url.getContractCasesCount, model,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#Contract_Cases_Count').text(data.Contract_Cases_Count + "件");
            }
        }
    )
    common.callAjax(_url.getDeclineCasesCount, model,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#Decline_Cases_Count').text(data.Decline_Cases_Count + "件");
            }
        }
    )
});