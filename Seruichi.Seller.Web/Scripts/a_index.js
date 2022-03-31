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
    _url.checkZipCode = common.appPath + '/a_index/CheckZipCode';
    _url.getMansionListByMansionWord = common.appPath + '/a_index/GetMansionListByMansionWord';
    _url.getMansinoData = common.appPath + '/a_index/GetMansionData';
    _url.insertSellerMansionData = common.appPath + '/a_index/InsertSellerMansionData';

    setValidation();
    addEvents();
    createBloodhound();
    setTypeahead('#MansionName');
    $('#ZipCode1').focus();
});

function setValidation() {
    //郵便番号
    $('#ZipCode1')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_singlebyte_number();
    $('#ZipCode2')
        .addvalidation_errorElement("#errorZipCode")
        .addvalidation_singlebyte_number();
    //都道府県
    $('#PrefCD')
        .addvalidation_errorElement("#errorPrefCD")
        .addvalidation_reqired();
    //マンション名
    $('#MansionName')
        .addvalidation_errorElement("#errorMansionName")
        .addvalidation_reqired()
        .addvalidation_doublebyte();
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
    //交通アクセス
    $('#LineCD_1')
        .addvalidation_errorElement("#errorMansionStationInfo")
        .addvalidation_reqired();
    $('.js-StationCD')
        .addvalidation_errorElement("#errorMansionStationInfo")
        .addvalidation_custom('customValidation_checkStation');
    $('.js-Distance')
        .addvalidation_errorElement("#errorMansionStationInfo")
        .addvalidation_singlebyte_number()
        .addvalidation_custom('customValidation_checkDistance');
    //建物構造
    $('input[name="StructuralKBN"]:radio')
        .addvalidation_errorElement("#errorStructuralKBN")
        .addvalidation_reqired();
    //築年月
    $('#ConstYYYYMM')
        .addvalidation_errorElement("#errorConstYYYYMM")
        .addvalidation_reqired();
    //総戸数
    $('#Rooms')
        .addvalidation_errorElement("#errorRooms")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();
    //階
    $('#LocationFloor')
        .addvalidation_errorElement("#errorFloors")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();
    //階建て
    $('#Floors')
        .addvalidation_errorElement("#errorFloors")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();
    //部屋番号
    $('#RoomNumber')
        .addvalidation_errorElement("#errorRoomNumber")
        .addvalidation_reqired()
        .addvalidation_singlebyte_numberAlphabet();
    //専有面積
    $('#RoomArea')
        .addvalidation_errorElement("#errorRoomArea")
        .addvalidation_reqired(true)
        .addvalidation_numeric(3, 2);
    //バルコニー面積
    $('input[name="BalconyKBN"]:radio')
        .addvalidation_errorElement("#errorBalconyArea")
        .addvalidation_reqired();
    $('#BalconyArea')
        .addvalidation_errorElement("#errorBalconyArea")
        .addvalidation_numeric(3, 2);
    //主採光
    $('input[name="Direction"]:radio')
        .addvalidation_errorElement("#errorDirection")
        .addvalidation_reqired();
    //間取り
    $('#FloorType')
        .addvalidation_errorElement("#errorFloorType")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();
    //バス・トイレ
    $('input[name="BathKBN"]:radio')
        .addvalidation_errorElement("#errorBathKBN")
        .addvalidation_reqired();
    //土地・権利
    $('input[name="RightKBN"]:radio')
        .addvalidation_errorElement("#errorRightKBN")
        .addvalidation_reqired();
    //現況
    $('input[name="CurrentKBN"]:radio')
        .addvalidation_errorElement("#errorCurrentKBN")
        .addvalidation_reqired();
    //管理方式
    $('input[name="ManagementKBN"]:radio')
        .addvalidation_errorElement("#errorManagementKBN")
        .addvalidation_reqired();
    //月額家賃
    $('#RentFee')
        .addvalidation_errorElement("#errorRentFee")
        .addvalidation_reqired()
        .addvalidation_money(9);
    //管理費
    $('#ManagementFee')
        .addvalidation_errorElement("#errorManagementFee")
        .addvalidation_reqired()
        .addvalidation_money(9);
    //修繕積立金
    $('#RepairFee')
        .addvalidation_errorElement("#errorRepairFee")
        .addvalidation_reqired()
        .addvalidation_money(9);
    //その他費用
    $('#ExtraFee')
        .addvalidation_errorElement("#errorExtraFee")
        .addvalidation_reqired()
        .addvalidation_money(9);
    //固定資産税
    $('#PropertyTax')
        .addvalidation_errorElement("#errorPropertyTax")
        .addvalidation_reqired()
        .addvalidation_money(9);

    $('#btnProcess')
        .addvalidation_errorElement("#errorProcess");

}

