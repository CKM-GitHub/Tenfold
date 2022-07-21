const _url = {};
const _session = {};
$(function () {
    _url.get_t_assess_soudan_DisplayData = common.appPath + '/t_assess_soudan/get_t_assess_soudan_DisplayData';
    _url.get_t_assess_soudan_CSVData = common.appPath + '/t_assess_soudan/get_t_assess_soudan_CSVData';
    _url.get_Modal_infotrainData = common.appPath + '/t_assess_soudan/get_Modal_infotrainData';
    _url.get_Modal_consultResData = common.appPath + '/t_assess_soudan/get_Modal_consultResData';
    _url.modify_Modal_consultResData = common.appPath + '/t_assess_soudan/modify_Modal_consultResData';
    _url.get_Modal_profileData = common.appPath + '/t_assess_soudan/get_Modal_profileData';
    _url.get_Modal_contactData = common.appPath + '/t_assess_soudan/get_Modal_contactData';
    _url.get_Modal_fudousanData = common.appPath + '/t_assess_soudan/get_Modal_fudousanData';
    _url.modify_consultData = common.appPath + '/t_assess_soudan/modify_consultData';
    _url.insert_l_log = common.appPath + '/t_assess_soudan/Insert_l_log';
    setValidation();
    addEvents();
    $('#FreeWord').focus();
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

    $('#FreeWord')
        .addvalidation_errorElement("#errorFreeWord")
        .addvalidation_maxlengthCheck(50);

    $('.form-check-input')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112

    $('.comment')
        .addvalidation_errorElement("#errorcomment")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(2000)
        .addvalidation_singlebyte_doublebyte();
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

    let model = {
        FreeWord: $('#FreeWord').val(),
        untreated: $('#untreated').val(),
        processing: $('#processing').val(),
        solution: $('#solution').val(),
        Range: $('#ddlRange').val(),
        StartDate: $('#StartDate').val(),
        EndDate: $('#EndDate').val(),
        M_PIC: $('#M_PIC').val()
    };
    get_t_assess_soudan_DisplayData(model, this);

    sortTable.getSortingTable("tblsoudan");

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
        $('#tblsoudan tbody').empty();
        $('#type').val('display');
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        get_t_assess_soudan_DisplayData(model, $form);
    });

    $('#btnCSV').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#tblsoudan tbody').empty();
        $('#type').val('csv');
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        get_t_assess_soudan_DisplayData(model, $form);

        common.callAjax(_url.get_t_assess_soudan_CSVData, model,
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
                    downloadLink.download = "処理待ちリスト＿相談_" + dateString + ".csv";

                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }
                else {
                    $('#csv-error-modal').modal('show');
                }
            }
        )
    });

    $('#btn_addcomment').on('click', function () {
        $form = $('#div_comment').hideChildErrors();
        if (!common.checkValidityOnSave('#div_comment')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        var model = {
            comment: $('textarea#comment').val()
        }
        Add_Comment(model);
        $form = $('#div_comment').hideChildErrors();
    });

    $('#btnAdd_OK').on('click', function () {
        $('#btnProcess').trigger('click');
    });

    $('#btnSave').on('click', function () {
        var model = {
            Range: $('#status').val(),
            M_PIC: $('#pic').val(),
            ConsultID: _session.ConsultID,
            type: 'save'
        }
        modify_ConsultData(model);
    });

    $('#note_check').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if (this.value == 1)
            $('#btnSend').removeClass('disabled');
        else
            $('#btnSend').addClass('disabled');
    });

    $('#btnSend').on('click', function () {
        var model = {
            ConsultID: _session.ConsultID,
            type: 'send'
        }
        modify_ConsultSendData(model);
    });

    $('#btnSend_OK').on('click', function () {
        $('#btnProcess').trigger('click');
    });
}

