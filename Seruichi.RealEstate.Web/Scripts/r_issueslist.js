const _url = {};
$(function () {
    _url.get_issueslist_Data = common.appPath + '/r_issuelist/get_issueslist_Data';
    _url.insert_l_log = common.appPath + '/r_issuelist/Insert_l_log';
    setValidation();
    addEvents();
    $('#realECD').focus();
});

function setValidation() {
    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('.form-check-input')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112
}

function addEvents() {
    common.bindValidationEvent('#form1', '');

    let model = {
        chk_New: $("chk_New").val(),
        chk_Nego: $("chk_Nego").val(),
        chk_Contract: $("chk_Contract").val(),
        chk_SellerDeclined: $("chk_SellerDeclined").val(),
        chk_BuyerDeclined: $("chk_BuyerDeclined").val(),
        REStaffCD: $("REStaffCD").val(),
        Range: $("Range").val(),
        StartDate: $("StartDate").val(),
        EndDate: $("EndDate").val()
    };
    get_issueslist_Data(model, this);

    $('#StartDate, #EndDate').on('change', function () {
        const $this = $(this), $start = $('#StartDate').val(), $end = $('#EndDate').val();
        if (!common.checkValidityInput($this)) {
            return false;
        }
        let model = {
            StartDate: $start,
            EndDate: $end
        };
        if (model.StartDate && model.EndDate) {
            if (model.StartDate < model.EndDate) {
                $("#StartDate").hideError();
                $("#EndDate").hideError();
                $("#EndDate").focus();
                return;
            }
        }
    });

    $('.form-check-input').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.form-check-input').hideError();
        }
    }).change();

    $('#btnToday').on('click', function () {
        let today = common.getToday();
        $('#StartDate').val(today);
        $('#EndDate').val(today);
    });
    $('#btnThisWeek').on('click', function () {
        let start = common.getDayaweekbeforetoday();
        let end = common.getToday();
        $('#StartDate').val(start);
        $('#EndDate').val(end);
    });
    $('#btnThisMonth').on('click', function () {
        let today = common.getToday();
        let firstDay = common.getFirstDayofMonth();
        $('#StartDate').val(firstDay);
        $('#EndDate').val(today);
    });
    $('#btnLastMonth').on('click', function () {
        let firstdaypremonth = common.getFirstDayofPreviousMonth();
        let lastdaypremonth = common.getLastDayofPreviousMonth();
        $('#StartDate').val(firstdaypremonth);
        $('#EndDate').val(lastdaypremonth);
    });

    $('#btnProcess').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#mansiontable tbody').empty();
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        get_issueslist_Data(model, $form);
    });

    $('#btnCSV').on('click', function () {
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);

        common.callAjax(_url.generate_M_SellerMansionCSV, model,
            function (result) {
                //sucess
                var table_data = result.data;

                var csv = common.getJSONtoCSV(table_data);
                if (!(csv == "ERROR")) {
                    var downloadLink = document.createElement("a");
                    var blob = new Blob(["\ufeff", csv]);
                    var url = URL.createObjectURL(blob);
                    downloadLink.href = url;
                    let m = new Date();
                    var dateString =
                        m.getUTCFullYear() + "" +
                        ("0" + (m.getUTCMonth() + 1)).slice(-2) + "" +
                        ("0" + m.getUTCDate()).slice(-2) + "_" +
                        ("0" + m.getHours()).slice(-2) + "" +
                        ("0" + m.getMinutes()).slice(-2) + "" +
                        ("0" + m.getSeconds()).slice(-2);
                    downloadLink.download = "売主マンションリスト" + dateString + ".csv";

                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }
                else {
                    alert("There is no data!");
                }
            }
        )
    });
}

