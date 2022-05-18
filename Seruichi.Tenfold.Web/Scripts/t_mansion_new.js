const _url = {};
const placeHolder_city = '市区町村';
const placeHolder_town = '町域名';
const placeHolder_line = '路線選択'
const placeHolder_station = '駅選択'

$(function () {
    _url.getBuildingAge = common.appPath + commonApiUrl.getBuildingAge;
    _url.getPrefDropDownList = common.appPath + commonApiUrl.getDropDownListItemsOfPrefecture;
    _url.getCityDropDownList = common.appPath + commonApiUrl.getDropDownListItemsOfCity;
    _url.getTownDropDownList = common.appPath + commonApiUrl.getDropDownListItemsOfTown;
    _url.getLineDropDownList = common.appPath + commonApiUrl.getDropDownListItemsOfLine;
    _url.getStationDropDownList = common.appPath + commonApiUrl.getDropDownListItemsOfStation;
    _url.getMansinoData = common.appPath + '/a_index/GetMansionData';
    _url.checkZipCode = common.appPath + '/t_mansion_new/CheckZipCode';
    _url.getMansionListByMansionWord = common.appPath + '/t_mansion_new/GetMansionListByMansionWord';
    _url.checkAll = common.appPath + '/t_mansion_new/CheckAll';

    _url.insertSellerMansionData = common.appPath + '/t_mansion_new/InsertSellerMansionData';
    setValidation();
    addEvents();
});

function setValidation() {
    //マンション名
    $('#MansionName')
        .addvalidation_errorElement("#errorName")
        .addvalidation_reqired()
        .addvalidation_doublebyte();

    //郵便番号
    $('#ZipCode1')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_singlebyte_number();
    $('#ZipCode2')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_singlebyte_number();
    ////都道府県
    $('#PrefCD')
        .addvalidation_errorElement("#errorPrefCD")
        .addvalidation_reqired();
    //市区町村
    $('#CityCD')
        .addvalidation_errorElement("#errorCityCD")
        .addvalidation_reqired();
    //町域
    $('#TownCD')
        .addvalidation_errorElement("#errorTownCD")
        .addvalidation_reqired();
    //住所
    $('#Address')
        .addvalidation_errorElement("#errorAddress")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    $('input[name="StructuralKBN"]:radio')
        .addvalidation_errorElement("#errorStructuralKBN")
        .addvalidation_reqired();
    ////築年月
    $('#ConstYYYYMM')
        .addvalidation_errorElement("#errorConstYYYYMM")
        .addvalidation_reqired()
        .addvalidation_custom('customValidation_checkConstYYYYMM');

    $('#Rooms')
        .addvalidation_errorElement("#errorRooms")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();

    ////土地・権利
    $('input[name="RightKBN"]:radio')
        .addvalidation_errorElement("#errorRightKBN")
        .addvalidation_reqired();

    ////交通アクセス
    $('#LineCD_1')
        .addvalidation_errorElement("#errorMansionStationInfo")
        .addvalidation_reqired();
    $('.js-stationcd')
        .addvalidation_errorElement("#errorMansionStationInfo")
        .addvalidation_custom('customValidation_checkStation');
    $('.js-distance')
        .addvalidation_errorElement("#errorMansionStationInfo")
        .addvalidation_singlebyte_number()
        .addvalidation_custom('customValidation_checkDistance');
    //謄本表記
    $('#Noti')
        .addvalidation_errorElement("#errorNoti")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    //カタカナ
    $('#katakana')
        .addvalidation_errorElement("#errorkatakana")
        .addvalidation_reqired()
        .addvalidation_doublebyte();

    //ｶﾀｶﾅ
    $('#katakana1')
        .addvalidation_errorElement("#errorkatakana1")
        .addvalidation_reqired()
        .addvalidation_singlebyte_number();
    //ひらがな
    $('#hirakana')
        .addvalidation_errorElement("#errorhirakana")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    //その他1
    $('#Other1')
        .addvalidation_errorElement("#errorOther1")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    //その他2
    $('#Other2')
        .addvalidation_errorElement("#errorOther2")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    //その他3
    $('#Other3')
        .addvalidation_errorElement("#errorOther3")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    //その他4
    $('#Other4')
        .addvalidation_errorElement("#errorOther4")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    //その他5
    $('#Other5')
        .addvalidation_errorElement("#errorOther5")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    //その他6
    $('#Other6')
        .addvalidation_errorElement("#errorOther6")
        .addvalidation_reqired()
        .addvalidation_doublebyte();

    $('#btnShowConfirmation')
        .addvalidation_errorElement("#errorProcess");
}

