const _url = {};
$(function () {

    _url.CheckSellerName = common.appPath + '/t_Seller_list/CheckSellerName';
    _url.CheckDate = common.appPath + '/t_Seller_list/CheckDate';
    _url.DateCompare = common.appPath + '/t_Seller_list/DateCompare';

    setValidation();
    addEvents();
});

function setValidation() {

    $('#SellerName')
        .addvalidation_errorElement("#errorSeller")
        .addvalidation_singlebyte_doublebyte()
        .addvalidation_MaxLength(10);

    //$('#startdate')
    //    .addvalidation_errorElement("#errorStartDate")
    //    .addvalidation_datecheck()
    //    .addvalidation_datecompare();

    //$('#enddate')
    //    .addvalidation_errorElement("#errorEndDate")
    //    .addvalidation_datecheck()
    //    .addvalidation_datecompare();
   
}
function addEvents() {

    common.bindValidationEvent('#form1', "");

    //const $locationSelect = $("#locationSelect").val(),$SellerName = $("#SellerName").val(), $RangeSelect = $("#RangeSelect").val(),
    //    $startdate = $("#startdate").val(), $enddate = $("#enddate").val(), $ValidCheck = $("#ValidCheck").val(),
    //    $InValidCheck = $("#InValidCheck").val(), $StatusCheck1 = $("#StatusCheck1").val(), $StatusCheck2 = $("#StatusCheck2").val(),
    //    $StatusCheck3 = $("#StatusCheck3").val()

    $('#SellerName').on('change', function () {
        const $this = $(this), $SellerName = $('#SellerName')
        let model = {
            SellerName: $SellerName.val()
        };
        common.callAjax(_url.CheckSellerName,model,
            function (result) {
                if (result && result.isOK) {
                    $($email).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });

    $('#StartDate').on('change', function () {
        const $this = $(this),  $startdate = $('#startdate')
        let model = {
            startDate: $startdate.val(),
        };
        common.callAjax(_url.CheckDate, model,
            function (result) {
                if (result && result.isOK) {
                    $($email).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });

    $('#enddate').on('change', function () {
        const $this = $(this), $enddate = $('#enddate'), $startdate = $('#startdate')
        let model = {
            startDate: $startdate.val(),
            endDate: $enddate.val()
        };
        common.callAjax(_url.CheckDate, model.endDate,
            function (result) {
                if (result && result.isOK) {
                    $($email).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
        common.callAjax(_url.DateCompare, model,
            function (result) {
                if (result && result.isOK) {
                    $($email).hideError();
                    const data = result.data;
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });

    
    $('#InValidCheck').on('change', function () {
        const $this = $(this)
        //    $valid = $('#ValidCheck'), $invalid = $('#InValidCheck')
        //let model = {
        //    ValidCheck: $valid.val(),
        //    InValidCheck: $invalid.val()
        //};
        alert("aa");
        if ($('#ValidCheck').prop('checked', false) && $('#InValidCheck').prop('checked', false)) {
            alert("bb");
            $this.showError(common.getMessage('E112'));
        }
        
    });

    $('#checkbox1').change(function () {
        if ($(this).is(":checked")) {
            var returnVal = confirm("Are you sure?");
            $(this).attr("checked", returnVal);
        }
        $('#textbox1').val($(this).is(':checked'));
    });


    $('#btnToday').on('click', function () {
        var now = new Date();
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        Date = (month) + '/' + (day) + '/' + now.getFullYear();
        alert(Date);
        //$('#startDate').val(Date);
        //$('#endDate').val(Date);

    });

    $('#btnThisWeek').on('click', function () {
        alert("btnThisweek");
        //var myPastDate = new Date(myCurrentDate);
        //myPastDate.setDate(myPastDate.getDate() - 8);
        //alert(myPastDate);
        //$('#startDate').val(myPastDate);

        var now = new Date();
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var today = (month) + '/' + (day) + '/' + now.getFullYear();
        alert(today);
        //$('#endDate').val(today);

    });

    $('#btnThisMonth').on('click', function () {
        alert("btnThismonth");
        var now = new Date();
        var day = ("0" + "1");
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var today = (month) + '/' + (day) + '/' + now.getFullYear();
        alert(today);
        //$('#startDate').val(today);

        var now1 = new Date();
        var day1 = ("0" + now1.getDate()).slice(-2);
        var month1 = ("0" + (now1.getMonth() + 1)).slice(-2);
        var today1 = (month1) + '/' + (day1) + '/' + now1.getFullYear();
        alert(today1);
        //$('#endDate').val(today1);
    });

    $('#btnLastMonth').on('click', function () {
        alert("btnLastmonth");
        var now = new Date();
        now.setDate(0)
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var day = ("0" + "1");
        var year = now.getFullYear();
        newdate1 = month + "/" + day + "/" + year;
        alert(newdate1);
        //$('#startDate').val(prevMonthFirstDate);

        var dateObj = new Date();
        dateObj.setDate(0)
        var month = ("0" + (dateObj.getMonth() + 1)).slice(-2);
        var day = ("0" + dateObj.getDate()).slice(-2);
        var year = dateObj.getFullYear();
        newdate = month + "/" + day + "/" + year;
        alert(newdate);
        //$('#endDate').val(newdate);

    });

}

//function CompareDate() {
//    const $this = $(this), $enddate = $('#enddate'), $startdate = $('#startdate')
//    let model = {
//        startDate: $startdate.val(),
//        endDate: $enddate.val()
//    };

//    //alert(model.startDate);
//    //alert(model.endDate);
//    if (model.startDate > model.endDate) {
//        $this.showError(common.getMessage('E111'));
//        return false;
//    }

//    return true;
//}