function addEvents() {

    //共通チェック処理
    common.bindValidationEvent('#form1', ':not(#ZipCode1,#ZipCode2)');

    //郵便番号
    $('#ZipCode1,#ZipCode2').on('change', function () {
        const $this = $(this), $zipCode1 = $('#ZipCode1'), $zipCode2 = $('#ZipCode2')

        //if (!common.checkValidityInput($zipCode1) || !common.checkValidityInput($zipCode2)) {
        //    return false;
        //}
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
                    $($zipCode1, $zipCode2).hideError();
                    const data = result.data;

                    if (data.PrefCD) {
                        $('#PrefCD').val(data.PrefCD).hideError();
                        setCityList('add', data.PrefCD, data.CityCD);
                        setTownList('remove');
                        setLineList('add', data.PrefCD);
                        setStationList('remove');
                        setTypeahead('#MansionName');
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
            $('.js-Distance').val('').hideError();
            setTypeahead('#MansionName');
        }
        else {
            setCityList('remove');
            setTownList('remove');
            setLineList('remove');
            setStationList('remove');
            $('.js-Distance').val('').hideError();
            setTypeahead('#MansionName');
        }
    });

    //マンション名
    $('#MansionName').on('change', function () {
        const $MansionCD = $('.tt-mansioncd');
        if ($MansionCD.get().length === 1) {
            displayMansionData($MansionCD.text());
        }
        //else {
        //    $('#hdnMansionCD').val('');
        //}
    }).on('typeahead:selected', function (evt, data) {
        displayMansionData(data.Value);
    });

    //市区町村
    $('#CityCD').on('change', function () {
        setTownList('add', $('#PrefCD').val(), $(this).val());
    });

    //路線選択
    $('.js-LineCD').on('change', function () {
        const id = $(this).attr('id');
        const suffix = id.slice(-2);
        setStationList('add', $(this).val(), suffix);
    });

    //路線・駅追加
    $('#btnAddStation').on('click', function () {
        const stationContainer = $('.js-stationContainer');
        let index = stationContainer.find('.js-station').get().length;
        if (index < 20) {
            const station = $('.js-station-template').find('.js-station').clone(true);
            index++;
            station.find('.js-paragraph-number').text(getParagraphNumber(index))
            station.find('.js-LineCD').attr('id', 'LineCD_' + index);
            station.find('.js-StationCD').attr('id', 'StationCD_' + index);
            station.find('.js-Distance').attr('id', 'Distance_' + index);
            stationContainer.append(station);
        }
    });

    //築年月
    $('#ConstYYYYMM').on('blur', function () {
        common.callAjax(_url.getBuildingAge, $(this).val(), function (result) {
            if (result && result.isOK) {
                if (result.data) 
                    $('#BuildingAge').text('（築' + result.data + '年）')
                else
                    $('#BuildingAge').text('（築　年）')
            }
        })
    });

    //バルコニー区分
    $('input[name="BalconyKBN"]:radio').change(function () {
        if ($('input[name="BalconyKBN"]:radio:checked').val() === "1") {
            $('#BalconyArea').addvalidation_reqired(true);
        }
        else {
            $('#BalconyArea').val('').removeValidation_required().hideError();
        }
    });

    //バルコニー面積
    $('#BalconyArea').on('change', function () {
        if ($(this).val()) {
            $('#BalconyKBN_1').prop('checked', true).hideError();
            $(this).addvalidation_reqired(true);
        }
    });

    //登録
    $('#btnProcess').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        model.PrefName = $('#PrefCD option:selected').text();
        model.CityName = $('#CityCD option:selected').text();
        model.TownName = $('#TownCD option:selected').text();
        model.ConstYYYYMM = model.ConstYYYYMM.replace('-', '');
        model.MansionStationListJson = JSON.stringify(getMansionStationList());

        common.callAjaxWithLoading(_url.insertSellerMansionData, model, this, function (result) {
            if (result && result.isOK) {
                //sucess
                window.location.reload();
            }
            if (result && result.data) {
                //server validation error
                const errors = result.data;
                for (key in errors) {
                    const target = document.getElementById(key);
                    $(target).showError(errors[key]);
                    $form.getInvalidItems().get(0).focus();
                }
            }
        });
    });
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
                            $('#' + key).text('（築' + data[key] + '年）');
                        } else {
                            $('#' + key).val(data[key]).hideError();
                        }
                    }

                    for (let i = 0; i < length; i++) {
                        data = dataArray[i];
                        const index = i + 1;

                        if (!document.getElementById('LineCD_' + index)) {
                            const station = $('.js-station-template').find('.js-station').clone(true);
                            station.find('.js-paragraph-number').text(getParagraphNumber(index))
                            station.find('.js-LineCD').attr('id', 'LineCD_' + index);
                            station.find('.js-StationCD').attr('id', 'StationCD_' + index);
                            station.find('.js-Distance').attr('id', 'Distance_' + index);
                            $('.js-stationContainer').append(station);
                        }

                        if (data.PrefCD) {
                            setLineList('add', data.PrefCD, '_' + index, data.LineCD);
                            if (data.LineCD) setStationList('add', data.LineCD, '_' + index, data.StationCD);
                        }
                        $('#Distance_' + index).val(data.Distance).hideError();
                    }
                    $hdnMansionCD.val(mansionCD);
                    $('#PrefCD, #CityCD, #TownCD, #MansionName, .js-LineCD, .js-StationCD').hideError();
                    common.hideLoading();
                }
            }
        },
        function () {
            common.hideLoading(null, 0);
        }
    );
}

