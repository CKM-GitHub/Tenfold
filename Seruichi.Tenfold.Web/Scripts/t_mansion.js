const _url = {};

$(function () {
    _url.checkZipCode = common.appPath + '/t_mansion_new/CheckZipCode';
    _url.getCityDropDownList = common.appPath + commonApiUrl.getDropDownListItemsOfCity;
    _url.getTownDropDownList = common.appPath + commonApiUrl.getDropDownListItemsOfTown;
    _url.getLineDropDownList = common.appPath + commonApiUrl.getDropDownListItemsOfLine;
    _url.getStationDropDownList = common.appPath + commonApiUrl.getDropDownListItemsOfStation;

    setValidation();
    addEvents();
})

function setValidation() {
    //マンション名
    $('#MansionName')
        .addvalidation_errorElement("#errorName")
        .addvalidation_reqired()
        .addvalidation_maxlengthCheck(50)
        .addvalidation_doublebyte();

    //郵便番号
    $('#ZipCode1')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_maxlengthCheck(3)
        .addvalidation_singlebyte_number();
    $('#ZipCode2')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_maxlengthCheck(4)
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
        .addvalidation_errorElement("#errorLineCD1")
        .addvalidation_reqired();
    $('#StationCD_1')
        .addvalidation_errorElement("#errorStationCD1")
        .addvalidation_reqired();
    $('#Distance_1')
        .addvalidation_errorElement("#errorDistanceCD1")
        .addvalidation_reqired();
    $('#StationCD_2')
        .addvalidation_errorElement("#errorStationCD2")
    //.addvalidation_reqired();
    $('#LineCD_2')
        .addvalidation_errorElement("#errorLineCD2")
    //.addvalidation_reqired();
    $('#StationCD_3')
        .addvalidation_errorElement("#errorStationCD3")
    //.addvalidation_reqired();
    $('#LineCD_3')
        .addvalidation_errorElement("#errorLineCD3")
    //.addvalidation_reqired();
    $('#StationCD_4')
        .addvalidation_errorElement("#errorStationCD4")
    //.addvalidation_reqired();
    $('#LineCD_4')
        .addvalidation_errorElement("#errorLineCD4")
    //.addvalidation_reqired();
    $('#StationCD_5')
        .addvalidation_errorElement("#errorStationCD5")
    //.addvalidation_reqired();
    $('#LineCD_5')
        .addvalidation_errorElement("#errorLineCD5")
    //.addvalidation_reqired();
    $('#StationCD_6')
        .addvalidation_errorElement("#errorStationCD6")
    //.addvalidation_reqired();
    $('#LineCD_6')
        .addvalidation_errorElement("#errorLineCD6")
    //.addvalidation_reqired();
    $('#StationCD_7')
        .addvalidation_errorElement("#errorStationCD7")
    //.addvalidation_reqired();
    $('#LineCD_7')
        .addvalidation_errorElement("#errorLineCD7")
    //.addvalidation_reqired();
    $('#StationCD_8')
        .addvalidation_errorElement("#errorStationCD8")
    //.addvalidation_reqired();
    $('#LineCD_8')
        .addvalidation_errorElement("#errorLineCD8")
    //.addvalidation_reqired();
    $('#StationCD_9')
        .addvalidation_errorElement("#errorStationCD9")
    //.addvalidation_reqired();
    $('#LineCD_9')
        .addvalidation_errorElement("#errorLineCD9")
    //.addvalidation_reqired();
    $('#StationCD_10')
        .addvalidation_errorElement("#errorStationCD10")
    //.addvalidation_reqired();
    $('#LineCD_10')
        .addvalidation_errorElement("#errorLineCD10")
    //.addvalidation_reqired();
    $('#StationCD_11')
        .addvalidation_errorElement("#errorStationCD11")
    //.addvalidation_reqired();
    $('#LineCD_11')
        .addvalidation_errorElement("#errorLineCD11")
    //.addvalidation_reqired();
    $('#StationCD_12')
        .addvalidation_errorElement("#errorStationCD12")
    //.addvalidation_reqired();
    $('#Distance_2')
        .addvalidation_errorElement("#errorDistanceCD2")
    //.addvalidation_reqired();
    $('#StationCD_13')
        .addvalidation_errorElement("#errorStationCD13")
    //.addvalidation_reqired();
    $('#LineCD_13')
        .addvalidation_errorElement("#errorLineCD13")
    //.addvalidation_reqired();
    $('#StationCD_14')
        .addvalidation_errorElement("#errorStationCD14")
    //.addvalidation_reqired();
    $('#LineCD_14')
        .addvalidation_errorElement("#errorLineCD14")
    //.addvalidation_reqired();
    $('#StationCD_15')
        .addvalidation_errorElement("#errorStationCD15")
    //.addvalidation_reqired();
    $('#LineCD_15')
        .addvalidation_errorElement("#errorLineCD15")
    //.addvalidation_reqired();
    $('#StationCD_16')
        .addvalidation_errorElement("#errorStationCD16")
    //.addvalidation_reqired();
    $('#LineCD_16')
        .addvalidation_errorElement("#errorLineCD16")
    //.addvalidation_reqired();
    $('#StationCD_17')
        .addvalidation_errorElement("#errorStationCD17")
    //.addvalidation_reqired();
    $('#LineCD_17')
        .addvalidation_errorElement("#errorLineCD17")
    //.addvalidation_reqired();
    $('#StationCD_18')
        .addvalidation_errorElement("#errorStationCD18")

    $('#LineCD_18')
        .addvalidation_errorElement("#errorLineCD18")
    //.addvalidation_reqired();
    $('#StationCD_19')
        .addvalidation_errorElement("#errorStationCD19")
    //.addvalidation_reqired();
    $('#LineCD_19')
        .addvalidation_errorElement("#errorLineCD19")
    //.addvalidation_reqired();
    $('#StationCD_20')
        .addvalidation_errorElement("#errorStationCD20")
    //.addvalidation_reqired();
    $('#LineCD_20')
        .addvalidation_errorElement("#errorLineCD20")
    //.addvalidation_reqired();
    $('#Distance_3')
        .addvalidation_errorElement("#errorDistanceCD3")
    //.addvalidation_reqired();
    $('#Distance_4')
        .addvalidation_errorElement("#errorDistanceCD4")
    //.addvalidation_reqired();
    $('#Distance_5')
        .addvalidation_errorElement("#errorDistanceCD5")
    //.addvalidation_reqired();
    $('#Distance_6')
        .addvalidation_errorElement("#errorDistanceCD6")
    //.addvalidation_reqired();
    $('#Distance_7')
        .addvalidation_errorElement("#errorDistanceCD7")
    //.addvalidation_reqired();
    $('#Distance_8')
        .addvalidation_errorElement("#errorDistanceCD8")
    //.addvalidation_reqired();
    $('#Distance_9')
        .addvalidation_errorElement("#errorDistanceCD9")
    //.addvalidation_reqired();
    $('#Distance_10')
        .addvalidation_errorElement("#errorDistanceCD10")
    //.addvalidation_reqired();
    $('#Distance_11')
        .addvalidation_errorElement("#errorDistanceCD11")
    //.addvalidation_reqired();
    $('#Distance_12')
        .addvalidation_errorElement("#errorDistanceCD12")
    //.addvalidation_reqired();
    $('#Distance_13')
        .addvalidation_errorElement("#errorDistanceCD13")
    //.addvalidation_reqired();
    $('#Distance_14')
        .addvalidation_errorElement("#errorDistanceCD14")
    //.addvalidation_reqired();
    $('#Distance_15')
        .addvalidation_errorElement("#errorDistanceCD15")
    //.addvalidation_reqired();
    $('#Distance_16')
        .addvalidation_errorElement("#errorDistanceCD16")
    //.addvalidation_reqired();
    $('#Distance_17')
        .addvalidation_errorElement("#errorDistanceCD17")
    //.addvalidation_reqired();
    $('#Distance_18')
        .addvalidation_errorElement("#errorDistanceCD18")
    //.addvalidation_reqired();
    $('#Distance_19')
        .addvalidation_errorElement("#errorDistanceCD19")
    //.addvalidation_reqired();
    $('#Distance_20')
        .addvalidation_errorElement("#errorDistanceCD20")
    //.addvalidation_reqired();
    $('.js-stationcd')
        .addvalidation_custom('customValidation_checkStation');
    $('.js-distance')
        .addvalidation_singlebyte_number()
        .addvalidation_custom('customValidation_checkDistance');
    //謄本表記
    $('#Noti')
        .addvalidation_errorElement("#errorNoti")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
    //カタカナ
    $('#Katakana')
        .addvalidation_errorElement("#errorKatakana")
        .addvalidation_reqired()
        .addvalidation_doublebyte_kana();

    //ｶﾀｶﾅ
    $('#Katakana1')
        .addvalidation_errorElement("#errorKatakana1")
        .addvalidation_reqired()
        .addvalidation_singlebyte_kana();
    //ひらがな
    $('#Hirakana')
        .addvalidation_errorElement("#errorHirakana")
        .addvalidation_reqired()
        .addvalidation_doublebyte_hira();


    $('#btnShowConfirmation')
        .addvalidation_errorElement("#errorProcess");
}

function addEvents() {

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