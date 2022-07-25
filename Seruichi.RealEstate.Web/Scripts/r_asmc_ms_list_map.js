const _url = {};
let mem_condition_at_display = {};

$(function () {
    setValidation();
    _url.getMansionData = common.appPath + '/r_asmc_ms_list_map/GetMansionData';
    _url.gotoNextPage = common.appPath + '/r_asmc_ms_list_map/GotoNextPage';
    setValidation();
    addEvents();

    $('input[defaultcheck="1"]').prop('checked', true).change();
    $('#Unregistered').change();
    $('#MansionName').focus();

    //display Map
    const fd = new FormData(document.forms.form1);
    const model = Object.fromEntries(fd);
    model.CityCDList = $('#hdnCityCDList').val();
    model.TownCDList = $('#hdnTownCDList').val();

    mem_condition_at_display = model;

    common.callAjaxWithLoading(_url.getMansionData, model, this, function (result) {
        if (result && result.isOK) {
            displayMap(JSON.parse(result.data));
        }
    });
});

function setValidation() {
    $('#MansionName')
        .addvalidation_errorElement("#errorMansionName")
        .addvalidation_doublebyte();

    $('#StartYear')
        .addvalidation_errorElement("#errorYear")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104

    $('#EndYear')
        .addvalidation_errorElement("#errorYear")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104

    $('#StartDistance')
        .addvalidation_errorElement("#errorDistance")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104

    $('#EndDistance')
        .addvalidation_errorElement("#errorDistance")
        .addvalidation_maxlengthCheck(2)//E105
        .addvalidation_singlebyte_number()//E104

    $('#StartRooms')
        .addvalidation_errorElement("#errorRooms")
        .addvalidation_maxlengthCheck(3)//E105
        .addvalidation_singlebyte_number()//E104

    $('#EndRooms')
        .addvalidation_errorElement("#errorRooms")
        .addvalidation_maxlengthCheck(3)//E105
        .addvalidation_singlebyte_number()//E104

    $('#btnDisplay')
        .addvalidation_errorElement("#errorbtnDisplay"); //E303
}

function addEvents() {

    common.bindValidationEvent('#form1', ':not(#StartYear,#EndYear,#StartDistance,#EndDistance,#StartRooms,#EndRooms)');

    $('#StartYear, #EndYear').on('change', function () {
        checkCompareStartEnd($('#StartYear'), $('#EndYear'));
    });

    $('#StartDistance, #EndDistance').on('change', function () {
        checkCompareStartEnd($('#StartDistance'), $('#EndDistance'));
    });

    $('#StartRooms, #EndRooms').on('change', function () {
        checkCompareStartEnd($('#StartRooms'), $('#EndRooms'));
    });

    $('#Unregistered').on('change', function () {
        if ($('#Unregistered:checked').length > 0) {
            $('#chkPriority').prop('checked', false).prop("disabled", true);
            $('.Priority').prop('checked', false).prop("disabled", true);
        }
        else {
            $('#chkPriority').removeAttr("disabled");
        }
    });

    $('#chkPriority').on('change', function () {
        if ($('#chkPriority:checked').length > 0) {
            $('.Priority').removeAttr("disabled");
        }
        else {
            $('.Priority').prop('checked', false).prop("disabled", true);
        }
    });

    $('#btnDisplay').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            common.setFocusFirstError($form);
            return false;
        }

        if (!checkCompareStartEnd($('#StartYear'), $('#EndYear'))) {
            $('#StartYear').focus();
            return false;
        }

        if (!checkCompareStartEnd($('#StartDistance'), $('#EndDistance'))) {
            $('#StartYear').focus();
            return false;
        }

        if (!checkCompareStartEnd($('#StartRooms'), $('#EndRooms'))) {
            $('#StartYear').focus();
            return false;
        }

        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        model.TownCDList = getSelectedTownCDList();

        if (!model.MansionName && !model.TownCDList
            && !model.StartYear && !model.EndYear
            && !model.StartDistance && !model.EndDistance
            && !model.StartRooms && !model.EndRooms
            && !model.Priority) {
            $('#btnDisplay').showError(common.getMessage('E303'));
            return;
        }

        mem_condition_at_display = model;

        common.callAjaxWithLoading(_url.getMansionData, model, this, function (result) {
            if (result && result.isOK) {
                displayMap(JSON.parse(result.data));
            }
            else if (result && result.message) {
                $('#btnDisplay').showError(result.message.MessageText1);
            }
            else if (result && result.data) {
                //error
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    })

    $(document).on('click', '.js-infowin-mansionname', function () {
        const p = {
            mansionCD: $(this).data('mansioncd'),
            mansionName: this.innerHTML,
        }
        gotoNextPage(p);
    });

    $(document).on('click', '.js-infowin-btnnew', function () {
        const $ctrl = $(this).parents().parent().parent().find('.js-infowin-mansionname');
        if ($ctrl) {
            const p = {
                mansionCD: $ctrl.data('mansioncd'),
                mansionName: $ctrl.html(),
            }
            gotoNextPage(p);
        }
    });
}

