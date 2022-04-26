const _url = {};

$(function () {
    setValidation();
    _url.getM_SellerMansionList = common.appPath + '/t_seller_assessment/GetM_SellerMansionList';
    _url.generate_M_SellerMansionCSV = common.appPath + '/t_seller_assessment/Generate_M_SellerMansionCSV';
    _url.insert_l_log = common.appPath + '/t_seller_assessment/Insert_l_log';
    addEvents();
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

    const $Chk_Mi = $("#Chk_Mi").val(), $Chk_Kan = $("#Chk_Kan").val(), $Chk_Satei = $("#Chk_Satei").val(),
        $Chk_Kaitori = $("#Chk_Kaitori").val(), $Chk_Kakunin = $("#Chk_Kakunin").val(), $Chk_Kosho = $("#Chk_Kosho").val(),
        $Chk_Seiyaku = $("#Chk_Seiyaku").val(), $Chk_Urinushi = $("#Chk_Urinushi").val(), $Chk_Kainushi = $("#Chk_Kainushi").val(),
        $Chk_Sakujo = $("#Chk_Sakujo").val(),
        $Range = $("#Range").val(), $StartDate = $("#StartDate").val(),
        $EndDate = $("#EndDate").val()

    let model = {
        Chk_Mi: $Chk_Mi,
        Chk_Kan: $Chk_Kan,
        Chk_Satei: $Chk_Satei,
        Chk_Kaitori: $Chk_Kaitori,
        Chk_Kakunin: $Chk_Kakunin,
        Chk_Kosho: $Chk_Kosho,
        Chk_Seiyaku: $Chk_Seiyaku,
        Chk_Urinushi: $Chk_Urinushi,
        Chk_Kainushi: $Chk_Kainushi,
        Chk_Sakujo: $Chk_Sakujo,
        Range: $Range,
        StartDate: $StartDate,
        EndDate: $EndDate,
    };
    getM_SellerMansionList(model, this);

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
        getM_SellerMansionList(model, $form);
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
                    downloadLink.download = "売主リスト" + dateString + ".csv";

                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }
                else {
                    alert("該当データがありません。もう一度、条件を変更の上表示ボタンを押してください。");
                }
            }
        )
    });
}


function getM_SellerMansionList(model, $form) {

    common.callAjaxWithLoading(_url.getM_SellerMansionList, model, this, function (result) {
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
    for (var i = 0; i < data.length; i++) {

        if (isEmptyOrSpaces(data[i]["ステータス"])) {
            _letter = "";
            _class = "ms-1 ps-1 pe-1 rounded-circle";
            _sort_checkbox = "";
        }
        else {
            _letter = data[i]["ステータス"].charAt(0);
            if (_letter == "未") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-primary text-white fst-normal";
                _sort_checkbox = "One";
            }
            else if (_letter == "簡") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-info text-white fst-normal";
                _sort_checkbox = "Two";
            }
            else if (_letter == "詳" && data[i]["ステータス"] == "詳細査定") {
                _letter = "査";
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning text-white fst-normal";
                _sort_checkbox = "Three";
            }
            else if (_letter == "買" && data[i]["ステータス"] == "買取依頼") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white fst-normal";
                _sort_checkbox = "Four";
            }
            else if (_letter == "確") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning ext-dark fst-normal";
                _sort_checkbox = "Five";
            }
            else if (_letter == "交") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark fst-normal";
                _sort_checkbox = "Six";
            }
            else if (_letter == "成") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary fst-normal";
                _sort_checkbox = "Seven";
            }
            else if (_letter == "売" && data[i]["ステータス"] == "売主辞退") {
                _letter = "辞";
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-light text-danger fst-normal";
                _sort_checkbox = "Eight";
            }
            else if (_letter == "買"  && data[i]["ステータス"] == "買主辞退") {
                _letter = "辞";
                data[i]["ステータス"] = "買主辞退";
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-dark text-white fst-normal fst-normal";
                _sort_checkbox = "Nine";
            }
            else if (_letter == "削") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-danger text-white fst-normal fst-normal";
                _sort_checkbox = "Ten";
            }

        }
        html += '<tr>\
            <td class= "text-end" > ' + data[i]["NO"] + '</td>\
            <td class="'+ _sort_checkbox + '"><i class="'+ _class + '">' + _letter + '</i><span class="font-semibold">' + data[i]["ステータス"] + '</span></td>\
            <td><a class="text-heading font-semibold text-decoration-underline" data-bs-toggle="modal" data-bs-target="#mansion" id='+ data[i]["SellerMansionID"] + ' href="#">'+ data[i]["マンション名＆部屋番号"]+'</a></td>\
            <td><a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["RealECD"] + '&t_reale_purchase' + ' onclick="l_logfunction(this.id)">' + data[i]["不動産会社"] + '</a></td>\
            <td class="text-nowrap">' + data[i]["登録日時"] + '</td>\
            <td class="text-nowrap">' + data[i]["簡易査定日時"] + '</td>\
            <td class="text-nowrap"> <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["AssReqID"] + '&t_seller_assessment_detail' + ' onclick="l_logfunction(this.id)"> ' + data[i]["詳細査定日時"] + ' </a> </td>\
            <td class="text-nowrap"> '+ data[i]["買取依頼日時"] + ' </td>\
            <td class="text-nowrap"> '+ data[i]["送客日時"] + ' </td>\
            <td class="text-nowrap"> '+ data[i]["終了日時"] + '</td>\
            </tr>'
    }
    if (data.length > 0) {
        $('#total_record').text("検索結果：" + data.length + "件")
        $('#total_record_up').text("検索結果：" + data.length + "件")
    }
    else {
        $('#total_record').text("検索結果： 0件")
        $('#total_record_up').text("検索結果： 0件")
    }
    $('#mansiontable tbody').append(html);

    sortTable.getSortingTable("mansiontable");
}
function l_logfunction(id) {
    let model = {
        LoginKBN: null,
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        Page: null,
        Processing: null,
        Remarks: null,
        LogId: id.split('&')[0],
        LogStatus: id.split('&')[1]

    };
    common.callAjax(_url.insert_l_log, model,
        function (result) {
            if (result && result.isOK) {
                if (model.LogStatus = "t_reale_purchase") {
                    //alert("https://www.seruichi.com/t_reale_purchase?RealECD=" + model.LogId);
                   
                }
                else if (model.LogStatus = "t_seller_assessment_detail" ) {
                    
                    //alert("https://www.seruichi.com/t_seller_assessment_detail?AssReqID=" + model.LogId);
                   
                }
            }
            if (result && !result.isOK) {
            }
        });
}

function Popup_function(id) {
    
    var Popupurl = common.appPath+'/t_seller_assessment/PopUpPage?smID=' + id;
    //window.open(Popupurl, "WindowPopup", 'width=700px,height=700px,top=150,left=250');
    window.location.href = Popupurl;
}