function get_issueslist_Data(model, $form) {
    common.callAjaxWithLoading(_url.get_issueslist_Data, model, this, function (result) {
        if (result && result.isOK) {
            Bind_tbody(result.data);
        }
        if (result && !result.isOK) {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    });
}

function isEmptyOrSpaces(str) {
    return str === null || str.match(/^ *$/) !== null;
}

function Bind_tbody(result) {
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {

            if (isEmptyOrSpaces(data[i]["ステータス名"])) {
                _letter = "";
                _class = "ms-1 ps-1 pe-1 rounded-circle";
                _sort_checkbox = "";
            }
            else {
                _letter = data[i]["ステータス名"].charAt(0);
                if (_letter == "新規依頼") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white";
                    _sort_checkbox = "One";
                }
                else if (_letter == "交渉中") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark";
                    _sort_checkbox = "Two";
                }
                else if (_letter == "成約") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary";
                    _sort_checkbox = "Three";
                }
                else if (_letter == "売主辞退") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-light text-danger";
                    _sort_checkbox = "Four";
                }
                else if (_letter == "買主辞退") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-dark text-white";
                    _sort_checkbox = "Five";
                }

            }
            html += '<tr>\
            <td class= "text-end"> ' + data[i]["NO"] + '</td>\
            <td class="'+ _sort_checkbox + '"><i class="' + _class + '">' + _letter + '</i><span class="font-semibold"> ' + data[i]["ステータス名"] + '</span></td>\
            <td>'+ data[i]["査定依頼ID"] + '</td>\
            <td>'+ data[i]["売主保持物件ID"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["査定依頼ID"] + '&r_issueslist' + '  href="#" onclick="l_logfunction(this.id)">' + data[i]["物件名"] + '</a></td>\
            <td class="text-nowrap">' + data[i]["部屋番号"] + '</td>\
            <td>'+ data[i]["依頼売主CD"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["査定依頼ID"] + '&r_issueslist' + '  href="#" onclick="l_logfunction(this.id)">' + data[i]["お客様名"] + '</a></td>\
            <td class="text-nowrap">'+ data[i]["買取依頼日時"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["終了日時"] + '</td>\
            <td class="text-end text-nowrap"> '+ data[i]["査定価格"] + '</td>\
            <td>'+ data[i]["不動産担当者CD"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["担当者名"] + '</td>\
            </tr>'
        }

        $('#total_record').text("検索結果：" + data.length + "件");
        $('#total_record_up').text("検索結果：" + data.length + "件");
    }
    else {
        $('#total_record').text("検索結果： 0件 表示可能データがありません");
        $('#total_record_up').text("検索結果： 0件");
    }
    $('#tblissueslist tbody').append(html);

    sortTable.getSortingTable("tblissueslist");
}

function l_logfunction(id) {
    let model = {
        LoginKBN: null,
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        PageID: null,
        ProcessKBN: null,
        Remarks: null,
        AssReqID: id.split('&')[0],
        PageID: id.split('&')[1]
    };
    common.callAjax(_url.insert_l_log, model,
        function (result) {
            if (result && result.isOK) {
                if (model.LogStatus == "t_mansion_detail")
                    alert("https://www.seruichi.com/t_mansion_detail?ｍansionCD=" + model.LogId);
                else if (model.LogStatus == "t_seller_assessment")
                    // alert("https://www.seruichi.com/t_seller_assessment?seller=" + model.LogId);
                    window.location.href = common.appPath + '/t_seller_assessment/Index?SellerCD=' + model.LogId;
                else if (model.LogStatus == "t_seller_assessment_detail")
                    alert("https://www.seruichi.com/t_seller_assessment_detail?AssReqID=" + model.LogId);
                else if (model.LogStatus == "t_seller_assessment_detail_GReal")
                    alert("https://www.seruichi.com/t_reale_purchase?realestate=" + model.LogId);
                else if (model.LogStatus == "t_seller_assessment_detail_EReal")
                    alert("https://www.seruichi.com/t_reale_purchase?realestate=" + model.LogId);
                else if (model.LogStatus == "t_seller_assessment_detail_IRealECD")
                    alert("https://www.seruichi.com/t_reale_purchase?realestate=" + model.LogId);
            }
            if (result && !result.isOK) {
            }
        });
}