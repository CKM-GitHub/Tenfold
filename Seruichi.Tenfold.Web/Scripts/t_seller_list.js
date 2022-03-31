$(function () {

    setValidation();
    addEvents();
});

function setValidation() {
    //階
    //$('#inlineFormInput')
    //    .addvalidation_reqired(true)

    //$('#startdate')
    //    .addvalidation_datecheck()
   
}
function addEvents() {

    //共通チェック処理
    common.bindValidationEvent('#form1', "");

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