function checkCompareStartEnd($start, $end) {

    if (!common.checkValidityInput($start)) {
        return false;
    }
    if (!common.checkValidityInput($end)) {
        return false;
    }

    const start_val = $start.val();
    const end_val = $end.val();

    if (start_val && end_val) {
        if (Number(start_val) > Number(end_val)) {
            $start.showError(common.getMessage('E113'));
            return false;
        }
    }

    $start.hideError();
    return true;
}

function getSelectedTownCDList() {
    let townCD = '';
    $('.js-node-town:checkbox:checked').each(function () {
        townCD += $(this).val() + ',';
    });
    return townCD;
}

function displayMap(mansions) {
    $mapArea = $('#googleMap').empty();

    if (mansions.length == 0) {
        $('#no_record').text(common.getMessage('E116'));
        return;
    }
    else {
        $('#no_record').text('');
    }

    var map = new google.maps.Map(document.getElementById('googleMap'), {
        zoom: 15,
        center: new google.maps.LatLng(mansions[0].Latitude, mansions[0].Longitude),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });

    var infowindow = new google.maps.InfoWindow();

    var marker, i;
    var markers = [];

    for (var i = 0; i < mansions.length; i++) {
        const item = mansions[i];
        const color = item.ExpDateText ? '#FFFF00' : item.ValidFLG === 0 ? '#0000FF' : item.ValidFLG === 1 ? '#FF0000' : '#C0C0C0';

        marker = new google.maps.Marker({
            position: new google.maps.LatLng(item.Latitude, item.Longitude),
            map: map,
            icon: {
                fillColor: color,                       //塗り潰し色
                fillOpacity: 1.0,                       //塗り潰し透過率
                path: google.maps.SymbolPath.CIRCLE,    //円を指定
                scale: 16,                              //円のサイズ
                strokeColor: '#808080',                 //枠の色
                strokeWeight: 1.0                       //枠の透過率
            },
            label: {
                text: 'M',                              //ラベル文字
                color: '#FFFFFF',                       //文字の色
                fontSize: '20px'                        //文字のサイズ
            },
        });

        markers.push(marker);

        google.maps.event.addListener(marker, 'mouseover', (function (marker, i) {
            return function () {
                infowindow.setContent(createInfoWindowHtml(item));
                infowindow.open(map, marker);
            }
        })(marker, i));
    }
}

function createInfoWindowHtml(item) {
    const css_validflg = item.ValidFLG == 0 ? 'cap-text-primary' : item.ValidFLG == 1 ? 'text-danger' : 'text-dark';
    const css_registered = item.RegisteredFLG == 1 ? 'text-danger' : 'text-success';

    return '<div class=\'row justify-content-center\'> <div class=\'w-100\'> <div class=\'card shadow-0 border rounded-3 h-100\'> <div class=\'card-body\'> <div class=\'row\'> <div class=\'col-md-12  col-lg-9 col-xl-9\'> <h5><a href=\'#\' class=\'text-decoration-underline js-infowin-mansionname\' tabindex=\'0\' data-mansioncd=\'' + item.MansionCD + '\'>' + item.MansionName + '</a></h5> <div class=\'d-flex flex-row mt-2\'> <div class=\'text-danger mb-1 me-2\'>' + item.Priority + '</div> </div> <p class=\'' + css_validflg + '\'>' + item.ValidFLGText + '</p> <div class=\'mt-1 mb-2\'> <span>' + item.ConstYYYYMM + '</span> <span class=\'text-primary\'> • </span> <span>築' + item.BuildingAge + '年</span> <span class=\'text-primary\'> • </span> <span>駅から徒歩' + item.Distance + '分</span> <span class=\'text-primary\'> • </span> <span>' + item.Rooms + '戸<br></span> </div> <div class=\'mt-1 mb-2\'> <span class=\'small\'>最終更新日 : </span><span class=\'small\'>' + item.UpdateDateTime + '</span> <span class=\'text-primary\'> • </span> <span class=\'small pt-1\'>有効期限日 : </span><span class=\'small pt-1\'>' + item.ExpDate + '</span><span class=\'small pt-1 text-warning ps-md-2\'>' + item.ExpDateText + '</span><br> </div> <p class=\'text-dark mb-2 mb-md-0 small h-md-10\'>' + item.PrefName + item.CityName + item.TownName + item.Address + '</p> </div> <div class=\'col-md-12 col-lg-3 col-xl-3 border-start\'> <h6 class=\'' + css_registered + '\'>' + item.RegisteredText + '</h6> <div class=\'d-flex flex-column mt-5\'> <button class=\'btn btn-primary mt-2 js-infowin-btnnew\' type=\'button\'> 新規登録 </button> </div> </div> </div> </div> </div> </div> </div>'
}

function gotoNextPage(param) {

    const frm = document.createElement("form")
    const fd = new FormData()
    fd.append("MansionCD", param.mansionCD);
    fd.append("MansionName", param.mansionName);
    fd.append("ConditionJson", JSON.stringify(mem_condition_at_display));

    frm.addEventListener("formdata", eve => {
        for (const [name, value] of fd.entries()) {
            eve.formData.append(name, value)
        }
    })
    document.body.append(frm)

    common.callSubmit(frm, _url.gotoNextPage);
}