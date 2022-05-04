const _url = {};
$(function () {
 
    _url.GetREFaceImg = common.appPath + '/r_dashboard/GetREFaceImg';
    _url.GetREStaffName = common.appPath + '/r_dashboard/GetREStaffName';
    _url.GetREName = common.appPath + '/r_dashboard/GetREName';
    _url.GetOldestDate = common.appPath + '/r_dashboard/GetOldestDate';
    _url.GetOldestDatecount = common.appPath + '/r_dashboard/GetOldestDatecount';
    _url.GetNewRequestData = common.appPath + '/r_dashboard/GetNewRequestData';
    _url.GetNegotiationsData = common.appPath + '/r_dashboard/GetNegotiationsData';
    _url.GetNumberOfCompletedData = common.appPath + '/r_dashboard/GetNumberOfCompletedData';
    _url.GetNumberOfDeclineData = common.appPath + '/r_dashboard/GetNumberOfDeclineData';
});

$(document).ready(function () {
    common.bindValidationEvent('#form1', "");
    let date=null;
    let model = {
        RealECD: null,
        REStaffCD: null,
        ConfDateTime: null,
        Oldestdate:null
    };
    common.callAjaxWithLoading(_url.GetREFaceImg, model, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;
            if (length > 0) {
                let data = dataArray[0];
                //$('#imgProfile').text(data.REFaceImage);
                $('#imgProfile').attr('src', 'data:image/png;base64,' + data.REFaceImage);
            }
            else {
                $("#imgProfile").attr('src', 'data:image/png;base64,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');
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
    common.callAjaxWithLoading(_url.GetOldestDate, model, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;
            if (length > 0) {
                let data = dataArray[0];
                let d = data.MinDate;
                if (d == null) {
                    d = new Date().toISOString().slice(0, 10).replace(/-/g, '/');
                }
                    let arr1 = d.split('/');
                    let year = arr1[0];
                    let month = arr1[1];
                    let day = arr1[2];
                    date = year + '年' + month + '月' + day + '日';
                //$('#oldestDate').text(date);
               
            }
        }
    )
    common.callAjaxWithLoading(_url.GetOldestDatecount, model, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#oldestDate').text(date +" "+ data.datecount);
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