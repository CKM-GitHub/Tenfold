const _url = {};
$(function () {
    _url.GetREFaceImg = common.appPath + '/t_dashboard/GetCustomerInformationWaitingCount';
    _url.GetREStaffName = common.appPath + '/t_dashboard/GetChatConfirmationWaitingCount';
    _url.GetREName = common.appPath + '/t_dashboard/GetNewRequestCasesCount';
    //_url.GetOldestDate = common.appPath + '/t_dashboard/GetDuringnegotiationsCasesCount';
    //_url.GetOldestDatecount = common.appPath + '/t_dashboard/GetContractCasesCount';
    _url.GetNewRequestData = common.appPath + '/t_dashboard/GetDeclineCasesCount';
    _url.GetNegotiationsData = common.appPath + '/t_dashboard/GetDuringnegotiationsCasesCount';
    _url.GetNumberOfCompletedData = common.appPath + '/t_dashboard/GetContractCasesCount';
    _url.GetNumberOfDeclineData = common.appPath + '/t_dashboard/GetDeclineCasesCount';
});

$(document).ready(function () {
    common.bindValidationEvent('#form1', "");
    let model = {
        RealECD: null,
        REStaffCD:null
    };
    common.callAjaxWithLoading(_url.GetREFaceImg, model, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                debugger;
                let data = dataArray[0];
                $('#imgProfile').text(data.REFaceImage);
            }
        }
    )
    common.callAjaxWithLoading(_url.GetREStaffName, model, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#staffName').text(data.REStaffName);
            }
        }
    )
    common.callAjaxWithLoading(_url.GetREName, model, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#companyName').text(data.REName);
            }
        }
    )
    common.callAjaxWithLoading(_url.GetNewRequestData, model, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#newRequest').text(data.Number + "件");
            }
        }
    )
    common.callAjaxWithLoading(_url.GetNegotiationsData, model, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#negotiations').text(data.Number + "件");
            }
        }
    )
    common.callAjaxWithLoading(_url.GetNumberOfCompletedData, model, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#NumCompleted').text(data.Number + "件");
            }
        }
    )
    common.callAjaxWithLoading(_url.GetNumberOfDeclineData, model, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#NumDecline').text(data.Number + "件");
            }
        }
    )

});