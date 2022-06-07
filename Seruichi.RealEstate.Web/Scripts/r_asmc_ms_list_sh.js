const _url = {};
let rdoPriority = 0;
$(function () {
    setValidation();
    _url.get_DisplayData = common.appPath + '/r_asmc_ms_list_sh/get_DisplayData';
    _url.InsertL_Log = common.appPath + '/r_asmc_ms_list_sh/Insert_l_log';
    setValidation();
    addEvents();
    $('#MansionName').focus();
});
function setValidation() {
    $('#MansionName')
        .addvalidation_errorElement("#errorMansionName")
        .addvalidation_maxlengthCheck(25)//E105
        .addvalidation_doublebyte();

    $('#StartYear')
        .addvalidation_errorElement("#errorYear")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104
        .addvalidation_datecompare(); //E113

    $('#EndYear')
        .addvalidation_errorElement("#errorYear")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104
        .addvalidation_datecompare(); //E113

    $('#StartDistance')
        .addvalidation_errorElement("#errorDistance")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104

    $('#EndDistance')
        .addvalidation_errorElement("#errorDistance")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number();//E104

    $('#StartRooms')
        .addvalidation_errorElement("#errorRooms")
        .addvalidation_maxlengthCheck(3)//E105
        .addvalidation_singlebyte_number();//E104

    $('#EndRooms')
        .addvalidation_errorElement("#errorRooms")
        .addvalidation_maxlengthCheck(3)//E105
        .addvalidation_singlebyte_number();//E104

    $('#DisplayArea')
        .addvalidation_errorElement("#errorDisplayArea")
        .addvalidation_maxlengthCheck(3);//E105

    $('#btnDisplay')
        .addvalidation_errorElement("#errorbtnDisplay"); //E303
}

function addEvents() {
    $('#StartYear, #EndYear').on('change', function () {
        const $this = $(this), $start = $('#StartYear').val(), $end = $('#EndYear').val()

        if (!common.checkValidityInput($this)) {
            return false;
        }
        if ($start && $end) {
            if (Number($start) <= Number($end)) {
                $("#StartYear").hideError();
                $("#EndYear").hideError();
                $("#EndYear").focus();
                return;
            }
        }
    });
    $('#StartDistance, #EndDistance').on('change', function () {
        const $this = $(this), $start = $('#StartDistance').val(), $end = $('#EndDistance').val()

        if (!common.checkValidityInput($this)) {
            return false;
        }
        if ($start && $end) {
            if (!StartEndCompare('Distance'))
                return false;
            else if (Number($start) <= Number($end)) {
                $("#StartDistance").hideError();
                $("#StartDistance").hideError();
                $("#EndDistance").focus();
                return;
            }
        }
    });
    $('#StartRooms, #EndRooms').on('change', function () {
        const $this = $(this), $start = $('#StartRooms').val(), $end = $('#EndRooms').val()

        if (!common.checkValidityInput($this)) {
            return false;
        }
        if ($start && $end) {
            if (!StartEndCompare('Rooms'))
                return false;
            else if (Number($start) <= Number($end)) {
                $("#StartRooms").hideError();
                $("#StartRooms").hideError();
                $("#EndRooms").focus();
                return;
            }
        }
    });

    $('#UnregisteredMansion').on('click', function () {
        if ($('#UnregisteredMansion:checked').length > 0) {
            $('#chkPriority').attr("disabled", true);
            $('#chkPriority').prop('checked', false);
            $('.Priority').attr("disabled", true);
        }
        else {
            $('#chkPriority').removeAttr("disabled");
            $('.Priority').removeAttr("disabled");
        }
    })

    $('#UnregisteredMansion').click();

    $('#btnDisplay').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        if (!StartEndCompare('Distance')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        if (!StartEndCompare('Rooms')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

        const $MansionName = $("#MansionName").val().trim(), $StartYear = $("#StartYear").val(), $EndYear = $('#EndYear').val()
        var CityCD = '', CityGPCD = '';
        $('.node-parent:checkbox:checked').each(function () {
            CityGPCD += $(this).val() + ',';
        });
        $('.node-item:checkbox:checked').each(function () {
            CityCD += $(this).val() + ',';
        });
        $('.form-check-input:radio:checked').each(function () {
            rdoPriority = $(this).val();
        });

        let model = {
            MansionName: $MansionName,
            CityGPCD: CityGPCD,
            CityCD: CityCD,
            StartYear: Get_FT_Age($EndYear, 'F'),
            EndYear: Get_FT_Age($StartYear, 'T'),
            StartDistance: $('#StartDistance').val(),
            EndDistance: $('#EndDistance').val(),
            StartRooms: $('#StartRooms').val(),
            EndRooms: $('#EndRooms').val(),
            Unregistered: $('#UnregisteredMansion').is(':checked') ? 1 : 0,
            Priority: $('#chkPriority').is(':checked') ? 1 : 0,
            Radio_Rating: rdoPriority
        };

        if (model.MansionName == "" && model.CityGPCD == "" && model.CityCD == "" && model.StartYear == "" && model.EndYear == "" && model.Radio_Rating == 0) {
            $('#btnDisplay').showError(common.getMessage('E303'));
            $('#MansionName').focus();
        }
        else {
            $('#displayArea').empty();
            get_DisplayData(model, $form);
        }
    })
}

