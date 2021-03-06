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
    _url.getNearestStations = common.appPath + commonApiUrl.getNearestStations;
    _url.checkZipCode = common.appPath + '/t_mansion_new/CheckZipCode';
    _url.getMansionListByMansionWord = common.appPath + '/t_mansion_new/GetMansionListByMansionWord';
    _url.checkAll = common.appPath + '/t_mansion_new/CheckAll';
    _url.insertSellerMansionData = common.appPath + '/t_mansion_new/InsertSellerMansionData';
    _url.gotoNextPage = common.appPath + '/t_mansion_new/GotoNextPage';
    setValidation();
    addEvents();
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_mansion_list').addClass('font-bold text-underline');
    $('#MansionName').focus();
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

    $('#confirm_BuildingAge').text($('#BuildingAge').text());

    $('#Rooms')
        .addvalidation_errorElement("#errorRooms")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();


    $('#Floors')
        .addvalidation_errorElement("#errorFloors")
        .addvalidation_reqired(true)
        .addvalidation_singlebyte_number();

    ////土地・権利
    $('input[name="RightKBN"]:radio')
        .addvalidation_errorElement("#errorRightKBN")
        .addvalidation_reqired();

    ////交通アクセス
    ErrorLineStationDistance();
   
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

    $('#Remark')
        .addvalidation_errorElement("#errorRemark")
        .addvalidation_singlebyte_doublebyte();

    $('#btnShowConfirmation')
        .addvalidation_errorElement("#errorProcess");

}