function get_t_assess_soudan_DisplayData(model, type, $form) {
    common.callAjaxWithLoading(_url.get_t_assess_soudan_DisplayData, model, this, function (result) {
        if (result && result.isOK) {
            Bind_DisplayData(result.data);
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

function Bind_DisplayData(result) {
    let _letter = "", _class = "", _snbg_color = "", _rnbg_color = "";
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {

            if (isEmptyOrSpaces(data[i]["ステータス"])) {
                _letter = "";
                _class = "ms-1 ps-1 pe-1 rounded-circle";
            }
            else {
                _letter = data[i]["ステータス"].charAt(0);
                if (_letter == "未") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-primary text-white fst-normal fst-normal";
                }
                else if (_letter == "処") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning text-white fst-normal fst-normal";
                }
                else if (_letter == "解") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white fst-normal fst-normal";
                }
            }

            if (data[i]["ConsultFrom"] = '1') {
                _snbg_color = "bg-warning";
                _rnbg_color = "";
            }
            else {
                _snbg_color = "";
                _rnbg_color = "bg-warning";
            }

            html += '<tr>\
            <td class= "text-end"> ' + (i + 1) + '</td>\
            <td><span class="' + _class + '">' + _letter + '</span><span class="font-semibold"> ' + data[i]["ステータス"] + '</span></td>\
            <td class="text-nowrap">'+ data[i]["相談ID"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id="'+ data[i]["SellerMansionID"] + '&' + data[i]["SellerCD"] + '&' + data[i]["相談ID"] + '&' + data[i]["RealECD"] + '&' + data[i]["PurchReqDateTime"].replace(" ", "&") + '" data-bs-toggle="modal" data-bs-target="#soudan" href="#" onclick="Display_Detail(this.id, this)"><span class="' + _snbg_color + ' pt-1 pb-1 ps-2 pe-2">' + data[i]["売主名"] + '</span></a></td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id="'+ data[i]["SellerMansionID"] + '&' + data[i]["SellerCD"] + '&' + data[i]["相談ID"] + '&' + data[i]["RealECD"] + '&' + data[i]["PurchReqDateTime"].replace(" ", "&") + '" data-bs-toggle="modal" data-bs-target="#soudan" href="#" onclick="Display_Detail(this.id, this)"><span class="' + _rnbg_color + ' pt-1 pb-1 ps-2 pe-2">' + data[i]["不動産会社"] + '</span></a></td>\
            <td class="text-nowrap">'+ data[i]["管理担当"] + '</td>\
            <td class="text-nowrap">'+ data[i]["相談発生日時"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["相談解決日時"] + '</td>\
            <td class="text-center"> '+ data[i]["相談区分"] + '</td>\
            <td class="d-none">' + data[i]["相談ID"] + '&' + data[i]["M_ConsultForm"] + '&' + data[i]["ConsultDateTime"] + '&' + data[i]["相談区分"] + '&' + data[i]["Consultation"] + '&' + data[i]["ProgressKBN"] + '&' + data[i]["TenStaffCD"] + '</td>\
            </tr>'
        }

        $('#total_record_up').text("検索結果： " + data.length + "件");
        $('#total_record_down').text("検索結果： " + data.length + "件");
        $('#no_record').text("");
    }
    else {
        $('#total_record_up').text("検索結果： 0件 ");
        $('#total_record_down').text("検索結果： 0件");
        $('#no_record').text("表示可能データがありません");
    }
    $('#tblsoudan tbody').append(html);
}

function Display_Detail(id, ctrl) {
    var PurchReqDateTime = '';
    if (id.split('&')[4] == '') {
    }
    else {
        var Date = id.split('&')[4];
        var Time = id.split('&')[5];
        PurchReqDateTime = Date + ' ' + Time;
    }

    let model = {
        SellerMansionID: id.split('&')[0],
        SellerCD: id.split('&')[1],
        ConsultID: id.split('&')[2],
        RealECD: id.split('&')[3]
    };

    _session.ConsultID = model.ConsultID;

    $('#pills-info-tab').click();
    var mc_data = $(ctrl).parent().closest('tr').children('td:eq(9)').text();
    $('#ConsultID').text(mc_data.split('&')[0]);
    $('#ConsultFrom').text(mc_data.split('&')[1]);
    $('#ConsultDateTime').text(mc_data.split('&')[2]);
    $('#ConsultType').text(mc_data.split('&')[3]);
    $('#Consultation').text(mc_data.split('&')[4]);
    $('#status').val(mc_data.split('&')[5]);
    $('#pic').val(mc_data.split('&')[6]);

    Bind_ModalPopupData(model, PurchReqDateTime);
}

function Bind_ModalPopupData(model, PurchReqDateTime) {
    common.callAjax(_url.get_Modal_infotrainData, model, function (result) {
        if (result && result.isOK) {
            Bind_Modal_InfoTrainData(result.data, PurchReqDateTime);
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

    Bind_Comment(model);

    common.callAjax(_url.get_Modal_profileData, model, function (result) {
        if (result && result.isOK) {
            Bind_Modal_ProfileData(result.data);
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

    common.callAjax(_url.get_Modal_contactData, model, function (result) {
        if (result && result.isOK) {
            Bind_Modal_ContactData(result.data);
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

    common.callAjax(_url.get_Modal_fudousanData, model, function (result) {
        if (result && result.isOK) {
            Bind_Modal_FudousanData(result.data);
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

function Bind_Comment(model) {
    common.callAjax(_url.get_Modal_consultResData, model, function (result) {
        if (result && result.isOK) {
            Bind_Modal_ConsultResData(result.data);
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

function Bind_Modal_InfoTrainData(result, PurchReqDateTime) {
    let data = JSON.parse(result);
    let MansionInfo = "";
    let html = "";
    let htmlTemp = "";
    const isEven = data.length % 2 === 0 ? 'Even' : 'Odd';
    $('#H_MansionInfo').empty();
    $('#ekikoutsu').empty();

    if (data.length > 0) {
        //Display Data of MansionInfo
        MansionInfo = '<h4 class="text-center fw-bold">' + data[0]["MansionName"] + '<strong>' + data[0]["RoomNumber"] + '</strong></h4>\
        <p class="font-monospace small text-start pt-3">'+ data[0]["Address"] + '</p >\
        <p class="font-monospace small text-center pt-3">買取依頼日時 ' + PurchReqDateTime + '</p>'
        $('#H_MansionInfo').append(MansionInfo);

        //Bind Data of ekikoutsu
        for (var i = 0; i < data.length; i++) {
            html += '<div class="card p-1 col-12 col-md-6 shadow-sm">\
                            <div class="card-body p-2 row">\
                                <div class="col-12">\
                                    <h5>'+ data[i]["LineName"] + '</h5>\
                                </div>\
                                <div class="col-12">\
                                    <p class="text-dark">'+ data[i]["StationName"] + '</p>\
                                </div>\
                                <div class="col-12 text-end">\
                                    <p class="text-dark">'+ data[i]["Distance"] + '</p>\
                                </div>\
                            </div>\
                         </div>'
        }
        htmlTemp = '<div class="invisible card p-1 col-12 col-md-6 shadow-sm"> </div>'
        if (isEven == 'Odd') {
            $('#ekikoutsu').append(html + htmlTemp);
        }
        else {
            $('#ekikoutsu').append(html);
        }
    }
}

function Bind_Modal_ConsultResData(result) {
    var data = JSON.parse(result);
    var html = '';
    for (var i = 0; i < data.length; i++) {
        html += '<div class="row pb-3">\
                    <div class="col-12" >\
                        <p class="text-secondary text-center">' + data[i]['入力日時'] + '</p>\
                    </div>\
                    <div class="col-12">\
                        <p><strong>' + data[i]['入力スタッフCD'] + '</strong></p>\
                        <div class="clearfix"></div>\
                        <p>' + data[i]['コメント'] + '</p>\
                        <p><a class="float-right btn text-white btn-danger" onclick="Delete_ConsultResData(\'' + data[i]['ResponseSEQ'] + '\',\'' + data[i]['ConsultID'] + '\')"> <i class="fa fa-times"></i> 削除</a></p>\
                    </div>\
                </div >';
    }

    $('#consultRes_comment').append(html);
}

function Bind_Modal_ProfileData(result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        $('#Building_structure').text(data[0]['建物構造']);
        $('#Year_of_construction').text(data[0]['築年数']);
        $('#Room_No').text(data[0]['部屋番号']);
        $('#Area').text(data[0]['専有面積']);
        $('#Balcony_area').text(data[0]['バルコニー面積']);
        $('#Sunlighting').text(data[0]['主採光']);
        $('#Number_of_rooms').text(data[0]['間取り']);
        $('#Bathrooms_and_toilets').text(data[0]['バス・トイレ']);
        $('#land_rights').text(data[0]['土地権利']);
        $('#present_state').text(data[0]['現況']);
        $('#management_system').text(data[0]['管理方式']);
        $('#Desired_time_of_sale').text(data[0]['売却希望時期']);
        $('#rent_fee').text(data[0]['家賃'] + '円');
        $('#management_fee').text(data[0]['管理費'] + '円');
        $('#repair_fee').text(data[0]['修繕積立金'] + '円');
        $('#etc_fee').text(data[0]['その他費用'] + '円');
        $('#tax').text(data[0]['固定資産税'] + '円');
    }
}

function Bind_Modal_ContactData(result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        $('#Name_Katakana').text(data[0]['カナ名']);
        $('#Name_Kanji').text(data[0]['漢字名']);
        $('#pc_SellerCD').text(data[0]['ユーザーCD']);
        $('#pc_Address').text(data[0]['PrefName'] + data[0]['CityName'] + data[0]['TownName'] + data[0]['Address1'] + data[0]['Address2']);
        $('#pc_Phone').text(data[0]['固定電話']);
        $('#pc_Mobile_phone').text(data[0]['携帯電話']);
        $('#pc_mail_address').text(data[0]['メールアドレス']);
    }
}

function Bind_Modal_FudousanData(result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        $('#REImage').prop('src', 'data:image/gif;base64,' + data[0]['REFaceImage']);
        $('#REStaffName').text(data[0]['REStaffName']);
        $('#REKana').text(data[0]['REKana']);
        $('#REName').text(data[0]['REName']);
        $('#RealECD').text(data[0]['RealECD']);
        $('#REAddress').text(data[0]['REAddress']);
        $('#REHousePhone').text(data[0]['REHousePhone']);
        $('#REFax').text(data[0]['REFax']);
        $('#REMailAddress').text(data[0]['REMailAddress']);
    }
}

function Delete_ConsultResData(ResponseSEQ, ConsultID) {
    var model = {
        ResponseSEQ: ResponseSEQ,
        ConsultID: ConsultID,
        type: 'delete'
    }

    common.callAjaxWithLoadingSync(_url.modify_Modal_consultResData, model, this, function (result) {
        if (result && result.isOK) {
            var model = {
                ConsultID: _session.ConsultID
            }
            Bind_Comment(model);
        }
        else {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                this.getInvalidItems().get(0).focus();
            }
        }
    });
}

function Add_Comment(model) {
    common.callAjaxWithLoadingSync(_url.modify_Modal_consultResData, model, this, function (result) {
        if (result && result.isOK) {
            var model = {
                ConsultID: _session.ConsultID
            }
            Bind_Comment(model);
        }
        else {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                this.getInvalidItems().get(0).focus();
            }
        }
    });
}

function modify_ConsultData(model) {
    common.callAjaxWithLoadingSync(_url.modify_consultData, model, this, function (result) {
        if (result && result.isOK) {
            $('#soudan').modal('hide');
            $('#message-changed').modal('show');
        }
        else {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                this.getInvalidItems().get(0).focus();
            }
        }
    });
}

function modify_ConsultSendData(model) {
    common.callAjaxWithLoadingSync(_url.modify_consultData, model, this, function (result) {
        if (result && result.isOK) {
            $('#modal-send').modal('hide');
            $('#modal-sendok').modal('show');
        }
        else {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                this.getInvalidItems().get(0).focus();
            }
        }
    });
}