var bloodhoundSuggestions;
function createBloodhound() {
    bloodhoundSuggestions = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('DisplayText'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        //sufficient: 3000,
        remote: {
            url: _url.getMansionListByMansionWord,
            prepare: function (query, settings) {
                //リモートサーバーに渡すクエリを加工
                //settingsはjQueryの$.ajaxへ渡すオプション
                var data = { prefCD: $('#PrefCD').val(), searchWord: query };
                settings.type = 'POST';
                settings.contentType = 'application/json';
                settings.xhrFields = {
                    withCredentials: false,
                };
                settings.headers = {
                    'RequestVerificationToken': $("#_RequestVerificationToken").val(),
                };
                settings.data = JSON.stringify(data);
                return settings;
            },
            transform: function (res) {
                return JSON.parse(res.data);
            },
            rateLimitWait: 300
        },
        limiter: 3000,
        name: 'typeaheadSourceCache',
        ttl: 0,
        ajax: {
            cache: false
        }
    });
}

function setTypeahead(selector) {

    $(selector).typeahead('destroy').typeahead(
        {
            hint: false,
            highlight: true,
            minLength: 2
        },
        {
            name: 'data',
            display: 'DisplayText',
            source: bloodhoundSuggestions,
            templates: {
                suggestion: function (data) {
                    return '<div>' + data.DisplayText + '<span class="tt-mansioncd" style="display:none;">' + data.Value + '</span></div>';
                }
            },
            limit: 3000,
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

function setLineList(mode, prefCd, suffix, defaultValue) {
    let selector = ".js-LineCD";
    if (suffix) selector = '#LineCD' + suffix;

    if (mode === 'add') {
        common.callAjax(_url.getLineDropDownList, { PrefCD: prefCd },
            function (result) {
                if (result && result.isOK) {
                    common.setDropDownListItems(selector, result.data, placeHolder_line, defaultValue);
                }
            });
    }
    if (mode === 'remove') {
        common.removeDropDownListItems(selector, placeHolder_line);
    }
}

function setStationList(mode, lineCd, suffix, defaultValue) {
    let selector = ".js-StationCD";
    if (suffix) selector = '#StationCD' + suffix;

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
    const suffix = $this.attr('id').slice(-2);

    if ($('#LineCD' + suffix).val() && !$this.val()) {
        $this.showError(common.getMessage('E102'));
        return false;
    }

    return true;
}

function customValidation_checkDistance(e) {
    const $this = $(e)
    const suffix = $this.attr('id').slice(-2);
    const inputVal = $this.val();

    if ($('#LineCD' + suffix).val()
        && $('#StationCD' + suffix).val()
        && (!inputVal || parseInt(inputVal) === 0)) {
        $this.showError(common.getMessage('E101'));
        return false;
    }

    return true;
}

function getMansionStationList() {
    let array = [];

    $('.js-station').each(function () {
        const $this = $(this);
        const data = {
            LineCD: $this.find('.js-LineCD').val(),
            StationCD: $this.find('.js-StationCD').val(),
            Distance: $this.find('.js-Distance').val()
        }
        if (data.LineCD) {
            array[array.length] = data;
        }
    });

    return array;
}

function getParagraphNumber(number) {
    if (number === 1) return '①'
    if (number === 2) return '②'
    if (number === 3) return '③'
    if (number === 4) return '④'
    if (number === 5) return '⑤'
    if (number === 6) return '⑥'
    if (number === 7) return '⑦'
    if (number === 8) return '⑧'
    if (number === 9) return '⑨'
    if (number === 10) return '⑩'
    if (number === 11) return '⑪'
    if (number === 12) return '⑫'
    if (number === 13) return '⑬'
    if (number === 14) return '⑭'
    if (number === 15) return '⑮'
    if (number === 16) return '⑯'
    if (number === 17) return '⑰'
    if (number === 18) return '⑱'
    if (number === 19) return '⑲'
    if (number === 20) return '⑳'
}

