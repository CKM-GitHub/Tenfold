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
    setValidation();
    addEvents();
});

function setValidation() {
    //マンション名
    $('#Name')
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
    //$('input[name="StructuralKBN"]:radio')
    //    .addvalidation_errorElement("#errorStructuralKBN")
    //    .addvalidation_reqired();
    ////築年月
    //$('#ConstYYYYMM')
    //    .addvalidation_errorElement("#errorConstYYYYMM")
    //    .addvalidation_reqired()
    //    .addvalidation_custom('customValidation_checkConstYYYYMM');

    //$('#Rooms')
    //    .addvalidation_errorElement("#errorRooms")
    //    .addvalidation_reqired(true)
    //    .addvalidation_singlebyte_number();

    ////土地・権利
    //$('input[name="RightKBN"]:radio')
    //    .addvalidation_errorElement("#errorRightKBN")
    //    .addvalidation_reqired();

    ////交通アクセス
    //$('#LineCD_1')
    //    .addvalidation_errorElement("#errorMansionStationInfo")
    //    .addvalidation_reqired();
    //$('.js-stationcd')
    //    .addvalidation_errorElement("#errorMansionStationInfo")
    //    .addvalidation_custom('customValidation_checkStation');
    //$('.js-distance')
    //    .addvalidation_errorElement("#errorMansionStationInfo")
    //    .addvalidation_singlebyte_number()
    //    .addvalidation_custom('customValidation_checkDistance');

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
    //$('.js-linecd').on('change', function () {
    //    const id = $(this).attr('id');
    //    const suffix = id.slice(-2).replace('_', '');
    //    const inputval = $(this).val();
    //    if (inputval) {
    //        setStationList('add', inputval, '#StationCD_' + suffix);
    //    }
    //    else {
    //        setStationList('remove', inputval, '#StationCD_' + suffix);
    //    }
    //});

    ////路線・駅追加
    //$('#btnAddStation').on('click', function () {
    //    const stationContainer = $('.js-stationContainer');
    //    let index = stationContainer.find('.js-station').get().length;
    //    if (index < 20) {
    //        const station = $('.js-station-template').find('.js-station').clone(true);
    //        index++;
    //        station.find('.js-paragraph-number').text(getParagraphNumber(index))
    //        station.find('.js-linecd').attr('id', 'LineCD_' + index);
    //        station.find('.js-stationcd').attr('id', 'StationCD_' + index);
    //        station.find('.js-distance').attr('id', 'Distance_' + index);
    //        stationContainer.append(station);
    //    }
    //});

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