function addEvents() {
  

    //共通チェック処理
    common.bindValidationEvent('#form1', ':not(#ZipCode1,#ZipCode2)');

    //郵便番号
    $('#ZipCode1,#ZipCode2').on('change', function () {
        const $this = $(this), $zipCode1 = $('#ZipCode1'), $zipCode2 = $('#ZipCode2')

        if (!common.checkValidityInput($this)) {
            return false;
        }

        let model = {
            ZipCode1: $zipCode1.val(),
            ZipCode2: $zipCode2.val()
        };

        if (!model.ZipCode1 && !model.ZipCode2) {
            $($zipCode1, $zipCode2).hideError();
            return;
        }

        common.callAjax(_url.checkZipCode, model,
            function (result) {
                if (result && result.isOK) {
                    $($zipCode1).hideError();
                    $($zipCode2).hideError();

                    const data = result.data;

                    if (data.PrefCD) {
                        $('#PrefCD').val(data.PrefCD).hideError();
                        setCityList('add', data.PrefCD, data.CityCD);
                        setTownList('remove');
                        setLineList('add', data.PrefCD);
                        setStationList('remove');
                        //setTypeahead('#Name');
                    }
                    if (data.CityCD) {
                        setTownList('add', data.PrefCD, data.CityCD, data.TownCD);
                        $('#CityCD').hideError();
                    }
                    if (data.TownCD) {
                        $('#TownCD').val(data.TownCD).hideError();
                    }
                }
                if (result && !result.isOK) {
                    const message = result.message;
                    $this.showError(message.MessageText1);
                }
            });
    });

    //都道府県
    $('#PrefCD').on('change', function () {
        const inputval = $(this).val();
        if (inputval) {
            setCityList('add', inputval);
            setTownList('remove');
            setLineList('add', inputval);
            setStationList('remove');
            $('.js-distance').val('').hideError();
            //setTypeahead('#Name');
        }
        else {
            setCityList('remove');
            setTownList('remove');
            setLineList('remove');
            setStationList('remove');
            $('.js-distance').val('').hideError();
            //setTypeahead('#Name');
        }
    });
    //市区町村
    $('#CityCD').on('change', function () {
        setTownList('add', $('#PrefCD').val(), $(this).val());
    });

    ////路線選択
    $('.js-linecd').on('change', function () {
        const id = $(this).attr('id');
        const suffix = id.slice(-2).replace('_', '');
        const inputval = $(this).val();
        if (inputval) {
            setStationList('add', inputval, '#StationCD_' + suffix);
        }
        else {
            setStationList('remove', inputval, '#StationCD_' + suffix);
        }
    });
   
    //築年月
    $('#ConstYYYYMM').on('blur', function () {
        const $this = $(this);
        common.callAjax(_url.getBuildingAge, $this.val(), function (result) {
            if (result && result.isOK) {
                if (result.data) {
                    $('#BuildingAge').text('（築' + result.data + '年）').data('building-age', result.data);
                    if (result.data == 0)
                        $this.showError(common.getMessage('E208'));
                    else
                        $this.hideError();
                }
                else
                    $('#BuildingAge').text('（築　年）')
            }
        })
    });
    $('.js-distance').on('click', function () {
        const id = $(this).attr('id');
        const suffix = id.slice(-2).replace('_', '');
        var newValue = parseInt(suffix) + 1;
        $('#Dline_' + newValue).removeClass("bg-secondary");
        $('#Dline_' + newValue).find("*").prop("disabled", false);
    });

    $('#btnShowConfirmation').on('click', function () {
        debugger;
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            common.setFocusFirstError($form);
            return false;
        }

        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        model.PrefName = $('#PrefCD option:selected').text();
        model.CityName = $('#CityCD option:selected').text();
        model.TownName = $('#TownCD option:selected').text();
        model.ConstYYYYMM = model.ConstYYYYMM.replace('-', '');
        model.MansionStationListJson = JSON.stringify(getMansionStationList());
        common.callAjaxWithLoading(_url.checkAll, model, this, function (result) {
            if (result && result.isOK) {
                debugger;
                updateData = model;
                setScreenComfirm(updateData);
                $('#modal_1').modal('show');
            }
            if (result && result.data) {
                //error
                debugger;
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    });

    $('#btnRegistration').on('click', function () {
        debugger;
        alert("Hello! I am an alert box!!");
        common.callAjaxWithLoading(_url.insertSellerMansionData, updateData, this, function (result) {
            if (result && result.isOK) {
                //sucess
                $('#modal_1').modal('hide');
                $('#modal_2').modal('show');
            }
            if (result && result.data) {
                //error
                $('#modal_1').modal('hide');
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    });

    $('#MansionName').on('change', function () {
        $('#hdnMansionCD').val('');
        const $MansionCD = $('.tt-mansioncd');
        if ($MansionCD.get().length === 1) {
            displayMansionData($MansionCD.text());
        }
    });

}
function setScreenComfirm(data) {
    for (key in data) {
        const target = document.getElementById('confirm_' + key);
        if (target) $(target).val(data[key]);
    }

    $('#confirm_PrefCD').val(data.PrefName);
    $('#confirm_CityCD').val(data.CityName);
    $('#confirm_TownCD').val(data.TownName);

    $('#confirm_StructuralKBN').val($('input[name="StructuralKBN"]:radio:checked').next().text());

    $('#confirm_ConstYYYYMM').val($('#ConstYYYYMM').val());
    $('#confirm_RightKBN').val($('input[name="RightKBN"]:radio:checked').next().text());

    const stationContainer = $('.js-confirm-stationContainer');
    stationContainer.children().remove();

    let index = 0;
    $('.js-stationContainer .js-station').each(function () {
        const $this = $(this);
        const data = {
            LineCD: $this.find('.js-linecd').val(),
            LineName: $this.find('.js-linecd option:selected').text(),
            StationName: $this.find('.js-stationcd option:selected').text(),
            Distance: $this.find('.js-distance').val()
        }
        if (data.LineName) {
            const station = $('.js-confirm-station-template').find('.js-confirm-station').clone(true);
            index++;
            station.find('.js-linecd').attr('id', 'confirm_LineCD_' + index).val(data.LineName);
            station.find('.js-stationcd').attr('id', 'confirm_StationCD_' + index).val(data.StationName);
            station.find('.js-distance').attr('id', 'confirm_Distance_' + index).val(data.Distance);
            stationContainer.append(station);
        }
    });
}

function setCityList(mode, prefCd, defaultValue) {
    if (mode === 'add') {
        common.callAjax(_url.getCityDropDownList, { PrefCD: prefCd },
            function (result) {
                if (result && result.isOK) {
                    common.setDropDownListItems('#CityCD', result.data, placeHolder_city, defaultValue);
                }
            });
    }
    if (mode === 'remove') {
        common.removeDropDownListItems('#CityCD', placeHolder_city);
    }
}

function setTownList(mode, prefCd, cityCd, defaultValue) {
    if (mode === 'add') {
        common.callAjax(_url.getTownDropDownList, { PrefCD: prefCd, CityCD: cityCd },
            function (result) {
                if (result && result.isOK) {
                    common.setDropDownListItems('#TownCD', result.data, placeHolder_town, defaultValue);
                }
            });
    }
    if (mode === 'remove') {
        common.removeDropDownListItems('#TownCD', placeHolder_town);
    }
}

function setLineList(mode, prefCd, selector, defaultValue, callback) {
    if (!selector) selector = ".js-linecd";

    if (mode === 'add') {
        common.callAjax(_url.getLineDropDownList, { PrefCD: prefCd },
            function (result) {
                if (result && result.isOK) {
                    common.setDropDownListItems(selector, result.data, placeHolder_line, defaultValue);
                    if (callback) callback();
                }
            });
    }
    if (mode === 'remove') {
        common.removeDropDownListItems(selector, placeHolder_line);
    }
}

function setStationList(mode, lineCd, selector, defaultValue) {
    if (!selector) selector = ".js-stationcd";

    if (mode === 'add') {
        common.callAjax(_url.getStationDropDownList, { LineCD: lineCd },
            function (result) {
                if (result && result.isOK) {
                    common.setDropDownListItems(selector, result.data, placeHolder_station, defaultValue);
                }
            });
    }
    if (mode === 'remove') {
        common.removeDropDownListItems(selector, placeHolder_station);
    }
}
function customValidation_checkStation(e) {
    const $this = $(e)
    const suffix = $this.attr('id').slice(-2).replace('_', '');

    if ($('#LineCD_' + suffix).val() && !$this.val()) {
        $this.showError(common.getMessage('E102'));
        return false;
    }

    return true;
}

function customValidation_checkDistance(e) {
    const $this = $(e)
    const suffix = $this.attr('id').slice(-2).replace('_', '');
    const inputVal = $this.val();

    if ($('#LineCD_' + suffix).val()
        && $('#StationCD_' + suffix).val()
        && (!inputVal || parseInt(inputVal) === 0)) {
        $this.showError(common.getMessage('E101'));
        return false;
    }

    return true;
}

function customValidation_checkConstYYYYMM(e) {
    const $this = $(e)
    const buildingAge = $('#BuildingAge').data('building-age');

    if (buildingAge && buildingAge == 0) {
        $this.showError(common.getMessage('E208'));
        return false;
    }

    return true;
}

function getMansionStationList() {
    let array = [];

    $('.js-stationContainer .js-station').each(function (index) {
        const $this = $(this);
        const data = {
            RowNo: index + 1,
            LineCD: $this.find('.js-linecd').val(),
            StationCD: $this.find('.js-stationcd').val(),
            Distance: $this.find('.js-distance').val()
        }
        if (data.LineCD) {
            array[array.length] = data;
        }
    });

    return array;
}

function displayMansionData(mansionCD) {
    $hdnMansionCD = $('#hdnMansionCD');

    common.showLoading();
    common.callAjax(_url.getMansinoData, { mansionCD },
        function (result) {
            if (result && result.isOK) {
                const dataArray = JSON.parse(result.data);
                const length = dataArray.length;

                if (length > 0) {
                    //Clears the value of an element
                    $('.js-detail :input:not(.form-check-input):not(button):not([type=raido]):not(:hidden):not(:disabled):not([readonly])').val('').hideError();
                    $('.js-detail .form-check-input').val(["0"]).hideError(); //radio button
                    $('.js-stationContainer').children().remove();
                    for (let i = 0; i < 3; i++) {
                        const index = i + 1;
                        const station = $('.js-station-template').find('.js-station').clone(true);
                        station.find('.js-paragraph-number').text(getParagraphNumber(index))
                        station.find('.js-linecd').attr('id', 'LineCD_' + index);
                        station.find('.js-stationcd').attr('id', 'StationCD_' + index);
                        station.find('.js-distance').attr('id', 'Distance_' + index);
                        $('.js-stationContainer').append(station);
                    }

                    let data = dataArray[0];

                    $('#PrefCD').val(data.PrefCD);
                    if (data.PrefCD) {
                        setCityList('add', data.PrefCD, data.CityCD);
                        setTypeahead('#MansionName');
                    }
                    else {
                        setCityList('remove');
                        setTownList('remove');
                        setLineList('remove');
                        setStationList('remove');
                        setTypeahead('#MansionName');
                    }

                    if (data.CityCD) {
                        setTownList('add', data.PrefCD, data.CityCD, data.TownCD);
                    }
                    else {
                        setTownList('remove');
                    }

                    for (var key in data) {
                        if (key === 'PrefCD' || key === 'CityCD' || key === 'TownCD') continue;
                        if (key === 'StructuralKBN' || key === 'RightKBN') {
                            $('input:radio[name="' + key + '"]').val([data[key]]).hideError();
                        } else if (key === 'BuildingAge') {
                            $('#' + key).text('（築' + data[key] + '年）').data('building-age', data[key]);;
                        } else {
                            $('#' + key).val(data[key]).hideError();
                        }
                    }

                    var setLineAndStation = function () {
                        for (let i = 0; i < length; i++) {
                            data = dataArray[i];
                            const index = i + 1;

                            if (!document.getElementById('LineCD_' + index)) {
                                const station = $('.js-station-template').find('.js-station').clone(true);
                                station.find('.js-paragraph-number').text(getParagraphNumber(index))
                                station.find('.js-linecd').attr('id', 'LineCD_' + index);
                                station.find('.js-stationcd').attr('id', 'StationCD_' + index);
                                station.find('.js-distance').attr('id', 'Distance_' + index);
                                $('.js-stationContainer').append(station);
                            }

                            if (data.PrefCD && data.LineCD) {
                                $('#LineCD_' + index).val(data.LineCD);
                                setStationList('add', data.LineCD, '#StationCD_' + index, data.StationCD);
                            }
                            $('#Distance_' + index).val(data.Distance);
                        }
                    }

                    if (data.PrefCD)
                        setLineList('add', data.PrefCD, null, null, setLineAndStation);
                    else
                        setLineAndStation();

                    $hdnMansionCD.val(mansionCD);
                    $('#PrefCD, #CityCD, #TownCD, #MansionName, .js-linecd, .js-stationcd').hideError();
                    common.hideLoading();
                }
            }
        },
        function () {
            common.hideLoading(null, 0);
        }
    );
}