function addEvents() {
  

    //共通チェック処理
    common.bindValidationEvent('#form1', ':not(#ZipCode1,#ZipCode2)');


    $(function () { $("#form1").submit(function () { return false; }); });

    $('.container-fluid .card-body').keypress(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
        }
    });

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

        removeLineAndStation();

        if (inputval) {
            setCityList('add', inputval);
            setTownList('remove');
            setLineList('add', inputval);
            //setStationList('remove');
            //$('.js-distance').val('').hideError();          
        }
        else {
            setCityList('remove');
            setTownList('remove');
            //setLineList('remove');
            //setStationList('remove');
            //$('.js-distance').val('').hideError();
        }
    });
    //市区町村
    $('#CityCD').on('change', function () {
        setTownList('add', $('#PrefCD').val(), $(this).val());
    });

    //町域、住所
    $('#TownCD, #Address').on('change', function () {
        displayNearestStationData();
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
        model.MansionWordListJson = JSON.stringify(getMansionWordList());
        common.callAjaxWithLoading(_url.checkAll, model, this, function (result) {
            if (result && result.isOK) {
                updateData = model;
                setScreenComfirm(updateData);
                $('#modal_1').modal('show');
            }
            if (result && result.data) {
                //error
                
                common.setValidationErrors(result.data);
                common.setFocusFirstError($form);
            }
        });
    });

    $('#btnRegistration').on('click', function () {
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

    $('#btnreturn').on('click', function () {
        $form = $('#form1').hideChildErrors();
        window.location.href = common.appPath + '/t_mansion_list/Index';
    }); 

    $('#btnComplete, #btnCancel').on('click', function () {
        window.location.reload();
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
    $('#confirm_BuildingAge').text($('#BuildingAge').text());
    $('#confirm_Noti').text($('#Noti').text());
    $('#confirm_Katakana').text($('#Katakana').text());
    $('#confirm_Katakana1').text($('#Katakana1').text());
    $('#confirm_Hirakana').text($('#Hirakana').text());
    $('#confirm_Other1').text($('#Other1').text());
    $('#confirm_Other2').text($('#Other2').text());
    $('#confirm_Other3').text($('#Other3').text());
    $('#confirm_Other4').text($('#Other4').text());
    $('#confirm_Other5').text($('#Other5').text());
    $('#confirm_Other6').text($('#Other6').text());
    $('#confirm_Remark').text($('#Remark').text());
    $('#confirm_Floors').text($('#Floors').text());
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
            station.find('.js-paragraph-number').text(getParagraphNumber(index))
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

function getParagraphNumber(number) {
    if (number === 1) return '路線1'
    if (number === 2) return '路線2'
    if (number === 3) return '路線3'
    if (number === 4) return '路線4'
    if (number === 5) return '路線5'
    if (number === 6) return '路線6'
    if (number === 7) return '路線7'
    if (number === 8) return '路線8'
    if (number === 9) return '路線9'
    if (number === 10) return '路線10'
    if (number === 11) return '路線11'
    if (number === 12) return '路線12'
    if (number === 13) return '路線13'
    if (number === 14) return '路線14'
    if (number === 15) return '路線15'
    if (number === 16) return '路線16'
    if (number === 17) return '路線17'
    if (number === 18) return '路線18'
    if (number === 19) return '路線19'
    if (number === 20) return '路線20'
}

function getMansionWordList() {
    let array = [];
    $('.js-wordContainer .js-word').each(function (index) {
        const $this = $(this);
        const data = {
            RowNo: index + 1,
            WordSEQ:  index+1,
            Word: $this.find('.noti').val()
        }
        if (data.Word) {
            array[array.length] = data;
        }
    });
      
    return array;
}

function displayNearestStationData() {
    if ($('#hdnMansionCD').val()) return;

    const model = {
        PrefName: $('#PrefCD option:selected').text(),
        CityName: $('#CityCD option:selected').text(),
        TownName: $('#TownCD option:selected').text(),
        Address: $('#Address').val()
    }

    if (model.PrefName && model.CityName && model.TownName && model.Address) {

        common.showLoading();
        common.callAjax(_url.getNearestStations, model,
            function (result) {
                if (result && result.isOK) {

                    removeLineAndStation();

                    if (result.data) {
                        const dataArray = JSON.parse(result.data);
                        const length = dataArray.length;

                        if (length > 0) {
                            let data = dataArray[0];
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

                                if (data.LineCD) {
                                    $('#LineCD_' + index).val(data.LineCD);
                                    setStationList('add', data.LineCD, '#StationCD_' + index, data.StationCD);
                                }

                                $('#Distance_' + index).val(data.Distance);

                                if (data.Distance) {
                                    const lastDistance = index + 1;
                                    $('#Dline_' + lastDistance ).removeClass("bg-secondary");
                                    $('#Dline_' + lastDistance ).find("*").prop("disabled", false);
                                }
                                
                            }

                        }
                    }
                }
                common.hideLoading();
            },
            function () {
                common.hideLoading(null, 0);
            }
        );
    }
}

function removeLineAndStation() {
    $('.js-stationContainer').children().remove();
    for (let i = 0; i < 20; i++) {
        const index = i + 1;
        const station = $('.js-station-template').find('.js-station').clone(true);
        station.find('.card').attr('id', 'Dline_' + index);
        station.find('.js-paragraph-number').text(getParagraphNumber(index));
        station.find('.line-error').attr('id', 'errorLineCD' + index);
        station.find('.js-linecd').attr('id', 'LineCD_' + index);
        station.find('.station-error').attr('id', 'errorStationCD' + index);
        station.find('.js-stationcd').attr('id', 'StationCD_' + index);
        station.find('.distance-error').attr('id', 'errorDistanceCD' + index);
        station.find('.js-distance').attr('id', 'Distance_' + index);
        $('.js-stationContainer').append(station);
        if (i != 0) {
            $('#Dline_' + index).addClass("bg-secondary");
            $('#Dline_' + index).find("*").prop("disabled", true);
        }
    }
    ErrorLineStationDistance();
}

function ErrorLineStationDistance() {
    $('#LineCD_1')
        .addvalidation_errorElement("#errorLineCD1")
        .addvalidation_reqired();
    $('#StationCD_1')
        .addvalidation_errorElement("#errorStationCD1")
        .addvalidation_reqired();
    $('#Distance_1')
        .addvalidation_errorElement("#errorDistanceCD1")
      //  .addvalidation_reqired();
    $('#LineCD_2')
        .addvalidation_errorElement("#errorLineCD2")
    //.addvalidation_reqired();
    $('#StationCD_2')
        .addvalidation_errorElement("#errorStationCD2")
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
}