function StartEndCompare(type) {
    switch (type) {
        case 'Distance':
            if ($("#StartDistance").val() != "" && $("#EndDistance").val() != "") {
                if (!start_endCompare($("#StartDistance").val(), $("#EndDistance").val())) {
                    $("#StartDistance").showError(common.getMessage('E113'));
                    $("#StartDistance").focus();
                    return false;
                }
            }
            break;
        case 'Rooms':
            if ($("#StartRooms").val() != "" && $("#EndRooms").val() != "") {
                if (!start_endCompare($("#StartRooms").val(), $("#EndRooms").val())) {
                    $("#StartRooms").showError(common.getMessage('E113'));
                    $("#StartRooms").focus();
                    return false;
                }
            }
            break;
    }
    return true;
}

function start_endCompare(v1, v2) {
    const val1 = Number(v1);
    const val2 = Number(v2);
    let success = true;
    if (val1 > val2) {
        success = false;
    }
    return success;
}

function Get_FT_Age(age, type) {

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    var start_yyyymm = '', end_yyyymm = '';

    //Forｍ.築年数（To） => 築年月(From)
    if (type == 'F') {
        if (age !== '') {
            start_yyyymm = (yyyy - age) + String(parseInt(mm) + 1).padStart(2, '0');
        }
        return start_yyyymm;
    }
    //Forｍ.築年数（From） => 築年月(To)
    else if (type == 'T') {
        if (age !== '') {
            end_yyyymm = (yyyy - (age - 1)) + mm;
        }
        return end_yyyymm;
    }
}

function get_DisplayData(model, $form) {
    common.callAjaxWithLoading(_url.get_DisplayData, model, this, function (result) {
        if (result && result.isOK) {
            Bind_Data(JSON.parse(result.data));
        }
        if (result && !result.isOK) {
            if (result.message.MessageText1 != "") {
                $('#btnDisplay').showError(result.message.MessageText1);
            }
            else {
                const errors = result.data;
                for (key in errors) {
                    const target = document.getElementById(key);
                    $(target).showError(errors[key]);
                    $form.getInvalidItems().get(0).focus();
                }
            }
        }
    });
}

function Bind_Data(data) {
    var div_Display = '';
    for (var i = 0; i < data.length; i++) {
        var txt_class = '', validFLG_class = '';
        if (data[i]["登録有無"] == '未登録')
            txt_class = 'text-success';
        else if (data[i]["登録有無"] == '登録済')
            txt_class = 'text-danger';

        if (data[i]["公開状況"] == '(未公開)')
            validFLG_class = 'text-danger';
        else if (data[i]["公開状況"] == '(公開済)')
            validFLG_class = 'text-primary';

        div_Display += '\
            <div class="col" >\
                <div class="row justify-content-center">\
                    <div class="col-md-12">\
                        <div class="card shadow-0 border rounded-3 h-100">\
                            <div class="card-body">\
                                <div class="row">\
                                    <div class="col-12">\
                                        <h5 class="d-md-inline-flex">\
                                            <a href="#" id='+ data[i]["マンションCD"] + ' class="text-decoration-underline" onclick="MansionName_Click(this)">' + data[i]["マンション名"] + '</a>\
                                            <h6 class="' + txt_class + ' float-end">' + data[i]["登録有無"] + '</h6>\
                                        </h5>\
                                        <hr class="opacity-20 my-5 mt-2 mb-2">\
                                            <p class="text-dark mb-2 mb-md-0 small">' + data[i]["住所"] + '</p>\
                                            <div class="mt-1 mb-2">\
                                                <span>' + data[i]["築年月"] + '</span>\
                                                <span class="text-primary"> • </span>\
                                                <span>築' + data[i]["築年数"] + '年</span>\
                                                <span class="text-primary"> • </span>\
                                                <span>駅から徒歩' + data[i]["最寄り駅距離"] + '分</span>\
                                                <span class="text-primary"> • </span>\
                                                <span>' + data[i]["総戸数"] + '戸<br /></span>\
                                            </div>\
                                            </div>\
                                        <div class="">\
                                            <div class=" d-xxl-flex">\
                                                <span class="small">最終更新日 : </span><span class="small">' + data[i]["最終更新日"] + '</span>\
                                                <span class="' + validFLG_class + ' ps-2 ps-xxl-5 text-end small"> ' + data[i]["公開状況"] + '</span>\
                                            </div>\
                                            <div class="d-xxl-flex">\
                                                <span class="small pt-1">有効期限日 : </span><span class="small pt-1">' + data[i]["有効期限日"] + '</span>\
                                                <div class="d-flex flex-row ps-xxl-5">\
                                                    <div class="text-danger mb-1 me-2 em">' + data[i]["優先度表示"] + '</div>\
                                                </div>\
                                            </div>\
                                        </div>\
                                    </div>\
                                </div>\
                            </div>\
                        </div>\
                    </div>\
                </div>'
    }

    $('#displayArea').append(div_Display);
}

function MansionName_Click(ctrl) {
    let model = {
        LogDateTime: null,
        LoginKBN: '2',
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        PageID: 'r_asmc_ms_list_sh',
        ProcessKBN: 'link',
        Remarks: 'r_asmc_set_ms ' + ctrl.id + ' ' + ctrl.innerHTML,
    };
    common.callAjax(_url.InsertL_Log, model,
        function (result) {
            if (result && result.isOK) {
                window.location.href = common.appPath + '/r_asmc_set_ms/Index?MansionCD=' + ctrl.id;
            }
        });
}