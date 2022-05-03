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
    common.callAjaxWithLoading(_url.GetOldestDate, model, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                let d = data.MinDate;
                let arr1 = d.split('/');
                let year = arr1[0];
                let month = arr1[1];
                let day = arr1[2];
                date = year + '年' + month +'月'+ day+ '日';
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