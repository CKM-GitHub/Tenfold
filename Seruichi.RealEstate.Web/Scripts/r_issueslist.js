const _url = {};
$(function () {
    _url.get_issueslist_Data = common.appPath + '/r_issueslist/get_issueslist_Data';
    _url.generate_r_issueslist_CSV = common.appPath + '/r_issueslist/generate_r_issueslist_CSV';
    _url.insert_l_log = common.appPath + '/r_issueslist/Insert_l_log';
    _url.get_SellerDetails_Data = common.appPath + '/r_issueslist/get_SellerDetails_Data';
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

    $('#txtFreeWord')
        .addvalidation_errorElement("#errorFreeWord")
        .addvalidation_maxlengthCheck(50);

    $('.form-check-input')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112
}

function addEvents() {
    common.bindValidationEvent('#form1', '');

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

    const $chk_New = $("#chk_New").val(), $chk_Nego = $("#chk_Nego").val(), $chk_Contract = $("#chk_Contract").val(), $chk_SellerDeclined = $("#chk_SellerDeclined").val(), $chk_BuyerDeclined = $("#chk_BuyerDeclined").val(),
        $ddlStaff = $("#ddlStaff").val(), $ddlRange = $("#ddlRange").val(), $StartDate = $("#StartDate").val(), $EndDate = $("#EndDate").val(), $txtFreeWord = $("#txtFreeWord").val()

    let model = {
        chk_New: $chk_New,
        chk_Nego: $chk_Nego,
        chk_Contract: $chk_Contract,
        chk_SellerDeclined: $chk_SellerDeclined,
        chk_BuyerDeclined: $chk_BuyerDeclined,
        REStaffCD: $ddlStaff,
        Range: $ddlRange,
        StartDate: $StartDate,
        EndDate: $EndDate,
        FreeWord: $txtFreeWord
    };
    get_issueslist_Data(model, this);

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
        $('#tblissueslist tbody').empty();
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        get_issueslist_Data(model, $form);
    });

    $('#btnCSV').on('click', function () {
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);

        common.callAjax(_url.generate_r_issueslist_CSV, model,
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
                    downloadLink.download = "案件一覧_" + dateString + ".csv";

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
    let _letter = "", _class = "", _sort_checkbox = "";
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
                if (_letter == "新") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white";
                    _sort_checkbox = "One";
                }
                else if (_letter == "交") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark";
                    _sort_checkbox = "Two";
                }
                else if (_letter == "成") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary";
                    _sort_checkbox = "Three";
                }
                else if (_letter == "売") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-light text-danger";
                    _sort_checkbox = "Four";
                }
                else if (_letter == "買") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-dark text-white";
                    _sort_checkbox = "Five";
                }

            }
            html += '<tr>\
            <td class= "text-center"> ' + data[i]["NO"] + '</td>\
            <td class="'+ _sort_checkbox + '"><span class="' + _class + '">' + _letter + '</span><span class="font-semibold"> ' + data[i]["ステータス名"] + '</span></td>\
            <td>'+ data[i]["査定依頼ID"] + '</td>\
            <td>'+ data[i]["売主保持物件ID"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["査定依頼ID"] + '&r_issueslist' + '  href="#" onclick="l_logfunction(this.id)">' + data[i]["物件名"] + ' ' + data[i]["部屋番号"] + '</a></td>\
            <td>'+ data[i]["依頼売主CD"] + '</td>\
            <td>'+ data[i]["売主_カナ"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" data-bs-toggle="modal" data-bs-target="#SellerDetails" id='+ data[i]["依頼売主CD"] + ' href="#" onclick="Bind_SellerDetails_Popup(this)">' + data[i]["お客様名"] + '</a></td>\
            <td>'+ data[i]["売主_住所１"] + data[i]["売主_住所２"] + data[i]["売主_住所３"] + data[i]["売主_住所４"] + data[i]["売主_住所５"] + '</td>\
            <td>'+ data[i]["売主_固定電話番号"] + '</td>\
            <td>'+ data[i]["売主_携帯電話番号"] + '</td>\
            <td>'+ data[i]["売主_メールアドレス"] + '</td>\
            <td class="text-nowrap text-center">'+ data[i]["買取依頼日時"] + '</td>\
            <td class="text-nowrap text-center"> '+ data[i]["終了日時"] + '</td>\
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
                if (model.PageID == "r_issueslist")
                    alert("https://www.seruichi.com/r_chat?AssReqID=" + model.AssReqID);
            }
            if (result && !result.isOK) {
            }
        });
}

function Bind_SellerDetails_Popup(ctrl) {
    var r_index = ctrl.parentNode.parentNode.rowIndex - 1;
    var tr = $('#tblissueslist tbody tr')[r_index];
    var kana_name = tr.querySelectorAll('td')[6].innerHTML;
    var name = tr.querySelectorAll('td')[7].querySelectorAll('a')[0].innerHTML;
    var address = tr.querySelectorAll('td')[8].innerHTML;
    var landline_no = tr.querySelectorAll('td')[9].innerHTML;
    var mobile_no = tr.querySelectorAll('td')[10].innerHTML;
    var mail = tr.querySelectorAll('td')[11].innerHTML;
    $('#kana_name').text(kana_name);
    $('#name').text(name);
    $('#address').text(address);
    $('#landline_no').text(landline_no);
    $('#mobile_no').text(mobile_no);
    $('#mail').text(mail);

    var model = {
        SellerID: ctrl.id
    }
    common.callAjaxWithLoading(_url.get_SellerDetails_Data, model, this, function (result) {
        if (result && result.isOK) {
            Bind_Model_tbody(result.data);
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

function Bind_Model_tbody(result) {
    let _letter = "", _class = "", _sort_checkbox = "";
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
                if (_letter == "新") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white";
                    _sort_checkbox = "One";
                }
                else if (_letter == "交") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark";
                    _sort_checkbox = "Two";
                }
                else if (_letter == "成") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary";
                    _sort_checkbox = "Three";
                }
                else if (_letter == "売") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-light text-danger";
                    _sort_checkbox = "Four";
                }
                else if (_letter == "買") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-dark text-white";
                    _sort_checkbox = "Five";
                }

            }
            html += '<tr>\
            <td class= "text-center"> ' + data[i]["NO"] + '</td>\
            <td class="'+ _sort_checkbox + '"><span class="' + _class + '">' + _letter + '</span><span class="font-semibold"> ' + data[i]["ステータス名"] + '</span></td>\
            <td>'+ data[i]["査定依頼ID"] + '</td>\
            <td>'+ data[i]["売主保持物件ID"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["査定依頼ID"] + '&r_issueslist' + '  href="#" onclick="l_logfunction(this.id)">' + data[i]["物件名"] + ' ' + data[i]["部屋番号"] + '</a></td>\
            <td class="text-nowrap text-center">'+ data[i]["買取依頼日時"] + '</td>\
            <td class="text-nowrap text-center"> '+ data[i]["終了日時"] + '</td>\
            <td class="text-end text-nowrap"> '+ data[i]["査定価格"] + '</td>\
            <td>'+ data[i]["不動産担当者CD"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["担当者名"] + '</td>\
            </tr>'
        }

        $('#mtotal_record').text("検索結果：" + data.length + "件");
        $('#mtotal_record_up').text("検索結果：" + data.length + "件");
    }
    else {
        $('#mtotal_record').text("検索結果： 0件 表示可能データがありません");
        $('#mtotal_record_up').text("検索結果： 0件");
    }
    $('#tblsellerdetails tbody').append(html);

    sortTable.getSortingTable("tblsellerdetails");
}